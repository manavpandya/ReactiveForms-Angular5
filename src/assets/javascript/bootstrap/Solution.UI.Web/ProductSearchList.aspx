<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="ProductSearchList.aspx.cs" Inherits="Solution.UI.Web.ProductSearchList" %>

<%@ MasterType VirtualPath="~/Site.Master" %>
<%@ Register Src="~/Controls/LeftMenu.ascx" TagName="LeftMenu" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript" src="/js/jquery.min.js"></script>
    <script type="text/javascript" src="/js/jquery.jcarousel.pack.js"></script>
    <script type="text/javascript" src="/js/gallery.js"></script>
    <script src="/js/popup.js" type="text/javascript"></script>
    <link rel="stylesheet" href="/css/general.css" type="text/css" media="screen" />
    <script type="text/javascript">
        function resetcheckvalue(valid) {
            if (document.getElementById("prepage1") != null) {
                document.getElementById("prepage1").style.display = '';

            }
            document.getElementById('ContentPlaceHolder1_hdnallpages').value = '11';

            document.getElementById('ContentPlaceHolder1_hdnsearchvalue').value = valid; __doPostBack('ctl00$ContentPlaceHolder1$lnkresetdata', '');
        }
        function IndexValidation() {
            if ((document.getElementById('ContentPlaceHolder1_txtFrom').value).replace(/^\s*\s*$/g, '') != '' || (document.getElementById('ContentPlaceHolder1_txtTo').value).replace(/^\s*\s*$/g, '') != '') {
                if ((document.getElementById('ContentPlaceHolder1_txtFrom').value).replace(/^\s*\s*$/g, '') == '') {
                    alert('Please Enter Valid Price.');
                    document.getElementById('ContentPlaceHolder1_txtFrom').focus();
                    return false;
                }
                if ((document.getElementById('ContentPlaceHolder1_txtTo').value).replace(/^\s*\s*$/g, '') == '') {
                    alert('Please Enter Valid Price.');
                    document.getElementById('ContentPlaceHolder1_txtTo').focus();
                    return false;
                }
                var FromPrice = parseFloat((document.getElementById('ContentPlaceHolder1_txtFrom').value)).toFixed(2);
                var ToPrice = parseFloat((document.getElementById('ContentPlaceHolder1_txtTo').value)).toFixed(2);
                if (parseFloat(ToPrice) < parseFloat(FromPrice)) {
                    alert('Low Price should be Less than High Price.');
                    document.getElementById('ContentPlaceHolder1_txtFrom').focus();
                    return false;
                }
            }
            document.getElementById("ContentPlaceHolder1_hdnallpages").value = '2';
            return true;
        }
        function checkPricevalidation() {

            if (document.getElementById("ContentPlaceHolder1_txtFrom") != null && document.getElementById("ContentPlaceHolder1_txtFrom").value == "") {
                alert("Please enter from price.");
                document.getElementById("ContentPlaceHolder1_txtFrom").focus();
                return false;
            }
            else if (document.getElementById("ContentPlaceHolder1_txtTo") != null && document.getElementById("ContentPlaceHolder1_txtTo").value == "") {
                alert("Please enter to price.");
                document.getElementById("ContentPlaceHolder1_txtTo").focus();
                return false;
            }
            else {
                var fromPrice = parseFloat(document.getElementById("ContentPlaceHolder1_txtFrom").value);
                var toPrice = parseFloat(document.getElementById("ContentPlaceHolder1_txtTo").value);
                if (parseFloat(fromPrice) > parseFloat(toPrice)) {
                    alert("Please enter valid price range.");
                    document.getElementById("ContentPlaceHolder1_txtFrom").focus();
                    return false;
                }
                else {
                    return true;
                }
            }
            return true;
        }
        function checkallpage()
        { 
            $(".toggle1").slideToggle('1100');
            
            if(document.getElementById("option-arrow") != null && document.getElementById("option-arrow").src.toString().toLowerCase().indexOf('option-arrow-up.png') > -1)
            {
                document.getElementById("option-arrow").src ='/images/option-arrow-down.png';
                 
                
                
            }
            else if(document.getElementById("option-arrow") != null && document.getElementById("option-arrow").src.toString().toLowerCase().indexOf('option-arrow-down.png') > -1)
            {
                document.getElementById("option-arrow").src ='/images/option-arrow-up.png';
            }
            
        }
        function CheckSelection() {
            if (document.getElementById("prepage1") != null) {
                document.getElementById("prepage1").style.display = '';
            }
            document.getElementById("ContentPlaceHolder1_hdnallpages").value = '1';
            document.getElementById("ContentPlaceHolder1_btnIndexPriceGo1").click();
        }

        function ColorSelection(clrvalue) {
            if (document.getElementById("prepage1") != null) {
                document.getElementById("prepage1").style.display = '';
            }
            document.getElementById("ContentPlaceHolder1_hdnallpages").value = '1';
            document.getElementById("ContentPlaceHolder1_hdnColorSelection").value = clrvalue;
            document.getElementById("ContentPlaceHolder1_btnIndexPriceGo1").click();
        }
        function pagingSelection(pageval) {


            document.getElementById("ContentPlaceHolder1_hdnallpages").value = '8';
            document.getElementById("ContentPlaceHolder1_hdnpagenumber").value = pageval;
            if (document.getElementById("ContentPlaceHolder1_hdnquickview").value == '1') {
                document.getElementById('form1').submit();
            }
        }
        function keyRestrict(e, validchars) {
            var key = '', keychar = '';
            key = getKeyCode(e);
            if (key == null) return true;
            keychar = String.fromCharCode(key);
            keychar = keychar.toLowerCase();
            validchars = validchars.toLowerCase();
            if (validchars.indexOf(keychar) != -1)
                return true;
            if (key == null || key == 0 || key == 8 || key == 9 || key == 13 || key == 27 || key == 46)
                return true;
            return false;
        }
        function getKeyCode(e) {
            if (window.event)
                return window.event.keyCode;
            else if (e)
                return e.which;
            else
                return null;
        }

        function Checkselectcheckbox(ChkValue) {
            var allElts = document.getElementById('divPrice').getElementsByTagName('INPUT');
            var i;
            for (i = 0; i < allElts.length; i++) {
                var elt = allElts[i];
                if (elt.type == "checkbox") {
                    if (elt.value == ChkValue) {
                        elt.checked = true;
                    }
                }
            }
        }


        function CheckSelectedCustomValue(ChkValue) {
            var allElts = document.getElementById('divCustom').getElementsByTagName('INPUT');
            var i;
            for (i = 0; i < allElts.length; i++) {
                var elt = allElts[i];
                if (elt.type == "checkbox") {
                    if (elt.value == ChkValue) {
                        elt.checked = true;
                    }
                }
            }
        }

        function unselectcheckbox(chkelement) {
            var allElts = document.getElementById('divPrice').getElementsByTagName('INPUT');
            var i;
            for (i = 0; i < allElts.length; i++) {
                var elt = allElts[i];
                if (elt.type == "checkbox") {
                    if (elt.name != chkelement) {
                        elt.checked = false;
                    }
                }
            }
            CheckSelection();
        }

        function unselectcheckboxforCustom(chkelement) {
            var allElts = document.getElementById('divCustom').getElementsByTagName('INPUT');
            var i;
            for (i = 0; i < allElts.length; i++) {
                var elt = allElts[i];
                if (elt.type == "checkbox") {
                    if (elt.name != chkelement) {
                        elt.checked = false;
                    }
                }
            }
            CheckSelection();
        }

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
    <script type="text/javascript">
        var pc = 0;
        var ProductId=',';
        $(document).ready(function () {
            $("#ContentPlaceHolder1_grid_view").attr('class', 'grid-click');
            $("#ContentPlaceHolder1_grid_bottom").attr('class', 'grid-click');
        });
        function DeleteProductData(i, id) {
            //            document.getElementById('ContentPlaceHolder1_hdnCompare').value = id;
            //            document.getElementById('ContentPlaceHolder1_btnCompareID').click();

            if (i == -1 && id == -1) {

                if(document.getElementById('ContentPlaceHolder1_topMiddle') != null)
                {
                    var allElts = document.getElementById('ContentPlaceHolder1_topMiddle').getElementsByTagName('input');
                    for (i = 0; i < allElts.length; i++) {
                        var elt = allElts[i];
                        if (elt.type == "checkbox") {
                            if (elt.checked == true) {
                                elt.checked = false;
                            }
                        }
                    }
                }


                compare(-1, -1);
            }
            else {
                var chkProduct = 'add_to_compare' + id.toString();
                if (document.getElementById(chkProduct) != null) {
                    document.getElementById(chkProduct).checked = false;
                }
                chkProduct = 'add_to_compare_list' + id.toString();
                if (document.getElementById(chkProduct) != null) {
                    document.getElementById(chkProduct).checked = false;
                }
                compare(id, 0);

            }

        }
        function comaprebutonclicklist(id) {

            var pid = 0;
            var cid = 0;
            if (document.getElementById(id) != null && document.getElementById(id).checked == true) {
                pid = id.replace('add_to_compare_list', '');
                cid = 1;
                compare(pid, cid);
            }
            else {
                pid = id.replace('add_to_compare_list', '');
                cid = 0;
                compare(pid, cid);
                //if (chkstatus == 0 && pid != 0) { $("input:checkbox[id= add_to_compare_grid" + pid + "]").attr("checked", false); }
            }
            $('html, body').animate({ scrollTop: 0 }, 'slow');
        }

        function comaprebutonclick(id) {

            var pid = 0;
            var cid = 0;
            //  alert(document.getElementById(id));
            if (document.getElementById(id) != null && document.getElementById(id).checked == true) {
                pid = id.replace('add_to_compare', '');
                cid = 1;
                compare(pid, cid);
            }
            else {

                pid = id.replace('add_to_compare', '');
                cid = 0;
                compare(pid, cid);
            }

            $('html, body').animate({ scrollTop: 0 }, 'slow');
        }

        function compare(pid, chkstatus) {

            //            $('#co').load("/comapretable.aspx?cid=" + chkstatus + "&pid=" + pid, function (response, status, xhr) {
            //                if (status == "error") {
            //                    var msg = "Sorry but there was an error: ";
            //                    $("#error").html(msg + xhr.status + " " + xhr.statusText);
            //                }

            //            });
            //            if (document.getElementById('view1').value == "grid") {
            //                if (chkstatus == 0 && pid != 0) { $("input:checkbox[id= add_to_compare" + pid + "]").attr("checked", false); }
            //            }
            //            if (document.getElementById('view1').value == "list") {
            //                if (chkstatus == 0 && pid != 0) { $("input:checkbox[id= add_to_compare_grid" + pid + "]").attr("checked", false); }
            //            }

            if(pid != -1 && chkstatus != -1)
            {
                var numItems = $('.comparison-box').length;

                if (numItems > 4 && chkstatus !=0) {
                
                    alert( 'You can compare max. 5 product');

                    if (document.getElementById('view1').value == "grid") {
                        $("input:checkbox[id= add_to_compare" + pid + "]").attr("checked", false);
                    }
                    if (document.getElementById('view1').value == "list") {
                        $("input:checkbox[id= add_to_compare_list" + pid + "]").attr("checked", false);
                    }
                    $('html, body').animate({ scrollTop: 0 }, 'slow');

                    return;

                }
            }


            var requestUrl = "/comparetable.aspx?cid=" + chkstatus + "&pid=" + pid;

            $.ajax({
                type: "POST",
                url: "/comparetable.aspx?cid=" + chkstatus + "&pid=" + pid + "",
                data: "",
                dataType: "html",
                async: "false",
                cache: "false",
                success: function (msg) {
                    $('#co').html(msg);
                },
                Error: function (x, e) {
                    alert("Sorry but there was an error");
                }
            });


            if (document.getElementById('view1').value == "grid") {
                if (chkstatus == 0 && pid != 0) { $("input:checkbox[id= add_to_compare" + pid + "]").attr("checked", false); }
            }
            if (document.getElementById('view1').value == "list") {
                if (chkstatus == 0 && pid != 0) { $("input:checkbox[id= add_to_compare_list" + pid + "]").attr("checked", false); }
            }
        }
        //   $(document).ready(function () {
        var flagin = 0;

        function changeview() {
            //flagin = 0; var reseet = document.getElementById('ContentPlaceHolder1_hdncnt');
            // reseet.value = '12';
            // var setdiv = document.getElementById('ContentPlaceHolder1_divcount');
            // setdiv.value = '1';
            var view = document.getElementById('view1');
            var vie = view.value;
            if (vie == 'grid') {
                var change = document.getElementById('view1');
                change.value = 'list';
            } else if (vie == 'list') {
                var change1 = document.getElementById('view1');
                change1.value = 'grid';
            }
        }
        function Load() {

            //            var productcount = document.getElementById('ContentPlaceHolder1_productcount');
            //            var totalproductstodisplay = productcount.value;
            //            // alert(totalproductstodisplay);
            //            //    if (flagin == 1) { return; }
            //            if (totalproductstodisplay == 0) { return; }
            //            var checkdiv = document.getElementById('ContentPlaceHolder1_divcount');
            //            var temp = document.getElementById('ContentPlaceHolder1_hdncnt');
            //            var Take; var Skip;
            //            Skip = parseInt(temp.value) + 1;
            //            Take = parseInt(temp.value) + 12;
            //            if (totalproductstodisplay < Skip) { return; }
            //            $('#divPostsLoader').html('<img src="/images/220.gif">');
            //            var view = document.getElementById('view1');
            //            var v = view.value;
            //            var ddlsortby = document.getElementById('ContentPlaceHolder1_ddlTopPrice').selectedIndex;

            //            var divcount = document.getElementById('ContentPlaceHolder1_divcount');
            //            var newcount; newcount = parseInt(divcount.value) + 1;
            //            if (totalproductstodisplay <= Take && flagin == 0) {
            //                // alert('last');
            //                $('#ContentPlaceHolder1_Div' + parseInt(divcount.value)).load("/scrolling.aspx?catid=" + catid + "&skip=" + Skip + "&take=" + Take + "&view=" + v + "&sortby=" + ddlsortby, function (response, status, xhr) {
            //                    if (status == "error") {
            //                        var msg = "Sorry but there was an error: ";
            //                        $("#error").html(msg + xhr.status + " " + xhr.statusText);
            //                    }
            //                    if (status == "success") { $('#divPostsLoader').empty(); }
            //                });
            //                // $('#ContentPlaceHolder1_Div' + parseInt(divcount.value)).after("<div id='ContentPlaceHolder1_Div" + newcount + "' class='fp-main-bg-scroll'></div>");

            //                flagin = 1;
            //            }
            //            else {
            //                $('#ContentPlaceHolder1_Div' + parseInt(divcount.value)).load("/scrolling.aspx?catid=" + catid + "&skip=" + Skip + "&take=" + Take + "&view=" + v + "&sortby=" + ddlsortby, function (response, status, xhr) {
            //                    if (status == "error") {
            //                        var msg = "Sorry but there was an error: ";
            //                        $("#error").html(msg + xhr.status + " " + xhr.statusText);
            //                    }
            //                    if (status == "success") { $('#divPostsLoader').empty(); }
            //                });
            //                $('#ContentPlaceHolder1_Div' + parseInt(divcount.value)).after("<div id='ContentPlaceHolder1_Div" + newcount + "' class='fp-main-bg-scroll'></div>");
            //            }
            //            var newdivcount = document.getElementById('ContentPlaceHolder1_divcount');
            //            newdivcount.value = newcount;
            //            var e = document.getElementById('ContentPlaceHolder1_hdncnt');
            //            e.value = Take;
        };

        $(window).scroll(function () {
            //var e = $("#comare_div");
            // if ($(this).scrollTop() > 300) {  e.attr("class", "comparison-main-fixed"); }
            if ($(window).scrollTop() == $(document).height() - $(window).height()) {
                //Load();
            }
        });
        //    });
    </script>
    <script type="text/javascript">

        function ShowModelQuick(id, pcnt, pcnt1) {
            disablePopup();
            //document.getElementById("frmdisplayquick").contentWindow.document.body.innerHTML = '';
            document.getElementById('frmdisplayquick').height = '500px';
            document.getElementById('frmdisplayquick').width = '1000px';
            document.getElementById('popupContact').setAttribute("style", "z-index: 1000001; top: 0px; padding: 0px;width:1000px;height:500px;position:absolute;background: none repeat scroll 0 0 rgba(3, 3, 3, 0.3) !important; padding: 10px !important;border:none !important;");
            document.getElementById('popupContact').style.width = '1000px';
            document.getElementById('popupContact').style.height = '500px';
            //            window.scrollTo(0, 0);

            document.getElementById('ContentPlaceHolder1_hdnquickview').value = '1';
            var prev1 = parseInt(pcnt) - parseInt(1);
            var iVarnameprev = 'ContentPlaceHolder1_RepProduct_hdnProductId_' + prev1;
            var next1 = parseInt(pcnt) + parseInt(1);
            var iVarnamenext = 'ContentPlaceHolder1_RepProduct_hdnProductId_' + next1;




            var iVarnameprev1 = 'ContentPlaceHolder1_Repeaterlistview_hdnProductId_' + prev1;

            var iVarnamenext1 = 'ContentPlaceHolder1_Repeaterlistview_hdnProductId_' + next1;


            if (document.getElementById(iVarnameprev) != null) {

                if (document.getElementById(iVarnamenext) != null) {
                    document.getElementById('frmdisplayquick').src = '/QuickView.aspx?PID=' + id + '&next=' + document.getElementById(iVarnamenext).value.toString() + '&prev=' + document.getElementById(iVarnameprev).value.toString() + '&pcnt=' + pcnt;
                }
                else {
                    document.getElementById('frmdisplayquick').src = '/QuickView.aspx?PID=' + id + '&next=0&prev=' + document.getElementById(iVarnameprev).value.toString() + '&pcnt=' + pcnt;
                }
            }
            else if (document.getElementById(iVarnamenext) != null) {
                if (document.getElementById(iVarnameprev) != null) {
                    document.getElementById('frmdisplayquick').src = '/QuickView.aspx?PID=' + id + '&next=' + document.getElementById(iVarnamenext).value.toString() + '&prev=' + document.getElementById(iVarnameprev).value.toString() + '&pcnt=' + pcnt;
                }
                else {
                    document.getElementById('frmdisplayquick').src = '/QuickView.aspx?PID=' + id + '&next=' + document.getElementById(iVarnamenext).value.toString() + '&prev=0&pcnt=' + pcnt;
                }

            }
            else if (document.getElementById(iVarnameprev1) != null) {

                if (document.getElementById(iVarnamenext1) != null) {
                    document.getElementById('frmdisplayquick').src = '/QuickView.aspx?PID=' + id + '&next=' + document.getElementById(iVarnamenext1).value.toString() + '&prev=' + document.getElementById(iVarnameprev1).value.toString() + '&pcnt=' + pcnt;
                }
                else {
                    document.getElementById('frmdisplayquick').src = '/QuickView.aspx?PID=' + id + '&next=0&prev=' + document.getElementById(iVarnameprev1).value.toString() + '&pcnt=' + pcnt;
                }
            }
            else if (document.getElementById(iVarnamenext1) != null) {
                if (document.getElementById(iVarnameprev1) != null) {
                    document.getElementById('frmdisplayquick').src = '/QuickView.aspx?PID=' + id + '&next=' + document.getElementById(iVarnamenext1).value.toString() + '&prev=' + document.getElementById(iVarnameprev1).value.toString() + '&pcnt=' + pcnt;
                }
                else {
                    document.getElementById('frmdisplayquick').src = '/QuickView.aspx?PID=' + id + '&next=' + document.getElementById(iVarnamenext1).value.toString() + '&prev=0&pcnt=' + pcnt;
                }

            }
            else {
                document.getElementById('frmdisplayquick').src = '/QuickView.aspx?PID=' + id + '&next=0&prev=0&pcnt=' + pcnt;
            }
            //ContentPlaceHolder1_RepProduct_imgName_1

            centerPopup();
            loadPopup();

            //            centerPopup1();
            //            loadPopup1();
        }

        function Loader() {
            //   reset values for scrolling
            flagin = 0; var rese = document.getElementById('ContentPlaceHolder1_hdncnt');
            rese.value = '12';
            var setdiv = document.getElementById('ContentPlaceHolder1_divcount');
            setdiv.value = '1';
            //
            if (document.getElementById('prepage') != null) {
                var windowHeight = 0;
                windowHeight = $(document).height(); //window.innerHeight;

                document.getElementById('prepage').style.height = windowHeight + 'px';
                document.getElementById('prepage').style.display = '';
            }
        }
        function adtocart(price, id) {
            if (document.getElementById('prepage') != null) {
                var windowHeight = 0;
                windowHeight = $(document).height(); //window.innerHeight;

                document.getElementById('prepage').style.height = windowHeight + 'px';
                document.getElementById('prepage').style.display = '';
            }
            document.getElementById("ContentPlaceHolder1_hdnPrice").value = price;
            document.getElementById("ContentPlaceHolder1_hdnproductId").value = id;
            //  document.getElementById("ContentPlaceHolder1_btnAddtocart").click();
        }

        function chkHeight() {

            if (document.getElementById('prepage')) {
                var windowHeight = 0;
                windowHeight = $(document).height(); //window.innerHeight;

                document.getElementById('prepage').style.height = windowHeight + 'px';
                document.getElementById('prepage').style.display = '';
            }
        }
      
    </script>
    <link rel="stylesheet" type="text/css" href="/css/jquery-ui.css" />
    <script type="text/javascript">
        function newPopup(url) {
            popupWindow = window.open(url, 'popUpWindow', 'height=900,width=1024,left=10,top=10,resizable=no,scrollbars=yes,toolbar=no,menubar=no,location=no,directories=no,status=yes')
        }
        
    </script>
    <asp:ScriptManager ID="sc1" runat="server">
    </asp:ScriptManager>
    <div class="featured-product-bg">
        <div class="fp-title" id="titlebox" runat="server">
            <h2>
                <asp:Literal ID="ltTitle" runat="server"></asp:Literal>&nbsp;&nbsp;<asp:LinkButton
                    ID="lnkresetdata" runat="server" Style="font-size: 14px; color: #848383; display: none;"
                    Text="[Reset]" OnClick="lnkresetdata_Click"></asp:LinkButton></h2>
        </div>
        <div visible="false" id="divSelectedList" runat="server" class="fp-title search-title" >
            <a href="javascript:void(0);" onclick="resetcheckvalue(6);">
                <img src="/images/reset_search_all.png" style="margin-top: 0px; margin-right: 2px;
                    vertical-align: middle; float: right;"></a>
            <asp:Literal ID="ltbreadcrmbs" runat="server"></asp:Literal>
        </div>
        <asp:UpdatePanel ID="updliteral" runat="server">
            <ContentTemplate>
                <div id="co">
                </div>
                <div>
                    <div class="comparison-main" id="comare_div" runat="server" visible="false" style="width: 98.5% !important;">
                        <asp:Literal ID="ltrProduct" runat="server" Visible="false"></asp:Literal>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
        <div class="search-main">
            <div class="search-content-left">

            
        <div class="option-pro-main" id="hideIndexOptionDiv" runat="server" >
            <div class="colors-box">
                <div class="option-probox-title">
                    <span>Colors</span></div>
                <asp:Literal ID="ltrColor" runat="server"></asp:Literal>
            </div>
            <div class="pattern-box">
                <div class="option-probox-title">
                    <span>Header</span></div>
                <asp:Literal ID="ltrHeader" runat="server"></asp:Literal>
            </div>
            <div class="pattern-box">
                <div class="option-probox-title">
                    <span>Pattern</span></div>
                <asp:Literal ID="ltrPattern" runat="server"></asp:Literal>
            </div>
            <div class="pattern-box">
                <div class="option-probox-title">
                    <span>Fabric</span></div>
                <asp:Literal ID="ltrFabric" runat="server"></asp:Literal>
            </div>
            <div class="pattern-box">
                <div class="option-probox-title">
                    <span>Style</span></div>
                <asp:Literal ID="ltrStyle" runat="server"></asp:Literal>
            </div>
            <div class="pattern-box">
                <div class="option-probox-title">
                    <span>Custom</span></div>
                <div class="toggle1" id="divCustom">
                    <ul id="mycarousel6" class="jcarousel-skin-tango2">
                        <li>
                            <ul class="option-pro">
                                <li class="pattern-pro-box">
                                    <input type="checkbox" class="checkbox" name="chkCustom_Yes" value="Yes" onchange="unselectcheckboxforCustom('chkCustom_Yes');"
                                        onclick="unselectcheckboxforCustom('chkCustom_Yes');" />
                                    <span>Yes</span></li>
                                <li class="pattern-pro-box">
                                    <input type="checkbox" class="checkbox" name="chkCustom_No" value="No" onchange="unselectcheckboxforCustom('chkCustom_No');"
                                        onclick="unselectcheckboxforCustom('chkCustom_No');" />
                                    <span>No</span> </li>
                            </ul>
                        </li>
                    </ul>
                </div>
            </div>
            <div class="price-box-main">
                <div class="option-probox-title">
                    <span>Price</span>
                </div>
                <div class="toggle1" id="divPrice" style="height: 238px;">
                    <div class="price-box-bg">
                        <ul>
                            <li>
                                <input type="checkbox" class="checkbox" name="chkPrice_0" value="< 10" onchange="unselectcheckbox('chkPrice_0');" />
                                <span>Less than $10</span></li>
                            <li>
                                <input type="checkbox" class="checkbox" name="chkPrice_1" value=">= 10 ~ <= 20" onchange="unselectcheckbox('chkPrice_1');"
                                    onclick="unselectcheckbox('chkPrice_1');" />
                                <span>$10 to $20</span></li>
                            <li>
                                <input type="checkbox" class="checkbox" name="chkPrice_2" value=">= 20 ~ <= 40" onchange="unselectcheckbox('chkPrice_2');"
                                    onclick="unselectcheckbox('chkPrice_2');" />
                                <span>$20 to $40</span></li>
                            <li>
                                <input type="checkbox" class="checkbox" name="chkPrice_3" value=">= 40 ~ <= 60" onchange="unselectcheckbox('chkPrice_3');"
                                    onclick="unselectcheckbox('chkPrice_3');" />
                                <span>$40 to $60</span></li>
                            <li>
                                <input type="checkbox" class="checkbox" name="chkPrice_4" value=">= 60 ~ <= 80" onchange="unselectcheckbox('chkPrice_4');"
                                    onclick="unselectcheckbox('chkPrice_4');" />
                                <span>$60 to $80</span></li>
                            <li>
                                <input type="checkbox" class="checkbox" name="chkPrice_5" value=">= 80 ~ <= 100"
                                    onchange="unselectcheckbox('chkPrice_5');" onclick="unselectcheckbox('chkPrice_5');" />
                                <span>$80 to $100</span></li>
                            <li>
                                <input type="checkbox" class="checkbox" name="chkPrice_6" value=">= 100 ~ <= 200"
                                    onchange="unselectcheckbox('chkPrice_6');" onclick="unselectcheckbox('chkPrice_6');" />
                                <span>$100 to $200</span></li>
                            <li>
                                <input type="checkbox" class="checkbox" name="chkPrice_7" value="> 200" onchange="unselectcheckbox('chkPrice_7');"
                                    onclick="unselectcheckbox('chkPrice_7');" />
                                <span>Greater then $200</span></li>
                        </ul>
                        <div class="qty-box">
                            <p>
                                $<span>
                                    <asp:TextBox ID="txtFrom" runat="server" MaxLength="10" Width="28px" CssClass="qty-input"
                                        onkeypress="return keyRestrict(event,'0123456789.');"></asp:TextBox>
                                </span>to $<span>
                                    <asp:TextBox ID="txtTo" runat="server" MaxLength="10" Width="28px" CssClass="qty-input"
                                        onkeypress="return keyRestrict(event,'0123456789.');"></asp:TextBox>
                                </span>
                            </p>
                            <asp:ImageButton ID="btnIndexPriceGo" runat="server" ImageUrl="/images/go-button.jpg"
                                OnClientClick="return checkPricevalidation();" ToolTip="Go" OnClick="btnIndexPriceGo_Click"
                                Style="margin-top: 10px !important;" /><asp:ImageButton ID="btnIndexPriceGo1" runat="server"
                                    Style="display: none;" ImageUrl="/images/go-button.jpg" ToolTip="Go" OnClick="btnIndexPriceGo1_Click" />
                        </div>
                    </div>
                </div>
            </div>
            <a href="javascript:void(0);" onclick="checkallpage();">
                <img src="/images/option-arrow-up.png" height="31" width="28" alt="arrow" id="option-arrow"></a>
        </div>
                </div>
            <div class="search-content-right">
        
        <asp:UpdatePanel ID="UpdatePanelUpload" runat="server">
            <ContentTemplate>
                <div class="inner-banner" id="divCatBanner" runat="server" visible="false">
                    <asp:Repeater ID="RptCategoryDescription" runat="server" Visible="false">
                        <ItemTemplate>
                            <div class="inner-bg-left">
                                <asp:Image ImageUrl='<%# GetIconImageCategory(Convert.ToString(DataBinder.Eval(Container.DataItem,"ImageName"))) %>'
                                    ID="imgCat" runat="server" Width="94" Height="97" />
                            </div>
                            <div class="inner-bg-right">
                                <%#Eval("Description").ToString().Length<=0?Eval("Name"):Eval("Description")%>
                            </div>
                        </ItemTemplate>
                    </asp:Repeater>
                </div>
                <div class="fp-main" id="dvMessage" runat="server" style="min-height: 200px; border: 1px solid #CCCCCC;
                    width: 99.9% !important;" visible="false">
                    <div class="fp-row1 search-pro-row">
                        <div style="text-align: center; font-size: 18px; margin-top: 20px;">
                            <strong style="color: #393939;">Oops! It seems we don't have what you are looking for. However, we are sure you will like Drapes, Curtains, Hardware and Accesories. <br />

