<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="crt.aspx.cs" Inherits="Solution.UI.Web.crt" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link href="https://fonts.googleapis.com/css?family=Open+Sans:400,700" rel="stylesheet" type="text/css" />
    <meta http-equiv="Content-Type" content="text/html;charset=utf-8" />
    <meta name="msvalidate.01" content="282128F51A9978AD8936B2243A5D5E1B" />
    <meta name="p:domain_verify" content="682603a315f35972f601852e86ff7606" />
    <meta name="google-site-verification" content="R3F4fr_mDQAyDz3fjpdtjwPi6L8ba_SkbRV0zMMLL2E" />
    <link href="/css/style.css" rel="stylesheet" type="text/css" />
    <link href="/css/menucss-new.css" rel="stylesheet" type="text/css" />
    <link id="canonical" rel="canonical" href="http://www.halfpricedrapes.com/login.aspx" />


    <script type="text/javascript" src="/js/contentslider.js"></script>
    <script language="javascript" src="/js/BubbleToolTips.js" type="text/javascript"></script>

    <style type="text/css">
        #divInventoryCart {
            color: #393939;
            font-family: "museo_sans_300",Helvetica, "Helvetica Neue",Arial,sans-serif;
            font-size: 12px;
        }
    </style>
    <script type="text/javascript">
        function hideshowdiv(id, id2) {
            for (var i = 1; i < 70; i++) {
                if (document.getElementById('divsubcat' + i) != null && i != id) {
                    document.getElementById('divsubcat' + i).style.display = 'none';
                }
                else if (document.getElementById('divsubcat' + i) != null && i == id) {
                    document.getElementById('divsubcat' + i).style.display = '';
                }
            }

            var alllianchor = document.getElementById('menu_detail' + id2).getElementsByTagName('a');

            for (var k = 0; k < alllianchor.length; k++) {
                var alllianchor1 = alllianchor[k];
                alllianchor1.removeAttribute("style");
                alllianchor1.setAttribute('style', 'color:#000000');
            }

            var allli = document.getElementById('menu_detail' + id2).getElementsByTagName('li');
            for (var j = 0; j < allli.length; j++) {
                if (allli[j].id.toString() == 'lisub' + id.toString()) {
                    var litag = allli[j];
                    litag.className = 'liselectedhover';
                    var alll = litag.getElementsByTagName('a');
                    var tttt = alll[0];

                    tttt.style.color = '#ffffff';

                }
                else {
                    var litag = allli[j];
                    litag.removeAttribute("class");
                    litag.style.backgroundColor = '#fff';
                    litag.className = '';
                    var alll = litag.getElementsByTagName('a');
                    var tttt = alll[0];
                    tttt.style.color = '#000000 !important;';
                    tttt.style.backgroundColor = '#fff';

                }
            }
        }
    </script>
    <script type="text/javascript">
        /*  function jqueryfadeinout(id) {
              $("#menu_detail" + id).fadeIn();
              $("#menu_detail" + id).mouseleave(function () {
                  $("#menu_detail" + id).fadeOut();
              });
              for (var i = 1; i < 12; i++) {
                  if (document.getElementById('menu_detail' + i) != null && i != id) {
                      $("#menu_detail" + i).fadeOut();
                  }
              }
          }
          function jqueryfadeout() {
              for (var i = 1; i < 12; i++) {
                  if (document.getElementById('menu_detail' + i) != null) {
                      $("#menu_detail" + i).fadeOut();
                  }
              }
          }*/
    </script>
    <script type="text/javascript">



        function ShowModelSearch() {

            // document.getElementById('header').style.zIndex = -1;


            document.getElementById("diviframecontactus").style.display = 'none';
            document.getElementById("diviframesearch").style.display = 'block';

            if (document.getElementById("diviframesearch") != null) {  document.getElementById("diviframesearch").innerHTML=''; }
            var link = "/SearchControl.aspx"
            var iframe = document.createElement('iframe');
            iframe.frameBorder = 0;
            iframe.width = "1440px";
            iframe.height = "296px";
            iframe.id = "frmsearch";
            iframe.scrolling = "no";
            iframe.frameborder = "0"
            //iframe.setAttribute("src", link);
            document.getElementById("diviframesearch").appendChild(iframe);
            document.getElementById("diviframesearch").style.display = 'block';
            document.getElementById("diviframecontactus").style.display = 'none';
            iframe.src = "/SearchControl.aspx";
            

            document.getElementById('popupContactpricequote1').setAttribute("style", "z-index: 1000001; top: 30px; padding: 0px;width:1440px;height:296px;border:solid 1px #7d7d7d;");
            document.getElementById('popupContactpricequote1').style.width = '1440px';
            document.getElementById('popupContactpricequote1').style.height = '296px';
            //document.getElementById("frmsearch").style.width = '1440px';
            //document.getElementById("frmsearch").style.height = '296px';

            //document.getElementById("frmsearch").src = "/SearchControl.aspx";
            //var ifr = document.getElementById("frmsearch");

            //ifr.contentWindow.document.getElementById('mycarousel2').style.width = '92px';
            //ifr.contentWindow.document.getElementById('mycarousel3').style.width = '180px';
            //ifr.contentWindow.document.getElementById('mycarousel4').style.width = '180px';
            //ifr.contentWindow.document.getElementById('mycarousel5').style.width = '180px';
            //ifr.contentWindow.document.getElementById('mycarousel6').style.width = '180px';
            //ifr.contentWindow.document.getElementById('mycarousel7').style.width = '185px';


            //if(document.getElementById('ContentPlaceHolder1_hideIndexOptionDiv').innerHTML != "")
            //{
            //    document.getElementById('divcolorhtml').innerHTML = document.getElementById('ContentPlaceHolder1_hideIndexOptionDiv').innerHTML;
            //}
            //document.getElementById('ContentPlaceHolder1_hideIndexOptionDiv').innerHTML='';


            window.scrollTo(0, 0);
            centerPopupmaster(); loadPopupmaster();
            // document.getElementById('btnreadmore').click();


        }
        function ShowModelSearchContactus() {


            if (document.getElementById("diviframecontactus") != null) { $('#diviframecontactus').html(''); }
            var link = "/Contactuspopup.aspx"
            var iframe = document.createElement('iframe');
            iframe.frameBorder = 0;
            iframe.width = "1440px";
            iframe.height = "296px";
            iframe.id = "frmContactus";
            iframe.scrolling = "no";
            iframe.frameborder = "0"
            //iframe.setAttribute("src", link);
            document.getElementById("diviframecontactus").appendChild(iframe);
            document.getElementById("diviframecontactus").style.display = 'block';
            document.getElementById("diviframesearch").style.display = 'none';


            // document.getElementById('header').style.zIndex = -1;
            document.getElementById('popupContactpricequote1').removeAttribute("style");
            document.getElementById('popupContactpricequote1').setAttribute("style", "z-index: 2000001; top: 0px; padding: 0px;width:600px;height:680px;background: none repeat scroll 0 0 rgba(3, 3, 3, 0.3) !important; padding: 10px !important;border:none !important;");
            //document.getElementById('popupContactpricequote1').setAttribute("style", "z-index: 1000001; top: 30px; padding: 0px;width:1440px;height:800px;border:solid 1px #7d7d7d;");
            document.getElementById('popupContactpricequote1').style.width = '600px';
            document.getElementById('popupContactpricequote1').style.height = '680px';
            document.getElementById('frmContactus').height = '680px';
            document.getElementById('frmContactus').width = '625px';
            document.getElementById("frmContactus").contentWindow.document.body.innerHTML = '';
            document.getElementById("frmContactus").src = "/Contactuspopup.aspx";




            //if(document.getElementById('ContentPlaceHolder1_hideIndexOptionDiv').innerHTML != "")
            //{
            //    document.getElementById('divcolorhtml').innerHTML = document.getElementById('ContentPlaceHolder1_hideIndexOptionDiv').innerHTML;
            //}
            //document.getElementById('ContentPlaceHolder1_hideIndexOptionDiv').innerHTML='';


            window.scrollTo(0, 0);
            centerPopupmaster(); loadPopupmaster();
            // document.getElementById('btnreadmore').click();


        }
        function showfreeshipping() {
            if (document.getElementById('divfreeshipping') != null) {

                document.getElementById('backgroundfree').style.display = '';
                document.getElementById('divfreeshipping').style.display = '';

            }
            tabheaderhide('lifreeshipp', 'litradepartner');

        }
        function showtradeshipping() {
            if (document.getElementById('divtradepartner') != null) {

                document.getElementById('backgroundfree').style.display = '';
                document.getElementById('divtradepartner').style.display = '';

            }
            tabheaderhide('litradepartner', 'lifreeshipp');

        }

        function tabheaderhide(id, id1) {

            if (document.getElementById(id) != null) {

                document.getElementById(id).className = 'active';
            }
            if (document.getElementById(id1) != null) {

                document.getElementById(id1).className = '';
            }

        }
        function OpenCenterWindow(pid, wi, he) {
            var w = wi;
            var h = he;
            var left = (screen.width / 2) - (w / 2);
            var top = (screen.height / 2) - (h / 2);
            window.open(pid, '', 'toolbar=no, location=no, directories=no, status=no, menubar=no, scrollbars=yes, resizable=no, copyhistory=no, width=' + w + ', height=' + h + ', top=' + top + ', left=' + left);
        }
    </script>
    <script type="text/javascript">
        function ChkInventoryUpdate(result) {
            if (document.getElementById('divInventoryCart') != null) {
                document.getElementById('divInventoryCart').innerHTML = result;
                document.getElementById('divInventoryCart').style.display = '';
            }
        }

        //$(document).ready(function () {
        // // setupRotator();
        //});
        function setupRotator() {
            setInterval('textRotate()', 5000);
        }
        function textRotate() {

            var irota = document.getElementById('hdnrotator').value;
            var alltext = document.getElementById('rotatebannertextall').getElementsByTagName('div');

            if ((parseInt(irota) + 1) > alltext.length) {
                $('#rotatebannertext').fadeOut();
                irota = 0;
                if (document.getElementById('divheder' + irota.toString()) != null) {
                    document.getElementById('rotatebannertext').innerHTML = document.getElementById('divheder' + irota.toString()).innerHTML;
                    $('#rotatebannertext').fadeIn();
                }
                irota = parseInt(irota) + 1;
                document.getElementById('hdnrotator').value = irota;

            }
            else {
                $('#rotatebannertext').fadeOut();

                if (document.getElementById('divheder' + irota.toString()) != null) {


                    document.getElementById('rotatebannertext').innerHTML = document.getElementById('divheder' + irota.toString()).innerHTML;
                    $('#rotatebannertext').fadeIn();
                }

                irota = parseInt(irota) + 1;
                document.getElementById('hdnrotator').value = irota;


            }

        }
    </script>
    <script type="text/javascript">
        (function () {
            var bs = document.createElement('script');
            bs.type = 'text/javascript';
            bs.async = true;
            bs.src = ('https:' == document.location.protocol ? 'https' : 'http') + '://d2so4705rl485y.cloudfront.net/widgets/tracker/tracker.js';
            var s = document.getElementsByTagName('script')[0];
            s.parentNode.insertBefore(bs, s);
        })();
    </script>

    <meta name="Title" content="Shop Designer Window Curtains and Drapes" />
    <meta name="Keywords" content="curtains, drapes, window curtains, window treatments, window covering, custom curtains, designer curtains, designer window curtains, roman shades, silk curtains, linen curtains, blackout curtains, velvet curtains, modern curtains" />
    <meta name="Description" content="Buy Custom Curtains &amp; Drapery, Roman Shades made of quality and designer Fabric at our online store. Choose the best from our collection and SAVE huge." />
    <meta name="googlebot" content="index, follow" />
    <meta name="revisit-after" content="3 Days" />
    <meta name="robots" content="all" />
    <meta name="robots" content="index, follow" />
    <meta name="author" content="halfpricedrapes.com" />
    <title>Shop Designer Window Curtains and Drapes
    </title>
