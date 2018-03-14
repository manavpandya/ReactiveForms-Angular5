<%@ Page Title="" Language="C#" MasterPageFile="~/ADMIN/Admin.Master" AutoEventWireup="true" CodeBehind="MultipleShippingSection.aspx.cs" Inherits="Solution.UI.Web.ADMIN.Orders.MultipleShippingSection" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../JS/jquery-1.3.2.js" type="text/javascript"></script>
    <script type="text/javascript" src="/js/popup.js"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts-main.js"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts.js"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-calender-custom.js"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-calender.js"></script>

    <%--<link href="/css/general.css" rel="stylesheet" type="text/css" />--%>
    <script type="text/javascript">
        function PrintBarcode(fname) {

            //document.getElementById('ifmcontentstoprint').src = fname;
            // document.getElementById('ifmcontentstoprint').focus();
            //  document.getElementById('ifmcontentstoprint').print();
            //printpdffile();
            w = window.open(fname, 'msg', 'directories=no, location=no, menubar=no, status=yes,titlebar=no,toolbar=no,scrollbars=no,resizable=no,Height=10,Width=10,left=0,top=0,visible=false,alwaysLowered=yes');

            w.print();
            w.close();

        }

        function printpdffile() {
            //document.getElementById('ifmcontentstoprint').focus();
            // document.getElementById('ifmcontentstoprint').print();
            //var PDF = document.getElementById('ifmcontentstoprint');
            // PDF.focus();
            // PDF.contentWindow.print();
        }
        function OpenCenterWindow(pid, wi, he) {
            var w = wi;
            var h = he;
            var left = (screen.width / 2) - (w / 2);
            var top = (screen.height / 2) - (h / 2);
            window.open(pid, '', 'toolbar=no, location=no, directories=no, status=no, menubar=no, scrollbars=yes, resizable=no, copyhistory=no, width=' + w + ', height=' + h + ', top=' + top + ', left=' + left);
        }

        $(function () {

            $('#ContentPlaceHolder1_txtMailFrom').datetimepicker({
                showButtonPanel: true, ampm: false,
                showHour: false, showMinute: false, showSecond: false, showTime: false, showOn: "button",
                buttonImage: "/App_Themes/<%=Page.Theme %>/images/date-icon.png", buttonImageOnly: true
            });
            $('#ContentPlaceHolder1_txtMailTo').datetimepicker({
                showButtonPanel: true, ampm: false,
                showHour: false, showMinute: false, showSecond: false, showTime: false, showOn: "button",
                buttonImage: "/App_Themes/<%=Page.Theme %>/images/date-icon.png", buttonImageOnly: true
            });
        });

        function SearchValidation(id) {

            if (id == 0) {
                //                if (document.getElementById('ContentPlaceHolder1_ddlSearch').selectedIndex == 0) {
                //                    jAlert('Please Select Search By.', 'Required Information', 'ContentPlaceHolder1_ddlSearch');
                //                    //document.getElementById('ContentPlaceHolder1_ddlSearch').focus();
                //                    return false;
                //                }
                //                if (document.getElementById('ContentPlaceHolder1_txtSearch').value == '') {
                //                    jAlert('Please Enter Search Keyword.', 'Required Information', 'ContentPlaceHolder1_txtSearch');
                //                    //document.getElementById('ContentPlaceHolder1_txtSearch').focus();
                //                    return false;
                //                }
                //                else if (document.getElementById('ContentPlaceHolder1_txtSearch').value != '' && (document.getElementById('ContentPlaceHolder1_txtSearch').value.toString().toLowerCase() == 'search keyword' || document.getElementById('ContentPlaceHolder1_txtSearch').value.toString().toLowerCase() == 'searchkeyword')) {
                //                    jAlert('Please Enter Search Keyword.', 'Required Information', 'ContentPlaceHolder1_txtSearch');
                //                    //document.getElementById('ContentPlaceHolder1_txtSearch').focus();
                //                    return false;
                //                }

            }
            if (document.getElementById('ContentPlaceHolder1_txtMailFrom').value == '') {
                jAlert('Please Enter From Date.', 'Required Information', 'ContentPlaceHolder1_txtMailFrom');
                //document.getElementById('ContentPlaceHolder1_txtMailFrom').focus();
                return false;
            }
            else if (document.getElementById('ContentPlaceHolder1_txtMailTo').value == '') {
                jAlert('Please Enter To Date.', 'Required Information', 'ContentPlaceHolder1_txtMailTo');
                // document.getElementById('ContentPlaceHolder1_txtMailTo').focus();
                return false;
            }

            var startDate = new Date(document.getElementById('ContentPlaceHolder1_txtMailFrom').value);
            var endDate = new Date(document.getElementById('ContentPlaceHolder1_txtMailTo').value);
            if (startDate > endDate) {
                jAlert('Please Select Valid Date.', 'Required Information', 'ContentPlaceHolder1_txtMailTo');
                return false;
            }
            return true;
        }


        var XmlHttp;
        function HandleResponseforInsertProduct() {
            if (XmlHttp.readyState == 4) {
                if (XmlHttp.status == 200) {
                    var result = XmlHttp.responseText;
                    var BrowserName = navigator.appName.toString();
                    if (BrowserName.toString().toLowerCase().indexOf('internet explorer') > -1) {
                        w = window.open('', 'msg', 'directories=no, location=no, menubar=no, status=yes,titlebar=no,toolbar=no,scrollbars=no,resizable=no,Height=10,Width=10,left=0,top=0,visible=false,alwaysLowered=yes');
                        w.document.write(result);
                        w.document.close();
                        w.print();
                        w.close();
                    }
                    else {
                        var pri = null;
                        if (document.getElementById("frmPrintShippingLabel").contentWindow == null) {
                            pri = document.getElementById("frmPrintShippingLabel").contentDocument;
                        }
                        else {
                            pri = document.getElementById("frmPrintShippingLabel").contentWindow;
                        }
                        pri.document.open();
                        pri.document.write(result);
                        pri.document.close();
                        pri.focus();
                        pri.print();
                    }
                }
                else { jAlert("There was a problem retrieving data from the server."); }
            }
        }
        function CreateXmlHttp() {
            XmlHttp = null; try
            { XmlHttp = new ActiveXObject("Msxml2.XMLHTTP"); }
            catch (e) {
                try
                { XmlHttp = new ActiveXObject("Microsoft.XMLHTTP"); }
                catch (e1)
                { XmlHttp = null; }
            }
            if (!XmlHttp && typeof XMLHttpRequest != "undefined")
            { XmlHttp = new XMLHttpRequest(); }
        }
        function iframeAutoheight(BatchId, OrderNum) {
            CreateXmlHttp(); XmlHttp.onreadystatechange = HandleResponseforInsertProduct; XmlHttp.open("GET", "/Admin/orders/PrintMultipleShippingLabel.aspx?BatchId=" + BatchId + "&OrderNumber=" + OrderNum + "", true); XmlHttp.send(null);
        }

        function printlabelAll() {
            var result = document.getElementById("ContentPlaceHolder1_litSlip").innerHTML;
            var BrowserName = navigator.appName.toString();
            if (BrowserName.toString().toLowerCase().indexOf('internet explorer') > -1) {
                w = window.open('', 'msg', 'directories=no, location=no, menubar=no, status=yes,titlebar=no,toolbar=no,scrollbars=no,resizable=no,Height=10,Width=10,left=0,top=0,visible=false,alwaysLowered=yes');
                w.document.write(result);
                w.document.close();
                w.print();
                w.close();
            }
            else {
                var pri = null;
                if (document.getElementById("frmPrintShippingLabel").contentWindow == null) {
                    pri = document.getElementById("frmPrintShippingLabel").contentDocument;
                }
                else {
                    pri = document.getElementById("frmPrintShippingLabel").contentWindow;
                }
                pri.document.open();
                pri.document.write(result);
                pri.document.close();
                pri.focus();
                pri.print();
            }
        }


    </script>
    <script type="text/javascript">
        function checkaddproductdetail(id) {
            var idsku = id.toString().replace("btnaddproduct", "txtaddsku");
            var idname = id.toString().replace("btnaddproduct", "txtaddname");
            var idqty = id.toString().replace("btnaddproduct", "txtaddqty");
            var idprice = id.toString().replace("btnaddproduct", "txtaddprice");

            if ($("#" + idsku).val() == "") {
                jAlert('Please enter SKU.', 'Required Information');
                return false;
            }
            else if ($("#" + idname).val() == "") {
                jAlert('Please enter name.', 'Required Information');
                return false;
            }
            else if ($("#" + idqty).val() == "") {
                jAlert('Please enter quantity.', 'Required Information');
                return false;
            }
            else if ($("#" + idprice).val() == "") {
                jAlert('Please enter price.', 'Required Information');
                return false;
            }
            return true;
        }

        function chkselectcheckbox() {
            var allElts = document.getElementById('ContentPlaceHolder1_grvOrderlist').getElementsByTagName('input')
            var i;
            var Chktrue;
            Chktrue = 0;
            for (i = 0; i < allElts.length; i++) {
                var elt = allElts[i];
                if (elt.type == "checkbox") {
                    if (elt.checked == true) {
                        Chktrue = Chktrue + 1;
                    }
                }
            }
            if (Chktrue < 1) {
                jAlert('Select at least one Batch(s) to Export !', 'Message');
                return false;
            }
            return true;
        }
        function chkselectcheckboxprint() {
            var allElts = document.getElementById('ContentPlaceHolder1_grvOrderlist').getElementsByTagName('input')
            var i;
            var Chktrue;
            Chktrue = 0;
            for (i = 0; i < allElts.length; i++) {
                var elt = allElts[i];
                if (elt.type == "checkbox" && elt.id.toString().indexOf('chkSelect') > -1) {
                    if (elt.checked == true) {
                        Chktrue = Chktrue + 1;
                    }
                }
            }
            if (Chktrue < 1) {
                jAlert('Select at least one order to print label !', 'Message');
                return false;
            }
            return true;
        }

        function chkselectcheckboxprintlabels() {
            var allElts = document.getElementById('ContentPlaceHolder1_grvOrderlist').getElementsByTagName('input')
            var i;
            var Chktrue;
            Chktrue = 0;
            for (i = 0; i < allElts.length; i++) {
                var elt = allElts[i];
                if (elt.type == "checkbox" && elt.id.toString().indexOf('chkpackageSelect') > -1) {
                    if (elt.checked == true) {
                        Chktrue = Chktrue + 1;
                    }
                }
            }
            if (Chktrue < 1) {
                jAlert('Select at least one order to print label !', 'Message');
                return false;
            }
            return true;
        }

        function chkselectcheckboxprintpackage() {
            var allElts = document.getElementById('ContentPlaceHolder1_grvOrderlist').getElementsByTagName('input')
            var i;
            var Chktrue;
            Chktrue = 0;
            for (i = 0; i < allElts.length; i++) {
                var elt = allElts[i];
                if (elt.type == "checkbox" && elt.id.toString().indexOf('chkgeneratelbl') > -1) {
                    if (elt.checked == true) {
                        Chktrue = Chktrue + 1;
                    }
                }
            }
            if (Chktrue < 1) {
                jAlert('Select at least one order to print package slip !', 'Message');
                return false;
            }
            return true;
        }
        function selectAll1(idname) {
            var on;
            if ('chkMainHeraderSelect' == idname) {
                if (document.getElementById('ContentPlaceHolder1_grvOrderlist_chkMainHeraderSelect') != null) {
                    if (document.getElementById('ContentPlaceHolder1_grvOrderlist_chkMainHeraderSelect').checked == true) {
                        on = true;
                    }
                    else { on = false; }
                    var allElts = document.forms['form1'].elements;
                    var i;
                    for (i = 0; i < allElts.length; i++) {
                        var elt = allElts[i];
                        if (elt.type == "checkbox" && elt.id.toString().indexOf('chkSelect') > -1) {
                            elt.checked = on;
                        }
                    }
                }
            }
            else if ('chkallgeneratelbl' == idname) {
                if (document.getElementById('ContentPlaceHolder1_grvOrderlist_chkallgeneratelbl') != null) {
                    if (document.getElementById('ContentPlaceHolder1_grvOrderlist_chkallgeneratelbl').checked == true) {
                        on = true;
                    }
                    else { on = false; }
                    var allElts = document.forms['form1'].elements;
                    var i;
                    for (i = 0; i < allElts.length; i++) {
                        var elt = allElts[i];
                        if (elt.type == "checkbox" && elt.id.toString().indexOf('_chkgeneratelbl_') > -1) {
                            elt.checked = on;
                        }
                    }
                }
            }
            else {
                if (document.getElementById('ContentPlaceHolder1_grvOrderlist_chkMainpackageSelect') != null) {
                    if (document.getElementById('ContentPlaceHolder1_grvOrderlist_chkMainpackageSelect').checked == true) {
                        on = true;
                    }
                    else { on = false; }
                    var allElts = document.forms['form1'].elements;
                    var i;
                    for (i = 0; i < allElts.length; i++) {
                        var elt = allElts[i];
                        if (elt.type == "checkbox" && elt.id.toString().indexOf('chkpackageSelect') > -1) {
                            elt.checked = on;
                        }
                    }
                }
            }
        }


        function selectAllMainHerader() {
            var myform = document.getElementById('ContentPlaceHolder1_grvOrderlist').getElementsByTagName('ContentPlaceHolder1_grvOrderlist_chkMainHeraderSelect');
            for (var i = 0; i < myform.length; i++) {
                var elt = myform[i];
                if (elt.type == 'checkbox') {
                    if (!$('#chkMainHeraderSelect').attr('checked')) {
                        elt.checked = true;
                    }
                    else {
                        elt.checked = false;
                    }
                }
            }
        }

        function selectAllMainpackage() {
            var myform = document.getElementById('ContentPlaceHolder1_gvSubCategory').getElementsByTagName('INPUT');
            for (var i = 0; i < myform.length; i++) {
                var elt = myform[i];
                if (elt.type == 'checkbox') {
                    if (on.toString() == 'false') {
                        elt.checked = false;
                    }
                    else {
                        elt.checked = true;
                    }
                }
            }
        }

        function calculateAll() {

            var allchk = document.getElementById('ContentPlaceHolder1_grvOrderlist').getElementsByTagName('input');
            var tt = 0;
            if (allchk.length > 0) {
                for (i = 0; i < allchk.length; i++) {
                    var elt = allchk[i];
                    if (elt.id.toString().indexOf('_chkgeneratelbl_') > -1 && elt.checked == true) {
                        tt = 1;
                    }
                }
            }

            if (tt == 0) {
                alert('Please select at least one order.');

            }
            //else if (allchk.length > 0) {

            //    return;
            //}
            //else{}


            var allElts = document.getElementById('ContentPlaceHolder1_grvOrderlist').getElementsByTagName('input')
            var i; var hdncheck;
            for (i = 0; i < allElts.length; i++) {
                var elt = allElts[i];
                if (elt.id.toString().indexOf('ContentPlaceHolder1_grvOrderlist_txt') > -1) {
                    var split = elt.id.toString().split('_');
                    var hdnid = split[3];
                    hdncheck = document.getElementById('ContentPlaceHolder1_grvOrderlist_hdnchecklblgenerate_' + hdnid);
                    if (hdncheck != null && hdncheck.value == "0") {
                        var Height = parseInt(document.getElementById("ContentPlaceHolder1_grvOrderlist_txtHeightgrid_" + hdnid + "").value);
                        var Width = parseInt(document.getElementById("ContentPlaceHolder1_grvOrderlist_txtWidthgrid_" + hdnid + "").value);
                        var Length = parseInt(document.getElementById("ContentPlaceHolder1_grvOrderlist_txtLengthgrid_" + hdnid + "").value);
                        var ProWeight = parseFloat(document.getElementById("ContentPlaceHolder1_grvOrderlist_txtProWeight_" + hdnid + "").value);

                        if (isNaN(Height) || Height <= 0) {
                            jAlert('Please provide height for shipment.', 'Message', 'ContentPlaceHolder1_grvOrderlist_txtHeightgrid_' + hdnid);
                            return false;
                        }
                        else if (isNaN(Width) || Width <= 0) {
                            jAlert('Please provide Width for shipment.', 'Message', 'ContentPlaceHolder1_grvOrderlist_txtWidthgrid_' + hdnid);
                            return false;
                        }
                        else if (isNaN(Length) || Length <= 0) {
                            jAlert('Please provide Length for shipment.', 'Message', 'ContentPlaceHolder1_grvOrderlist_txtLengthgrid_' + hdnid);
                            return false;
                        }
                        else if (isNaN(ProWeight) || ProWeight <= 0) {
                            jAlert('Please provide Weight for shipment.', 'Message', 'ContentPlaceHolder1_grvOrderlist_txtProWeight_' + hdnid);
                            return false;
                        }
                    }
                }
            }
            chkHeight();
            return true;
        }

        function Clacu(row, name,idnew) {
          
            var id = document.getElementById("ContentPlaceHolder1_grvOrderlist_ddlDimensions_" + row)[document.getElementById("ContentPlaceHolder1_grvOrderlist_ddlDimensions_" + row).selectedIndex].text;
            if (id.toLowerCase() == "select") {
                jAlert('Please select dimension shipment.', 'Message');
                return false;
            }

            if (id.toLowerCase() == "other") {
                if (document.getElementById("ContentPlaceHolder1_grvOrderlist_ddlDimensions_" + row))
                    var Height = parseFloat(document.getElementById("ContentPlaceHolder1_grvOrderlist_txtHeightgrid_" + row + "").value);
                var Width = parseFloat(document.getElementById("ContentPlaceHolder1_grvOrderlist_txtWidthgrid_" + row + "").value);
                var Length = parseFloat(document.getElementById("ContentPlaceHolder1_grvOrderlist_txtLengthgrid_" + row + "").value);
                var ProWeight = parseFloat(document.getElementById("ContentPlaceHolder1_grvOrderlist_txtProWeight_" + row + "").value);
                if (isNaN(Height) || Height <= 0) {
                    jAlert('Please provide height for shipment.', 'Message', 'ContentPlaceHolder1_grvOrderlist_txtHeightgrid_' + row);
                    return false;
                }
                else if (isNaN(Width) || Width <= 0) {
                    jAlert('Please provide Width for shipment.', 'Message', 'ContentPlaceHolder1_grvOrderlist_txtWidthgrid_' + row);
                    return false;
                }
                else if (isNaN(Length) || Length <= 0) {
                    jAlert('Please provide Length for shipment.', 'Message', 'ContentPlaceHolder1_grvOrderlist_txtLengthgrid_' + row);
                    return false;
                }
                else if (isNaN(ProWeight) || ProWeight <= 0) {
                    jAlert('Please provide Weight for shipment.', 'Message', 'ContentPlaceHolder1_grvOrderlist_txtProWeight_' + row);
                    return false;
                }
            }
            else {
                chkHeight();
                __doPostBack(name, '');
                return true;
            }
            return true;
        }

        function dimensions(row, name) {
            if (document.getElementById("ContentPlaceHolder1_grvOrderlist_ddlDimensions_" + row + "") != null && document.getElementById("ContentPlaceHolder1_grvOrderlist_dimHWL_" + row + "") != null) {
                var ddldimension = document.getElementById("ContentPlaceHolder1_grvOrderlist_ddlDimensions_" + row + "").value;
                var divdimension = document.getElementById("ContentPlaceHolder1_grvOrderlist_dimHWL_" + row + "");
                var e = document.getElementById("ContentPlaceHolder1_grvOrderlist_ddlDimensions_" + row + "");
                var ddldimesselectedvalue = e.options[e.selectedIndex].text;
                if (ddldimesselectedvalue.toLowerCase() == 'select') { divdimension.style.display = 'none'; }
                else { divdimension.style.display = 'block'; }
            }
        }

        function chkHeight() {
            var windowHeight = 0;
            windowHeight = $(document).height(); //window.innerHeight;
            document.getElementById('prepage').style.height = windowHeight + 'px';
            document.getElementById('prepage').style.display = '';
            $(".removetr").hide(); window.scrollTo(0, 0);
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
            if (key == null || key == 0 || key == 8 || key == 9 || key == 13 || key == 27)
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

        function showhideGrid(id) {
            if (document.getElementById('imgminmize' + id) != null) {
                var src = document.getElementById('imgminmize' + id).src;
                if (src.indexOf('expand_1.png') > -1) {
                    document.getElementById('imgminmize' + id).src = src.replace('expand_1.png', 'minus_1.png');
                    document.getElementById('imgminmize' + id).title = 'Minimize';
                    document.getElementById('imgminmize' + id).alt = 'Minimize';
                    document.getElementById('imgminmize' + id).className = 'minimize';
                    document.getElementById(id).style.display = '';
                }
                else if (src.indexOf('minus_1.png') > -1) {
                    document.getElementById('imgminmize' + id).src = src.replace('minus_1.png', 'expand_1.png');
                    document.getElementById('imgminmize' + id).title = 'Show';
                    document.getElementById('imgminmize' + id).alt = 'Show';
                    document.getElementById('imgminmize' + id).className = 'close';
                    document.getElementById(id).style.display = 'none';
                }
            }
        }
    </script>
    <style type="text/css">
        .top-link a {
            color: #6a6a6a !important;
            padding: 0 5px;
        }

        #backgroundPopup {
            display: none;
            position: fixed;
            _position: absolute;
            height: 100%;
            width: 100%;
            top: 0;
            left: 0;
            background: #000000;
            border: 1px solid #cecece;
            z-index: 1;
        }

        .page_enabled, .page_disabled {
            display: inline-block;
            height: 20px;
            min-width: 20px;
            line-height: 20px;
            text-align: center;
            text-decoration: none;
            border: 1px solid #ccc;
            color: #fff !important;
        }

        .page_enabled {
            background-color: #4f99c6;
            color: #fff !important;
        }

        .page_disabled {
            background-color: #6C6C6C;
            color: #fff !important;
        }



        .rightpadding {
            float: right;
            margin-top: 7px;
            margin-right: 10px;
        }

        .order-no {
        }

        a:hover {
            color: #4c8fbd !important;
        }

        .Green {
            color: #73AB20;
        }

        .buttonStyle {
            height: 23px;
            cursor: pointer;
            background-color: #B92127;
            color: #ffffff;
            padding-left: 4px;
            padding-right: 4px;
        }

        .page_disabled a, a:active, a:focus, a:hover, a:visited {
            color: #fff !important;
        }

        .span4 {
            width: 55px;
            height: 35px;
            text-align: center;
        }
        /*.add-product a { color:#b92127 !important;}*/
        #lnkPage a {
            color: #ffffff !important;
        }

        .page_disabled {
            background-color: #b92127 !important;
        }

        .page_enabled {
            background-color: #6C6C6C !important;
        }

        .oddrow td {
            border: 1px solid #eee;
        }

        .altrow td {
            border: 1px solid #eee;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="content-width">
        <div class="content-row1">
            <div class="create-new-order" style="width: 100%;">
                <table>
                    <tr>
                        <td align="left">Store :&nbsp;<asp:DropDownList ID="ddlStore" runat="server" CssClass="order-list"
                            Width="175px" AutoPostBack="true" Style="margin-top: 5px;" OnSelectedIndexChanged="ddlStore_SelectedIndexChanged">
                        </asp:DropDownList>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
        <div class="content-row2">
            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                <tr>
                    <td height="5" align="left" valign="top">
                        <img src="/App_Themes/<%=Page.Theme %>/images/spacer.gif" alt="" width="1" height="5" />
                    </td>
                </tr>
                <tr>
                    <td height="5" align="left" valign="top">
                        <img src="/App_Themes/<%=Page.Theme %>/images/spacer.gif" alt="" width="1" height="5" />
                    </td>
                </tr>
                <tr>
                    <td class="border-td">
                        <table width="100%" border="0" cellpadding="0" cellspacing="0" bgcolor="#FFFFFF"
                            class="content-table">
                            <tr>
                                <td class="border-td-sub">
                                    <table width="100%" border="0" cellpadding="0" cellspacing="0" class="add-product">
                                        <tr>
                                            <th colspan="3">
                                                <div class="main-title-left">
                                                    <img class="img-left" title="" alt="" src="/App_Themes/<%=Page.Theme %>/Images/mail-log-icon.png" />
                                                    <h2>Multiple Shipping Section</h2>
                                                </div>
                                            </th>
                                        </tr>
                                        <tr>

                                            <td align="left"></td>
                                            <td align="right" colspan="2">
                                                <table cellpadding="3" cellspacing="3">
                                                    <tr>
                                                        <td>IsPrime :
                                                            <asp:CheckBox ID="ChkIsPrime" Checked="true" runat="server" /></td>
                                                        <td valign="middle" align="right">Status:
                                                        </td>
                                                        <td align="left" style="width: 110px;">
                                                            <asp:DropDownList ID="ddlStatus" runat="server" AutoPostBack="false"
                                                                CssClass="status-list">
                                                                <asp:ListItem Text="All" Value="" Selected="True"></asp:ListItem>
                                                                <asp:ListItem Text="Shipped" Value="Shipped"></asp:ListItem>
                                                                <asp:ListItem Text="Not Shipped" Value="Not Shipped"></asp:ListItem>
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td valign="middle" align="right">From Date:
                                                        </td>
                                                        <td valign="middle" align="right">
                                                            <asp:TextBox ID="txtMailFrom" runat="server" CssClass="from-textfield" Width="70px"
                                                                Style="margin-right: 3px;"></asp:TextBox>
                                                        </td>
                                                        <td valign="middle" align="right">To Date:
                                                        </td>
                                                        <td valign="middle" align="right">
                                                            <asp:TextBox ID="txtMailTo" runat="server" CssClass="from-textfield" Width="70px"
                                                                Style="margin-right: 3px;"></asp:TextBox>
                                                        </td>
                                                        <td align="left" style="width: 190px;">Search By Order / Amazon Order #:
                                                        </td>
                                                        <%-- <td align="left" style="width:180px;">
                                                            <asp:DropDownList ID="ddlSearch" runat="server" CssClass="order-list" Width="175px"
                                                                AutoPostBack="false">
                                                                <asp:ListItem Text="Search By" Value="0"></asp:ListItem>
                                                                <asp:ListItem Text="Order No" Value="OrderNumber"></asp:ListItem>
                                                                <asp:ListItem Text="Ref Order No" Value="RefOrderID"></asp:ListItem>
                                                            </asp:DropDownList>
                                                        </td>--%>
                                                        <td align="left">
                                                            <%-- <asp:TextBox ID="txtSearch" onfocus="javascript:if(this.value=='OrderNo / Reforder No'){this.value=''};"
                                                                onblur="javascript:if(this.value==''){this.value='OrderNo / Reforder No'};" Text="OrderNo / Reforder No"
                                                                CssClass="order-textfield" Width="144px" runat="server"></asp:TextBox>--%>
                                                            <asp:TextBox ID="txtSearch" CssClass="order-textfield" Width="144px" runat="server"></asp:TextBox>
                                                        </td>
                                                        <td align="right">
                                                            <asp:Button ID="btnSearch" runat="server" OnClick="btnSearch_Click" OnClientClick="return SearchValidation(0);" />

                                                        </td>
                                                        <%-- <td align="right">
                                                            <asp:Button ID="btnShowall" runat="server" OnClick="btnShowall_Click" OnClientClick="return SearchValidation(1);" />
                                                        </td>--%>
                                                        <%-- <td align="right">
                                                            <asp:ImageButton ID="btnresend" runat="server" ImageUrl="/App_Themes/gray/images/send-mail.png" OnClick="btnresend_Click" OnClientClick="return chkSelectresend();" />
                                                        </td>--%>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center" colspan="3">
                                                <div class="row-fluid removetr">
                                                    <div class="widget-body">
                                                        <div class="widget-main padding-10">
                                                            <span style="margin-top: 5px; font-size: 12px; font-weight: bold;">
                                                                <asp:Label ID="lblPackerName" Font-Bold="false" Font-Size="13px" runat="server"></asp:Label>
                                                            </span>
                                                        </div>

                                                    </div>
                                                    <table id="sample-table-2" width="100%" cellpadding="0" cellspacing="0" border="0" style="margin-top: 10px;">
                                                        <tr id="trpagesize" runat="server">
                                                            <td style="float: right; margin-top: 12px; margin-bottom: 12px; background: none;">
                                                                <strong>Page Size :</strong>
                                                                <asp:DropDownList ID="ddlPageSize" CssClass="op" runat="server" AutoPostBack="true" OnSelectedIndexChanged="PageSize_Changed">
                                                                    <asp:ListItem Text="10" Value="10" />
                                                                    <asp:ListItem Text="20" Value="20" />
                                                                    <%--<asp:ListItem Text="35" Value="35" />--%>
                                                                </asp:DropDownList>
                                                                &nbsp;&nbsp;
                                                                        <asp:Repeater ID="rptPager" runat="server">
                                                                            <ItemTemplate>
                                                                                <asp:LinkButton ID="lnkPage" runat="server" Text='<%#Eval("Text") %>' CommandArgument='<%# Eval("Value") %>' ForeColor="White"
                                                                                    CssClass='<%# Convert.ToBoolean(Eval("Enabled")) ? "page_enabled" : "page_disabled" %>'
                                                                                    OnClick="Page_Changed" OnClientClick='chkHeight();'></asp:LinkButton>
                                                                            </ItemTemplate>
                                                                        </asp:Repeater>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="padding-top: 0px; padding-bottom: 0px;">
                                                                <asp:GridView ID="grvOrderlist" runat="server" CssClass="table table-border-display table-striped link no-margin-bottom background-default"
                                                                    CellPadding="0" AutoGenerateColumns="false" BorderWidth="0" CellSpacing="0" BorderStyle="Solid"
                                                                    Width="99%" BorderColor="#E7E7E7"
                                                                    EmptyDataRowStyle-HorizontalAlign="Center" EmptyDataText="No Record(s) Found." PagerSettings-Mode="NumericFirstLast"
                                                                    OnRowDataBound="grvOrderlist_RowDataBound" DataKeyNames="OrderNumber" GridLines="None" AllowPaging="false" PageSize="5" OnPageIndexChanging="grvOrderlist_PageIndexChanging"
                                                                    ShowFooter="false" AllowSorting="true" ShowHeaderWhenEmpty="True" OnRowCommand="grvOrderlist_RowCommand"
                                                                    OnRowEditing="grvOrderlist_RowEditing">
                                                                    <EmptyDataTemplate>
                                                                        <span style="color: Red; font-size: 12px; text-align: center;">No Record(s) Found !</span>
                                                                    </EmptyDataTemplate>
                                                                    <Columns>
                                                                        <asp:TemplateField HeaderText="Seelct">
                                                                            <HeaderTemplate>
                                                                                <br />
                                                                                <asp:CheckBox ID="chkallgeneratelbl" onchange="selectAll1('chkallgeneratelbl');"
                                                                                    onclick="selectAll1('chkallgeneratelbl');" runat="server" Style="margin-right: 18px;"
                                                                                    CssClass="input-checkbox" /><br />
                                                                                Select
                                                                            </HeaderTemplate>
                                                                            <HeaderStyle HorizontalAlign="Center" CssClass="center" />
                                                                            <ItemTemplate>
                                                                                <asp:CheckBox ID="chkgeneratelbl" runat="server" Style="margin-right: 18px;" CssClass="input-checkbox" />
                                                                            </ItemTemplate>
                                                                            <ItemStyle HorizontalAlign="Center" CssClass="center" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField SortExpression="Order#" HeaderText="Order# / Reforder#">
                                                                            <HeaderTemplate>
                                                                                Order # / Amz. Order #
                                                                            </HeaderTemplate>
                                                                            <HeaderStyle HorizontalAlign="Center" CssClass="center" />
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblOrderNumber" runat="server" Text='<%# Eval("OrderNumber")%>' Visible="false"></asp:Label>
                                                                                <asp:Literal ID="ltr2" runat="server" Text='<%# Eval("OrderNumber")%>'></asp:Literal>
                                                                                <asp:Literal ID="ltrRefOrderNo" runat="server" Text='<%# Eval("RefOrderID")%>'></asp:Literal><br />
                                                                                <input type="hidden" id="hdnRefOrderNo" runat="server" value='<%# Eval("RefOrderID")%>' />
                                                                                <br />
                                                                                <asp:Literal ID="ltramazonlogo" runat="server"></asp:Literal>
                                                                            </ItemTemplate>
                                                                            <ItemStyle HorizontalAlign="Center" CssClass="center" VerticalAlign="Top" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField SortExpression="OrderDate" HeaderText="Order Date">
                                                                            <HeaderTemplate>
                                                                                Order Date
                                                                            </HeaderTemplate>
                                                                            <ItemTemplate>
                                                                                <asp:Literal ID="ltr1" runat="server" Text='<%#String.Format("{0:dd&nbsp;MMM,&nbsp;yyyy <br/> hh:mm:ss&nbsp;ttt}",Convert.ToDateTime(Eval("OrderDate")))%>'></asp:Literal>
                                                                            </ItemTemplate>
                                                                            <HeaderStyle HorizontalAlign="Center" Width="8%" />
                                                                            <ItemStyle HorizontalAlign="Center" Width="8%" VerticalAlign="Top" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="&nbsp;Shipping Address">
                                                                            <ItemTemplate>
                                                                                <asp:Literal ID="ltripadONO" runat="server" Text='<%# Eval("OrderNumber")%>' Visible="false"></asp:Literal>
                                                                                <%--<asp:Literal ID="ltr4" runat="server" Text='<%# "<a href=\"/Admin/Customers/Customer.aspx?mode=edit&CustID="+ Eval("CustomerID") +"\" title=\"\" class=\"order-no\" style=\"color:"+ Eval("Storename") +"\">"+ Eval("ShippingFirstName") + " " + Eval("ShippingLastName")  +"</a>" %>'></asp:Literal>--%>
                                                                                <asp:Literal ID="ltr4" runat="server" Text='<%# "<a href=\"/Admin/Customers/Customer.aspx?mode=edit&CustID="+ Eval("CustomerID") +"\" title=\"\" class=\"order-no\" style=\"color:#b92127 !important;\">"+ Eval("ShippingFirstName") + " " + Eval("ShippingLastName")  +"</a>" %>'></asp:Literal>

                                                                                <input type="hidden" id="hdnAddress1" runat="server" value='<%# Eval("ShippingAddress1")%>' />
                                                                                <input type="hidden" id="hdnAddress2" runat="server" value='<%# Eval("ShippingAddress2")%>' />
                                                                                <input type="hidden" id="hdnSuite" runat="server" value='<%# Eval("ShippingSuite")%>' />
                                                                                <input type="hidden" id="hdnCity" runat="server" value='<%# Eval("ShippingCity")%>' />
                                                                                <input type="hidden" id="hdnState" runat="server" value='<%# Eval("ShippingState")%>' />
                                                                                <input type="hidden" id="hdnPhone" runat="server" value='<%# Eval("ShippingPhone")%>' />
                                                                                <input type="hidden" id="hdnCountry" runat="server" value='<%# Eval("ShippingCountry")%>' />
                                                                                <input type="hidden" id="hdnZip" runat="server" value='<%# Eval("ShippingZip")%>' />
                                                                                <input type="hidden" id="hdnCompany" runat="server" value='<%# Eval("ShippingCompany")%>' />
                                                                                <input type="hidden" id="hdnShippingMethod" runat="server" value='<%# Eval("ShippingMethod")%>' />
                                                                                <input type="hidden" id="hdnOrderTotalNew" runat="server" value='<%# Eval("OrderTotal")%>' />
                                                                                <asp:HiddenField ID="hdnOrderShippingCosts" runat="server" Value='<%# Eval("OrderShippingCosts")%>' />
                                                                                <asp:TextBox ID="txtCustomername" runat="server" CssClass="order-textfield" Width="234px" Visible="false"></asp:TextBox>

                                                                            </ItemTemplate>
                                                                            <HeaderStyle HorizontalAlign="left" />
                                                                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="&nbsp;Product Details">
                                                                            <ItemTemplate>
                                                                                <input type="hidden" id="hdnshoppingcartid" runat="server" value='<%# Eval("ShoppingCardID")%>' />
                                                                                <input type="hidden" id="hdnorderStatus" runat="server" value='<%# Eval("orderStatus")%>' />
                                                                                <input type="hidden" id="hndProductId" runat="server" />
                                                                                <table cellpadding="0" cellspacing="0" width="100%" border="0" id="tblordernodetails"
                                                                                    runat="server">
                                                                                    <tr>
                                                                                        <td align="left" valign="top" style="border: none; border-left: 0 solid #DDDDDD !important; border-bottom: 0px solid #DDDDDD !important;">
                                                                                            <asp:Literal ID="ltr3" runat="server"></asp:Literal>
                                                                                        </td>
                                                                                    </tr>
                                                                                </table>
                                                                                <div id="divaddproductdetail" runat="server" style="display: none;">
                                                                                    <div>
                                                                                        <div>SKU</div>
                                                                                        <div>
                                                                                            <asp:TextBox ID="txtaddsku" CssClass="span4" Width="100px" runat="server"></asp:TextBox>
                                                                                        </div>
                                                                                    </div>
                                                                                    <div>
                                                                                        <div>Name</div>
                                                                                        <div>
                                                                                            <asp:TextBox ID="txtaddname" CssClass="span4" Width="100px" MaxLength="5" runat="server"></asp:TextBox>
                                                                                        </div>
                                                                                    </div>
                                                                                    <div>
                                                                                        <div>Qty</div>
                                                                                        <div>
                                                                                            <asp:TextBox ID="txtaddqty" onKeyPress="return keyRestrict(event,'0123456789.')" CssClass="span4" Width="100px" MaxLength="5" runat="server"></asp:TextBox>
                                                                                        </div>
                                                                                    </div>
                                                                                    <div>
                                                                                        <div>Price</div>
                                                                                        <div>
                                                                                            <asp:TextBox ID="txtaddprice" onKeyPress="return keyRestrict(event,'0123456789.')" CssClass="span4" Width="100px" MaxLength="5" runat="server"></asp:TextBox>
                                                                                        </div>
                                                                                    </div>
                                                                                    <div>
                                                                                        <asp:Button ID="btnaddproduct" runat="server" ToolTip="Add Product" OnClientClick="return checkaddproductdetail(this.id);" data-toggle="button" Text="Add Product" class="btn btn-small btn-info" CommandArgument='<%# Eval("OrderNumber") %>' CommandName="AddProduct" />
                                                                                    </div>
                                                                                </div>
                                                                            </ItemTemplate>
                                                                            <HeaderStyle HorizontalAlign="left" Width="32%" />
                                                                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" Width="32%" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Insured" Visible="false">
                                                                            <ItemTemplate>
                                                                                <div>
                                                                                    Price:<asp:TextBox ID="txtaddtionalprice" runat="server" CssClass="span4" Width="75px" MaxLength="5" onkeypress="return keyRestrict(event,'0123456789.')"></asp:TextBox>
                                                                                </div>
                                                                                <div>
                                                                                    <span>
                                                                                        <asp:CheckBox ID="chkinsured" runat="server" CssClass="input-checkbox" Style="width: 15px; margin-top: -1px; padding-right: 7px;" />
                                                                                        <asp:Literal ID="lt" runat="server" Text="Insured"></asp:Literal>
                                                                                    </span>
                                                                                </div>
                                                                                <br />
                                                                                <br />
                                                                                <div>
                                                                                    <span>
                                                                                        <asp:CheckBox ID="chkMail" runat="server" CssClass="input-checkbox" Style="width: 20px; padding-top: 0px; padding-bottom: 5px; margin-top: -1px;" />Signature Required </span>
                                                                                </div>
                                                                            </ItemTemplate>
                                                                            <ItemStyle HorizontalAlign="Left" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Warehouse " Visible="false">
                                                                            <ItemTemplate>
                                                                                <asp:DropDownList ID="ddlWareHouse" Style="padding-left: 3px; width: 100px;" AutoPostBack="false"
                                                                                    runat="server" CssClass="op">
                                                                                </asp:DropDownList>
                                                                                <asp:Label ID="lblddlWareHouseName" Visible="false" runat="server"></asp:Label><br />
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Dimensions&nbsp;(H x W x L)" ItemStyle-CssClass="griditemst" Visible="false">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblDimensionName" runat="server" Visible="false"></asp:Label><br />
                                                                                <div id="dimHWL" runat="server">
                                                                                    <asp:TextBox ID="txtHeightgrid" onKeyPress="return keyRestrict(event,'0123456789.')" CssClass="span4"
                                                                                        MaxLength="5" runat="server" Text='<%#Eval("Height").ToString().Length>0?Eval("Height"):"0" %>' Columns="3"></asp:TextBox>
                                                                                    X
                                            <asp:TextBox ID="txtWidthgrid" onKeyPress="return keyRestrict(event,'0123456789.')" CssClass="span4"
                                                MaxLength="5" runat="server" Columns="3" Text='<%#Eval("Width").ToString().Length>0?Eval("Width"):"0" %>'></asp:TextBox>
                                                                                    X
                                            <asp:TextBox ID="txtLengthgrid" onKeyPress="return keyRestrict(event,'0123456789.')" CssClass="span4"
                                                MaxLength="5" runat="server" Text='<%#Eval("Length").ToString().Length>0?Eval("Length"):"0" %>'
                                                Columns="3"></asp:TextBox>
                                                                                </div>
                                                                                <asp:HiddenField ID="hdnDimensionValue" runat="server" Value='<%# Eval("DimensionValue")%>' />
                                                                            </ItemTemplate>
                                                                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Weight" Visible="false">
                                                                            <ItemTemplate>
                                                                                <br />
                                                                                <asp:TextBox ID="txtProWeight" autocomplete="off" AutoPostBack="true" onKeyPress="var ret=keyRestrict(event,'0123456789.');return ret;"
                                                                                    MaxLength="5" runat="server" Width="50px" CssClass="span4" Text="0.00" OnTextChanged="txtProWeight_TextChanged"></asp:TextBox>
                                                                            </ItemTemplate>
                                                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Top" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="USPS Mail Piece" Visible="false">
                                                                            <ItemTemplate>
                                                                                <br />
                                                                                <asp:DropDownList ID="ddlMailpieceShape" Style="padding-left: 3px; width: 100%;"
                                                                                    runat="server" CssClass="op">
                                                                                    <asp:ListItem>Package</asp:ListItem>
                                                                                    <asp:ListItem>FlatRateEnvelope</asp:ListItem>
                                                                                    <asp:ListItem>LargeEnvelopeorFlat</asp:ListItem>
                                                                                    <asp:ListItem>FlatRateBox</asp:ListItem>
                                                                                    <asp:ListItem>LargeFlatRateBox</asp:ListItem>
                                                                                    <asp:ListItem>SmallFlatRateBox</asp:ListItem>

                                                                                </asp:DropDownList>
                                                                            </ItemTemplate>
                                                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Top" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Shipping Method" Visible="false">
                                                                            <ItemTemplate>
                                                                                <asp:Literal ID="ltrCustomerShipping" runat="server"></asp:Literal><br />
                                                                                <asp:DropDownList ID="ddlShippingMethod" Style="padding-left: 3px; width: 60%; height: 35px;"
                                                                                    AutoPostBack="false" runat="server" CssClass="product-type">
                                                                                </asp:DropDownList>
                                                                                <%--    <asp:Button ID="btnRebindShippingMethods" runat="server" ToolTip="Refresh"
                                                                                    data-toggle="button" Text="Refresh" class="btn btn-mini btn-info buttonStyle"
                                                                                    CommandArgument='<%# Eval("OrderNumber") %>' CommandName="RefreshShipping" />--%>
                                                                                <asp:ImageButton ID="btnRebindShippingMethods" runat="server" ToolTip="Refresh" CommandArgument='<%# Eval("OrderNumber") %>' CommandName="RefreshShipping" ImageUrl="/App_Themes/<%=Page.Theme %>/images/refresh-icon.png" />

                                                                                <asp:Label ID="lblShippingMethod" runat="server"></asp:Label><br />
                                                                                <asp:Literal ID="ltrTackingNo" runat="server"></asp:Literal>
                                                                                <input type="hidden" id="hdnShippingLabelMethod" runat="server" value='<%# Eval("ShippingLabelMethod")%>' />
                                                                                <input type="hidden" id="hdnShippingLabelWeight" runat="server" value='<%# Eval("ShippingLabelWeight")%>' />
                                                                                <input type="hidden" id="hdnShippingLabelPackageHeight" runat="server" value='<%# Eval("ShippingLabelPackageHeight")%>' />
                                                                                <input type="hidden" id="hdnShippingLabelPackageWidth" runat="server" value='<%# Eval("ShippingLabelPackageWidth")%>' />
                                                                                <input type="hidden" id="hdnShippingLabelPackageLength" runat="server" value='<%# Eval("ShippingLabelPackageLength")%>' />
                                                                            </ItemTemplate>
                                                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Top" />
                                                                            <HeaderStyle Width="17%" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Print Label/Package Slip">
                                                                            <HeaderTemplate>
                                                                                <asp:CheckBox ID="chkMainHeraderSelect" onchange="selectAll1('chkMainHeraderSelect');"
                                                                                    onclick="selectAll1('chkMainHeraderSelect');" runat="server" Style="margin-right: 10px; margin-left: 3px;"
                                                                                    CssClass="input-checkbox" />Print Label &nbsp;
                                                                            </HeaderTemplate>
                                                                            <HeaderStyle CssClass="center" />
                                                                            <ItemTemplate>
                                                                                <asp:CheckBox ID="chkSelect" Visible="false" runat="server" Style="margin-right: 18px;"
                                                                                    CssClass="input-checkbox" />
                                                                            </ItemTemplate>
                                                                            <ItemStyle CssClass="center" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Print Package Slip" Visible="false">
                                                                            <HeaderTemplate>
                                                                                <asp:CheckBox ID="chkMainpackageSelect" onchange="selectAll1('chkMainpackageSelect');"
                                                                                    onclick="selectAll1('chkMainpackageSelect');" runat="server" Style="margin-right: 18px;"
                                                                                    CssClass="input-checkbox" />Print Package Slip
                                                                            </HeaderTemplate>
                                                                            <HeaderStyle HorizontalAlign="Center" CssClass="center" />
                                                                            <ItemTemplate>
                                                                                <asp:CheckBox ID="chkpackageSelect" runat="server" Style="margin-right: 18px;" CssClass="input-checkbox" />
                                                                            </ItemTemplate>
                                                                            <ItemStyle HorizontalAlign="Center" CssClass="center" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Action">
                                                                            <HeaderTemplate>
                                                                                Action<br />
                                                                                <asp:Literal ID="ltrUseBr" runat="server" Visible="false"></asp:Literal>
                                                                                <%-- <asp:Button ID="btngeneratealllabel" runat="server" ToolTip="Save & Generate All Label" data-toggle="button" Style="margin-top: 3px; white-space: normal; "
                                                                                    Text="Save & Generate All Label" class="btn btn-small btn-primary buttonStyle" OnClick="btnGeneratealllabel_Click" OnClientClick="return calculateAll();" />&nbsp;--%>
                                                                                <asp:ImageButton ID="btngeneratealllabel" Visible="False" ToolTip="Save & Generate All Label" ImageUrl="/App_Themes/<%=Page.Theme %>/images/save-and-generate-label.gif" Style="margin-top: 5px; margin-bottom: 5px;" runat="server" OnClick="btnGeneratealllabel_Click" OnClientClick="return calculateAll();" />
                                                                                <asp:ImageButton runat="server" ID="btnlnkPrintalllabel" OnClick="btnlnkPrintallLabel_Click" ImageUrl="/App_Themes/<%=Page.Theme %>/images/print-label.gif" ToolTip="Print All Labels & Document(s)" Style="white-space: normal; width: 137px; display: none;" OnClientClick="return chkselectcheckboxprintlabels();" />
                                                                                <%-- <asp:Button ID="btnlnkPrintalllabel" runat="server"  OnClick="btnlnkPrintallLabel_Click" Text="Print All Labels & Document(s)" CssClass="btn btn-small btn-primary buttonStyle" Style="white-space: normal; width: 137px; display: none;"
                                                                                    ToolTip="Print All Labels" OnClientClick="return chkselectcheckboxprintlabels();" />--%>
                                                                            </HeaderTemplate>
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblShippingLabelFileName" Visible="False" runat="server" Text='<%# Eval("ShippingLabelFileName") %>'></asp:Label>
                                                                                <asp:ImageButton ID="btnSearchlabel" runat="server" Visible="False" ImageUrl="/App_Themes/<%=Page.Theme %>/images/save-and-generate-label.gif" ToolTip="Save & Generate Label" CommandArgument='<%# Eval("OrderNumber") %>' CommandName="Edit" /><br />
                                                                                <a style="color: #212121;" href="MultipleShippingPopupForLabel.aspx?Ono=<%# DataBinder.Eval(Container.DataItem,"OrderNumber") %>" target="_blank">
                                                                                    <img id="btnPartiallyGenerateLabel" src="/App_Themes/gray/images/partially-generate-label.gif" style="margin-top: 5px; margin-bottom: 5px;" runat="server" /></a><br />
                                                                                <%--<asp:Button ID="btnSearch" runat="server" ToolTip="Save & Generate Label" data-toggle="button"
                                                                                    Text="Save & Generate Label" class="btn btn-mini btn-info buttonStyle"
                                                                                    CommandArgument='<%# Eval("OrderNumber") %>' CommandName="Edit" />--%>
                                                                                <br />
                                                                                <br />
                                                                                <%--<asp:Button ID="btnRemoveBatchOrder" runat="server" OnClientClick="javascript:jConfirm('Are you sure want to delete order from batch?', 'Confirmation', function (r) { return true;}else{ return false;}});" ToolTip="Remove From Batch" data-toggle="button" Text="Remove From Batch" class="btn btn-mini btn-info buttonStyle" CommandArgument='<%# Eval("OrderNumber") %>' CommandName="RemoveBatchOrder" />--%>
                                                                                <asp:ImageButton ID="btnRemoveBatchOrder" Visible="false" runat="server" OnClientClick="javascript:jConfirm('Are you sure want to delete order from batch?', 'Confirmation', function (r) { return true;}else{ return false;}});" ToolTip="Remove From Batch" ImageUrl="/App_Themes/<%=Page.Theme %>/images/remove-from-batch.gif" />
                                                                                <asp:Label ID="lblGenerateLabelMsg" ForeColor="Green" Font-Bold="true" Visible="false" runat="server"></asp:Label>
                                                                                <asp:HiddenField ID="hdnchecklblgenerate" runat="server" Value="0" />
                                                                                <br />
                                                                                <%--<asp:Button ID="bntDownUSPS" runat="server" ToolTip="Download Generated Label(s)" data-toggle="button" Text="Download Generated Label(s)" class="btn btn-mini btn-info" CommandArgument='<%# Eval("ShippingLabelFileName") %>' CommandName="Download" />--%>
                                                                                <asp:Literal ID="lblAmazonTrackingNo" runat="server"></asp:Literal><br />
                                                                                 <asp:ImageButton ID="btnPrintLabel" runat="server"   ToolTip="Print Label(s)"  CommandArgument='' CommandName="Print"  ImageUrl="/App_Themes/<%=Page.Theme %>/images/print-label.gif" /><br />
                                                                                <asp:ImageButton ID="btnprintslip" runat="server" CommandArgument='' CommandName="Packaingslip"  ImageUrl="/App_Themes/gray/images/print-packing-slip-shipping.gif" /><br />
                                                                                <asp:ImageButton ID="bntDownUSPS" runat="server" ToolTip="Download Generated Label(s)" CommandArgument='<%# Eval("ShippingLabelFileName") %>' CommandName="Download" ImageUrl="/App_Themes/<%=Page.Theme %>/images/downloadAmazon-pdf.jpg" />
                                                                                <br />
                                                                                <%--<asp:Button ID="btnprintalldoc" runat="server" ToolTip="Print All Document(s)" data-toggle="button" Text="Print All Document(s)" Style="margin-top: 2%;display:none;" class="btn btn-mini btn-info" CommandArgument='<%# Eval("ShippingLabelFileName") %>' CommandName="Document" />--%>
                                                                                <asp:ImageButton ID="btnprintalldoc" runat="server" ToolTip="Print All Document(s)" CommandArgument='<%# Eval("ShippingLabelFileName") %>' Style="margin-top: 2%; display: none;" CommandName="Document" />
                                                                            </ItemTemplate>
                                                                            <ItemStyle HorizontalAlign="Center" CssClass="center" />
                                                                            <HeaderStyle HorizontalAlign="Center" CssClass="center" Width="12%" />
                                                                        </asp:TemplateField>
                                                                    </Columns>
                                                                    <%-- <PagerSettings Mode="NumericFirstLast" Position="TopAndBottom" PageButtonCount="18"></PagerSettings>
                                                                    <PagerStyle HorizontalAlign="right" CssClass="numbering" />
                                                                    <AlternatingRowStyle CssClass="even-row" VerticalAlign="top" />
                                                                    <RowStyle CssClass="odd-row" VerticalAlign="top" />
                                                                    <HeaderStyle HorizontalAlign="Center" />
                                                                    <FooterStyle BackColor="#E1E1E1" ForeColor="#000" Font-Bold="true" HorizontalAlign="Right" BorderWidth="0" />--%>
                                                                    <PagerSettings Mode="NumericFirstLast" Position="TopAndBottom"></PagerSettings>
                                                                    <PagerStyle HorizontalAlign="Right" CssClass="grid_paging" />
                                                                    <RowStyle HorizontalAlign="Center" CssClass="oddrow" />
                                                                    <AlternatingRowStyle CssClass="altrow" />
                                                                    <HeaderStyle ForeColor="White" Font-Bold="false" />
                                                                    <FooterStyle ForeColor="White" Font-Bold="true" BackColor="#545454" />

                                                                </asp:GridView>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </div>
                                                <div class="row-fluid removetr">
                                                    <div class="widget-body">
                                                        <div class="span5 rightpadding">
                                                            <div class="pagination">
                                                                <ul>
                                                                    <%--<asp:Button runat="server" ID="btnPrintLabel" ToolTip="Print Label(s)" OnClick="btnPrintLabel_Click" data-toggle="button" class="btn btn-mini btn-info" Text="Print Label(s)" OnClientClick="return chkselectcheckboxprint();" />--%>
                                                                    <asp:ImageButton ID="btnPrintLabel1" runat="server" OnClick="btnPrintLabel_Click" ToolTip="Print Label(s)" OnClientClick="return chkselectcheckboxprint();" ImageUrl="/App_Themes/<%=Page.Theme %>/images/print-label.gif" />
                                                                    &nbsp;&nbsp;
                            <%--<asp:Button ID="btnprintslip" Text="Print Packing Slip" data-toggle="button" class="btn btn-mini btn-info" runat="server" OnClientClick="return chkselectcheckboxprintpackage();" OnClick="btnprintslip_Click" />--%>
                                                                    <asp:ImageButton ID="btnprintslip1" runat="server" OnClientClick="return chkselectcheckboxprintpackage();" OnClick="btnprintslip_Click" ImageUrl="/App_Themes/<%=Page.Theme %>/images/print-packing-slip-shipping.gif" />
                                                                </ul>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </div>
        <div style="visibility: hidden">
            <iframe id="frmPrintShippingLabel" frameborder="0" marginheight="0" marginwidth="0" width="100%" scrolling="auto"></iframe>
        </div>
        <div id="prepage" style="position: absolute; font-family: arial; font-size: 16px; left: 0px; top: 0px; opacity: 0.7; filter: alpha(opacity=70); layer-background-color: white; height: 100%; width: 100%; z-index: 1000; display: none;">
            <center>
                <div class="modal-content" style="margin-top: 15%; margin-left: 10%;">
                    <div class="modal-body">
                        <div class="bootbox-body">
                            <style>
                                .modal1 &gt; .modal-dialog {
                                    width: 240px !important;
                                }

                                .bootbox-body {
                                    height: 80px;
                                }
                            </style>
                            <div style="text-align: center;">
                                <div style="overflow: hidden;">
                                    <div id="loader"></div>
                                </div>
                            </div>
                            <div style="padding-top: 10%; font-weight: bold; text-align: center; overflow: hidden; width: 100%;">
                                <center>
                                    <img src="/admin/images/loading.gif" title="Loading" alt="Loading" /><br />
                                    Loading...</center>
                            </div>
                        </div>
                    </div>
                </div>
            </center>
        </div>
        <div id="backgroundPopup" style="z-index: 1000000;">
        </div>
        <div id="litSlip" runat="server" style="display: none;">
        </div>
        <div style="display: none">
            <iframe id="ifmcontentstoprint" onload="printpdffile();"></iframe>
        </div>
    </div>
</asp:Content>