Happy Shopping! </strong>
                        </div>
                    </div>
                </div>
                <%--<div class="fp-main" id="dvSubcategory" runat="server">
                    <div class="fp-row1">
                        <asp:Repeater ID="RepSubCategory" runat="server" OnItemDataBound="RepSubCategory_ItemDataBound">
                            <HeaderTemplate>
                                <ul>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <li>
                                    <asp:Literal ID="ltTop" runat="server"></asp:Literal>
                                    <div class="fp-box" id="Catbox" runat="server">
                                        <div class="fp-display" id="catDisplay" runat="server">
                                            <div class="fp-box-div">
                                                <div class="img-center">
                                                    <a href="<%= fullPath.ToLower() %>/<%# Convert.ToString(DataBinder.Eval(Container.DataItem,"SEName")) %>"
                                                        title="<%# SetAttribute(Convert.ToString(DataBinder.Eval(Container.DataItem,"Tooltip")))%>">
                                                        <asp:Image ImageUrl='<%# GetIconImageCategory(Convert.ToString(DataBinder.Eval(Container.DataItem,"ImageName"))) %>'
                                                            ID="imgSubCat" runat="server" Width="230" Height="309" />
                                                    </a>
                                                </div>
                                            </div>
                                        </div>
                                        <h2 class="fp-box-h2" style="height: 45px;">
                                            <a href="<%= fullPath.ToLower() %>/<%# Convert.ToString(DataBinder.Eval(Container.DataItem,"SEName")) %>"
                                                title="<%# SetAttribute(Convert.ToString(DataBinder.Eval(Container.DataItem,"Tooltip")))%>">
                                                <%# SetName(Convert.ToString(DataBinder.Eval(Container.DataItem, "Name")))%></a></h2>
                                    </div>
                                </li>
                            </ItemTemplate>
                            <FooterTemplate>
                                </ul></FooterTemplate>
                        </asp:Repeater>
                    </div>
                </div>--%>
                <div id="topNumber" runat="server">
                    <div class="numbering" style="border-bottom: 1px solid #DDDDDD; ">
                        <p style="width: 5%; font-weight: bold; margin-top: 5px;" id="divtopitemcount" runat="server" class="search-mumbering-pt1">
                            <asp:Literal ID="ltrtopproductcount" runat="server"></asp:Literal>
                        </p>
                        <p id="Div2" runat="server" style="padding-left: 13px; width: 9%; font-weight: bold;
                            vertical-align: middle;" class="search-mumbering-pt2">
                            <span style="float: left; margin-top: 5px;">View as:</span>
                            <asp:Button ID="grid_view" class="grid-view" Style="margin-left: 6px; margin-top: 5px;"
                                title="Grid View" runat="server" OnClientClick="javascript:document.getElementById('ContentPlaceHolder1_hdnallpages').value='6';Loader();changeview();"
                                OnClick="grid_view_Click" />
                            <asp:Button ID="list_view" class="list-view" Style="margin-left: 6px; margin-top: 5px;"
                                title="List View" runat="server" OnClientClick="javascript:document.getElementById('ContentPlaceHolder1_hdnallpages').value='7';Loader();changeview();"
                                OnClick="list_view_Click" /></p>
                        <p style="width: 15%; font-weight: bold; margin-top: 2px;" class="search-mumbering-pt3">
                            Sort By :
                            <asp:DropDownList ID="ddlTopPrice" runat="server" CssClass="select-box" AutoPostBack="True"
                                onchange="javascript:document.getElementById('ContentPlaceHolder1_hdnallpages').value='3';Loader();"
                                OnSelectedIndexChanged="ddlTopPrice_SelectedIndexChanged1">
                                <asp:ListItem Value="Price Range">Price Range</asp:ListItem>
                                <asp:ListItem Value="Low to High">Low to High</asp:ListItem>
                                <asp:ListItem Value="High to Low">High to Low</asp:ListItem>
                            </asp:DropDownList>
                        </p>
                    </div>
                    <div class="numbering" id="divTopPaging" runat="server" style="border-bottom: 1px solid #DDDDDD;
                        display: none" visible="false">
                        <span class="num-prev">
                            <asp:LinkButton ID="lnkViewAll" OnClientClick="javascript:document.getElementById('ContentPlaceHolder1_hdnallpages').value='4';chkHeight();"
                                runat="server" OnClick="lnkViewAll_Click" Style="margin-right: 5px; font-weight: bold;">View All</asp:LinkButton>
                            <asp:LinkButton ID="lnkTopprevious" OnClientClick="javascript:document.getElementById('ContentPlaceHolder1_hdnallpages').value = '10';chkHeight();if (document.getElementById('ContentPlaceHolder1_hdnquickview').value == '1') {document.getElementById('form1').submit();}"
                                runat="server" OnClick="lnkPrevious_Click">Previous</asp:LinkButton>
                        </span><span class="num-next">
                            <asp:LinkButton ID="lnktopNext" runat="server" OnClientClick="javascript:document.getElementById('ContentPlaceHolder1_hdnallpages').value = '9';chkHeight();if (document.getElementById('ContentPlaceHolder1_hdnquickview').value == '1') {document.getElementById('form1').submit();}"
                                OnClick="lnkNext_Click">Next</asp:LinkButton></span>
                        <asp:DataList ID="dlToppaging" runat="server" HorizontalAlign="Center" RepeatDirection="Horizontal"
                            OnItemCommand="RepeaterPaging_ItemCommand" OnItemDataBound="RepeaterPaging_ItemDataBound">
                            <ItemTemplate>
                                <p>
                                    <asp:LinkButton ID="Pagingbtn" runat="server" OnClientClick='pagingSelection(<%# Eval("PageIndex") %>);'
                                        CommandArgument='<%# Eval("PageIndex") %>' CommandName="newpage" Text='<%# Eval("PageText") %> '></asp:LinkButton>
                                </p>
                            </ItemTemplate>
                        </asp:DataList>
                    </div>
                    <div class="numbering" id="divViewAllPages" runat="server" style="border-bottom: 1px solid #DDDDDD;
                        display: none" visible="false">
                        <span class="num-prev">
                            <asp:LinkButton ID="lnkViewAllPages" OnClientClick="javascript:document.getElementById('ContentPlaceHolder1_hdnallpages').value='5';chkHeight();"
                                runat="server" Visible="false" OnClick="lnkViewAllPages_Click" Style="font-weight: bold;">View Pages</asp:LinkButton></span></div>
                </div>
                <div class="fp-main" id="topMiddle" runat="server">
                    <div class="fp-row1 search-pro-row">
                        <asp:Repeater ID="RepProduct" runat="server" OnItemDataBound="RepProduct_ItemDataBound">
                            <HeaderTemplate>
                                <ul>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Literal ID="ltTop" runat="server"></asp:Literal>
                                <div class="fp-box" id="Probox" runat="server">
                                    <div class="fp-display">
                                        <div class="fp-box-div" id="proDisplay" runat="server">
                                            <div class="img-center">
                                                <%--   <a href="/<%# Convert.ToString(Eval("MainCategory")).ToLower()%>/<%# Convert.ToString(Eval("SEName")).ToLower()%>-<%#Eval("ProductID")%>.aspx"
                                                        title="<%# SetAttribute(Convert.ToString(DataBinder.Eval(Container.DataItem,"Tooltip")))%>">--%>
                                                <a id="innerAddtoCart" runat="server">
                                                    <asp:Image ImageUrl='<%# GetIconImageProduct(Convert.ToString(Eval("ImageName"))) %>'
                                                        ID="imgName" runat="server" AlternateText='<%# Server.HtmlEncode(Convert.ToString(Eval("Name")))%>'
                                                        ToolTip='<%# Server.HtmlEncode(Convert.ToString(Eval("Name")))%>' Width="230"
                                                        Height="309" /></a>
                                                <img id="imgAddToCart" runat="server" src="/images/quick-view.png" alt="QUICK ORDER"
                                                    class="preview" width="150" height="30" title="QUICK ORDER" style="cursor: pointer;" />
                                                <input type="hidden" id="hdnImgName" runat="server" value='<%#Eval("ImageName") %>' />
                                                <asp:Literal ID="lblTag" runat="server"></asp:Literal>
                                                <asp:Label ID="lblTName" Visible="false" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"TagName")%>'></asp:Label>
                                            </div>
                                        </div>
                                        <h2 class="fp-box-h2" style="height: 30px; overflow: hidden;">
                                            <%-- <a href="/<%# Convert.ToString(Eval("MainCategory")).ToLower()%>/<%# Convert.ToString(Eval("SEName")).ToLower()%>-<%#Eval("ProductID")%>.aspx"
                                                title="<%# SetAttribute(Convert.ToString(DataBinder.Eval(Container.DataItem,"Tooltip")))%>">--%>
                                            <a href="/<%# Convert.ToString(Eval("ProductURL")).ToLower()%>" title="<%# SetAttribute(Convert.ToString(DataBinder.Eval(Container.DataItem,"Tooltip")))%>">
                                                <%# SetName(Convert.ToString(Eval("Name")))%>
                                            </a>
                                        </h2>
                                        <p class="fp-box-p">
                                            <span>
                                                <asp:Literal ID="ltrRegularPrice" runat="server" Visible="false"></asp:Literal>
                                                <asp:Literal ID="ltrYourPrice" runat="server"></asp:Literal>
                                            </span>
                                            <asp:Literal ID="ltrInventory" runat="server" Visible="false" Text='<%#Eval("Inventory")%>'></asp:Literal>
                                            <asp:Literal ID="ltrproductid" runat="server" Visible="false" Text='<%#Eval("ProductID")%>'></asp:Literal>
                                            <input type="hidden" id="hdnItemIndex" runat="server" value='<%#Container.ItemIndex%>' />
                                            <input type="hidden" id="hdnProductId" runat="server" value='<%#Eval("ProductID")%>' />
                                            <input type="hidden" id="ltrProductURL" runat="server" value='<%#Convert.ToString(Eval("ProductURL")).ToLower()%>' />
                                            <input type="hidden" id="ltrLink" runat="server" value='<%#Convert.ToString(Eval("MainCategory")).ToLower()%>' />
                                            <input type="hidden" id="ltrlink1" runat="server" value='<%#Convert.ToString(Eval("SEName")).ToLower()%>' />
                                            <asp:Label ID="lblPrice" runat="server" Visible="false" Text='<%# Math.Round(Convert.ToDecimal(Eval("price")), 2)%>'></asp:Label>
                                            <asp:Label ID="lblSalePrice" runat="server" Visible="false" Text='<%# Math.Round(Convert.ToDecimal(Eval("SalePrice")), 2)%>'></asp:Label>
                                            <input type="hidden" id="hdnRegularPrice" runat="server" value='<%#Convert.ToString(Eval("price")).ToLower()%>' />
                                            <input type="hidden" id="hdnYourPrice" runat="server" value='<%#Convert.ToString(Eval("SalePrice")).ToLower()%>' />
                                            <input type="hidden" id="hdnswatchprice" value="0" runat="server" />
                                            <span><a href="javascript:void(0);" id="aProductlink" runat="server" title="View Detail" style="display:none;">
                                                <img style="margin-top: 5px;" src="/images/view-detail.png" alt="View Detail" title="View Detail"></a>
                                                <img id="outofStockDiv" visible="false" runat="server" style="margin-top: 5px;" src="/images/out-of-stock.png"
                                                    alt="OUT OF STOCK" title="OUT OF STOCK" />
                                                <a href="javascript:void(0);" id="aorderswatch" runat="server" title="ORDER SWATCH">
                                                    <img id="orderswatch" visible="false" runat="server" style="margin-top: 5px;" src="/images/order-swatch.png"
                                                        alt="ORDER SWATCH" title="ORDER SWATCH" /></a> </span>
                                        </p>
                                        <div class="rating" id="divSpace" runat="server" style="height: 20px; padding: 0 0 0;">
                                        </div>
                                        <div class="rating" id="Divratinglist" runat="server">
                                            <asp:Literal ID="ltrating" runat="server"></asp:Literal><a href='/<%#Convert.ToString(Eval("ProductURL")).ToLower().Replace(".html","")%>.html'>&nbsp;Rating
                                                <asp:Label ID="ltrRatingCount" runat="server"></asp:Label></a>
                                        </div>
                                        <div class="add-to-compare">
                                            <span>&nbsp; <span>&nbsp;&nbsp;<input type="checkbox" id="add_to_compare<%#Eval("ProductID")%>"
                                                    onchange="comaprebutonclick('add_to_compare<%#Eval("ProductID")%>');" onclick="comaprebutonclick('add_to_compare<%#Eval("ProductID")%>');" />&nbsp;&nbsp;Add
                                                to compare </span>&nbsp;&nbsp;&nbsp; </span>
                                        </div>
                                    </div>
                                </div>
                                <asp:Literal ID="ltbottom" runat="server"></asp:Literal>
                            </ItemTemplate>
                            <FooterTemplate>
                                </ul>
                            </FooterTemplate>
                        </asp:Repeater>
                        <asp:Repeater ID="Repeaterlistview" runat="server" OnItemDataBound="RepProduct_List_ItemDataBound"
                            Visible="false">
                            <HeaderTemplate>
                                <div class="fp-list-bg">
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Literal ID="ltTop" runat="server"></asp:Literal>
                                <div class="fp-box-list" id="Probox" runat="server">
                                    <table style="width: 100%;">
                                        <tr>
                                            <td valign="top" style="width: 18%;">
                                                <div class="fp-pro-img" id="proDisplay" runat="server">
                                                    <div class="pro-center-img">
                                                        <%--<a href="/<%# Convert.ToString(Eval("MainCategory")).ToLower()%>/<%# Convert.ToString(Eval("SEName")).ToLower()%>-<%#Eval("ProductID")%>.aspx"
                                                            title="<%# SetAttribute(Convert.ToString(DataBinder.Eval(Container.DataItem,"Tooltip")))%>">
                                                            <asp:Image ImageUrl='<%# GetIconImageProduct(Convert.ToString(Eval("ImageName"))) %>'
                                                                ID="imgName" runat="server" Width="230" Height="309" Style="padding: 10px 0 3px 14px;" /></a>--%>
                                                        <div class="img-center">
                                                            <a id="innerAddtoCart" runat="server">
                                                                <asp:Image ImageUrl='<%# GetIconImageProduct(Convert.ToString(Eval("ImageName"))) %>'
                                                                    ID="imgName" runat="server" AlternateText='<%# Server.HtmlEncode(Convert.ToString(Eval("Name")))%>'
                                                                    ToolTip='<%# Server.HtmlEncode(Convert.ToString(Eval("Name")))%>' Width="230"
                                                                    Height="309" /></a>
                                                            <img id="imgAddToCart" runat="server" src="/images/quick-view.png" alt="QUICK ORDER"
                                                                class="preview" style="cursor: pointer;" width="135" height="30" title="QUICK ORDER" />
                                                        </div>
                                                        <input type="hidden" id="hdnImgName" runat="server" value='<%#Eval("ImageName") %>' />
                                                        <asp:Literal ID="lblTag" runat="server"></asp:Literal>
                                                        <asp:Label ID="lblTName" Visible="false" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"TagName")%>'></asp:Label>
                                                    </div>
                                                </div>
                                            </td>
                                            <td valign="top" style="width: 33%;">
                                                <div class="fp-details-list">
                                                    <h2 style="height: 30px; padding: 0px; padding-top: 10px;">
                                                        <%-- <a href="/<%# Convert.ToString(Eval("MainCategory")).ToLower()%>/<%# Convert.ToString(Eval("SEName")).ToLower()%>-<%#Eval("ProductID")%>.aspx"
                                                            title="<%# SetAttribute(Convert.ToString(DataBinder.Eval(Container.DataItem,"Tooltip")))%>">--%>
                                                        <a href="/<%# Convert.ToString(Eval("ProductURL")).ToLower()%>" title="<%# SetAttribute(Convert.ToString(DataBinder.Eval(Container.DataItem,"Tooltip")))%>">
                                                            <%#SetName(Convert.ToString(Eval("Name")))%>
                                                        </a>
                                                    </h2>
                                                    <div class="fp-pro-img" style="color: #848383; margin: 0px;">
                                                        <%#SetDescription(Convert.ToString(Eval("description")))%>
                                                    </div>
                                                </div>
                                            </td>
                                            <td valign="top" style="width: 33%;">
                                                <div class="fp-details-list search-fp-details-list">
                                                    <div class="add-to-cart" id="Divratinglist" runat="server" style="width: 160px;">
                                                        <asp:Literal ID="ltrating" runat="server"></asp:Literal><a href='/<%#Convert.ToString(Eval("ProductURL")).ToLower().Replace(".html","")%>.html'
                                                            class="Rating-class" style="color: #393939; font-size: 14px; margin: 0 5px;">&nbsp;Rating
                                                            <asp:Label ID="ltrRatingCount" runat="server"></asp:Label></a>
                                                    </div>
                                                    <div class="add-to-cart" style="padding-top: 10px; float: right; width: 100%; color: #393939;">
                                                        <asp:Literal ID="ltrRegularPrice" Visible="false" runat="server"></asp:Literal>
                                                        <asp:Literal ID="ltrYourPrice" runat="server"></asp:Literal>
                                                    </div>
                                                    <div class="add-to-cart" style="width: 100%; margin-top: 10px;">
                                                        <input type="hidden" id="hdnswatchprice" value="0" runat="server" />
                                                        <a href="javascript:void(0);" id="aProductlink" runat="server" title="VIEW DETAIL" style="display:none;">
                                                            <img src="/images/view-detail.png" alt="View Detail" title="View Detail"></a>
                                                        <img src="/images/out-of-stock.png" id="aoutofstock" alt="OUT OF STOCK" runat="server"
                                                            visible="false" title="OUT OF STOCK" />
                                                        <a href="javascript:void(0);" id="aorderswatch" runat="server" title="ORDER SWATCH">
                                                            <img id="orderswatch" visible="false" runat="server" style="margin-top: 5px;" src="/images/order-swatch.png"
                                                                alt="ORDER SWATCH" title="ORDER SWATCH" /></a>
                                                    </div>
                                                    <div style="float: right; width: 100%;">
                                                        <span style="float: right; font-size: 12px; color: #393939; padding: 20px 0 0 0;">
                                                            <input type="checkbox" id="add_to_compare_list<%#Eval("ProductID")%>" onchange="comaprebutonclicklist('add_to_compare_list<%#Eval("ProductID")%>');" />
                                                            &nbsp;&nbsp;Add to compare</span>&nbsp;&nbsp;&nbsp;
                                                    </div>
                                                </div>
                                                <asp:Literal ID="ltrInventory" runat="server" Visible="false" Text='<%#Eval("Inventory")%>'></asp:Literal>
                                                <asp:Literal ID="ltrproductid" runat="server" Visible="false" Text='<%#Eval("ProductID")%>'></asp:Literal>
                                                <input type="hidden" id="ltrLink" runat="server" value='<%#Convert.ToString(Eval("MainCategory")).ToLower()%>' />
                                                <input type="hidden" id="ltrProductURL" runat="server" value='<%#Convert.ToString(Eval("ProductURL")).ToLower()%>' />
                                                <input type="hidden" id="ltrlink1" runat="server" value='<%#Convert.ToString(Eval("SEName")).ToLower()%>' />
                                                <asp:Label ID="lblPrice" runat="server" Visible="false" Text='<%# Math.Round(Convert.ToDecimal(Eval("price")), 2)%>'></asp:Label>
                                                <asp:Label ID="lblSalePrice" runat="server" Visible="false" Text='<%# Math.Round(Convert.ToDecimal(Eval("SalePrice")), 2)%>'></asp:Label>
                                                <input type="hidden" id="hdnRegularPrice" runat="server" value='<%#Convert.ToString(Eval("Price")).ToLower()%>' />
                                                <input type="hidden" id="hdnYourPrice" runat="server" value='<%#Convert.ToString(Eval("SalePrice")).ToLower()%>' />
                                                <input type="hidden" id="hdnItemIndex" runat="server" value='<%#Container.ItemIndex%>' />
                                                <input type="hidden" id="hdnProductId" runat="server" value='<%#Eval("ProductID")%>' />
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </ItemTemplate>
                            <FooterTemplate>
                                </div>
                            </FooterTemplate>
                        </asp:Repeater>
                    </div>
                </div>
                <asp:ImageButton ID="btnCompareID" runat="server" Text='<%#Eval("ProductID")%>' Style="display: none"
                    OnClick="btnCompareID_Click" />
                <div style="width: 100%;" class="fp-main-bg-scroll" id="Div1" runat="server">
                </div>
                <div style="margin-left: auto; width: 100%; margin-right: auto; width: 120px;" id="divPostsLoader">
                </div>
                <div id="topBottom" runat="server">
                    <div class="numbering" id="divBottomPaging" runat="server" visible="false" style="display: none;">
                        <span class="num-prev">
                            <asp:LinkButton Style="margin-right: 5px; font-weight: bold;" ID="lnkBottomViewAll"
                                OnClientClick="javascript:document.getElementById('ContentPlaceHolder1_hdnallpages').value='4';chkHeight();"
                                runat="server" OnClick="lnkViewAll_Click">View All</asp:LinkButton>
                            <asp:LinkButton ID="lnkPrevious" OnClientClick="javascript:document.getElementById('ContentPlaceHolder1_hdnpagenumber').value = '<%=CurrentPage%>';document.getElementById('ContentPlaceHolder1_hdnallpages').value = '10';chkHeight();if (document.getElementById('ContentPlaceHolder1_hdnquickview').value == '1') {document.getElementById('form1').submit();}"
                                runat="server" OnClick="lnkPrevious_Click">Previous</asp:LinkButton>
                        </span><span class="num-next">
                            <asp:LinkButton ID="lnkNext" runat="server" OnClientClick="javascript:document.getElementById('ContentPlaceHolder1_hdnpagenumber').value = '<%=CurrentPage%>';document.getElementById('ContentPlaceHolder1_hdnallpages').value = '9';chkHeight();if (document.getElementById('ContentPlaceHolder1_hdnquickview').value == '1') {document.getElementById('form1').submit();}"
                                OnClick="lnkNext_Click">Next</asp:LinkButton></span>
                        <asp:DataList ID="RepeaterPaging" runat="server" HorizontalAlign="Center" RepeatDirection="Horizontal"
                            OnItemCommand="RepeaterPaging_ItemCommand" OnItemDataBound="RepeaterPaging_ItemDataBound">
                            <ItemTemplate>
                                <p>
                                    <asp:LinkButton ID="Pagingbtn" runat="server" OnClientClick='pagingSelection(<%# Eval("PageIndex") %>);'
                                        CommandArgument='<%# Eval("PageIndex") %>' CommandName="newpage" Text='<%# Eval("PageText") %> '></asp:LinkButton>
                                </p>
                            </ItemTemplate>
                        </asp:DataList>
                    </div>
                    <div class="numbering" id="divViewAllPagesBottom" runat="server" visible="false"
                        style="display: none;">
                        <span class="num-prev">
                            <asp:LinkButton ID="lnkBottomViewAllPages" OnClientClick="javascript:document.getElementById('ContentPlaceHolder1_hdnallpages').value='5';chkHeight();"
                                runat="server" Visible="false" OnClick="lnkViewAllPages_Click" Style="font-weight: bold;">View Pages</asp:LinkButton></span></div>
                    <div class="numbering" style="border-bottom: 1px solid #DDDDDD;">
                        <p style="width: 5%; font-weight: bold; margin-top: 5px;" id="divbottomitemcount"
                            runat="server" class="search-mumbering-pt1">
                            <asp:Literal ID="ltrbottomcount" runat="server"></asp:Literal>
                        </p>
                        <p id="listbottom" runat="server" style="padding-left: 13px; width: 9%; font-weight: bold;
                            vertical-align: middle;" class="search-mumbering-pt2">
                            <span style="float: left; margin-top: 5px;">View as:</span>
                            <asp:Button ID="grid_bottom" class="grid-view" runat="server" Style="margin-left: 6px;
                                margin-top: 5px;" title="Grid View" OnClientClick="javascript:document.getElementById('ContentPlaceHolder1_hdnallpages').value='6';Loader();changeview();"
                                OnClick="grid_bottom_Click" />
                            <asp:Button ID="list_bottom" class="list-view" runat="server" Style="margin-top: 5px;
                                margin-left: 6px;" OnClientClick="javascript:document.getElementById('ContentPlaceHolder1_hdnallpages').value='7';Loader();changeview();"
                                title="List View" OnClick="list_bottom_Click" />
                        </p>
                        <p style="width: 15%; font-weight: bold; margin-top: 2px;" class="search-mumbering-pt3">
                            Sort By :
                            <asp:DropDownList ID="ddlbottomprice" runat="server" CssClass="select-box" AutoPostBack="True"
                                onchange="javascript:document.getElementById('ContentPlaceHolder1_hdnallpages').value='3';Loader();"
                                OnSelectedIndexChanged="ddlbottomprice_SelectedIndexChanged">
                                <asp:ListItem Value="Price Range">Price Range</asp:ListItem>
                                <asp:ListItem Value="Low to High">Low to High</asp:ListItem>
                                <asp:ListItem Value="High to Low">High to Low</asp:ListItem>
                            </asp:DropDownList>
                        </p>
                    </div>
                    <div id="prepage" style="position: absolute; font-family: arial; font-size: 16; left: 0px;
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
            </ContentTemplate>
        </asp:UpdatePanel>
                </div>
            </div>
    </div>
    <%--   <div id="popupContact" style="z-index: 1000001; top: 30px; padding: 0px; width: 750px;">--%>
    <%-- <div id="popupContact" style="z-index: 1000001; top: 30px; padding: 0px;">
        <table border="0" cellspacing="0" cellpadding="0" class="table_border">
            <tr>
                <td align="left">
                    <iframe id="frmquickview" frameborder="0" height="425" width="750" scrolling="auto">
                    </iframe>
                </td>
            </tr>
        </table>
    </div>--%>
    <div id="popupContact1" style="z-index: 1000001; top: 30px; padding: 0px; width: 750px;">
        <table border="0" cellspacing="0" cellpadding="0" class="table_border">
            <tr>
                <td align="left">
                    <iframe id="frmdisplay1" frameborder="0" height="650" width="580" scrolling="auto">
                    </iframe>
                </td>
            </tr>
        </table>
    </div>
    <div id="backgroundPopup" style="z-index: 1000000;">
    </div>
    <div style="display: none;">
        <input type="button" id="btnreadmore" />
        <input type="button" id="btnhelpdescri" />
        <asp:ImageButton ID="popupContactClose" ImageUrl="/images/close.png" runat="server"
            ToolTip="Close" OnClientClick="return false;"></asp:ImageButton>
    </div>
    <div style="display: none;">
        <input type="hidden" id="hdnPrice" runat="server" />
        <input type="hidden" id="hdnproductId" runat="server" />
        <input type="hidden" id="hdnCompare" runat="server" />
        <input type="hidden" name="view" value="grid" id="view1" />
        <input type="hidden" id="hdnColorSelection" runat="server" value="" />
        <asp:HiddenField ID="hdncnt" runat="server" Value="12" />
        <asp:HiddenField ID="productcount" runat="server" Value="0" />
        <asp:HiddenField ID="divcount" runat="server" Value="1" />
        <input type="hidden" id="hdncheckproductid" value="0" runat="server" />
        <input type="hidden" id="hdncheckedproduct" value="0" runat="server" />
        <input type="hidden" id="hdnsearchvalue" runat="server" value="" />
        <input type="hidden" id="hdnpagenumber" value="0" runat="server" />
        <input type="hidden" id="hdnquickview" value="0" runat="server" />
        <input type="hidden" id="hdnallpages" value="0" runat="server" />
        <input type="hidden" id="hdnswatchmaxlength" value="0" runat="server" />
        <input type="hidden" id="hiddenCustID" value="0" runat="server" />
        <asp:Button ID="btntempclick" runat="server" Text="" OnClick="btntempclick_Click" />
    </div>
    <script src="/js/JQueryStoreIndex.js" type="text/javascript"></script>
    <%--<script type="text/javascript" src="/js/subcategoryscript.js"></script>--%>
    <div id="prepage1" style="position: absolute; font-family: arial; font-size: 16;
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
    <div id="popupContact" style="z-index: 1000001; top: 30px; padding: 0px; width: 750px;">
        <div style='float: right; background-color: transparent; right: -15px; top: -18px;
            position: absolute;'>
            <a href="javascript:void(0);" onclick="javascript:document.getElementById('ContentPlaceHolder1_popupContactClose').click();"
                title="">
                <img src="/images/popupclose.png" alt="" /></a>
        </div>
        <div style='float: right; background-color: transparent; right: -11px; top: 49%;
            position: absolute;'>
            <input type="button" class="next-productdisabled" id="subnext" runat="server">
        </div>
        <div style='float: left; background-color: transparent; left: -11px; top: 49%; position: absolute;'>
            <input type="button" class="pre-productdisabled" id="subpre" runat="server">
        </div>
        <div style="background: none repeat scroll 0 0 white;">
            <table border="0" cellspacing="0" cellpadding="0" class="table_border">
                <tr>
                    <td align="left">
                        <iframe id="frmdisplayquick" frameborder="0" height="650" width="580" scrolling="auto">
                        </iframe>
                    </td>
                </tr>
            </table>
        </div>
        <div id="prepagequick" style="position: absolute; font-family: arial; font-size: 16;
            left: 0px; top: 0px; background-color: #000; opacity: 0.7; filter: alpha(opacity=70);
            layer-background-color: white; height: 515px; width: 1015px; z-index: 1000; display: none;">
            <div style="border: 1px solid #ccc;">
                <table width="100%" style="position: fixed; top: 50%; left: 50%; margin: -50px 0 0 -100px;">
                    <tr>
                        <td>
                            <div style="background: none repeat scroll 0 0 rgba(0, 0,0, 0.9) !important; border: 1px solid #ccc;
                                width: 10%; height: 3%; padding: 20px; -webkit-border-radius: 10px; -moz-border-radius: 10px;
                                border-radius: 10px;">
                                <center>
                                    <img alt="" src="/images/loding.png" style="text-align: center;" /><br />
                                    <b style="color: #fff;">Loading&nbsp;...&nbsp;...&nbsp;Please&nbsp;wait!</b></center>
                            </div>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
        <%--<input type="button" class="prev-nav-btn" disabled="disabled">--%>
    </div>
    <%=strcretio%>
</asp:Content>