</head>
<body>
    <!-- Google Tag Manager -->
    <noscript><iframe src="https://www.googletagmanager.com/ns.html?id=GTM-NWMXP9"height="0" width="0" style="display:none;visibility:hidden"></iframe></noscript>
    <script>(function (w, d, s, l, i) { w[l] = w[l] || []; w[l].push({ 'gtm.start': new Date().getTime(), event: 'gtm.js' }); var f = d.getElementsByTagName(s)[0], j = d.createElement(s), dl = l != 'dataLayer' ? '&l=' + l : ''; j.async = true; j.src = 'https://www.googletagmanager.com/gtm.js?id=' + i + dl; f.parentNode.insertBefore(j, f); })(window, document, 'script', 'dataLayer', 'GTM-NWMXP9');</script>
    <!-- End Google Tag Manager -->
    <form method="post" action="login.aspx" onkeypress="javascript:return WebForm_FireDefaultButton(event, 'btnTemp')" id="form1">
        <div class="aspNetHidden">
            <input type="hidden" name="__LASTFOCUS" id="__LASTFOCUS" value="" />
            <input type="hidden" name="__EVENTTARGET" id="__EVENTTARGET" value="" />
            <input type="hidden" name="__EVENTARGUMENT" id="__EVENTARGUMENT" value="" />
            <input type="hidden" name="__VIEWSTATE" id="__VIEWSTATE" value="/wEPDwULLTEyODcyODQ2NzIPZBYCZg8PFgIeBlJvd1VybAULL2xvZ2luLmFzcHhkFgQCAg9kFgICBw8WAh4EaHJlZgUpaHR0cDovL3d3dy5oYWxmcHJpY2VkcmFwZXMuY29tL2xvZ2luLmFzcHhkAgQPZBYgZg8WAh4HVmlzaWJsZWgWBAIBDxYCHgRUZXh0Bd0LPGRpdiBpZD0nc2xpZGVzJz48ZGl2IGNsYXNzPSdzbGlkZXNfY29udGFpbmVyJz48YSBocmVmPSJodHRwOi8vd3d3LmhhbGZwcmljZWRyYXBlcy5jb20vc29saWQtZmF1eC1zaWxrLWN1cnRhaW5zLmh0bWwiIHRpdGxlPSJIYWxmIFByaWNlIERyYXBlcyI+PGltZyBzdHlsZT0nY3Vyc29yOnBvaW50ZXI7JyBzcmM9J2h0dHA6Ly93d3cuaGFsZnByaWNlZHJhcGVzLnVzL1Jlc291cmNlcy9oYWxmcHJpY2VkcmFwcy9CYW5uZXIvMl9TdG9yZV8xXzMuanBnJyAgYWx0PSdIYWxmIFByaWNlIERyYXBlcycgVGl0bGU9J0hhbGYgUHJpY2UgRHJhcGVzJyAvPjwvYT48YSBocmVmPSJodHRwOi8vd3d3LmhhbGZwcmljZWRyYXBlcy5jb20vY290dG9uLWxpbmVuLWN1cnRhaW5zLmh0bWwiIHRpdGxlPSJIYWxmIFByaWNlIERyYXBlcyI+PGltZyBzdHlsZT0nY3Vyc29yOnBvaW50ZXI7JyBzcmM9J2h0dHA6Ly93d3cuaGFsZnByaWNlZHJhcGVzLnVzL1Jlc291cmNlcy9oYWxmcHJpY2VkcmFwcy9CYW5uZXIvMl9TdG9yZV8xXzQuanBnJyAgYWx0PSdIYWxmIFByaWNlIERyYXBlcycgVGl0bGU9J0hhbGYgUHJpY2UgRHJhcGVzJyAvPjwvYT48YSBocmVmPSJodHRwOi8vd3d3LmhhbGZwcmljZWRyYXBlcy5jb20vcGF0dGVybi1mYXV4LXNpbGstY3VydGFpbnMuaHRtbCIgdGl0bGU9IkhhbGYgUHJpY2UgRHJhcGVzIj48aW1nIHN0eWxlPSdjdXJzb3I6cG9pbnRlcjsnIHNyYz0naHR0cDovL3d3dy5oYWxmcHJpY2VkcmFwZXMudXMvUmVzb3VyY2VzL2hhbGZwcmljZWRyYXBzL0Jhbm5lci8yX1N0b3JlXzFfNS5qcGcnICBhbHQ9J0hhbGYgUHJpY2UgRHJhcGVzJyBUaXRsZT0nSGFsZiBQcmljZSBEcmFwZXMnIC8+PC9hPjxhIGhyZWY9Imh0dHA6Ly93d3cuaGFsZnByaWNlZHJhcGVzLmNvbS9wcmludGVkLWNvdHRvbi1jdXJ0YWlucy5odG1sIiB0aXRsZT0iSGFsZiBQcmljZSBEcmFwZXMiPjxpbWcgc3R5bGU9J2N1cnNvcjpwb2ludGVyOycgc3JjPSdodHRwOi8vd3d3LmhhbGZwcmljZWRyYXBlcy51cy9SZXNvdXJjZXMvaGFsZnByaWNlZHJhcHMvQmFubmVyLzJfU3RvcmVfMV82LmpwZycgIGFsdD0nSGFsZiBQcmljZSBEcmFwZXMnIFRpdGxlPSdIYWxmIFByaWNlIERyYXBlcycgLz48L2E+PGEgaHJlZj0iaHR0cDovL3d3dy5oYWxmcHJpY2VkcmFwZXMuY29tL3NpZ25hdHVyZS1zaWxrLWN1cnRhaW5zLmh0bWwiIHRpdGxlPSJIYWxmIFByaWNlIERyYXBlcyI+PGltZyBzdHlsZT0nY3Vyc29yOnBvaW50ZXI7JyBzcmM9J2h0dHA6Ly93d3cuaGFsZnByaWNlZHJhcGVzLnVzL1Jlc291cmNlcy9oYWxmcHJpY2VkcmFwcy9CYW5uZXIvMl9TdG9yZV8xXzcuanBnJyAgYWx0PSdIYWxmIFByaWNlIERyYXBlcycgVGl0bGU9J0hhbGYgUHJpY2UgRHJhcGVzJyAvPjwvYT48L2Rpdj4gPGEgaHJlZj0iamF2YXNjcmlwdDp2b2lkKDApOyIgY2xhc3M9InByZXYiPiZuYnNwOyA8L2E+IDxhIGhyZWY9ImphdmFzY3JpcHQ6dm9pZCgwKTsiIGNsYXNzPSJuZXh0Ij4mbmJzcDsgPC9hPjwvZGl2PmQCAw8WAh4JaW5uZXJodG1sBYAIPGRpdiBjbGFzcz0ic21hbGwtYmFubmVyLTEiPjxhIGhyZWY9ImphdmFzY3JpcHQ6U2hvd01vZGVsU2VhcmNoKCk7IiB0aXRsZT0iUHJvZHVjdCBGaW5kZXIiPjxpbWcgc3JjPSJodHRwOi8vd3d3LmhhbGZwcmljZWRyYXBlcy51cy9SZXNvdXJjZXMvaGFsZnByaWNlZHJhcHMvQmFubmVyLzJfU3RvcmVfMV8zX3NtYWxsXy5qcGciIGFsdD0iUHJvZHVjdCBGaW5kZXIiIHRpdGxlPSJQcm9kdWN0IEZpbmRlciI+PC9hPjwvZGl2PjxkaXYgY2xhc3M9InNtYWxsLWJhbm5lci0yIj48YSBocmVmPSJodHRwOi8vd3d3LmhhbGZwcmljZWRyYXBlcy5jb20vY3VydGFpbi1oYXJkd2FyZS5odG1sIiB0aXRsZT0iQ3VydGFpbiBIYXJkd2FyZSI+PGltZyBzcmM9Imh0dHA6Ly93d3cuaGFsZnByaWNlZHJhcGVzLnVzL1Jlc291cmNlcy9oYWxmcHJpY2VkcmFwcy9CYW5uZXIvMl9TdG9yZV8xXzFfc21hbGxfLmpwZyIgYWx0PSJDdXJ0YWluIEhhcmR3YXJlIiB0aXRsZT0iQ3VydGFpbiBIYXJkd2FyZSI+PC9hPjwvZGl2PjxkaXYgY2xhc3M9InNtYWxsLWJhbm5lci0zIj48YSBocmVmPSJodHRwOi8vd3d3LmhhbGZwcmljZWRyYXBlcy5jb20vcm9tYW4tc2hhZGVzLmh0bWwiIHRpdGxlPSJSb21hbiBTaGFkZXMiPjxpbWcgc3JjPSJodHRwOi8vd3d3LmhhbGZwcmljZWRyYXBlcy51cy9SZXNvdXJjZXMvaGFsZnByaWNlZHJhcHMvQmFubmVyLzJfU3RvcmVfMV8yX3NtYWxsXy5qcGciIGFsdD0iUm9tYW4gU2hhZGVzIiB0aXRsZT0iUm9tYW4gU2hhZGVzIj48L2E+PC9kaXY+PGRpdiBjbGFzcz0ic21hbGwtYmFubmVyLTQiPjxhIGhyZWY9Imh0dHA6Ly93d3cuaGFsZnByaWNlZHJhcGVzLmNvbS9zaGVlci1jdXJ0YWlucy5odG1sIiB0aXRsZT0iU2hlZXIgQ3VydGFpbiI+PGltZyBzcmM9Imh0dHA6Ly93d3cuaGFsZnByaWNlZHJhcGVzLnVzL1Jlc291cmNlcy9oYWxmcHJpY2VkcmFwcy9CYW5uZXIvMl9TdG9yZV8xXzRfc21hbGxfLmpwZyIgYWx0PSJTaGVlciBDdXJ0YWluIiB0aXRsZT0iU2hlZXIgQ3VydGFpbiI+PC9hPjwvZGl2PmQCAQ9kFgZmDxYCHwMFBUxvZ2luZAIBDxYCHwMFBUxvZ2luZAICD2QWBAIEDxYCHwJoFgJmDxYCHwEFDy9BZGRUb2NhcnQuYXNweGQCBQ8WAh4Fd2lkdGgFAzY2JWQCAw9kFgQCAQ8WBB8BBQ8vQWRkVG9DYXJ0LmFzcHgfBAWpATxwPjxzcGFuIGNsYXNzPSduYXZRdHknPigwIGl0ZW0pPC9zcGFuPjxzcGFuIGNsYXNzPSduYXZUb3RhbCc+ICQwLjAwPC9zcGFuPjwvcD48aW1nIHNyYz0nL2ltYWdlcy9jYXJ0LWljb24uanBnJyB3aWR0aD0nNDgnIGhlaWdodD0nMTUnIGFsdD0nJyB0aXRsZT0nJyBjbGFzcz0nY2FydC1pY29uJz5kAgMPZBYCZg8WAh8DBQg8L3RhYmxlPmQCBA9kFgJmDxYGHgV0aXRsZQUIUmVnaXN0ZXIfBAUIUmVnaXN0ZXIfAQUTL0NyZWF0ZUFjY291bnQuYXNweGQCBw8WAh8DBQYyNDkuMDBkAggPZBYCAgEPD2QWAh4Jb25rZXlkb3duBaUBaWYoZXZlbnQud2hpY2ggfHwgZXZlbnQua2V5Q29kZSl7aWYgKChldmVudC53aGljaCA9PSAxMykgfHwgKGV2ZW50LmtleUNvZGUgPT0gMTMpKSB7ZG9jdW1lbnQuZ2V0RWxlbWVudEJ5SWQoJ2J0blNlYXJjaCcpLmNsaWNrKCk7cmV0dXJuIGZhbHNlO319IGVsc2Uge3JldHVybiB0cnVlfTsgZAIJDxYCHwMFZjxwPg0KCTxzdHJvbmc+PHNwYW4gc3R5bGU9ImZvbnQtZmFtaWx5OiI+RnJlZSBTaGlwcGluZzwvc3Bhbj48L3N0cm9uZz4gdG8gVS5TLiBvbiBvcmRlcnMgb3ZlciAkMjQ5PC9wPmQCCg8WAh8DBZYBPGRpdiBpZD0iZGl2aGVkZXIwIiBzdHlsZT0iZGlzcGxheTpub25lOyI+PHA+DQoJPHN0cm9uZz48c3BhbiBzdHlsZT0iZm9udC1mYW1pbHk6Ij5GcmVlIFNoaXBwaW5nPC9zcGFuPjwvc3Ryb25nPiB0byBVLlMuIG9uIG9yZGVycyBvdmVyICQyNDk8L3A+PC9kaXY+ZAILDxYCHwMFHkhhbGYgUHJpY2UgPHNwYW4+RHJhcGVzPC9zcGFuPmQCDA8WAh8DBbcLPHA+DQoJRHJhcGVyaWVzIGFuZCBjdXJ0YWlucyBhcmUgdGhlIHNvdWwgb2Ygb3VyIGJ1c2luZXNzIGFuZCB3aXRoIHRoYXQga2luZCBvZiBwYXNzaW9uIGZvciBmYWJyaWMsIEhhbGYgUHJpY2UgRHJhcGVzIGhhcyBiZWVuIHNwZWNpYWxpemluZyBpbiBkZXNpZ25lciBxdWFsaXR5IHdpbmRvdyB0cmVhdG1lbnRzIGZvciB5ZWFycy4gT3VyIHRhbGVudGVkIHN0YWZmIGlzIGFkZXB0IGF0IHdvcmtpbmcgY2xvc2VseSB3aXRoIG91ciBjbGllbnRzIHRvIGRlbGl2ZXIgdGhlIGJlc3QgaW4gd2luZG93IHRyZWF0bWVudCBwcm9kdWN0cyBzdWNoIGFzIHZlbHZldCBkcmFwZXMgYW5kIGxpbmVuIGN1cnRhaW5zIHRvIG5hbWUgYnV0IGEgZmV3IG9mIG91ciBwb3B1bGFyIHBpZWNlcy4gPGJyIC8+DQoJPGJyIC8+DQoJV2UgYXQgSGFsZiBQcmljZSBEcmFwZXMgaGF2ZSBtYWRlIGEgY29tbWl0bWVudCB0byBwcm92aWRpbmcgeW91IHdpdGggdGhlIGhpZ2hlc3QgcXVhbGl0eSBkcmFwZXJpZXMgYXQgdGhlIGxvd2VzdCBwcmljZSBhdmFpbGFibGUuIE1hbnkgcGVvcGxlIGludGVyZXN0ZWQgaW4gcmVkZWNvcmF0aW5nIHRoZWlyIGhvbWVzIGFzc3VtZSB0aGF0IGhpZ2gtZW5kIGx1eHVyeSBtdXN0IGNvbWUgYXQgYSBoaWdoIHByaWNlLiBBdCBIYWxmIFByaWNlIERyYXBlcywgd2UgcGFzcyB0aGUgc2F2aW5ncyBvbiB0byBvdXIgY3VzdG9tZXJzIGFuZCBvZmZlciB5b3UgYSBjb25zaXN0ZW50IGJhcmdhaW4gbm8gbWF0dGVyIHdoYXQgdHlwZSBvZiBjdXJ0YWlucyB5b3UncmUgbG9va2luZyBmb3IuIFdlIGludml0ZSB5b3UgdG8gdmlzaXQgb3VyIGRlc2lnbiBzdHVkaW8gbG9jYXRlZCBpbiB0aGUgQ2FsaWZvcm5pYSdzIEJheSBBcmVhIHRvIGV4cGxvcmUgb3VyIHNlbGVjdGlvbiBvZiBkcmFwZXJpZXMgb2ZmZXJlZCBpbiBhIHZhcmlldHkgb2YgZmFicmljcy0gdGFmZmV0YSBzaWxrIGN1cnRhaW5zLCBsaW5lbiwgdmVsdmV0IGFuZCBzaGVlciBkcmFwZXMgdG8gbmFtZSBhIGZldy4gRm9yIHRoZSByZXN0IG9mIG91ciBsb3lhbCBjdXN0b21lcnMgYWNyb3NzIHRoZSBnbG9iZSwgb3VyIGZ1bGwgd2ViIHNpdGUgd2lsbCBwcm92aWRlIGEgd2luZG93IGludG8gb3VyIGRlc2lnbiBwb3NzaWJpbGl0aWVzLiA8YnIgLz4NCgk8YnIgLz4NCglXZSBtYWludGFpbiBhIGhlbHBmdWwgYW5kIGtub3dsZWRnZWFibGUgc3RhZmYgYXZhaWxhYmxlIHRvIGFzc2lzdCB5b3UgaW4gYWxsIHlvdXIgZHJhcGVyeSBuZWVkcy4gSGFsZiBQcmljZSBEcmFwZXMgaXMgcHJvdWQgb2YgaXRzIGxvbmcgaGlzdG9yeSBpbiB0aGUgaW5kdXN0cnkgYW5kIGxvb2tzIGZvcndhcmQgdG8gbWFraW5nIHlvdXIgaG9tZSBkZXNpZ24gZHJlYW1zIGEgcmVhbGl0eS4gQ2FsbCB1cyB0b2RheSB0byB3b3JrIHdpdGggb3VyIGRlc2lnbiBwcm9mZXNzaW9uYWxzIG9yIGNoZWNrIG91dCBvdXIgUmVmZXJlbmNlcyBzZWN0aW9uLjwvcD5kAg4PFgIfAwWlATxwPg0KCTEuODY2LjQxMy43MjczPGJyIC8+DQoJTW9uIC0gRnJpIGJldHdlZW4gOCBhbSAtIDUgcG0gUFNUPGJyIC8+DQoJU2F0dXJkYXkgJiBTdW5kYXkgQ2xvc2VkPGJyIC8+DQoJU2hvd3Jvb20gOUFNLSA0UE0sIDxzdHJvbmc+QXBwb2ludG1lbnQgUHJlZmVycmVkPC9zdHJvbmc+PC9wPmQCEA9kFgICAQ8WAh8DZWQCEQ8WAh8DBQ1GcmVlIFNoaXBwaW5nZAISDxYCHwMFpCA8cD4NCgk8c3R5bGU+DQouc2F0aXNmaWNhdGlvbi1jb250ZW50IHsgd2lkdGg6IDczMHB4OyBmbG9hdDogbGVmdDsgcGFkZGluZzogMDsgbWFyZ2luOiAwOyB9DQouc2F0aXNmaWNhdGlvbi1jb250ZW50IGgxIHsgd2lkdGg6IDczMHB4OyBmb250LXNpemU6IDE4cHg7IGNvbG9yOiAjQzMyNTI2OyBsaW5lLWhlaWdodDogMThweDsgcGFkZGluZzogNXB4IDA7IHRleHQtYWxpZ246IGNlbnRlcjsgfQ0KLnNhdGlzZmljYXRpb24tY29udGVudCBwIHsgd2lkdGg6IDczMHB4OyBmb250LXNpemU6IDEycHg7IGNvbG9yOiAjM0IzQjNCOyBsaW5lLWhlaWdodDogMThweDsgcGFkZGluZzogNXB4IDA7IC8qdGV4dC1hbGlnbjpjZW50ZXI7IGZvbnQtd2VpZ2h0OmJvbGQ7Ki8gfQ0KLnNhdGlzZmljYXRpb24tY29udGVudCBwIGEgeyBjb2xvcjogI0MzMjUyNjsgdGV4dC1kZWNvcmF0aW9uOiB1bmRlcmxpbmU7IH0NCi5zYXRpc2ZpY2F0aW9uLWNvbnRlbnQgcCBhOmhvdmVyIHsgY29sb3I6ICMzQjNCM0I7IHRleHQtZGVjb3JhdGlvbjogbm9uZTsgfQ0KLnNoaXBwaW5nLXRhYmxlIHsgYm9yZGVyLXRvcDogMXB4IHNvbGlkICM5OTk5OTk7IGJvcmRlci1sZWZ0OiAxcHggc29saWQgIzk5OTk5OTsgYm9yZGVyLXJpZ2h0OiAxcHggc29saWQgIzk5OTk5OTsgfQ0KLnNoaXBwaW5nbGVmdCB7IGJvcmRlci1yaWdodDogMXB4IHNvbGlkICM5OTk5OTk7IGJvcmRlci1ib3R0b206IDFweCBzb2xpZCAjOTk5OTk5OyBwYWRkaW5nOiA1cHg7IH0NCi5zaGlwcGluZ3JpZ2h0IHsgYm9yZGVyLWJvdHRvbTogMXB4IHNvbGlkICM5OTk5OTk7IHBhZGRpbmc6IDVweDsgfQ0KLnN0YXRpY19jb250ZW50IGg2IHsNCiAgICBjb2xvcjogI0MzMjUyNjsNCiAgICBmb250LXNpemU6IDEycHg7DQogICAgZm9udC13ZWlnaHQ6IGJvbGQ7DQogICAgcGFkZGluZzogNXB4IDAgMDsNCn0NCg0KPC9zdHlsZT48L3A+DQo8ZGl2IGNsYXNzPSJzdGF0aWNfY29udGVudCI+DQoJPGRpdiBjbGFzcz0ic2F0aXNmaWNhdGlvbi1jb250ZW50Ij4NCgkJPHAgc3R5bGU9InRleHQtYWxpZ246Y2VudGVyOyI+DQoJCQk8Yj5FdmVyeWRheSBGUkVFIEdyb3VuZCBTaGlwcGluZyBPbiBBbGwgRG9tZXN0aWMgT3JkZXJzIE9mICQyNDkgJiBVcC4gRm9yIG9yZGVycyBsZXNzIHRoYW4gdGhpcywgdGhlIGJlbG93IHJhdGVzIGFwcGx5LiA8L2I+PC9wPg0KCQk8cCBzdHlsZT0idGV4dC1hbGlnbjpjZW50ZXI7Ij4NCgkJCVN0YW5kYXJkIFNoaXBwaW5nIFJhdGVzIEZvciBGRURFWCBHcm91bmQgJiBVU1BTIFNlcnZpY2U8L3A+DQoJCTx0YWJsZSBhbGlnbj0iY2VudGVyIiBjZWxscGFkZGluZz0iMCIgY2VsbHNwYWNpbmc9IjAiIGNsYXNzPSJzaGlwcGluZy10YWJsZSIgd2lkdGg9IjcwJSI+DQoJCQk8dGJvZHk+DQoJCQkJPHRyPg0KCQkJCQk8dGggYWxpZ249ImxlZnQiIGNsYXNzPSJzaGlwcGluZ2xlZnQiIHdpZHRoPSIzNSUiPg0KCQkJCQkJQW1vdW50PC90aD4NCgkJCQkJPHRoIGFsaWduPSJsZWZ0IiBjbGFzcz0ic2hpcHBpbmdyaWdodCIgd2lkdGg9IjM1JSI+DQoJCQkJCQlDb3N0PC90aD4NCgkJCQk8L3RyPg0KCQkJCTx0cj4NCgkJCQkJPHRkIGNsYXNzPSJzaGlwcGluZ2xlZnQiPg0KCQkJCQkJMC0kMjkuOTk8L3RkPg0KCQkJCQk8dGQgY2xhc3M9InNoaXBwaW5ncmlnaHQiPg0KCQkJCQkJJDUuOTk8L3RkPg0KCQkJCTwvdHI+DQoJCQkJPHRyPg0KCQkJCQk8dGQgY2xhc3M9InNoaXBwaW5nbGVmdCI+DQoJCQkJCQkkMzAuMDAtJDY5Ljk5PC90ZD4NCgkJCQkJPHRkIGNsYXNzPSJzaGlwcGluZ3JpZ2h0Ij4NCgkJCQkJCSQ3Ljk5PC90ZD4NCgkJCQk8L3RyPg0KCQkJCTx0cj4NCgkJCQkJPHRkIGNsYXNzPSJzaGlwcGluZ2xlZnQiPg0KCQkJCQkJJDcwLjAwLSQxMjkuOTk8L3RkPg0KCQkJCQk8dGQgY2xhc3M9InNoaXBwaW5ncmlnaHQiPg0KCQkJCQkJJDEyLjk5PC90ZD4NCgkJCQk8L3RyPg0KCQkJCTx0cj4NCgkJCQkJPHRkIGNsYXNzPSJzaGlwcGluZ2xlZnQiPg0KCQkJCQkJJDEzMC4wMC0kMTk5Ljk5PC90ZD4NCgkJCQkJPHRkIGNsYXNzPSJzaGlwcGluZ3JpZ2h0Ij4NCgkJCQkJCSQxNi45OTwvdGQ+DQoJCQkJPC90cj4NCgkJCQk8dHI+DQoJCQkJCTx0ZCBjbGFzcz0ic2hpcHBpbmdsZWZ0Ij4NCgkJCQkJCSQyMDAuMDAtJDI0OC45OTwvdGQ+DQoJCQkJCTx0ZCBjbGFzcz0ic2hpcHBpbmdyaWdodCI+DQoJCQkJCQkkMjEuOTk8L3RkPg0KCQkJCTwvdHI+DQoJCQkJPHRyPg0KCQkJCQk8dGQgY2xhc3M9InNoaXBwaW5nbGVmdCI+DQoJCQkJCQkkMjQ5ICYgVXA8L3RkPg0KCQkJCQk8dGQgY2xhc3M9InNoaXBwaW5ncmlnaHQiPg0KCQkJCQkJRnJlZSBTaGlwcGluZzwvdGQ+DQoJCQkJPC90cj4NCgkJCTwvdGJvZHk+DQoJCTwvdGFibGU+DQoJCTxiciAvPg0KCQnCoDwvZGl2Pg0KCTxiciAvPg0KCTxiciAvPg0KCTxzdHJvbmc+T3JkZXJzIG9mIFJlYWR5IE1hZGUgaXRlbXMgYXJlIHByb2Nlc3NlZCBhbmQgc2hpcHBlZCBmcm9tIG91ciBDQSB3YXJlaG91c2Ugd2l0aGluIDcyIGhycyBvZiB0aGUgb3JkZXIgYmVpbmcgcGxhY2VkLCBNLUYgKEhvbGlkYXlzIEV4Y2x1ZGVkKS4gWW91IGNhbiBleHBlY3QgeW91ciBvcmRlciB0byBiZSBkZWxpdmVyZWQgaW4gMTAtMTIgYnVzaW5lc3MgZGF5cy48YnIgLz4NCgk8YnIgLz4NCglCYWNrb3JkZXJlZCBhbmQgQ3VzdG9tIGl0ZW1zIGdlbmVyYWxseSBzaGlwIGluIDMtNSB3ZWVrcyB1bmxlc3Mgb3RoZXJ3aXNlIG5vdGVkLjxiciAvPg0KCTxiciAvPg0KCUludGVybmF0aW9uYWwgZGVsaXZlcnkgdGltZXMgd2lsbCB2YXJ5IGRlcGVuZGluZyB1cG9uIHRoZSBkZXN0aW5hdGlvbiBjb3VudHJ5LiBEdXRpZXMgYW5kIHRheGVzIHdpbGwgYmUgdGhlIHJlc3BvbnNpYmlsaXR5IG9mIHRoZSByZWNpcGllbnQuPGJyIC8+DQoJPGJyIC8+DQoJPGJyIC8+DQoJPC9zdHJvbmc+DQoJPGg2Pg0KCQlTaGlwcGluZyBtZXRob2RzIGF2YWlsYWJsZSBmb3Igb25saW5lIHB1cmNoYXNlcyBpbiB0aGUgVVNBOjwvaDY+DQoJPGg2Pg0KCQk8YnIgLz4NCgkJT3JkZXJzIHNoaXBwaW5nIHRvIFBPIEJveCAmIEFQTy9GUE8vRFBPIGFkZHJlc3NlcyB3aWxsIGJlIHNlbnQgdXNpbmcgdGhlIFVuaXRlZCBTdGF0ZXMgUG9zdGFsIFNlcnZpY2UsIFVTUFMuPGJyIC8+DQoJCcKgPC9oNj4NCgk8cD4NCgkJPHN0cm9uZz5GZWRFeCBHcm91bmQ8YnIgLz4NCgkJRmVkRXggM3JkIERheSBTZWxlY3QgU008YnIgLz4NCgkJRmVkRXggTkVYVCBEQVk8YnIgLz4NCgkJPGJyIC8+DQoJCU9yZGVycyBhcmUgc2hpcHBlZCBvdXQgTW9uLUZyaSAoSG9saWRheXMgRXhjbHVkZWQpLiBSdXNoIG9yZGVycyBtdXN0IGJlIHJlY2VpdmVkL3Byb2Nlc3NlZCBieSAxcG0sIFBTVCBNb24tRnJpIChIb2xpZGF5cyBFeGNsdWRlZCkuIEFueSBvcmRlcnMgcmVjZWl2ZWQgYWZ0ZXIgMXBtIFBTVCBNb24tRnJpIGFuZCBvcmRlcnMgcGxhY2VkIG92ZXIgYSB3ZWVrZW5kIHdpbGwgYmUgcHJvY2Vzc2VkIHRoZSBuZXh0IGJ1c2luZXNzIGRheS48YnIgLz4NCgkJPGJyIC8+DQoJCVVTUFMgUHJpb3JpdHkgTWFpbCBGb3IgU3dhdGNoZXMgT05MWTxiciAvPg0KCQk8YnIgLz4NCgkJPC9zdHJvbmc+PC9wPg0KCTxoNj4NCgkJPHN0cm9uZz5JbnRlcm5hdGlvbmFsIHNoaXBwaW5nIG1ldGhvZHMgYXZhaWxhYmxlIGZvciBvbmxpbmUgcHVyY2hhc2VzIHRvIENhbmFkYSwgdGhlIFVuaXRlZCBLaW5nZG9tIGFuZCBBdXN0cmFsaWE6PC9zdHJvbmc+PC9oNj4NCgk8c3Ryb25nPjxiciAvPg0KCTwvc3Ryb25nPg0KCTxwPg0KCQk8c3Ryb25nPkZlZEV4IEludGVybmF0aW9uYWwgKE5vdGU6IER1dGllcyBhbmQgdGF4ZXMgd2lsbCBiZSB0aGUgcmVzcG9uc2liaWxpdHkgb2YgdGhlIHJlY2lwaWVudCk8YnIgLz4NCgkJPGJyIC8+DQoJCTwvc3Ryb25nPjwvcD4NCgk8aDY+DQoJCTxzdHJvbmc+VVNQUyBEZWxpdmVyeSBhdmFpbGFibGUgdG8gb3RoZXIgY291bnRyaWVzLiBUbyBwbGFjZSB5b3VyIG9yZGVyIG9yIHJlcXVlc3QgYSBxdW90ZSBwbGVhc2UgY2FsbCAxLjg2Ni40MTMuNzI3MyBvciBlbWFpbCB1cyBhdDo8YSBocmVmPSJtYWlsdG86Y3NAaGFsZnByaWNlZHJhcGVzLmNvbSI+IGNzQGhhbGZwcmljZWRyYXBlcy5jb20gPC9hPjxiciAvPg0KCQk8L3N0cm9uZz48L2g2Pg0KPC9kaXY+ZAITDxYCHwMFFVRyYWRlIFBhcnRuZXIgUHJvZ3JhbWQCFA8WAh8DBe8dPGRpdiBjbGFzcz0ic3RhdGljX2NvbnRlbnQiPg0KCTxoMj4NCgkJVFJBREUgU0FMRVM8L2gyPg0KCTxwPg0KCQk8c3Ryb25nPllPVVIgSU5EVVNUUlkgUEFSVE5FUjwvc3Ryb25nPjwvcD4NCgk8cD4NCgkJSWYgeW91IGFyZSBhIGRlc2lnbiBwcm9mZXNzaW9uYWwgd2UgZW5jb3VyYWdlIHlvdSB0byBqb2luIG91ciBIYWxmIFByaWNlIERyYXBlcy1UcmFkZSBNZW1iZXJzaGlwIFByb2dyYW0uIFdlJ3JlIGhlcmUgdG8gc3VwcG9ydCB5b3VyIGJ1c2luZXNzIGFuZCBmb3N0ZXIgeW91ciBzdWNjZXNzIGFzIGEgZGVzaWduZXIuIEFzIGEgSGFsZiBQcmljZSBEcmFwZXMtVHJhZGUgTWVtYmVyLCB5b3UnbGwgZW5qb3kgYSBnZW5lcm91cyBkaXNjb3VudCBvbiBhbGwgb2Ygb3VyIG5vbi1zYWxlIG1lcmNoYW5kaXNlLiBPbmNlIHlvdSBoYXZlIGZpbGxlZCBvdXQgYWxsIGFwcHJvcHJpYXRlIGZvcm1zIGFuZCB5b3VyIGFwcGxpY2F0aW9uIGhhcyBiZWVuIHByb2Nlc3NlZCwgeW91IHdpbGwgcmVjZWl2ZSBhIHdlbGNvbWUgcGFja2V0IGxpc3RpbmcgYWxsIG9mIHlvdXIgbmV3IGJlbmVmaXRzIGFuZCBkaXNjb3VudHMuPC9wPg0KCTxwPg0KCQlIYWxmIFByaWNlIERyYXBlcyBtYW51ZmFjdHVyZXJzIHJlYWR5LW1hZGUgY3VydGFpbnMgJiBjdXN0b20gZHJhcGVyaWVzIGFuZCBoYXMgYmVlbiBvcGVyYXRpbmcgaW4gdGhlIEJheSBBcmVhIHNpbmNlIDIwMDQuIFdoYXQgbWFrZXMgdXMgdW5pcXVlIGlzIHRoYXQgd2Ugd2VhdmUgb3VyIG93biBmYWJyaWMsIGNyZWF0ZSBvdXIgb3duIGVtYnJvaWRlcnkgcGF0dGVybnMsIGFuZCBtYW51ZmFjdHVyZSBvdXIgb3duIGN1cnRhaW5zICYgZHJhcGVzLiBXZSBoYXZlIGEgMTAsMDAwIHNxdWFyZSBmb290IHdvcmtzaG9wIHdoZXJlIGEgZGVkaWNhdGVkIHN0YWZmIG1hbnVmYWN0dXJlcyBoaWdoLXF1YWxpdHksIHN0YW5kYXJkaXplZCwgYW5kIGN1c3RvbSBjdXJ0YWlucyAmIGRyYXBlcy4gSGVuY2UsIHdlIGNhbiBlbGltaW5hdGUgYWxsIG1hcmstdXBzIGFuZCBwcm92aWRlIGhpZ2gtIGVuZCBsdXh1cmlvdXMgY3VzdG9taXplZCBjdXJ0YWlucyAmIGRyYXBlcyBhdCBhIGNvc3QgdGhhdCBpcyB1bm1hdGNoZWQuIFRoYXQgaXMgd2h5IHdlIGFyZSBwcm91ZCB0byBjYWxsIG91cnNlbHZlcyBIYWxmIFByaWNlIERyYXBlcy4gT3VyIGJlYXV0aWZ1bCBmYWJyaWMgY2FuIGJlIHRhaWxvcmVkIGluIG1hbnkgZGlmZmVyZW50IGhlYWRlciBzdHlsZXMuIFRoYXQncyBub3QgYWxsLCBpZiB5b3UgaGF2ZSBhIHNwZWNpZmljIGRlc2lnbiBpbiBtaW5kLCBsZXQgdXMga25vdyBhbmQgd2Ugd2lsbCB3b3JrIHdpdGggeW91IGFuZCB5b3VyIGNsaWVudHMgdG8gY3VzdG9taXplIHRoZSBmYWJyaWMgdG8geW91ciBzcGVjaWZpY2F0aW9ucy48L3A+DQoJPHA+DQoJCTxzdHJvbmc+Tk9UIFlFVCBBIE1FTUJFUj88L3N0cm9uZz48L3A+DQoJPHA+DQoJCUFwcGx5aW5nIGZvciBtZW1iZXJzaGlwIGlzIGVhc3ksIGp1c3QgZmlsbCBvdXQgb3VyIHNpbXBsZSA8YSBocmVmPSIvT25saW5lQXBwbGljYXRpb25Gb3JtLmFzcHgiIHRhcmdldD0iX2JsYW5rIj5PbmxpbmUgQXBwbGljYXRpb24uPC9hPiBPbmNlIHlvdXIgZWxlY3Ryb25pYyBhcHBsaWNhdGlvbiBoYXMgYmVlbiBzdWJtaXR0ZWQgd2l0aCB2YWxpZCBkb2N1bWVudGF0aW9uIChzZWUgbGlzdCBiZWxvdykgeW91IHdpbGwgcmVjZWl2ZSBhbiBlbWFpbCBpbmRpY2F0aW5nIHRoYXQgeW91ciBhcHBsaWNhdGlvbiBpcyBiZWluZyByZXZpZXdlZC4gSWYgdGhlIHN1cHBvcnRpbmcgZG9jdW1lbnRhdGlvbiBkb2VzIG5vdCBtZWV0IHRoZSByZXF1aXJlbWVudHMsIHlvdXIgYXBwbGljYXRpb24gd2lsbCBiZSBjYW5jZWxsZWQuPC9wPg0KCTxwPg0KCQlBdCBsZWFzdCBvbmUgb2YgdGhlIGZvbGxvd2luZyBpcyByZXF1aXJlZDo8L3A+DQoJPHVsPg0KCQk8bGk+DQoJCQlDdXJyZW50IEJ1c2luZXNzIG9yIFN0YXRlIFByb2Zlc3Npb25hbCBMaWNlbnNlIGluIGEgUmVzaWRlbnRpYWwgb3IgQ29tbWVyY2lhbCBEZXNpZ24tYmFzZWQgYnVzaW5lc3MsIG9yIHRoZSBIb3NwaXRhbGl0eSBpbmR1c3RyeTwvbGk+DQoJCTxsaT4NCgkJCVByb29mIG9mIGN1cnJlbnQgQUkgb3IgSURJIHByb3ZpbmNpYWwgcmVnaXN0cmF0aW9uPC9saT4NCgkJPGxpPg0KCQkJQnVzaW5lc3MgSUQgbnVtYmVyPC9saT4NCgkJPGxpPg0KCQkJUHJvb2Ygb2YgY3VycmVudCBBU0lEIG1lbWJlcnNoaXA8L2xpPg0KCQk8bGk+DQoJCQlJbnRlcmlvciBkZXNpZ24gY2VydGlmaWNhdGlvbiAoZS5nLiBOQ0lEUSwgQ0NJREMpPC9saT4NCgkJPGxpPg0KCQkJVzksIEZlZGVyYWwgSUQgZm9ybSwgb3IgRUlOIG51bWJlcjwvbGk+DQoJCTxsaT4NCgkJCVJlc2FsZSBvciBTYWxlcyBUYXggQ2VydGlmaWNhdGU8L2xpPg0KCTwvdWw+DQoJPHA+DQoJCUlmIHlvdSBpbnRlbmQgdG8gcHVyY2hhc2UgbWVyY2hhbmRpc2UgZm9yIHJlc2FsZSwgeW91IHdpbGwgYmUgcmVxdWlyZWQgdG8gc3VwcGx5IGEgUmVzYWxlIG9yIFNhbGVzIFRheCBDZXJ0aWZpY2F0ZS4gV2l0aG91dCB0aGlzIGRvY3VtZW50YXRpb24sIHNhbGVzIHRheCB3aWxsIGJlIGFwcGxpZWQgdG8gYWxsIG9yZGVycy4gVHJhZGUgRGlzY291bnRzIGNhbm5vdCBiZSBhcHBsaWVkIHRvIHByZXZpb3VzIHB1cmNoYXNlcyBvciBhcHBsaWVkIGFmdGVyIGFuIG9ubGluZSBvcmRlciBoYXMgYmVlbiBwcm9jZXNzZWQuPC9wPg0KCTxwPg0KCQlGb3IgbWVtYmVyc2hpcCBpbmZvcm1hdGlvbiBhbmQgYXNzaXN0YW5jZSwgY29udGFjdCB1cyBhdDo8YnIgLz4NCgkJUGhvbmUgMS44NjYuNDEzLjcyNzM8YnIgLz4NCgkJRmF4IDkyNS40NTUuNTUwNDxiciAvPg0KCQlFbWFpbCA8YSBocmVmPSJtYWlsdG86YWRtaW5AaGFsZnByaWNlZHJhcGVzLmNvbSI+YWRtaW5AaGFsZnByaWNlZHJhcGVzLmNvbTwvYT48L3A+DQoJPHA+DQoJCTxzdHJvbmc+UExBQ0lORyBUUkFERSBPUkRFUlMgT1IgUkVRVUVTVElORyBBIENVU1RPTSBRVU9URSBJUyBTSU1QTEU8L3N0cm9uZz48L3A+DQoJPHA+DQoJCU91ciBUcmFkZSBUZWFtIGV4cGVydHMgb2ZmZXIgZGVkaWNhdGVkIHN1cHBvcnQgdG8gZW5zdXJlIHlvdSBoYXZlIGV2ZXJ5dGhpbmcgeW91IG5lZWQuIFRvIGVuc3VyZSB3ZSBoYXZlIGFsbCB5b3VyIGNvcnJlY3Qgb3JkZXIgZGV0YWlscywgcGxlYXNlIGRvd25sb2FkIGFuZCBjb21wbGV0ZSBhIFRyYWRlIE9yZGVyIEZvcm0uIFRyYWRlIG9yZGVycyBjYW4gYmUgcGxhY2VkIGJ5IGNhbGxpbmcgb3VyIEhhbGYgUHJpY2UgRHJhcGVzIFRyYWRlIFRlYW0gYXQgMS44NjYuNDEzLjcyNzMgb3IgYnkgZmF4aW5nIHRoZSA8YSBocmVmPSJodHRwOi8vc2l0ZS5oYWxmcHJpY2VkcmFwZXMuY29tL25ldy1ocGQvVHJhZGVfT3JkZXJGb3JtLnBkZiIgdGFyZ2V0PSJfYmxhbmsiPlRyYWRlIE9yZGVyIEZvcm08L2E+IGJhY2sgdG8gdXMgYXQgOTI1LjQ1NS41NTA0IG9yIGVtYWlsaW5nIHRoZSBmb3JtIHRvIHVzIGF0OiA8YSBocmVmPSJtYWlsdG86YWRtaW5AaGFsZnByaWNlZHJhcGVzLmNvbSI+YWRtaW5AaGFsZnByaWNlZHJhcGVzLmNvbTwvYT4uPC9wPg0KCTxwPg0KCQk8YSBocmVmPSIvaW1hZ2VzL3BkZi9UcmFkZV9PcmRlckZvcm0ucGRmIiB0YXJnZXQ9Il9ibGFuayI+Q2xpY2sgaGVyZSB0byBkb3dubG9hZCBhIFRyYWRlIE9yZGVyIEZvcm0uPC9hPjwvcD4NCgk8cD4NCgkJPGEgaHJlZj0iL2ltYWdlcy9wZGYvVHJhZGVfQ0NhdXRob3JpemF0aW9uLnBkZiIgdGFyZ2V0PSJfYmxhbmsiPkNsaWNrIGhlcmUgdG8gZG93bmxvYWQgYSBDcmVkaXQgQ2FyZCBBdXRob3JpemF0aW9uIEZvcm0uPC9hPjwvcD4NCjwvZGl2PmQYAQUeX19Db250cm9sc1JlcXVpcmVQb3N0QmFja0tleV9fFgQFImN0bDAwJENvbnRlbnRQbGFjZUhvbGRlcjEkYnRuTG9naW4FKmN0bDAwJENvbnRlbnRQbGFjZUhvbGRlcjEkYnRuQ3JlYXRlQWNjb3VudAUPY3RsMDAkYnRuU2VhcmNoBRZjdGwwMCRpbWdTdWJzY3JpYmVOZXdzPO1P/cMKSs5/K6Ki/bM6JAhOoKDcNFy6erzs9i610k0=" />
        </div>

        <script type="text/javascript">
            //<![CDATA[
            var theForm = document.forms['form1'];
            if (!theForm) {
                theForm = document.form1;
            }
            function __doPostBack(eventTarget, eventArgument) {
                if (!theForm.onsubmit || (theForm.onsubmit() != false)) {
                    theForm.__EVENTTARGET.value = eventTarget;
                    theForm.__EVENTARGUMENT.value = eventArgument;
                    theForm.submit();
                }
            }
            //]]>
        </script>


        <script src="/WebResource.axd?d=_TLUB999ZKBlPylKXFPU02OCUOM5Wy1uWtNK3vicRiUiskPZeZgL8KhZVx4aBQUCp6PSvWoQJcO2hEPjniX9caVq-j1WdSBcMFK7NNluKWo1&amp;t=635090657518590119" type="text/javascript"></script>


        <script src="/WebResource.axd?d=teDakTTrQOyYQylH29JpUcLfU3a1hYKctE2j8h_0y33Z1DLzfIXwaEGZM1aWiwrIrPhKgIi8NlOHYpr_LMdf-tkXOrAKXPQHtsgMdCyw9lY1&amp;t=635090657518590119" type="text/javascript"></script>
        <div id="wrapper">
            <div id="doc-width">
                <div id="content-width">


                    <script type="text/javascript">
                        function ForLoginPopupFacebook() {
                            var url = window.location.pathname;
                            var myPageName = url.substring(url.lastIndexOf('/') + 1);
                            if (myPageName == '')
                                myPageName = 'index.aspx';
                            var FacebookUrl = 'Facebook.aspx?RquestPageName=' + myPageName;
                            window.location.href = FacebookUrl;
                        }

                        function ForLoginGoogle() {
                            var url = window.location.pathname;
                            var myPageName = url.substring(url.lastIndexOf('/') + 1);
                            if (myPageName == '')
                                myPageName = 'index.aspx';
                            var GoogleUrl = 'GoogleLogin.aspx?googleauth=true&RquestPageName=' + myPageName;
                            window.location.href = GoogleUrl;
                        }

                        function ForLoginTwitter() {
                            var url = window.location.pathname;
                            var myPageName = url.substring(url.lastIndexOf('/') + 1);
                            if (myPageName == '')
                                myPageName = 'index.aspx';
                            var TwitterUrl = 'Twitter.aspx?RquestPageName=' + myPageName;
                            window.location.href = TwitterUrl;
                        }
                    </script>
                    <script type="text/javascript">
                        function checkfieldsforlogin() {


                            if (document.getElementById('ContentPlaceHolder1_txtusername').value.replace(/^\s+|\s+$/g, '') == '') {
                                alert('Please enter email address.');
                                document.getElementById('ContentPlaceHolder1_txtusername').value = '';
                                document.getElementById('ContentPlaceHolder1_txtusername').focus();
                                return false;
                            }
                            else if (document.getElementById('ContentPlaceHolder1_txtusername').value.replace(/^\s+|\s+$/g, '') != '' && !checkemail1(document.getElementById('ContentPlaceHolder1_txtusername').value)) {
                                alert('Please enter valid email address.');
                                document.getElementById('ContentPlaceHolder1_txtusername').value = '';
                                document.getElementById('ContentPlaceHolder1_txtusername').focus();
                                return false;
                            }
                            else if (document.getElementById('ContentPlaceHolder1_txtpassword').value.replace(/^\s+|\s+$/g, '') == '') {
                                alert('Please enter password.');
                                document.getElementById('ContentPlaceHolder1_txtpassword').value = '';
                                document.getElementById('ContentPlaceHolder1_txtpassword').focus();
                                return false;
                            }
                            return true;
                        }

                        function checkfieldsforForgotpwd() {

                            if (document.getElementById('ContentPlaceHolder1_txtEmail').value == '') {
                                alert('Please enter email address.');
                                document.getElementById('ContentPlaceHolder1_txtEmail').focus();
                                return false;
                            }
                            else if (document.getElementById('ContentPlaceHolder1_txtEmail').value != '' && !checkemail1(document.getElementById('ContentPlaceHolder1_txtEmail').value)) {
                                alert('Please enter valid email address.');
                                document.getElementById('ContentPlaceHolder1_txtEmail').focus();
                                return false;
                            }
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
                        <a href="/" title="Home">Home </a>
                        <img class="breadcrumbs-bullet" title="" alt="" src="/images/breadcrumbs-bullet.png" /><span>
            Login</span>
                    </div>
                    <div class="content-main">
                        <div class="static-title">
                            <span>Login</span>
                        </div>
                        <div class="static-big-main" style="text-align: center;">
                            <div id="ContentPlaceHolder1_pnlLogin" onkeypress="javascript:return WebForm_FireDefaultButton(event, &#39;ContentPlaceHolder1_btnLogin&#39;)" style="text-align: center;">

                                <table width="100%" cellspacing="0" cellpadding="0" border="0" style="margin-bottom: 10px;">
                                    <tbody>
                                        <tr>
                                            <td width="33%" align="left" valign="top" class="td-broder">
                                                <table width="100%" border="0" align="left" cellpadding="0" cellspacing="0" class="border-none">
                                                    <tbody>
                                                        <tr>
                                                            <th colspan="2">Returning Customers
                                                            </th>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="2">Enter your user name and password below to sign into your <strong>
                                                                <b>halfpricedrapes.com</b></strong> account.
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="padding-right: 4px;">User Name :
                                                            </td>
                                                            <td>
                                                                <input name="ctl00$ContentPlaceHolder1$txtusername" type="text" id="ContentPlaceHolder1_txtusername" class="login-field" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td valign="middle">Password :
                                                            </td>
                                                            <td valign="middle">
                                                                <input name="ctl00$ContentPlaceHolder1$txtpassword" type="password" id="ContentPlaceHolder1_txtpassword" class="login-field" style="font-size: 16pt;" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>&nbsp;
                                                            </td>
                                                            <td>
                                                                <input type="image" name="ctl00$ContentPlaceHolder1$btnLogin" id="ContentPlaceHolder1_btnLogin" alt="LOGIN" title="LOGIN" src="/images/login.png" onclick="return checkfieldsforlogin();" />
                                                            </td>
                                                        </tr>
                                                        <tr style="display: none;">
                                                            <td colspan="2" align="center">
                                                                <table cellpadding="0" cellspacing="0" align="center">
                                                                    <tr>
                                                                        <td style="padding: 0px;">
                                                                            <a href="javascript:void(0);" title="Facebook" onclick="return ForLoginPopupFacebook();">
                                                                                <img src="/images/fb-button.jpg" style="padding-right: 6px;" alt="Facebook" title="Facebook"
                                                                                    class="img-left" /></a> <a href="javascript:void(0);" title="Google" onclick="return ForLoginGoogle();">
                                                                                        <img src="images/google-button.jpg" width="80" height="29" alt="Google" title="Google"
                                                                                            class="img-left" /></a> <a href="javascript:void(0);" title="Twitter" onclick="return ForLoginTwitter();">
                                                                                                <img src="images/twitter-button.jpg" style="padding-left: 6px" width="80" height="29"
                                                                                                    alt="Twitter" title="Twitter" class="img-left" /></a>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td height="30" align="left" valign="middle">&nbsp;
                                                            </td>
                                                            <td height="30" align="left" valign="middle">
                                                                <a id="ContentPlaceHolder1_lkbForgetpwd" title="Forgot your password" href="javascript:__doPostBack(&#39;ctl00$ContentPlaceHolder1$lkbForgetpwd&#39;,&#39;&#39;)">Forgot your password?</a>
                                                            </td>
                                                        </tr>
                                                    </tbody>
                                                </table>
                                            </td>
                                            <td width="10">&nbsp;
                                            </td>

                                            <td width="10">&nbsp;
                                            </td>
                                            <td id="ContentPlaceHolder1_tdCreateAcc" width="66%" align="left" valign="top" class="td-broder">
                                                <table width="100%" border="0" align="left" cellpadding="0" cellspacing="0" class="border-none">
                                                    <tbody>
                                                        <tr>
                                                            <th>Creating an Account
                                                            </th>
                                                        </tr>
                                                        <tr>
                                                            <td height="75">Creating an account provides access to tools including saved addresses, order history
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="center" valign="middle">
                                                                <input type="image" name="ctl00$ContentPlaceHolder1$btnCreateAccount" id="ContentPlaceHolder1_btnCreateAccount" alt="CREATE AN ACCOUNT" title="CREATE AN ACCOUNT" src="/images/create-an-account.png" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>&nbsp;
                                                            </td>
                                                        </tr>
                                                    </tbody>
                                                </table>
                                            </td>

                                        </tr>
                                    </tbody>
                                </table>

                            </div>

                        </div>
                    </div>

                </div>
                <div id="header-part" style="left: 0;">
                    <div class="header-row2">
                        <div class="header-top-link">
                            <div id="hidePhone" class="header-row2-right-pt0">
                                <p>
                                    Call Toll Free: <span style="font-size: 20px; font-weight: bold;">1-866-413-7273</span> <span style="color: #393939; text-transform: none; font-size: 12px; padding-left: 5px;" class="display-none"><strong>Hours:</strong> Mon-Fri 8 am - 5 pm PST, Sat-Sun Closed</span>
                                </p>
                            </div>
                            <ul>

                                <li id="litradepartner" onclick="showtradeshipping();"><a href="javascript:void(0);">Trade &amp; Partner Program</a></li>
                            </ul>

                        </div>
                        <div class="header-row1-text" style="width: 83%; float: right;">

                            <div id="divShoppingCart" class="header-row2-right-pt4">
                                <div onmouseover="javascript:ShowHideCart();">
                                    <a href="/AddToCart.aspx" id="cartlink" title="Shopping Cart">
                                        <p><span class='navQty'>(0 item)</span><span class='navTotal'> $0.00</span></p>
                                        <img src='/images/cart-icon.jpg' width='48' height='15' alt='' title='' class='cart-icon'></a>
                                    <div id="divMiniCart" style="background-color: White;">

                                        <input type="hidden" id="hiddenCustID" value='0' />
                                        <input type="hidden" id="hiddenTotalItems" value='' />
                                        <div id="CartLayer" class="right-sub-content" onmouseover="showMiniCart();" onmouseout="resetHover();">
                                            <div id="DivMyCart" class="right-sub-row1">
                                                <div id="divMiniCart">
                                                    <div id="divCart">
                                                        </table>
               
                                                    </div>
                                                </div>
                                            </div>
                                        </div>

                                    </div>
                                    <input name="ctl00$hiddenTotalItems" type="hidden" id="hiddenTotalItems" value="0.00" />
                                    <input type="hidden" id="hdnrotator" value="1" />
                                    <input name="ctl00$hdnColorSelection" type="hidden" id="hdnColorSelection" />
                                </div>
                            </div>
                            <div class="header-row2-right-pt1" style="width: 290px !important;">
                                <ul>
                                    <li id="MyAccount" style="background: none;"><a href="/CreateAccount.aspx" id="ltregister" title="Register">Register</a></li>
                                    <li><a href="/login.aspx" id="ltLogin" title="Sign In">Sign In</a></li>
                                    <li><a title="Contact Us" href="javascript:void(0);" onclick="ShowModelSearchContactus();">Contact Us</a></li>
                                    <li><a href="/index.aspx" title="Home" class="active">Home</a></li>
                                </ul>
                            </div>
                        </div>
                        <div class="logo">
                            <a href="/index.aspx" title='halfpricedrapes'>
                                <img src="/images/logo.png" alt='halfpricedrapes'
                                    title='halfpricedrapes'
                                    class="img-left"></a>
                        </div>
                        <div class="header-row2-right" style="width: 440px;">
                            <div>
                                <a class="free-swatch" href="/free-swatch.html">
                                    <img src="/images/free-swatches.jpg" border="0" style="float: right; margin-left: 10px;" /></a>
                                <div class="header-row2-right-pro2">


                                    <p class="free-shipping"><span><a onclick="showfreeshipping();" href="javascript:void(0);">Free Shipping</a></span> on orders over $249.00</p>
                                    <div class="header-row2-right-pt2">
                                        <div class="search-box-bg">
                                            <div id="pnlSearch">

                                                <input name="ctl00$txtSearch" type="text" value="Enter Search here" id="txtSearch" class="textfield" onfocus="clear_Search(this)" onblur="if(this.value==&#39;&#39;){this.value=&#39;Enter Search here&#39;};" onkeypress="__defaultFired=false;" onkeydown="if(event.which || event.keyCode){if ((event.which == 13) || (event.keyCode == 13)) {document.getElementById(&#39;btnSearch&#39;).click();return false;}} else {return true}; " style="height: 33px; font-size: 12px; width: 272px;" />
                                                <input type="image" name="ctl00$btnSearch" id="btnSearch" title="Search" class="img-left" onfocus="return ValidSearchByPage();" src="/images/search.jpg" onclick="return ValidSearchByPage();" />

                                            </div>
                                        </div>
                                    </div>
                                    <a onclick="ShowModelSearch();" class="find-pro-link" href="javascript:void(0);">Product Finder click here...</a>
                                </div>


                            </div>

                        </div>
                    </div>

                    <div id="header-row3">
                        <div class="top-menu">
                            <a style="display: none;" class="toggleMenu" href="#">Menu</a>
                            <ul class="header-nav" style="z-index: 11;">
                                <li id="menu_link" class="link-1"><a onmouseover='hideshowdiv(1,1);' href='/signature-silk-curtains.html'>Signature Silk Curtains</a>
                                    <ul class="sub-menu" id="menu_detail1">
                                        <li id="lisub1"><a onmouseover="hideshowdiv(1,1);" href='/designer-silk-stripe-curtains.html' title='Designer Silk Stripe Curtains'>Designer Silk Stripe Curtains</a><div class="menu-desc" id="divsubcat1" style="display: none;">
                                            <div class="menu-desc-left">
                                                <span>Designer Silk Stripe Curtains</span><p>
                                                    A simple, yet stunning collection of stripe curtains made of lustrous silk. These sumptuous silks work in any style interior and are a perfect complement to your home decor.
                                                </p>
                                            </div>
                                            <div class="menu-desc-right">
                                                <a href='/designer-silk-stripe-curtains.html'>
                                                    <img title="Designer Silk Stripe Curtains" alt="Designer Silk Stripe Curtains" src="https://www.halfpricedrapes.us/Resources/halfpricedraps/Category/icon/designer-silk-stripe-curtains_1657.jpg?858"></a>
                                            </div>
                                        </div>
                                            <div class="menu-desc" id="divsubcat2" style="display: none;">
                                                <div class="menu-desc-left">
                                                    <span>Designer Silk Plaid Curtains</span><p>
                                                        A cozy collection of checks and plaid silk curtains in beautiful color combinations. They are hand weaved and drape perfectly.
                                                    </p>
                                                </div>
                                                <div class="menu-desc-right">
                                                    <a href='/designer-silk-plaid-curtains.html'>
                                                        <img title="Designer Silk Plaid Curtains" alt="Designer Silk Plaid Curtains" src="https://www.halfpricedrapes.us/Resources/halfpricedraps/Category/icon/designer-silk-plaid-curtains_1658.jpg?858"></a>
                                                </div>
                                            </div>
                                            <div class="menu-desc" id="divsubcat3" style="display: none;">
                                                <div class="menu-desc-left">
                                                    <span>Designer Silk Embroidered Curtains</span><p>
                                                        A luxurious collection of sophisticated embroideries on Thai Silk. The collection offers timeless designs creating unique, one-of-a-kind curtains for your home.
                                                    </p>
                                                </div>
                                                <div class="menu-desc-right">
                                                    <a href='/designer-silk-embroidered-curtains.html'>
                                                        <img title="Designer Silk Embroidered Curtains" alt="Designer Silk Embroidered Curtains" src="https://www.halfpricedrapes.us/Resources/halfpricedraps/Category/icon/designer-silk-embroidered-curtains_1659.jpg?858"></a>
                                                </div>
                                            </div>
                                            <div class="menu-desc" id="divsubcat4" style="display: none;">
                                                <div class="menu-desc-left">
                                                    <span>Thai Silk Curtains</span><p>
                                                        Our Thai Silk Drapes bring a subtle air of delicacy and culture. Our silk is distinctive in texture and is of the highest quality.
                                                    </p>
                                                </div>
                                                <div class="menu-desc-right">
                                                    <a href='/thai-silk-curtains.html'>
                                                        <img title="Thai Silk Curtains" alt="Thai Silk Curtains" src="https://www.halfpricedrapes.us/Resources/halfpricedraps/Category/icon/thai-silk-curtains_1666.jpg?858"></a>
                                                </div>
                                            </div>
                                            <div class="menu-desc" id="divsubcat5" style="display: none;">
                                                <div class="menu-desc-left">
                                                    <span>Textured Dupioni Silk Curtains</span><p>
                                                        Dupioni Silk has been around for centuries. The beautiful luster and sheen of this textured silk is timeless &amp; works in any d&eacute;cor.
                                                    </p>
                                                </div>
                                                <div class="menu-desc-right">
                                                    <a href='/textured-dupioni-silk-curtains.html'>
                                                        <img title="Textured Dupioni Silk Curtains" alt="Textured Dupioni Silk Curtains" src="https://www.halfpricedrapes.us/Resources/halfpricedraps/Category/icon/textured-dupioni-silk-curtains_1663.jpg?858"></a>
                                                </div>
                                            </div>
                                            <div class="menu-desc" id="divsubcat6" style="display: none;">
                                                <div class="menu-desc-left">
                                                    <span>Raw Silk Curtains</span><p>
                                                        Inspired by the ancient weaving techniques HPD is pleased to present our Hand-Made Raw Silk curtains. Made from a combination of the finest handspun silk yarns. From traditional to contemporary, our raw silk curtains provide the perfect complement to many interiors.
                                                    </p>
                                                </div>
                                                <div class="menu-desc-right">
                                                    <a href='/raw-silk-curtains.html'>
                                                        <img title="Raw Silk Curtains" alt="Raw Silk Curtains" src="https://www.halfpricedrapes.us/Resources/halfpricedraps/Category/icon/new-raw-silk-curtains_1664.jpg?858"></a>
                                                </div>
                                            </div>
                                            <div class="menu-desc" id="divsubcat7" style="display: none;">
                                                <div class="menu-desc-left">
                                                    <span>Taffeta Silk Curtains</span><p>
                                                        Our 100% Silk Taffeta drapes &amp; curtains represent extravagant luxury at unbeatable prices. Our team of designers have worked tirelessly to find the best colors to make our selection truly vibrant, timeless &amp; unique.
                                                    </p>
                                                </div>
                                                <div class="menu-desc-right">
                                                    <a href='/taffeta-silk-curtains.html'>
                                                        <img title="Taffeta Silk Curtains" alt="Taffeta Silk Curtains" src="https://www.halfpricedrapes.us/Resources/halfpricedraps/Category/icon/taffeta-silk-curtains_1665.jpg?499"></a>
                                                </div>
                                            </div>
                                            <div class="menu-desc" id="divsubcat8" style="display: none;">
                                                <div class="menu-desc-left">
                                                    <span>Ruched Thai Silk Curtains</span><p>
                                                        We&#39;ve taken our popular Thai Silk panels and added a ruched header creating the most luxurious, over the top style in window treatments out there at an incredible price.
                                                    </p>
                                                </div>
                                                <div class="menu-desc-right">
                                                    <a href='/ruched-thai-silk-curtains.html'>
                                                        <img title="Ruched Thai Silk Curtains" alt="Ruched Thai Silk Curtains" src="https://www.halfpricedrapes.us/Resources/halfpricedraps/Category/icon/ruched-thai-silk-curtains_1667.jpg?499"></a>
                                                </div>
                                            </div>
                                            <div class="menu-desc" id="divsubcat9" style="display: none;">
                                                <div class="menu-desc-left">
                                                    <span>Banded Thai Silk With Velvet Curtains</span><p>
                                                        The Banded Thai Silk with Velvet curtains &amp; drapes are the perfect combination of sophistication &amp; style. This is a design that can easily transition into any d&eacute;cor whether your home is classic &amp; traditional or modern &amp; contemporary.
                                                    </p>
                                                </div>
                                                <div class="menu-desc-right">
                                                    <a href='/banded-thai-silk-with-velvet-curtains.html'>
                                                        <img title="Banded Thai Silk With Velvet Curtains" alt="Banded Thai Silk With Velvet Curtains" src="https://www.halfpricedrapes.us/Resources/halfpricedraps/Category/icon/banded-thai-silk-with-velvet-curtains_1669.jpg?499"></a>
                                                </div>
                                            </div>
                                            <div class="menu-desc" id="divsubcat10" style="display: none;">
                                                <div class="menu-desc-left">
                                                    <span>Banded Thai Silk Curtains</span><p>
                                                        The Banded Thai Silk curtains &amp; drapes are the perfect combination of sophistication &amp; style. This is a design that can easily transition into any d&eacute;cor whether your home is classic &amp; traditional or modern &amp; contemporary.
                                                    </p>
                                                </div>
                                                <div class="menu-desc-right">
                                                    <a href='/banded-thai-silk-curtains.html'>
                                                        <img title="Banded Thai Silk Curtains" alt="Banded Thai Silk Curtains" src="https://www.halfpricedrapes.us/Resources/halfpricedraps/Category/icon/banded-thai-silk-curtains_1668.jpg?499"></a>
                                                </div>
                                            </div>
                                            <div class="menu-desc" id="divsubcat11" style="display: none;">
                                                <div class="menu-desc-left">
                                                    <span>Signature Silk Swatches</span><p>Half Price Drapes is pleased to introduce our NEW Swatch Program. As a valued customer we are offering Free 10 fabric swatches (Note: excluding USPS Priority Mail shipping charge)</p>
                                                </div>
                                                <div class="menu-desc-right">
                                                    <a href='/signature-silk-swatches.html'>
                                                        <img title="Signature Silk Swatches" alt="Signature Silk Swatches" src="https://www.halfpricedrapes.us/Resources/halfpricedraps/Category/icon/designer-silk-swatches_1660.jpg?499"></a>
                                                </div>
                                            </div>
                                            <div class="menu-desc" id="divsubcat12" style="display: none;">
                                                <div class="menu-desc-left">
                                                    <span>Shop All Signature Silk Curtains</span><p></p>
                                                </div>
                                                <div class="menu-desc-right">
                                                    <a href='/shop-all-signature-silk-curtains.html'>
                                                        <img title="Shop All Signature Silk Curtains" alt="Shop All Signature Silk Curtains" src="https://www.halfpricedrapes.us/Resources/halfpricedraps/Category/icon/shop-by-signature-silk-curtains_1742.jpg?499"></a>
                                                </div>
                                            </div>
                                        </li>
                                        <li id="lisub2"><a onmouseover="hideshowdiv(2,1);" href='/designer-silk-plaid-curtains.html' title='Designer Silk Plaid Curtains'>Designer Silk Plaid Curtains<li id="lisub3"><a onmouseover="hideshowdiv(3,1);" href='/designer-silk-embroidered-curtains.html' title='Designer Silk Embroidered Curtains'>Designer Silk Embroidered Curtains<li id="lisub4"><a onmouseover="hideshowdiv(4,1);" href='/thai-silk-curtains.html' title='Thai Silk Curtains'>Thai Silk Curtains<li id="lisub5"><a onmouseover="hideshowdiv(5,1);" href='/textured-dupioni-silk-curtains.html' title='Textured Dupioni Silk Curtains'>Textured Dupioni Silk Curtains<li id="lisub6"><a onmouseover="hideshowdiv(6,1);" href='/raw-silk-curtains.html' title='Raw Silk Curtains'>Raw Silk Curtains<li id="lisub7"><a onmouseover="hideshowdiv(7,1);" href='/taffeta-silk-curtains.html' title='Taffeta Silk Curtains'>Taffeta Silk Curtains<li id="lisub8"><a onmouseover="hideshowdiv(8,1);" href='/ruched-thai-silk-curtains.html' title='Ruched Thai Silk Curtains'>Ruched Thai Silk Curtains<li id="lisub9"><a onmouseover="hideshowdiv(9,1);" href='/banded-thai-silk-with-velvet-curtains.html' title='Banded Thai Silk With Velvet Curtains'>Banded Thai Silk With Velvet Curtains<li id="lisub10"><a onmouseover="hideshowdiv(10,1);" href='/banded-thai-silk-curtains.html' title='Banded Thai Silk Curtains'>Banded Thai Silk Curtains<li id="lisub11"><a onmouseover="hideshowdiv(11,1);" href='/signature-silk-swatches.html' title='Signature Silk Swatches'>Signature Silk Swatches<li id="lisub12"><a onmouseover="hideshowdiv(12,1);" href='/shop-all-signature-silk-curtains.html' title='Shop All Signature Silk Curtains'>
                                        Shop All Signature Silk Curtains
                                    </ul>
                                </li>
                                <li id="menu_link" class="link-2"><a onmouseover='hideshowdiv(13,2);' href='/pattern-faux-silk-curtains.html'>Pattern Faux Silk Curtains</a>
                                    <ul class="sub-menu" id="menu_detail2">
                                        <li id="lisub13"><a onmouseover="hideshowdiv(13,2);" href='/the-fifth-element-modern-contemporary.html' title='The Fifth Element - Modern & Contemporary'>The Fifth Element - Modern & Contemporary</a><div class="menu-desc" id="divsubcat13" style="display: none;">
                                            <div class="menu-desc-left">
                                                <span>The Fifth Element - Modern & Contemporary</span><p>
                                                    Sophisticated, Modern &amp; Contemporary Collection of decorative fabrics woven using textual yarns to provide depth and interest.
                                                </p>
                                            </div>
                                            <div class="menu-desc-right">
                                                <a href='/the-fifth-element-modern-contemporary.html'>
                                                    <img title="The Fifth Element - Modern &amp; Contemporary" alt="The Fifth Element - Modern &amp; Contemporary" src="https://www.halfpricedrapes.us/Resources/halfpricedraps/Category/icon/the-fifth-element-modern-contemporary_1725.jpg?499"></a>
                                            </div>
                                        </div>
                                            <div class="menu-desc" id="divsubcat14" style="display: none;">
                                                <div class="menu-desc-left">
                                                    <span>Faux Silk Jacquard Curtains</span><p>
                                                        Collection of Jacquard Weaves that are both elegant and practical. Priced for Every Budget.
                                                    </p>
                                                </div>
                                                <div class="menu-desc-right">
                                                    <a href='/faux-silk-jacquard-curtains.html'>
                                                        <img title="Faux Silk Jacquard Curtains" alt="Faux Silk Jacquard Curtains" src="https://www.halfpricedrapes.us/Resources/halfpricedraps/Category/icon/faux-silk-jacquard-curtains_1726.jpg?499"></a>
                                                </div>
                                            </div>
                                            <div class="menu-desc" id="divsubcat15" style="display: none;">
                                                <div class="menu-desc-left">
                                                    <span>Flocked Faux Silk Curtains</span><p>
                                                        Bright, contemporary colors and elegant shades are skillfully combined with neutrals and flocking to create striking visual effects in this collection of geometric &amp; traditional designs.
                                                    </p>
                                                </div>
                                                <div class="menu-desc-right">
                                                    <a href='/flocked-faux-silk-curtains.html'>
                                                        <img title="Flocked Faux Silk Curtains" alt="Flocked Faux Silk Curtains" src="https://www.halfpricedrapes.us/Resources/halfpricedraps/Category/icon/flocked-faux-silk-curtains_1722.jpg?499"></a>
                                                </div>
                                            </div>
                                            <div class="menu-desc" id="divsubcat16" style="display: none;">
                                                <div class="menu-desc-left">
                                                    <span>Embroidered & Patterned Faux Silk Curtains</span><p>
                                                        An elaborate collection that captures the spirit and poise of traditional and contemporary themes. Lustrous faux silk embroideries, appliqu&eacute;s in stunning colors.
                                                    </p>
                                                </div>
                                                <div class="menu-desc-right">
                                                    <a href='/embroidered-patterned-faux-silk-curtains.html'>
                                                        <img title="Embroidered &amp; Patterned Faux Silk Curtains" alt="Embroidered &amp; Patterned Faux Silk Curtains" src="https://www.halfpricedrapes.us/Resources/halfpricedraps/Category/icon/embroidered-patterned-faux-silk-curtains_1678.jpg?499"></a>
                                                </div>
                                            </div>
                                            <div class="menu-desc" id="divsubcat17" style="display: none;">
                                                <div class="menu-desc-left">
                                                    <span>Striped Faux Silk Taffeta Curtains</span><p>
                                                        Distinctly opulent, yarn dyed faux silk stripes in stunning color palettes give a touch of sumptuous luxury. Combine a classically colored two tone stripe or a smart multi colored stripe for that complete look.
                                                    </p>
                                                </div>
                                                <div class="menu-desc-right">
                                                    <a href='/striped-faux-silk-taffeta-curtains.html'>
                                                        <img title="Striped Faux Silk Taffeta Curtains" alt="Striped Faux Silk Taffeta Curtains" src="https://www.halfpricedrapes.us/Resources/halfpricedraps/Category/icon/striped-faux-silk-taffeta-curtains_1648.jpg?499"></a>
                                                </div>
                                            </div>
                                            <div class="menu-desc" id="divsubcat18" style="display: none;">
                                                <div class="menu-desc-left">
                                                    <span>Faux Silk Swatches</span><p>Half Price Drapes is pleased to introduce our NEW Swatch Program. As a valued customer we are offering Free 10 fabric swatches (Note: excluding USPS Priority Mail shipping charge)</p>
                                                </div>
                                                <div class="menu-desc-right">
                                                    <a href='/faux-silk-swatches.html'>
                                                        <img title="Faux Silk Swatches" alt="Faux Silk Swatches" src="https://www.halfpricedrapes.us/Resources/halfpricedraps/Category/icon/faux-silk-swatches_1679.jpg?499"></a>
                                                </div>
                                            </div>
                                            <div class="menu-desc" id="divsubcat19" style="display: none;">
                                                <div class="menu-desc-left">
                                                    <span>Shop All Pattern Faux Silk Curtains</span><p></p>
                                                </div>
                                                <div class="menu-desc-right">
                                                    <a href='/shop-all-pattern-faux-silk-curtains.html'>
                                                        <img title="Shop All Pattern Faux Silk Curtains" alt="Shop All Pattern Faux Silk Curtains" src="https://www.halfpricedrapes.us/Resources/halfpricedraps/Category/icon/shop-by-pattern-faux-silk-curtains_1743.jpg?499"></a>
                                                </div>
                                            </div>
                                        </li>
                                        <li id="lisub14"><a onmouseover="hideshowdiv(14,2);" href='/faux-silk-jacquard-curtains.html' title='Faux Silk Jacquard Curtains'>Faux Silk Jacquard Curtains<li id="lisub15"><a onmouseover="hideshowdiv(15,2);" href='/flocked-faux-silk-curtains.html' title='Flocked Faux Silk Curtains'>Flocked Faux Silk Curtains<li id="lisub16"><a onmouseover="hideshowdiv(16,2);" href='/embroidered-patterned-faux-silk-curtains.html' title='Embroidered & Patterned Faux Silk Curtains'>Embroidered & Patterned Faux Silk Curtains<li id="lisub17"><a onmouseover="hideshowdiv(17,2);" href='/striped-faux-silk-taffeta-curtains.html' title='Striped Faux Silk Taffeta Curtains'>Striped Faux Silk Taffeta Curtains<li id="lisub18"><a onmouseover="hideshowdiv(18,2);" href='/faux-silk-swatches.html' title='Faux Silk Swatches'>Faux Silk Swatches<li id="lisub19"><a onmouseover="hideshowdiv(19,2);" href='/shop-all-pattern-faux-silk-curtains.html' title='Shop All Pattern Faux Silk Curtains'>
                                        Shop All Pattern Faux Silk Curtains
                                    </ul>
                                </li>
                                li id="menu_link" class="link-3"><a onmouseover='hideshowdiv(20,3);' href='/solid-faux-silk-curtains.html'>Solid Faux Silk Curtains</a>
                                <ul class="sub-menu" id="menu_detail3">
                                    <li id="lisub20"><a onmouseover="hideshowdiv(20,3);" href='/vintage-textured-faux-dupioni-silk-curtains.html' title='Vintage Textured Faux Dupioni Silk Curtains'>Vintage Textured Faux Dupioni Silk Curtains</a><div class="menu-desc" id="divsubcat20" style="display: none;">
                                        <div class="menu-desc-left">
                                            <span>Vintage Textured Faux Dupioni Silk Curtains</span><p>
                                                A simple, yet effective dupioni weave with minimally textured details. This well-priced faux silk dupioni is both elegant and practical.
                                            </p>
                                        </div>
                                        <div class="menu-desc-right">
                                            <a href='/vintage-textured-faux-dupioni-silk-curtains.html'>
                                                <img title="Vintage Textured Faux Dupioni Silk Curtains" alt="Vintage Textured Faux Dupioni Silk Curtains" src="https://www.halfpricedrapes.us/Resources/halfpricedraps/Category/icon/vintage-textured-faux-dupioni-silk-curtains_1735.jpg?141"></a>
                                        </div>
                                    </div>
                                        <div class="menu-desc" id="divsubcat21" style="display: none;">
                                            <div class="menu-desc-left">
                                                <span>Faux Silk Taffeta Curtains</span><p>
                                                    Our Faux&nbsp; Silk Taffeta curtains offer the crisp texture of taffeta with a supple handle that drapes beautifully. With an elegant color palette and luster, this practical fabric is ideal for various rooms.
                                                </p>
                                            </div>
                                            <div class="menu-desc-right">
                                                <a href='/faux-silk-taffeta-curtains.html'>
                                                    <img title="Faux Silk Taffeta Curtains" alt="Faux Silk Taffeta Curtains" src="https://www.halfpricedrapes.us/Resources/halfpricedraps/Category/icon/faux-silk-taffeta-curtains_1736.jpg?141"></a>
                                            </div>
                                        </div>
                                        <div class="menu-desc" id="divsubcat22" style="display: none;">
                                            <div class="menu-desc-left">
                                                <span>Ruched Faux Silk Taffeta Curtains</span><p>
                                                    We&#39;ve taken our popular Faux Silk Taffeta panels and added a Ruched header creating the most luxurious style in window treatments at the most affordable prices.
                                                </p>
                                            </div>
                                            <div class="menu-desc-right">
                                                <a href='/ruched-faux-silk-taffeta-curtains.html'>
                                                    <img title="Ruched Faux Silk Taffeta Curtains" alt="Ruched Faux Silk Taffeta Curtains" src="https://www.halfpricedrapes.us/Resources/halfpricedraps/Category/icon/ruched-faux-silk-taffeta-curtains_1737.jpg?141"></a>
                                            </div>
                                        </div>
                                        <div class="menu-desc" id="divsubcat23" style="display: none;">
                                            <div class="menu-desc-left">
                                                <span>Yarn Dyed Faux Dupioni Silk Curtains</span><p>
                                                    Inspired by the ancient weaving techniques, this gorgeous collection of luxury yard dyed faux silks curtains are offered in neutral and iridescence palette.
                                                </p>
                                            </div>
                                            <div class="menu-desc-right">
                                                <a href='/yarn-dyed-faux-dupioni-silk-curtains.html'>
                                                    <img title="Yarn Dyed Faux Dupioni Silk Curtains" alt="Yarn Dyed Faux Dupioni Silk Curtains" src="https://www.halfpricedrapes.us/Resources/halfpricedraps/Category/icon/yarn-dyed-faux-dupioni-silk-curtains_1727.jpg?141"></a>
                                            </div>
                                        </div>
                                        <div class="menu-desc" id="divsubcat24" style="display: none;">
                                            <div class="menu-desc-left">
                                                <span>Solid Faux Silk Swatches</span><p>Half Price Drapes is pleased to introduce our NEW Swatch Program. As a valued customer we are offering Free 10 fabric swatches (Note: excluding USPS Priority Mail shipping charge)</p>
                                            </div>
                                            <div class="menu-desc-right">
                                                <a href='/solid-faux-silk-swatches.html'>
                                                    <img title="Solid Faux Silk Swatches" alt="Solid Faux Silk Swatches" src="https://www.halfpricedrapes.us/Resources/halfpricedraps/Category/icon/solid-faux-silk-swatches_1738.jpg?141"></a>
                                            </div>
                                        </div>
                                        <div class="menu-desc" id="divsubcat25" style="display: none;">
                                            <div class="menu-desc-left">
                                                <span>Shop All Solid Faux Silk Curtains</span><p></p>
                                            </div>
                                            <div class="menu-desc-right">
                                                <a href='/shop-all-solid-faux-silk-curtains.html'>
                                                    <img title="Shop All Solid Faux Silk Curtains" alt="Shop All Solid Faux Silk Curtains" src="https://www.halfpricedrapes.us/Resources/halfpricedraps/Category/icon/shop-by-solid-faux-silk-curtains_1744.jpg?141"></a>
                                            </div>
                                        </div>
                                    </li>
                                    <li id="lisub21"><a onmouseover="hideshowdiv(21,3);" href='/faux-silk-taffeta-curtains.html' title='Faux Silk Taffeta Curtains'>Faux Silk Taffeta Curtains<li id="lisub22"><a onmouseover="hideshowdiv(22,3);" href='/ruched-faux-silk-taffeta-curtains.html' title='Ruched Faux Silk Taffeta Curtains'>Ruched Faux Silk Taffeta Curtains<li id="lisub23"><a onmouseover="hideshowdiv(23,3);" href='/yarn-dyed-faux-dupioni-silk-curtains.html' title='Yarn Dyed Faux Dupioni Silk Curtains'>Yarn Dyed Faux Dupioni Silk Curtains<li id="lisub24"><a onmouseover="hideshowdiv(24,3);" href='/solid-faux-silk-swatches.html' title='Solid Faux Silk Swatches'>Solid Faux Silk Swatches<li id="lisub25"><a onmouseover="hideshowdiv(25,3);" href='/shop-all-solid-faux-silk-curtains.html' title='Shop All Solid Faux Silk Curtains'>
                                    Shop All Solid Faux Silk Curtains
                                </ul>
                                </li><li id="menu_link" class="link-4"><a onmouseover='hideshowdiv(26,4);' href='/velvet-curtains.html'>Velvet Curtains</a>
                                    <ul class="sub-menu" id="menu_detail4">
                                        <li id="lisub26"><a onmouseover="hideshowdiv(26,4);" href='/vintage-cotton-velvet-curtains.html' title='Vintage Cotton Velvet Curtains'>Vintage Cotton Velvet Curtains</a><div class="menu-desc" id="divsubcat26" style="display: none;">
                                            <div class="menu-desc-left">
                                                <span>Vintage Cotton Velvet Curtains</span><p>
                                                    Our&nbsp; luxurious cotton velvet curtains have a&nbsp; soft hand and an elegant&nbsp; look. Our collection is offered in&nbsp; a balanced color palette both in neutrals, bright&nbsp; &amp; softened tones.
                                                </p>
                                            </div>
                                            <div class="menu-desc-right">
                                                <a href='/vintage-cotton-velvet-curtains.html'>
                                                    <img title="Vintage Cotton Velvet Curtains" alt="Vintage Cotton Velvet Curtains" src="https://www.halfpricedrapes.us/Resources/halfpricedraps/Category/icon/vintage-cotton-velvet-curtains_1674.jpg?141"></a>
                                            </div>
                                        </div>
                                            <div class="menu-desc" id="divsubcat27" style="display: none;">
                                                <div class="menu-desc-left">
                                                    <span>Signature Double Wide Velvet Blackout Curtains</span><p>
                                                        Our Double Wide Velvet blackout curtains offer wide widths for practical purposes. The curtains have high performance polyester velvet lined with exceptionally soft&nbsp; blackout fabric for optimal insulation.
                                                    </p>
                                                </div>
                                                <div class="menu-desc-right">
                                                    <a href='/signature-double-wide-velvet-blackout-curtains.html'>
                                                        <img title="Signature Double Wide Velvet Blackout Curtains" alt="Signature Double Wide Velvet Blackout Curtains" src="https://www.halfpricedrapes.us/Resources/halfpricedraps/Category/icon/signature-double-wide-velvet-blackout-curtains_1682.jpg?141"></a>
                                                </div>
                                            </div>
                                            <div class="menu-desc" id="divsubcat28" style="display: none;">
                                                <div class="menu-desc-left">
                                                    <span>Signature Blackout Velvet Curtains</span><p>
                                                        The curtains have high performance polyester velvet face fabric and are lined with exceptionally soft blackout lining. The collection is offered in both Pole Pocket and Grommet headers at most attractive price.
                                                    </p>
                                                </div>
                                                <div class="menu-desc-right">
                                                    <a href='/signature-blackout-velvet-curtains.html'>
                                                        <img title="Signature Blackout Velvet Curtains" alt="Signature Blackout Velvet Curtains" src="https://www.halfpricedrapes.us/Resources/halfpricedraps/Category/icon/signature-blackout-velvet-curtains_1680.jpg?141"></a>
                                                </div>
                                            </div>
                                            <div class="menu-desc" id="divsubcat29" style="display: none;">
                                                <div class="menu-desc-left">
                                                    <span>Velvet & Blackout Swatches</span><p>Half Price Drapes is pleased to introduce our NEW Swatch Program. As a valued customer we are offering Free 10 fabric swatches (Note: excluding USPS Priority Mail shipping charge)</p>
                                                </div>
                                                <div class="menu-desc-right">
                                                    <a href='/velvet-blackout-swatches.html'>
                                                        <img title="Velvet &amp; Blackout Swatches" alt="Velvet &amp; Blackout Swatches" src="https://www.halfpricedrapes.us/Resources/halfpricedraps/Category/icon/velvet-blackout-swatches_1683.jpg?141"></a>
                                                </div>
                                            </div>
                                            <div class="menu-desc" id="divsubcat30" style="display: none;">
                                                <div class="menu-desc-left">
                                                    <span>Shop All Velvet Curtains</span><p></p>
                                                </div>
                                                <div class="menu-desc-right">
                                                    <a href='/shop-all-velvet-curtains.html'>
                                                        <img title="Shop All Velvet Curtains" alt="Shop All Velvet Curtains" src="https://www.halfpricedrapes.us/Resources/halfpricedraps/Category/icon/shop-by-velvet-curtains_1745.jpg?141"></a>
                                                </div>
                                            </div>
                                        </li>
                                        <li id="lisub27"><a onmouseover="hideshowdiv(27,4);" href='/signature-double-wide-velvet-blackout-curtains.html' title='Signature Double Wide Velvet Blackout Curtains'>Signature Double Wide Velvet Blackout Curtains<li id="lisub28"><a onmouseover="hideshowdiv(28,4);" href='/signature-blackout-velvet-curtains.html' title='Signature Blackout Velvet Curtains'>Signature Blackout Velvet Curtains<li id="lisub29"><a onmouseover="hideshowdiv(29,4);" href='/velvet-blackout-swatches.html' title='Velvet & Blackout Swatches'>Velvet & Blackout Swatches<li id="lisub30"><a onmouseover="hideshowdiv(30,4);" href='/shop-all-velvet-curtains.html' title='Shop All Velvet Curtains'>
                                        Shop All Velvet Curtains
                                    </ul>
                                </li>
                                <li id="menu_link" class="link-5"><a onmouseover='hideshowdiv(31,5);' href='/blackout-curtains.html'>Blackout Curtains</a>
                                    <ul class="sub-menu" id="menu_detail5">
                                        <li id="lisub31"><a onmouseover="hideshowdiv(31,5);" href='/bellino-blackout-curtains.html' title='Bellino Blackout Curtains'>Bellino Blackout Curtains</a><div class="menu-desc" id="divsubcat31" style="display: none;">
                                            <div class="menu-desc-left">
                                                <span>Bellino Blackout Curtains</span><p>
                                                    The look of a beautifully textured linen weave has been realized in this collection of blackout curtains. The Bellino Blackout Curtains combines the decorative look and practicality of blackout curtains.
                                                </p>
                                            </div>
                                            <div class="menu-desc-right">
                                                <a href='/bellino-blackout-curtains.html'>
                                                    <img title="Bellino Blackout Curtains" alt="Bellino Blackout Curtains" src="https://www.halfpricedrapes.us/Resources/halfpricedraps/Category/icon/bellino-blackout-curtains_1730.jpg?304"></a>
                                            </div>
                                        </div>
                                            <div class="menu-desc" id="divsubcat32" style="display: none;">
                                                <div class="menu-desc-left">
                                                    <span>Solid Blackout Curtains</span><p>
                                                        A simple yet refined finish that brings out the beautiful luster and fall.&nbsp; Our soft, solid blackout curtains are offered in colors for every shade. All incorporating the light blocking features at the most affordable prices.
                                                    </p>
                                                </div>
                                                <div class="menu-desc-right">
                                                    <a href='/solid-blackout-curtains.html'>
                                                        <img title="Solid Blackout Curtains" alt="Solid Blackout Curtains" src="https://www.halfpricedrapes.us/Resources/halfpricedraps/Category/icon/signature-blackout-curtains_1731.jpg?304"></a>
                                                </div>
                                            </div>
                                            <div class="menu-desc" id="divsubcat33" style="display: none;">
                                                <div class="menu-desc-left">
                                                    <span>Patterned Blackout Curtains</span><p>
                                                        A vibrant collection of contemporary prints all in a mix of tropical color and cool, citrus hues. Also in a wide range of patterns featuring medallions, bold geometrics and&nbsp; trellis designs. Our Patterned Blackout Curtains are ideal for all rooms and offers light blocking qualities.
                                                    </p>
                                                </div>
                                                <div class="menu-desc-right">
                                                    <a href='/patterned-blackout-curtains.html'>
                                                        <img title="Patterned Blackout Curtains" alt="Patterned Blackout Curtains" src="https://www.halfpricedrapes.us/Resources/halfpricedraps/Category/icon/patterned-blackout-curtains_1673.jpg?304"></a>
                                                </div>
                                            </div>
                                            <div class="menu-desc" id="divsubcat34" style="display: none;">
                                                <div class="menu-desc-left">
                                                    <span>Signature Doublewide Blackout Velvet Curtains</span><p>
                                                        Our double wide velvet blackout curtains offer wide widths for practical purposes. The curtains have high performance polyester velvet face fabric lined with exceptionally soft&nbsp; blackout lining offering complete blackout.
                                                    </p>
                                                </div>
                                                <div class="menu-desc-right">
                                                    <a href='/signature-doublewide-blackout-velvet-curtains.html'>
                                                        <img title="Signature Doublewide Blackout Velvet Curtains" alt="Signature Doublewide Blackout Velvet Curtains" src="https://www.halfpricedrapes.us/Resources/halfpricedraps/Category/icon/signature-doublewide-blackout-velvet-curtains_1732.jpg?304"></a>
                                                </div>
                                            </div>
                                            <div class="menu-desc" id="divsubcat35" style="display: none;">
                                                <div class="menu-desc-left">
                                                    <span>Signature Blackout Velvet Curtains</span><p>
                                                        The curtains have high performance polyester velvet face fabric and are lined with exceptionally soft blackout lining. The collection is offered in both Pole Pocket and Grommet headers at most attractive price.
                                                    </p>
                                                </div>
                                                <div class="menu-desc-right">
                                                    <a href='/signature-blackout-velvet-curtains.html'>
                                                        <img title="Signature Blackout Velvet Curtains" alt="Signature Blackout Velvet Curtains" src="https://www.halfpricedrapes.us/Resources/halfpricedraps/Category/icon/signature-blackout-velvet-curtains_1680.jpg?304"></a>
                                                </div>
                                            </div>
                                            <div class="menu-desc" id="divsubcat36" style="display: none;">
                                                <div class="menu-desc-left">
                                                    <span>Blackout Swatches</span><p>Half Price Drapes is pleased to introduce our NEW Swatch Program. As a valued customer we are offering Free 10 fabric swatches (Note: excluding USPS Priority Mail shipping charge)</p>
                                                </div>
                                                <div class="menu-desc-right">
                                                    <a href='/blackout-swatches.html'>
                                                        <img title="Blackout Swatches" alt="Blackout Swatches" src="https://www.halfpricedrapes.us/Resources/halfpricedraps/Category/icon/blackout-swatches_1733.jpg?304"></a>
                                                </div>
                                            </div>
                                            <div class="menu-desc" id="divsubcat37" style="display: none;">
                                                <div class="menu-desc-left">
                                                    <span>Shop By Blackout Curtains</span><p></p>
                                                </div>
                                                <div class="menu-desc-right">
                                                    <a href='/shop-by-blackout-curtains.html'>
                                                        <img title="Shop By Blackout Curtains" alt="Shop By Blackout Curtains" src="https://www.halfpricedrapes.us/Resources/halfpricedraps/Category/icon/shop-by-blackout-curtains_1746.jpg?304"></a>
                                                </div>
                                            </div>
                                        </li>
                                        <li id="lisub32"><a onmouseover="hideshowdiv(32,5);" href='/solid-blackout-curtains.html' title='Solid Blackout Curtains'>Solid Blackout Curtains<li id="lisub33"><a onmouseover="hideshowdiv(33,5);" href='/patterned-blackout-curtains.html' title='Patterned Blackout Curtains'>Patterned Blackout Curtains<li id="lisub34"><a onmouseover="hideshowdiv(34,5);" href='/signature-doublewide-blackout-velvet-curtains.html' title='Signature Doublewide Blackout Velvet Curtains'>Signature Doublewide Blackout Velvet Curtains<li id="lisub35"><a onmouseover="hideshowdiv(35,5);" href='/signature-blackout-velvet-curtains.html' title='Signature Blackout Velvet Curtains'>Signature Blackout Velvet Curtains<li id="lisub36"><a onmouseover="hideshowdiv(36,5);" href='/blackout-swatches.html' title='Blackout Swatches'>Blackout Swatches<li id="lisub37"><a onmouseover="hideshowdiv(37,5);" href='/shop-by-blackout-curtains.html' title='Shop By Blackout Curtains'>
                                        Shop By Blackout Curtains
                                    </ul>
                                </li>
                                <li id="menu_link" class="link-6"><a onmouseover='hideshowdiv(38,6);' href='/cotton-linen-curtains.html'>Cotton & Linen Curtains</a>
                                    <ul class="sub-menu" id="menu_detail6">
                                        <li id="lisub38"><a onmouseover="hideshowdiv(38,6);" href='/textured-faux-linen-curtains.html' title='Textured  Faux Linen Curtains'>Textured  Faux Linen Curtains</a><div class="menu-desc" id="divsubcat38" style="display: none;">
                                            <div class="menu-desc-left">
                                                <span>Textured  Faux Linen Curtains</span><p>
                                                    A wonderfully tactile, faux linen in an array of fresh, uplifting colors and popular neutrals. Soft to the touch, these Textured Faux Linen drapes beautiful and have an excellent durable quality.
                                                </p>
                                            </div>
                                            <div class="menu-desc-right">
                                                <a href='/textured-faux-linen-curtains.html'>
                                                    <img title="Textured  Faux Linen Curtains" alt="Textured  Faux Linen Curtains" src="https://www.halfpricedrapes.us/Resources/halfpricedraps/Category/icon/textured-linen-blend-curtains_1687.jpg?945"></a>
                                            </div>
                                        </div>
                                            <div class="menu-desc" id="divsubcat39" style="display: none;">
                                                <div class="menu-desc-left">
                                                    <span>Veranda Linen Blend Curtains</span><p>
                                                        Our Veranda Linen Blend Curtains are beautifully detailed. Coming in a variety of herringbone weaves and relaxed woven stripes with a vintage feel that drapes beautifully.
                                                    </p>
                                                </div>
                                                <div class="menu-desc-right">
                                                    <a href='/veranda-linen-blend-curtains.html'>
                                                        <img title="Veranda Linen Blend Curtains" alt="Veranda Linen Blend Curtains" src="https://www.halfpricedrapes.us/Resources/halfpricedraps/Category/icon/veranda-linen-blend-curtains_1723.jpg?945"></a>
                                                </div>
                                            </div>
                                            <div class="menu-desc" id="divsubcat40" style="display: none;">
                                                <div class="menu-desc-left">
                                                    <span>French Linen Curtains</span><p>
                                                        A simple, pure linen- plain with a soft natural texture weaved from finest French yarns. Our French linen curtains are available in neutral and modern shades.
                                                    </p>
                                                </div>
                                                <div class="menu-desc-right">
                                                    <a href='/french-linen-curtains.html'>
                                                        <img title="French Linen Curtains" alt="French Linen Curtains" src="https://www.halfpricedrapes.us/Resources/halfpricedraps/Category/icon/french-linen-curtains_1739.jpg?945"></a>
                                                </div>
                                            </div>
                                            <div class="menu-desc" id="divsubcat41" style="display: none;">
                                                <div class="menu-desc-left">
                                                    <span>Embroidered Cotton Crewel Curtains</span><p>
                                                        These subtle designs re-define antique embroideries for modern living. Our Embroidered Cotton Crewel Curtains represents relaxed character but with a couture finish to give a exceptionally decorative feel.
                                                    </p>
                                                </div>
                                                <div class="menu-desc-right">
                                                    <a href='/embroidered-cotton-crewel-curtains.html'>
                                                        <img title="Embroidered Cotton Crewel Curtains" alt="Embroidered Cotton Crewel Curtains" src="https://www.halfpricedrapes.us/Resources/halfpricedraps/Category/icon/embroidered-cotton-crewel-curtains_1686.jpg?945"></a>
                                                </div>
                                            </div>
                                            <div class="menu-desc" id="divsubcat42" style="display: none;">
                                                <div class="menu-desc-left">
                                                    <span>Printed Cotton Curtains</span><p>
                                                        An eclectic mix of decorative patterns from across the globe inspired this imaginative collection of stylized florals , Ikats, and&nbsp; geometric prints. Our Printed Cotton Twill collection features an abundance of colorful blooms printed on heavy 100% cotton twill for perfect drape.
                                                    </p>
                                                </div>
                                                <div class="menu-desc-right">
                                                    <a href='/printed-cotton-curtains.html'>
                                                        <img title="Printed Cotton Curtains" alt="Printed Cotton Curtains" src="https://www.halfpricedrapes.us/Resources/halfpricedraps/Category/icon/printed-cotton-curtains_1688.jpg?945"></a>
                                                </div>
                                            </div>
                                            <div class="menu-desc" id="divsubcat43" style="display: none;">
                                                <div class="menu-desc-left">
                                                    <span>Hand Woven Cotton Stripe Curtains</span><p>
                                                        Our hand woven cotton stripe curtains are a collection of simple, versatile stripes, incorporating a fresh mix of broad, narrow and multi striped designs. All in contemporary color combinations woven in a natural cotton and cotton mix yarns.
                                                    </p>
                                                </div>
                                                <div class="menu-desc-right">
                                                    <a href='/hand-woven-cotton-stripe-curtains.html'>
                                                        <img title="Hand Woven Cotton Stripe Curtains" alt="Hand Woven Cotton Stripe Curtains" src="https://www.halfpricedrapes.us/Resources/halfpricedraps/Category/icon/hand-woven-cotton-stripe-curtains_1689.jpg?945"></a>
                                                </div>
                                            </div>
                                            <div class="menu-desc" id="divsubcat44" style="display: none;">
                                                <div class="menu-desc-left">
                                                    <span>Linen & Cotton Swatches</span><p>Half Price Drapes is pleased to introduce our NEW Swatch Program. As a valued customer we are offering Free 10 fabric swatches (Note: excluding USPS Priority Mail shipping charge)</p>
                                                </div>
                                                <div class="menu-desc-right">
                                                    <a href='/linen-cotton-swatches.html'>
                                                        <img title="Linen &amp; Cotton Swatches" alt="Linen &amp; Cotton Swatches" src="https://www.halfpricedrapes.us/Resources/halfpricedraps/Category/icon/linen-cotton-swatches_1690.jpg?945"></a>
                                                </div>
                                            </div>
                                            <div class="menu-desc" id="divsubcat45" style="display: none;">
                                                <div class="menu-desc-left">
                                                    <span>Shop All Cotton & Linen Curtains</span><p></p>
                                                </div>
                                                <div class="menu-desc-right">
                                                    <a href='/shop-all-cotton-linen-curtains.html'>
                                                        <img title="Shop All Cotton &amp; Linen Curtains" alt="Shop All Cotton &amp; Linen Curtains" src="https://www.halfpricedrapes.us/Resources/halfpricedraps/Category/icon/shop-by-cotton-linen-curtains_1747.jpg?945"></a>
                                                </div>
                                            </div>
                                        </li>
                                        <li id="lisub39"><a onmouseover="hideshowdiv(39,6);" href='/veranda-linen-blend-curtains.html' title='Veranda Linen Blend Curtains'>Veranda Linen Blend Curtains<li id="lisub40"><a onmouseover="hideshowdiv(40,6);" href='/french-linen-curtains.html' title='French Linen Curtains'>French Linen Curtains<li id="lisub41"><a onmouseover="hideshowdiv(41,6);" href='/embroidered-cotton-crewel-curtains.html' title='Embroidered Cotton Crewel Curtains'>Embroidered Cotton Crewel Curtains<li id="lisub42"><a onmouseover="hideshowdiv(42,6);" href='/printed-cotton-curtains.html' title='Printed Cotton Curtains'>Printed Cotton Curtains<li id="lisub43"><a onmouseover="hideshowdiv(43,6);" href='/hand-woven-cotton-stripe-curtains.html' title='Hand Woven Cotton Stripe Curtains'>Hand Woven Cotton Stripe Curtains<li id="lisub44"><a onmouseover="hideshowdiv(44,6);" href='/linen-cotton-swatches.html' title='Linen & Cotton Swatches'>Linen & Cotton Swatches<li id="lisub45"><a onmouseover="hideshowdiv(45,6);" href='/shop-all-cotton-linen-curtains.html' title='Shop All Cotton & Linen Curtains'>
                                        Shop All Cotton & Linen Curtains
                                    </ul>
                                </li>
                                <li id="menu_link" class="link-7"><a onmouseover='hideshowdiv(46,7);' href='/sheer-curtains.html'>Sheer Curtains</a>
                                    <ul class="sub-menu" id="menu_detail7">
                                        <li id="lisub46"><a onmouseover="hideshowdiv(46,7);" href='/signature-double-layered-voile-sheers.html' title='Signature Double Layered Voile Sheers'>Signature Double Layered Voile Sheers</a><div class="menu-desc" id="divsubcat46" style="display: none;">
                                            <div class="menu-desc-left">
                                                <span>Signature Double Layered Voile Sheers</span><p></p>
                                            </div>
                                            <div class="menu-desc-right">
                                                <a href='/signature-double-layered-voile-sheers.html'>
                                                    <img title="Signature Double Layered Voile Sheers" alt="Signature Double Layered Voile Sheers" src="https://www.halfpricedrapes.us/Resources/halfpricedraps/Category/icon/signature-double-layered-voile-sheers_1741.jpg?945"></a>
                                            </div>
                                        </div>
                                            <div class="menu-desc" id="divsubcat47" style="display: none;">
                                                <div class="menu-desc-left">
                                                    <span>Stripe Sheers</span><p>
                                                        Our stripe sheers are softly translucent in texture. The collection is presented in light natural tones and stylish accent colors.
                                                    </p>
                                                </div>
                                                <div class="menu-desc-right">
                                                    <a href='/stripe-sheers.html'>
                                                        <img title="Stripe Sheers" alt="Stripe Sheers" src="https://www.halfpricedrapes.us/Resources/halfpricedraps/Category/icon/stripe-sheers_1695.jpg?945"></a>
                                                </div>
                                            </div>
                                            <div class="menu-desc" id="divsubcat48" style="display: none;">
                                                <div class="menu-desc-left">
                                                    <span>Pattern & Embroidered Sheers</span><p>
                                                        A distinctively sophisticated collection of embroideries, damask and burnout on voile and organza fabrics. Our Pattern &amp; Embroidered sheers offer a large selection of both traditional and modern designs.
                                                    </p>
                                                </div>
                                                <div class="menu-desc-right">
                                                    <a href='/pattern-embroidered-sheers.html'>
                                                        <img title="Pattern &amp; Embroidered Sheers" alt="Pattern &amp; Embroidered Sheers" src="https://www.halfpricedrapes.us/Resources/halfpricedraps/Category/icon/pattern-embroidered-sheers_1696.jpg?945"></a>
                                                </div>
                                            </div>
                                            <div class="menu-desc" id="divsubcat49" style="display: none;">
                                                <div class="menu-desc-left">
                                                    <span>Doublewide Voile Sheer Curtains</span><p>
                                                        Double wide-width sheers with subtle voile texture&nbsp; presented in neutral tones.
                                                    </p>
                                                </div>
                                                <div class="menu-desc-right">
                                                    <a href='/doublewide-voile-sheer-curtains.html'>
                                                        <img title="Doublewide Voile Sheer Curtains" alt="Doublewide Voile Sheer Curtains" src="https://www.halfpricedrapes.us/Resources/halfpricedraps/Category/icon/doublewide-voile-sheer-curtains_1728.jpg?945"></a>
                                                </div>
                                            </div>
                                            <div class="menu-desc" id="divsubcat50" style="display: none;">
                                                <div class="menu-desc-left">
                                                    <span>Voile & Organza Sheers</span><p>
                                                        Simple and elegant voile and organza sheers for all homes- sold by the pair!
                                                    </p>
                                                </div>
                                                <div class="menu-desc-right">
                                                    <a href='/voile-organza-sheers.html'>
                                                        <img title="Voile &amp; Organza Sheers" alt="Voile &amp; Organza Sheers" src="https://www.halfpricedrapes.us/Resources/halfpricedraps/Category/icon/voile-organza-sheers_1693.jpg?945"></a>
                                                </div>
                                            </div>
                                            <div class="menu-desc" id="divsubcat51" style="display: none;">
                                                <div class="menu-desc-left">
                                                    <span>Faux Linen Sheers</span><p>
                                                        A sophisticated muted palette is given to this selection of faux linen sheers. Shades of whites and creams&nbsp; &amp; soft greys. The supple natural quality of faux&nbsp; linen combined with a soft infusion of light evokes a sense of simple luxury.
                                                    </p>
                                                </div>
                                                <div class="menu-desc-right">
                                                    <a href='/faux-linen-sheers.html'>
                                                        <img title="Faux Linen Sheers" alt="Faux Linen Sheers" src="https://www.halfpricedrapes.us/Resources/halfpricedraps/Category/icon/faux-linen-sheers_1691.jpg?945"></a>
                                                </div>
                                            </div>
                                            <div class="menu-desc" id="divsubcat52" style="display: none;">
                                                <div class="menu-desc-left">
                                                    <span>Linen Sheers</span><p>
                                                        Our Linen Sheers are&nbsp; hazy, translucent sheer and are presented in a concise palette of subtle neutrals.
                                                    </p>
                                                </div>
                                                <div class="menu-desc-right">
                                                    <a href='/linen-sheers.html'>
                                                        <img title="Linen Sheers" alt="Linen Sheers" src="https://www.halfpricedrapes.us/Resources/halfpricedraps/Category/icon/linen-sheers_1692.jpg?945"></a>
                                                </div>
                                            </div>
                                            <div class="menu-desc" id="divsubcat53" style="display: none;">
                                                <div class="menu-desc-left">
                                                    <span>Silk Organza Sheers</span><p></p>
                                                </div>
                                                <div class="menu-desc-right">
                                                    <a href='/silk-organza-sheers.html'>
                                                        <img title="Silk Organza Sheers" alt="Silk Organza Sheers" src="https://www.halfpricedrapes.us/Resources/halfpricedraps/Category/icon/silk-organza-sheers_1694.jpg?945"></a>
                                                </div>
                                            </div>
                                            <div class="menu-desc" id="divsubcat54" style="display: none;">
                                                <div class="menu-desc-left">
                                                    <span>Sheer Swatches</span><p>Half Price Drapes is pleased to introduce our NEW Swatch Program. As a valued customer we are offering Free 10 fabric swatches (Note: excluding USPS Priority Mail shipping charge)</p>
                                                </div>
                                                <div class="menu-desc-right">
                                                    <a href='/sheer-swatches.html'>
                                                        <img title="Sheer Swatches" alt="Sheer Swatches" src="https://www.halfpricedrapes.us/Resources/halfpricedraps/Category/icon/sheer-swatches_1698.jpg?945"></a>
                                                </div>
                                            </div>
                                            <div class="menu-desc" id="divsubcat55" style="display: none;">
                                                <div class="menu-desc-left">
                                                    <span>Shop All Sheer Curtains</span><p></p>
                                                </div>
                                                <div class="menu-desc-right">
                                                    <a href='/shop-all-sheer-curtains.html'>
                                                        <img title="Shop All Sheer Curtains" alt="Shop All Sheer Curtains" src="https://www.halfpricedrapes.us/Resources/halfpricedraps/Category/icon/shop-by-sheer-curtains_1748.jpg?945"></a>
                                                </div>
                                            </div>
                                        </li>
                                        <li id="lisub47"><a onmouseover="hideshowdiv(47,7);" href='/stripe-sheers.html' title='Stripe Sheers'>Stripe Sheers<li id="lisub48"><a onmouseover="hideshowdiv(48,7);" href='/pattern-embroidered-sheers.html' title='Pattern & Embroidered Sheers'>Pattern & Embroidered Sheers<li id="lisub49"><a onmouseover="hideshowdiv(49,7);" href='/doublewide-voile-sheer-curtains.html' title='Doublewide Voile Sheer Curtains'>Doublewide Voile Sheer Curtains<li id="lisub50"><a onmouseover="hideshowdiv(50,7);" href='/voile-organza-sheers.html' title='Voile & Organza Sheers'>Voile & Organza Sheers<li id="lisub51"><a onmouseover="hideshowdiv(51,7);" href='/faux-linen-sheers.html' title='Faux Linen Sheers'>Faux Linen Sheers<li id="lisub52"><a onmouseover="hideshowdiv(52,7);" href='/linen-sheers.html' title='Linen Sheers'>Linen Sheers<li id="lisub53"><a onmouseover="hideshowdiv(53,7);" href='/silk-organza-sheers.html' title='Silk Organza Sheers'>Silk Organza Sheers<li id="lisub54"><a onmouseover="hideshowdiv(54,7);" href='/sheer-swatches.html' title='Sheer Swatches'>Sheer Swatches<li id="lisub55"><a onmouseover="hideshowdiv(55,7);" href='/shop-all-sheer-curtains.html' title='Shop All Sheer Curtains'>
                                        Shop All Sheer Curtains
                                    </ul>
                                </li>
                                <li id="menu_link" class="link-8"><a onmouseover='hideshowdiv(56,8);' href='/curtain-hardware.html'>Curtain Hardware</a>
                                    <ul class="sub-menu" id="menu_detail8">
                                        <li id="lisub56"><a onmouseover="hideshowdiv(56,8);" href='/sun-bleached-linen-wooden-hardware.html' title='Sun Bleached Linen Wooden Hardware'>Sun Bleached Linen Wooden Hardware</a><div class="menu-desc" id="divsubcat56" style="display: none;">
                                            <div class="menu-desc-left">
                                                <span>Sun Bleached Linen Wooden Hardware</span><p></p>
                                            </div>
                                            <div class="menu-desc-right">
                                                <a href='/sun-bleached-linen-wooden-hardware.html'>
                                                    <img title="Sun Bleached Linen Wooden Hardware" alt="Sun Bleached Linen Wooden Hardware" src="https://www.halfpricedrapes.us/Resources/halfpricedraps/Category/icon/sun-bleached-linen-wooden-hardware_1701.jpg?109"></a>
                                            </div>
                                        </div>
                                            <div class="menu-desc" id="divsubcat57" style="display: none;">
                                                <div class="menu-desc-left">
                                                    <span>English Walnut Wooden Hardware</span><p></p>
                                                </div>
                                                <div class="menu-desc-right">
                                                    <a href='/english-walnut-wooden-hardware.html'>
                                                        <img title="English Walnut Wooden Hardware" alt="English Walnut Wooden Hardware" src="https://www.halfpricedrapes.us/Resources/halfpricedraps/Category/icon/english-walnut-wooden-hardware_1702.jpg?109"></a>
                                                </div>
                                            </div>
                                            <div class="menu-desc" id="divsubcat58" style="display: none;">
                                                <div class="menu-desc-left">
                                                    <span>Cristal Wooden Hardware</span><p></p>
                                                </div>
                                                <div class="menu-desc-right">
                                                    <a href='/cristal-wooden-hardware.html'>
                                                        <img title="Cristal Wooden Hardware" alt="Cristal Wooden Hardware" src="https://www.halfpricedrapes.us/Resources/halfpricedraps/Category/icon/cristal-wooden-hardware_1703.jpg?109"></a>
                                                </div>
                                            </div>
                                            <div class="menu-desc" id="divsubcat59" style="display: none;">
                                                <div class="menu-desc-left">
                                                    <span>Antq. Bronze Wooden Hardware</span><p></p>
                                                </div>
                                                <div class="menu-desc-right">
                                                    <a href='/antq-bronze-wooden-hardware.html'>
                                                        <img title="Antq. Bronze Wooden Hardware" alt="Antq. Bronze Wooden Hardware" src="https://www.halfpricedrapes.us/Resources/halfpricedraps/Category/icon/antq-bronze-wooden-hardware_1704.jpg?109"></a>
                                                </div>
                                            </div>
                                            <div class="menu-desc" id="divsubcat60" style="display: none;">
                                                <div class="menu-desc-left">
                                                    <span>Historical Gold Wooden Hardware</span><p></p>
                                                </div>
                                                <div class="menu-desc-right">
                                                    <a href='/historical-gold-wooden-hardware.html'>
                                                        <img title="Historical Gold Wooden Hardware" alt="Historical Gold Wooden Hardware" src="https://www.halfpricedrapes.us/Resources/halfpricedraps/Category/icon/historical-gold-wooden-hardware_1705.jpg?109"></a>
                                                </div>
                                            </div>
                                            <div class="menu-desc" id="divsubcat61" style="display: none;">
                                                <div class="menu-desc-left">
                                                    <span>Tiebacks, Finials & Open Stock Hardware</span><p></p>
                                                </div>
                                                <div class="menu-desc-right">
                                                    <a href='/tiebacks-finials-open-stock-hardware.html'>
                                                        <img title="Tiebacks, Finials &amp; Open Stock Hardware" alt="Tiebacks, Finials &amp; Open Stock Hardware" src="https://www.halfpricedrapes.us/Resources/halfpricedraps/Category/icon/tiebacks-finials-open-stock-hardware_1706.jpg?109"></a>
                                                </div>
                                            </div>
                                        </li>
                                        <li id="lisub57"><a onmouseover="hideshowdiv(57,8);" href='/english-walnut-wooden-hardware.html' title='English Walnut Wooden Hardware'>English Walnut Wooden Hardware<li id="lisub58"><a onmouseover="hideshowdiv(58,8);" href='/cristal-wooden-hardware.html' title='Cristal Wooden Hardware'>Cristal Wooden Hardware<li id="lisub59"><a onmouseover="hideshowdiv(59,8);" href='/antq-bronze-wooden-hardware.html' title='Antq. Bronze Wooden Hardware'>Antq. Bronze Wooden Hardware<li id="lisub60"><a onmouseover="hideshowdiv(60,8);" href='/historical-gold-wooden-hardware.html' title='Historical Gold Wooden Hardware'>Historical Gold Wooden Hardware<li id="lisub61"><a onmouseover="hideshowdiv(61,8);" href='/tiebacks-finials-open-stock-hardware.html' title='Tiebacks, Finials & Open Stock Hardware'>
                                        Tiebacks, Finials & Open Stock Hardware
                                    </ul>
                                </li>
                                <li id="menu_link" class="link-9"><a onmouseover='hideshowdiv(62,9);' href='/roman-shades.html'>Roman Shades</a>
                                    <ul class="sub-menu" id="menu_detail9">
                                        <li id="lisub62"><a onmouseover="hideshowdiv(62,9);" href='/textured-dupioni-silk-roman-shade.html' title='Textured Dupioni Silk Roman Shades'>Textured Dupioni Silk Roman Shades</a><div class="menu-desc" id="divsubcat62" style="display: none;">
                                            <div class="menu-desc-left">
                                                <span>Textured Dupioni Silk Roman Shades</span><p></p>
                                            </div>
                                            <div class="menu-desc-right">
                                                <a href='/textured-dupioni-silk-roman-shade.html'>
                                                    <img title="Textured Dupioni Silk Roman Shades" alt="Textured Dupioni Silk Roman Shades" src="https://www.halfpricedrapes.us/Resources/halfpricedraps/Category/icon/textured-dupioni-silk-roman-shades_1712.jpg?109"></a>
                                            </div>
                                        </div>
                                            <div class="menu-desc" id="divsubcat63" style="display: none;">
                                                <div class="menu-desc-left">
                                                    <span>Thai Silk Shades</span><p></p>
                                                </div>
                                                <div class="menu-desc-right">
                                                    <a href='/thai-silk-roman-shade.html'>
                                                        <img title="Thai Silk Shades" alt="Thai Silk Shades" src="https://www.halfpricedrapes.us/Resources/halfpricedraps/Category/icon/thai-silk-shades_1713.jpg?750"></a>
                                                </div>
                                            </div>
                                            <div class="menu-desc" id="divsubcat64" style="display: none;">
                                                <div class="menu-desc-left">
                                                    <span>Vintage Faux Dupioni Silk Roman Shades</span><p></p>
                                                </div>
                                                <div class="menu-desc-right">
                                                    <a href='/faux-dupioni-silk-roman-shade.html'>
                                                        <img title="Vintage Faux Dupioni Silk Roman Shades" alt="Vintage Faux Dupioni Silk Roman Shades" src="https://www.halfpricedrapes.us/Resources/halfpricedraps/Category/icon/vintage-faux-dupioni-silk-roman-shades_1714.jpg?750"></a>
                                                </div>
                                            </div>
                                            <div class="menu-desc" id="divsubcat65" style="display: none;">
                                                <div class="menu-desc-left">
                                                    <span>Designer Silk Roman Shades</span><p></p>
                                                </div>
                                                <div class="menu-desc-right">
                                                    <a href='/designer-silk-roman-shade.html'>
                                                        <img title="Designer Silk Roman Shades" alt="Designer Silk Roman Shades" src="https://www.halfpricedrapes.us/Resources/halfpricedraps/Category/icon/designer-silk-roman-shades_1715.jpg?750"></a>
                                                </div>
                                            </div>
                                            <div class="menu-desc" id="divsubcat66" style="display: none;">
                                                <div class="menu-desc-left">
                                                    <span>Faux Silk Taffeta Stripe Roman Shades</span><p></p>
                                                </div>
                                                <div class="menu-desc-right">
                                                    <a href='/striped-faux-silk-taffeta-roman-shade.html'>
                                                        <img title="Faux Silk Taffeta Stripe Roman Shades" alt="Faux Silk Taffeta Stripe Roman Shades" src="https://www.halfpricedrapes.us/Resources/halfpricedraps/Category/icon/faux-silk-taffeta-stripe-roman-shades_1716.jpg?750"></a>
                                                </div>
                                            </div>
                                            <div class="menu-desc" id="divsubcat67" style="display: none;">
                                                <div class="menu-desc-left">
                                                    <span>Velvet Blackout Roman Shades</span><p></p>
                                                </div>
                                                <div class="menu-desc-right">
                                                    <a href='/signature-blackout-velvet-roman-shade.html'>
                                                        <img title="Velvet Blackout Roman Shades" alt="Velvet Blackout Roman Shades" src="https://www.halfpricedrapes.us/Resources/halfpricedraps/Category/icon/velvet-blackout-roman-shades-_1717.jpg?750"></a>
                                                </div>
                                            </div>
                                            <div class="menu-desc" id="divsubcat68" style="display: none;">
                                                <div class="menu-desc-left">
                                                    <span>Hand Weaved Fabric Roman Shades</span><p></p>
                                                </div>
                                                <div class="menu-desc-right">
                                                    <a href='/hand-weave-cotton-roman-shades.html'>
                                                        <img title="Hand Weaved Fabric Roman Shades" alt="Hand Weaved Fabric Roman Shades" src="https://www.halfpricedrapes.us/Resources/halfpricedraps/Category/icon/hand-weaved-fabric-roman-shades_1718.jpg?750"></a>
                                                </div>
                                            </div>
                                        </li>
                                        <li id="lisub63"><a onmouseover="hideshowdiv(63,9);" href='/thai-silk-roman-shade.html' title='Thai Silk Shades'>Thai Silk Shades<li id="lisub64"><a onmouseover="hideshowdiv(64,9);" href='/faux-dupioni-silk-roman-shade.html' title='Vintage Faux Dupioni Silk Roman Shades'>Vintage Faux Dupioni Silk Roman Shades<li id="lisub65"><a onmouseover="hideshowdiv(65,9);" href='/designer-silk-roman-shade.html' title='Designer Silk Roman Shades'>Designer Silk Roman Shades<li id="lisub66"><a onmouseover="hideshowdiv(66,9);" href='/striped-faux-silk-taffeta-roman-shade.html' title='Faux Silk Taffeta Stripe Roman Shades'>Faux Silk Taffeta Stripe Roman Shades<li id="lisub67"><a onmouseover="hideshowdiv(67,9);" href='/signature-blackout-velvet-roman-shade.html' title='Velvet Blackout Roman Shades'>Velvet Blackout Roman Shades<li id="lisub68"><a onmouseover="hideshowdiv(68,9);" href='/hand-weave-cotton-roman-shades.html' title='Hand Weaved Fabric Roman Shades'>
                                        Hand Weaved Fabric Roman Shades
                                    </ul>
                                </li>
                                <li id="menu_link" class="link-10"><a onmouseover='hideshowdiv(69,10);' href='/accents-accessories.html'>Accents & Accessories</a></li>
                            </ul>
                            <a class="qmparent link-11" href="/SalesOutlet.aspx">
                                <img border="0" src="/images/sales-outlet.png" alt="Sales Outlet" title="Sales Outlet" style="float: right;"></a>
                            <script type="text/javascript">qm_create(0, false, 0, 250, 0, false, false, false);</script>
                        </div>
                    </div>

                    <div class="header-banner">

                        <div class="header-banner" id="rotatebannertext">
                            <p>
                                <strong><span style="font-family: ">Free Shipping</span></strong> to U.S. on orders over $249
                            </p>
                        </div>
                        <div id="rotatebannertextall" style="display: none;">
                            <div id="divheder0" style="display: none;">
                                <p>
                                    <strong><span style="font-family: ">Free Shipping</span></strong> to U.S. on orders over $249
                                </p>
                            </div>
                        </div>

                    </div>
                </div>

                <div id="footer-part">
                    <div class="footer-row2" id="divhalfpricedrapes">
                        <div class="footer-row2-title">
                            <p>
                                Half Price <span>Drapes</span>
                            </p>
                        </div>
                        <div class="footer-row2-bg">
                            <p>
                                Draperies and curtains are the soul of our business and with that kind of passion for fabric, Half Price Drapes has been specializing in designer quality window treatments for years. Our talented staff is adept at working closely with our clients to deliver the best in window treatment products such as velvet drapes and linen curtains to name but a few of our popular pieces.
                                <br />
                                <br />
                                We at Half Price Drapes have made a commitment to providing you with the highest quality draperies at the lowest price available. Many people interested in redecorating their homes assume that high-end luxury must come at a high price. At Half Price Drapes, we pass the savings on to our customers and offer you a consistent bargain no matter what type of curtains you're looking for. We invite you to visit our design studio located in the California's Bay Area to explore our selection of draperies offered in a variety of fabrics- taffeta silk curtains, linen, velvet and sheer drapes to name a few. For the rest of our loyal customers across the globe, our full web site will provide a window into our design possibilities.
                                <br />
                                <br />
                                We maintain a helpful and knowledgeable staff available to assist you in all your drapery needs. Half Price Drapes is proud of its long history in the industry and looks forward to making your home design dreams a reality. Call us today to work with our design professionals or check out our References section.
                            </p>
                        </div>
                    </div>
                   

                   
                    
                </div>









               
                
                <div id="divtradepartner" style="z-index: 1000001; top: 100px; width: 1010px; height: 500px; background: none repeat scroll 0px 0px rgba(3, 3, 3, 0.3) ! important; padding: 10px ! important; border: medium none ! important; position: fixed; bottom: 5px; left: 305px; display: none;">
                    <div style="float: right; background-color: transparent; right: -15px; top: -18px; position: absolute;">
                        <a href="javascript:void(0);" onclick="javascript:document.getElementById('divtradepartner').style.display = 'none';document.getElementById('backgroundfree').style.display = 'none';document.getElementById('litradepartner').className = '';"
                            title="">
                            <img src="/images/popupclose.png" alt=""></a>
                    </div>
                    <div style="height: 500px; display: block; width: 100%; background: none repeat scroll 0% 0% rgb(255, 255, 255);">
                        <div class="title" style="text-align: left; font-weight: bold; height: 490px; padding: 5px; overflow: auto;">
                            <span style="font-size: 16px;">Trade Partner Program</span><div style="color: rgb(57, 57, 57);">
                                <br />
                                <div class="static_content">
                                    <h2>TRADE SALES</h2>
                                    <p>
                                        <strong>YOUR INDUSTRY PARTNER</strong>
                                    </p>
                                    <p>
                                        If you are a design professional we encourage you to join our Half Price Drapes-Trade Membership Program. We're here to support your business and foster your success as a designer. As a Half Price Drapes-Trade Member, you'll enjoy a generous discount on all of our non-sale merchandise. Once you have filled out all appropriate forms and your application has been processed, you will receive a welcome packet listing all of your new benefits and discounts.
                                    </p>
                                    <p>
                                        Half Price Drapes manufacturers ready-made curtains & custom draperies and has been operating in the Bay Area since 2004. What makes us unique is that we weave our own fabric, create our own embroidery patterns, and manufacture our own curtains & drapes. We have a 10,000 square foot workshop where a dedicated staff manufactures high-quality, standardized, and custom curtains & drapes. Hence, we can eliminate all mark-ups and provide high- end luxurious customized curtains & drapes at a cost that is unmatched. That is why we are proud to call ourselves Half Price Drapes. Our beautiful fabric can be tailored in many different header styles. That's not all, if you have a specific design in mind, let us know and we will work with you and your clients to customize the fabric to your specifications.
                                    </p>
                                    <p>
                                        <strong>NOT YET A MEMBER?</strong>
                                    </p>
                                    <p>
                                        Applying for membership is easy, just fill out our simple <a href="/OnlineApplicationForm.aspx" target="_blank">Online Application.</a> Once your electronic application has been submitted with valid documentation (see list below) you will receive an email indicating that your application is being reviewed. If the supporting documentation does not meet the requirements, your application will be cancelled.
                                    </p>
                                    <p>
                                        At least one of the following is required:
                                    </p>
                                    <ul>
                                        <li>Current Business or State Professional License in a Residential or Commercial Design-based business, or the Hospitality industry</li>
                                        <li>Proof of current AI or IDI provincial registration</li>
                                        <li>Business ID number</li>
                                        <li>Proof of current ASID membership</li>
                                        <li>Interior design certification (e.g. NCIDQ, CCIDC)</li>
                                        <li>W9, Federal ID form, or EIN number</li>
                                        <li>Resale or Sales Tax Certificate</li>
                                    </ul>
                                    <p>
                                        If you intend to purchase merchandise for resale, you will be required to supply a Resale or Sales Tax Certificate. Without this documentation, sales tax will be applied to all orders. Trade Discounts cannot be applied to previous purchases or applied after an online order has been processed.
                                    </p>
                                    <p>
                                        For membership information and assistance, contact us at:<br />
                                        Phone 1.866.413.7273<br />
                                        Fax 925.455.5504<br />
                                        Email <a href="mailto:admin@halfpricedrapes.com">admin@halfpricedrapes.com</a>
                                    </p>
                                    <p>
                                        <strong>PLACING TRADE ORDERS OR REQUESTING A CUSTOM QUOTE IS SIMPLE</strong>
                                    </p>
                                    <p>
                                        Our Trade Team experts offer dedicated support to ensure you have everything you need. To ensure we have all your correct order details, please download and complete a Trade Order Form. Trade orders can be placed by calling our Half Price Drapes Trade Team at 1.866.413.7273 or by faxing the <a href="http://site.halfpricedrapes.com/new-hpd/Trade_OrderForm.pdf" target="_blank">Trade Order Form</a> back to us at 925.455.5504 or emailing the form to us at: <a href="mailto:admin@halfpricedrapes.com">admin@halfpricedrapes.com</a>.
                                    </p>
                                    <p>
                                        <a href="/images/pdf/Trade_OrderForm.pdf" target="_blank">Click here to download a Trade Order Form.</a>
                                    </p>
                                    <p>
                                        <a href="/images/pdf/Trade_CCauthorization.pdf" target="_blank">Click here to download a Credit Card Authorization Form.</a>
                                    </p>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div id="backgroundfree" style="z-index: 1000; display: none; position: fixed; _position: absolute; height: 100%; width: 100%; top: 0; left: 0; background: #000000; border: 1px solid #cecece; opacity: 0.5; filter: alpha(opacity=50);">
                </div>

                <div id="popupContactpricequote1" style="z-index: 1000001; top: 30px; padding: 0px; width: 1440px; background: #fff;">
                    <div style='float: right; background-color: transparent; right: -15px; top: -18px; position: absolute;'>
                        <a href="javascript:void(0);" onclick="javascript:disablePopupmaster();document.getElementById('frmsearch').src = '/SearchControl.aspx';" title="">
                            <img src="/images/popupclose.png" alt="" />
                        </a>
                    </div>
                    <div style="background: none repeat scroll 0 0 white; width: 100%">
                        <table border="0" cellspacing="0" cellpadding="0" width="100%">
                            <tr>
                                <td align="left">
                                    <div id="diviframesearch" style="display: none;">
                                        <%--<iframe id="frmsearch" src="/SearchControl.aspx" width="1440px" height="296px" scrolling="no" frameborder="0"></iframe>--%>
                                    </div>
                                    <div id="diviframecontactus" style="display: none"></div>
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
            </div>


            
             



            




            <script type="text/javascript">
                //<![CDATA[
                $(document).ready(function () { setupRotator(); }); $('#divhalfpricedrapes').css('display', 'none'); WebForm_AutoFocus('ContentPlaceHolder1_txtusername');//]]>
            </script>
    </form>
</body>
</html>
