<%@ Page Title="" Language="C#" MasterPageFile="~/ADMIN/Admin.Master" AutoEventWireup="true"
    CodeBehind="Product.aspx.cs" Inherits="Solution.UI.Web.ADMIN.Products.Product"
    ValidateRequest="false" %>

<%@ Register Src="~/ADMIN/Controls/ProductHead.ascx" TagName="Header" TagPrefix="uc1" %>
<%@ Register Assembly="CKEditor.NET" Namespace="CKEditor.NET" TagPrefix="CKEditor" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts-main.js"></script>


    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts.js"></script>
    <script src="../JS/jquery-1.3.2.js" type="text/javascript"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/tabs.js"></script>
    <script src="../JS/ProductValidation.js?11111" type="text/javascript"></script>

    <script type="text/javascript" src="../../ckeditor/ckeditor.js"></script>
    <script src="../../ckeditor/_samples/sample.js" type="text/javascript"></script>

    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-calender-custom.js"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-calender.js"></script>
    <script type="text/javascript">

        function checkall(id, id2) {
            if (document.getElementById(id) != null && document.getElementById(id2) != null && document.getElementById(id2).checked == true) {
                document.getElementById(id).checked = false;
            }
            redirecturl(id, id2);
        }
        function checkalladmin(id, id2) {
            if (document.getElementById(id) != null && document.getElementById(id2) != null && document.getElementById(id).checked == true) {
                document.getElementById(id2).checked = false;
            }
            redirecturl(id, id2);
        }

        function redirecturl(id, id2) {
            if (document.getElementById(id) != null && document.getElementById(id).checked == true) {
                document.getElementById("ContentPlaceHolder1_txtpageredirecturl").readOnly = true;
                if (document.getElementById("ContentPlaceHolder1_txtpageredirecturl").value != '')
                    document.getElementById("ContentPlaceHolder1_hdnredirecturl").value = document.getElementById("ContentPlaceHolder1_txtpageredirecturl").value;
                document.getElementById("ContentPlaceHolder1_txtpageredirecturl").value = '';
                document.getElementById("ContentPlaceHolder1_tdpageredirect").style.display = 'none';
            }
            else {
                if (document.getElementById(id) != null) {
                    document.getElementById("ContentPlaceHolder1_txtpageredirecturl").removeAttribute("readonly");
                    if (document.getElementById("ContentPlaceHolder1_hdnredirecturl").value != '')
                        document.getElementById("ContentPlaceHolder1_txtpageredirecturl").value = document.getElementById("ContentPlaceHolder1_hdnredirecturl").value;
                    document.getElementById("ContentPlaceHolder1_tdpageredirect").style.display = '';
                }
            }
        }

    </script>
    <script type="text/javascript">
        var $j = jQuery.noConflict();
        $j(function () {

            $j('#ContentPlaceHolder1_txtNewArrivalFromDate').datetimepicker({
                showButtonPanel: true, ampm: false,
                showHour: false, showMinute: false, showSecond: false, showTime: false, showOn: "button",
                buttonImage: "/App_Themes/<%=Page.Theme %>/images/date-icon.png", buttonImageOnly: true
            });
            $j('#ContentPlaceHolder1_txtNewArrivalToDate').datetimepicker({
                showButtonPanel: true, ampm: false,
                showHour: false, showMinute: false, showSecond: false, showTime: false, showOn: "button",
                buttonImage: "/App_Themes/<%=Page.Theme %>/images/date-icon.png", buttonImageOnly: true
            });

            $j('#ContentPlaceHolder1_txtIsFeaturedFromDate').datetimepicker({
                showButtonPanel: true, ampm: false,
                showHour: false, showMinute: false, showSecond: false, showTime: false, showOn: "button",
                buttonImage: "/App_Themes/<%=Page.Theme %>/images/date-icon.png", buttonImageOnly: true
            });
            $j('#ContentPlaceHolder1_txtIsFeaturedToDate').datetimepicker({
                showButtonPanel: true, ampm: false,
                showHour: false, showMinute: false, showSecond: false, showTime: false, showOn: "button",
                buttonImage: "/App_Themes/<%=Page.Theme %>/images/date-icon.png", buttonImageOnly: true
            });


            $j('#ContentPlaceHolder1_txtBestSellerFromDate').datetimepicker({
                showButtonPanel: true, ampm: false,
                showHour: false, showMinute: false, showSecond: false, showTime: false, showOn: "button",
                buttonImage: "/App_Themes/<%=Page.Theme %>/images/date-icon.png", buttonImageOnly: true
            });
            $j('#ContentPlaceHolder1_txtBestSellerToDate').datetimepicker({
                showButtonPanel: true, ampm: false,
                showHour: false, showMinute: false, showSecond: false, showTime: false, showOn: "button",
                buttonImage: "/App_Themes/<%=Page.Theme %>/images/date-icon.png", buttonImageOnly: true
            });

            $j('#ContentPlaceHolder1_txtFreeShippingFromDate').datetimepicker({
                showButtonPanel: true, ampm: false,
                showHour: false, showMinute: false, showSecond: false, showTime: false, showOn: "button",
                buttonImage: "/App_Themes/<%=Page.Theme %>/images/date-icon.png", buttonImageOnly: true
            });
            $j('#ContentPlaceHolder1_txtFreeShippingToDate').datetimepicker({
                showButtonPanel: true, ampm: false,
                showHour: false, showMinute: false, showSecond: false, showTime: false, showOn: "button",
                buttonImage: "/App_Themes/<%=Page.Theme %>/images/date-icon.png", buttonImageOnly: true
            });

            if (document.getElementById("ContentPlaceHolder1_ddlItemType") != null) {
                if ((document.getElementById('ContentPlaceHolder1_ddlItemType').options[document.getElementById('ContentPlaceHolder1_ddlItemType').selectedIndex]).text.toLowerCase() == 'roman' && '<%=Request["ID"]%>' != null && '<%=Request["ID"]%>' != '') {
                    document.getElementById("ContentPlaceHolder1_chkIsRoman").checked = true;
                    //document.getElementById("ContentPlaceHolder1_divRomanShadeYard").style.display = "";
                    document.getElementById("ContentPlaceHolder1_divRomanShadeYard").style.display = "none";
                    document.getElementById("ContentPlaceHolder1_divRomanShadeYard").style.display = "none";
                    document.getElementById("liMenu11").style.display = "block";

                }
            }
        });
        function validatefields() {
            if (document.getElementById('ContentPlaceHolder1_txtminwidth').value == '') {
                jAlert('Please Enter Min Width Value.', 'Required Information', 'txtminwidth');
                return false;
            }
            if (document.getElementById('ContentPlaceHolder1_txtmaxwidth').value == '') {
                jAlert('Please Enter Max Width Value.', 'Required Information', 'txtmaxwidth');
                return false;
            }
            if (document.getElementById('ContentPlaceHolder1_txtminlength').value == '') {
                jAlert('Please Enter Min Length Value.', 'Required Information', 'txtminlength');
                return false;
            }
            if (document.getElementById('ContentPlaceHolder1_txtmaxlength').value == '') {
                jAlert('Please Enter Max Length Value.', 'Required Information', 'txtmaxlength');
                return false;
            }
            return true;
        }
        function changecanonical() {
            if (document.getElementById("ContentPlaceHolder1_chkcanonical").checked == true) {
                document.getElementById("ContentPlaceHolder1_txtcanonical").removeAttribute("readonly");
            }
            else {
                $('#ContentPlaceHolder1_txtcanonical').attr('readonly', 'true');
            }
        }
        function openCenteredCrossSaleWindowShadeSuggestedRetail() {
            var width = 600;
            var height = 400;
            var left = parseInt((screen.availWidth / 2) - (width / 2));
            var top = parseInt((screen.availHeight / 2) - (height / 2));

            var windowFeatures = "width=" + width + ",height=" + height + ",status,resizable,scrollbars=yes,left=" + left + ",top=" + top + "screenX=" + left + ",screenY=" + top;
            var ProductID = '<%=Request.QueryString["ID"]%>';
            myWindow = window.open('EditSuggestedRetail.aspx?&ProductID=' + ProductID, "subWind", windowFeatures);
            //}
        }
        function openCenteredCrossSaleWindowShadeMarkUp() {
            var width = 600;
            var height = 400;
            var left = parseInt((screen.availWidth / 2) - (width / 2));
            var top = parseInt((screen.availHeight / 2) - (height / 2));
            var ProductID = '<%=Request.QueryString["ID"]%>';
            var windowFeatures = "width=" + width + ",height=" + height + ",status,resizable,scrollbars=yes,left=" + left + ",top=" + top + "screenX=" + left + ",screenY=" + top;

            myWindow = window.open('EditShadeMarkup.aspx?&ProductID=' + ProductID, "subWind", windowFeatures);


            //}
        }
        function Texboxfabricwidthchange(id) {

            if (id != null && id != '') {
                var txtfabricwidth = $('#' + id).val();
                var divdedvalue = 0.00;

                var lblid = id.replace('txtFabricwidth', 'lblStylename');
                var divededid = id.replace('txtFabricwidth', 'txtdividedwidth');


                var lblStylename = $('#' + lblid).text().toLowerCase();
                if (lblStylename != '' && (lblStylename == "pole pocket" || lblStylename == "polepocket" || lblStylename == "pole pocket with back tabs & hook belt" || lblStylename == "pole pocket with hook belt")) {
                    if (txtfabricwidth != null && txtfabricwidth != '') {
                        divdedvalue = txtfabricwidth / 1;

                        $('#' + divededid).val(divdedvalue);

                    }
                }
                else {
                    if (txtfabricwidth != null && txtfabricwidth != '') {
                        divdedvalue = txtfabricwidth / 2;

                        $('#' + divededid).val(divdedvalue);
                    }
                }
            }

        }
        function CheckboxChange(id) {
            switch (id) {
                case 'ContentPlaceHolder1_chkisreadymade':
                    if (document.getElementById("ContentPlaceHolder1_chkisreadymade").checked == true) {
                        document.getElementById("ContentPlaceHolder1_chkisordermade").checked = false;
                    }
                    break;
                case 'ContentPlaceHolder1_chkisordermade':
                    if (document.getElementById("ContentPlaceHolder1_chkisordermade").checked == true) {
                        document.getElementById("ContentPlaceHolder1_chkisreadymade").checked = false;
                    }
                    break;
                case 'ContentPlaceHolder1_chkismadetoswatch':
                    if (document.getElementById("ContentPlaceHolder1_chkismadetoswatch").checked == true) {
                        document.getElementById("ContentPlaceHolder1_divProductSwatch").style.display = "block";
                    }
                    else {
                        document.getElementById("ContentPlaceHolder1_divProductSwatch").style.display = "none";
                        if (document.getElementById("ContentPlaceHolder1_txtProductSwatchId") != null) {
                            document.getElementById("ContentPlaceHolder1_txtProductSwatchId").value = '';
                        }
                    }
                    document.getElementById("ContentPlaceHolder1_chkIsfreefabricswatch").checked = document.getElementById("ContentPlaceHolder1_chkismadetoswatch").checked;
                    break;
                default:
            }
        }

        function SetItemtype() {
            if (document.getElementById("ContentPlaceHolder1_ddlItemType") != null) {
                if ((document.getElementById('ContentPlaceHolder1_ddlItemType').options[document.getElementById('ContentPlaceHolder1_ddlItemType').selectedIndex]).text.toLowerCase() == 'roman' && '<%=Request["ID"]%>' != null && '<%=Request["ID"]%>' != '') {

                    document.getElementById("ContentPlaceHolder1_chkIsRoman").checked = true;
                    //document.getElementById("ContentPlaceHolder1_divRomanShadeYard").style.display = "";
                    document.getElementById("ContentPlaceHolder1_divRomanShadeYard").style.display = "none";
                    document.getElementById("ContentPlaceHolder1_divRomanShadeYard").style.display = "none";
                    document.getElementById("liMenu11").style.display = "block";

                }
                else if ((document.getElementById('ContentPlaceHolder1_ddlItemType').options[document.getElementById('ContentPlaceHolder1_ddlItemType').selectedIndex]).text.toLowerCase() == 'roman') {
                    document.getElementById("ContentPlaceHolder1_chkIsRoman").checked = true;
                    //document.getElementById("ContentPlaceHolder1_divRomanShadeYard").style.display = "";
                    document.getElementById("ContentPlaceHolder1_divRomanShadeYard").style.display = "none";
                    document.getElementById("ContentPlaceHolder1_divRomanShadeYard").style.display = "none";

                    document.getElementById("liMenu11").style.display = "none";
                }
                else {
                    document.getElementById("ContentPlaceHolder1_chkIsRoman").checked = false;
                    document.getElementById("ContentPlaceHolder1_divRomanShadeYard").style.display = "none";
                    document.getElementById('ContentPlaceHolder1_ddlRomanShadeYardage').selectedIndex = 0;
                    document.getElementById("liMenu11").style.display = "none";
                }
            }
        }

        function CheckboxChangeReturn(id) {
            switch (id) {
                case 'ContentPlaceHolder1_chkReadyMadeReturn':
                    if (document.getElementById("ContentPlaceHolder1_chkReadyMadeReturn").checked == true) {
                        document.getElementById("ContentPlaceHolder1_chkMadeToOrderReturn").checked = false;
                    }
                    break;
                case 'ContentPlaceHolder1_chkMadeToOrderReturn':
                    if (document.getElementById("ContentPlaceHolder1_chkMadeToOrderReturn").checked == true) {
                        document.getElementById("ContentPlaceHolder1_chkReadyMadeReturn").checked = false;
                    }
                    break;

                default:
            }
        }


    </script>
    <style type="text/css">
        .divshadecalc table {
            border-collapse: collapse;
            /*border-top: 1px solid grey;*/
        }



        .divshadecalc td {
            margin: 0;
            border-collapse: collapse;
            border: 1px solid #eee;
            /* border-top-width: 0px;*/
            white-space: nowrap;
        }



        .divshadecalc div {
            width: 1024px;
            overflow-x: auto;
            margin-left: 4.5%;
            overflow-y: auto;
            padding-bottom: 1px;
        }



        .headcol {
            position: absolute;
            width: 5em;
            left: 5px;
            margin-left: 3.2% !important;
            top: auto;
            border-right: 0px none black;
            border-top-width: 1px; /*only relevant for first row*/
            margin-top: -1px !important; /*compensate for top border*/
            background-color: grey !important;
            color: #fff;
        }



        /*.headcol:before {

                content: 'Row ';

            }*/



        .long {
            background-color: grey !important;
            color: #fff;
            border: 1px solid #fff;
            /*letter-spacing: 1em;*/
        }
    </style>
    <style type="text/css">
        #ContentPlaceHolder1_ddlColors {
            margin: 10px 50px;
        }

            #ContentPlaceHolder1_ddlColors tr td {
                position: relative;
                height: 110px;
                width: 135px;
                text-align: center;
                float: left;
            }

            #ContentPlaceHolder1_ddlColors label {
                position: absolute;
                top: 10px;
                left: 0px;
                width: 100%;
            }

            #ContentPlaceHolder1_ddlColors input {
                position: absolute;
                top: 70px;
                left: 65px;
            }

            #ContentPlaceHolder1_ddlColors label span {
                position: absolute;
                top: 80px;
                left: 0px;
                text-transform: uppercase;
                width: 95%;
                float: left;
                text-align: center;
            }

        #ContentPlaceHolder1_ddlStyle {
            margin: 10px 50px;
        }

            #ContentPlaceHolder1_ddlStyle tr td {
                position: relative;
                height: 110px;
                width: 135px;
                text-align: center;
                float: left;
            }

            #ContentPlaceHolder1_ddlStyle label {
                position: absolute;
                top: 10px;
                left: 0px;
                width: 100%;
            }

            #ContentPlaceHolder1_ddlStyle input {
                position: absolute;
                top: 70px;
                left: 65px;
            }

            #ContentPlaceHolder1_ddlStyle label span {
                position: absolute;
                top: 80px;
                left: 0px;
                text-transform: uppercase;
                width: 95%;
                float: left;
                text-align: center;
            }

        #ContentPlaceHolder1_ddlNewStyle {
            margin: 10px 50px;
        }

            #ContentPlaceHolder1_ddlNewStyle tr td {
                position: relative;
                height: 110px;
                width: 135px;
                text-align: center;
                float: left;
            }

            #ContentPlaceHolder1_ddlNewStyle label {
                position: absolute;
                top: 10px;
                left: 0px;
                width: 100%;
            }

            #ContentPlaceHolder1_ddlNewStyle input {
                position: absolute;
                top: 70px;
                left: 65px;
            }

            #ContentPlaceHolder1_ddlNewStyle label span {
                position: absolute;
                top: 80px;
                left: 0px;
                text-transform: uppercase;
                width: 95%;
                float: left;
                text-align: center;
            }

        #ContentPlaceHolder1_chkHeader {
            margin: 10px 50px;
        }

            #ContentPlaceHolder1_chkHeader tr td {
                position: relative;
                height: 110px;
                width: 135px;
                text-align: center;
                float: left;
            }

            #ContentPlaceHolder1_chkHeader label {
                position: absolute;
                top: 10px;
                left: 0px;
                width: 100%;
            }

            #ContentPlaceHolder1_chkHeader input {
                position: absolute;
                top: 70px;
                left: 65px;
            }

            #ContentPlaceHolder1_chkHeader label span {
                position: absolute;
                top: 80px;
                left: 0px;
                text-transform: uppercase;
                width: 95%;
                float: left;
                text-align: center;
            }

        #ContentPlaceHolder1_ddlFab {
            margin: 10px 50px;
        }

            #ContentPlaceHolder1_ddlFab tr td {
                position: relative;
                height: 110px;
                width: 135px;
                text-align: center;
                float: left;
            }

            #ContentPlaceHolder1_ddlFab label {
                position: absolute;
                top: 10px;
                left: 0px;
                width: 100%;
            }

            #ContentPlaceHolder1_ddlFab input {
                position: absolute;
                top: 70px;
                left: 65px;
            }

            #ContentPlaceHolder1_ddlFab label span {
                position: absolute;
                top: 80px;
                left: 0px;
                text-transform: uppercase;
                width: 95%;
                float: left;
                text-align: center;
            }

        #ContentPlaceHolder1_ddlPatt {
            margin: 10px 50px;
        }

            #ContentPlaceHolder1_ddlPatt tr td {
                position: relative;
                height: 110px;
                width: 135px;
                text-align: center;
                float: left;
            }

            #ContentPlaceHolder1_ddlPatt label {
                position: absolute;
                top: 10px;
                left: 0px;
                width: 100%;
            }

            #ContentPlaceHolder1_ddlPatt input {
                position: absolute;
                top: 70px;
                left: 65px;
            }

            #ContentPlaceHolder1_ddlPatt label span {
                position: absolute;
                top: 80px;
                left: 0px;
                text-transform: uppercase;
                width: 95%;
                float: left;
                text-align: center;
            }

        #ContentPlaceHolder1_ddlRoom {
            margin: 10px 50px;
        }

            #ContentPlaceHolder1_ddlRoom tr td {
                position: relative;
                height: 110px;
                width: 135px;
                text-align: center;
                float: left;
            }

            #ContentPlaceHolder1_ddlRoom label {
                position: absolute;
                top: 10px;
                left: 0px;
                width: 100%;
            }

            #ContentPlaceHolder1_ddlRoom input {
                position: absolute;
                top: 70px;
                left: 65px;
            }

            #ContentPlaceHolder1_ddlRoom label span {
                position: absolute;
                top: 80px;
                left: 0px;
                text-transform: uppercase;
                width: 95%;
                float: left;
                text-align: center;
            }

        #tab-container-product ul.menu li {
            margin-bottom: 0;
        }

        #tab-container ul.menu li {
            margin-bottom: -1px;
        }

        #tab-container-1 ul.menu li {
            margin-bottom: -1px;
        }

        .slidingDiv {
            height: 300px;
            padding: 20px;
            margin-top: 10px;
        }

        .show_hide {
            display: block;
        }

        .footerBorder {
            border-top: 1px solid #DFDFDF;
            border-right: 1px solid #DFDFDF;
        }

        .footerBorderinventory {
            border-top: 1px solid #DFDFDF;
        }

        .divfloatingcss {
            bottom: 0;
            right: 0;
            position: fixed;
            width: 14%;
            margin-right: 43%;
            background-image: url("/Admin/images/title-bg-trans.png");
            -webkit-border-top-left-radius: 15px;
            -webkit-border-top-right-radius: 15px;
            -moz-border-radius-topleft: 15px;
            -moz-border-radius-topright: 15px;
            border-top-left-radius: 15px;
            border-top-right-radius: 15px;
        }
    </style>

    <script type="text/javascript">
        $(document).ready(function () {
            $('#divfloating').attr("class", "divfloatingcss");
            $(window).scroll(function () {
                if ($(window).scrollTop() == $(document).height() - $(window).height()) {
                    $('#divfloating').attr("class", "");
                }
                else {
                    $('#divfloating').attr("class", "divfloatingcss");
                }
            });
        });
    </script>
    <script type="text/javascript">
        function changereadonly() {
            document.getElementById('ContentPlaceHolder1_txtSKU').removeAttribute('readonly');

            document.getElementById('ContentPlaceHolder1_txtSKU').style.backgroundColor = '#FFFFBA';

            document.getElementById('ContentPlaceHolder1_hdnskuedit').value = '1';

        }
        function SetHammingValue() {
            if (document.getElementById('ContentPlaceHolder1_chkIsHamming') != null && document.getElementById('ContentPlaceHolder1_chkIsHamming').checked == true) {
                document.getElementById('ContentPlaceHolder1_tdHamminglbl').style.display = '';
                document.getElementById('ContentPlaceHolder1_tdHammingQty').style.display = '';
            }
            else {
                document.getElementById('ContentPlaceHolder1_tdHamminglbl').style.display = 'none';
                document.getElementById('ContentPlaceHolder1_tdHammingQty').style.display = 'none';
                document.getElementById('ContentPlaceHolder1_txtHammingQty').value = '0';
            }
        }

        function SelectSingleRadiobutton(rdbtnid) {
            var rdBtn = document.getElementById(rdbtnid);
            var rdBtnList = document.getElementsByTagName("input");
            for (i = 0; i < rdBtnList.length; i++) {
                if (rdBtnList[i].type == "radio" && rdBtnList[i].id != rdBtn.id) {
                    rdBtnList[i].checked = false;
                }
            }
        }

        function ChkSecondaryColorValidate() {
            var allElts = document.getElementById('ContentPlaceHolder1_grdSecondaryColor').getElementsByTagName('input')
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
                jAlert('Select at least one Record(s) !', 'Message');
                return false;
            }
            return true;
        }

        //        function TabdisplayProduct(mode, tab) {

        //            var divid = "divtab" + tab.toString()

        //            if (mode = 1) {

        //                if (document.getElementById(divid) != null) {
        //                    document.getElementById(divid).style.display = 'none';
        //                }

        //            }
        //            else {
        //                if (document.getElementById(divid) != null) {
        //                    document.getElementById(divid).style.display = 'block';
        //                }
        //            }
        //        }


        function Tabdisplay(id) {
            document.getElementById('ContentPlaceHolder1_hdnTabid').value = id;
            for (var i = 5; i < 15; i++) {

                var divid = "divtab" + i.toString()
                var liid = "liMenu" + i.toString()
                if (document.getElementById(divid) != null && ('divtab' + id == divid)) {
                    document.getElementById(divid).style.display = '';
                }
                else {
                    if (document.getElementById(divid) != null) {
                        document.getElementById(divid).style.display = 'none';
                    }
                }
                if (document.getElementById(liid) && ('liMenu' + id == liid)) {
                    document.getElementById(liid).className = 'active';
                }
                else {
                    if (document.getElementById(liid) != null) {
                        document.getElementById(liid).className = '';
                    }
                }
            }
        }

        function SubTabdisplay(id) {
            document.getElementById('ContentPlaceHolder1_hdnSubTabid').value = id;
            for (var i = 1; i < 15; i++) {
                var divid = "divSubTab" + i.toString()
                var liid = "lisubMenu" + i.toString()
                if (document.getElementById(divid) != null && ('divSubTab' + id == divid)) {
                    document.getElementById(divid).style.display = '';
                }
                else {
                    if (document.getElementById(divid) != null) {
                        document.getElementById(divid).style.display = 'none';
                    }
                }
                if (document.getElementById(liid) && ('lisubMenu' + id == liid)) {
                    document.getElementById(liid).className = 'active';
                }
                else {
                    if (document.getElementById(liid) != null) {
                        document.getElementById(liid).className = '';
                    }
                }
            }
        }
    </script>
    <script type="text/javascript">
        function iframeAutoheight(iframe) {

            var height = iframe.contentWindow.document.body.scrollHeight;

            iframe.height = height + 30;
        }
        function iframeAutoheightById(iframe, height1) {
            //var height = document.getElementById(iframe).contentWindow.document.body.scrollHeight;
            //document.getElementById(iframe).src = document.getElementById(iframe).src;
            //var hgt = $(window).height();

            height1 = parseInt(height1) - parseInt(100);
            $('#' + iframe).css('height', height1.toString() + 'px');
            //$('#div5').css('height', height1.toString() + 'px');

            //document.getElementById(iframe).height = height + 30;
        }

        function checkvalidationsearchtype(btnid) {

            var checkboxid = btnid.replace('_btnSave_', '_chkActive_');
            var textbox1id = btnid.replace('_btnSave_', '_txtAdditionalPrice_');
            var textbox2id = btnid.replace('_btnSave_', '_txtPerInch_');
            //if (document.getElementById(checkboxid) != null && document.getElementById(checkboxid).checked == false) {
            //    jAlert('Please Select Style !', 'Message', checkboxid);
            //    return false;
            //}
            if (document.getElementById(textbox1id) != null && document.getElementById(textbox1id).value == '') {
                jAlert('Please Enter Price !', 'Message', textbox1id);
                return false;
            }
            else if (document.getElementById(textbox2id) != null && document.getElementById(textbox2id).value == '') {
                jAlert('Please Enter Price !', 'Message', textbox2id);
                return false;
            }
            return true;
        }
        function chkselectcheckbox() {
            var allElts = document.getElementById('ContentPlaceHolder1_grdProductStyleType').getElementsByTagName('input')
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

                jAlert('Select at least one Style !', 'Message');
                return false;
            }
            return true;
        }

        function SelectDivValue() {
            if (document.getElementById('ContentPlaceHolder1_chkIsProperty') != null && document.getElementById('ContentPlaceHolder1_chkIsProperty').checked == true) {
                document.getElementById('ContentPlaceHolder1_divproperty').style.display = '';
            }
            else { document.getElementById('ContentPlaceHolder1_divproperty').style.display = 'none'; }
        }

        function SelectDivRomanYardValue() {
            if (document.getElementById('ContentPlaceHolder1_chkIsRoman') != null && document.getElementById('ContentPlaceHolder1_chkIsRoman').checked == true) {
                document.getElementById('ContentPlaceHolder1_divRomanShadeYard').style.display = '';
            }
            else { document.getElementById('ContentPlaceHolder1_divRomanShadeYard').style.display = 'none'; }
        }


        function SelectDivFreeShippingValue() {
            if (document.getElementById('ContentPlaceHolder1_chkIsFreeShipping') != null && document.getElementById('ContentPlaceHolder1_chkIsFreeShipping').checked == true) {
                document.getElementById('ContentPlaceHolder1_DiveFreeShipping').style.display = '';
            }
            else { document.getElementById('ContentPlaceHolder1_DiveFreeShipping').style.display = 'none'; }
        }

        function SelectDivFeaturedValue() {
            if (document.getElementById('ContentPlaceHolder1_chkFeatured') != null && document.getElementById('ContentPlaceHolder1_chkFeatured').checked == true) {
                document.getElementById('ContentPlaceHolder1_DivFeatured').style.display = '';
            }
            else { document.getElementById('ContentPlaceHolder1_DivFeatured').style.display = 'none'; }
        }

        function SelectDivNewArrivalValue() {
            if (document.getElementById('ContentPlaceHolder1_chkNewArrival') != null && document.getElementById('ContentPlaceHolder1_chkNewArrival').checked == true) {
                document.getElementById('ContentPlaceHolder1_DivNewArrival').style.display = '';
            }
            else { document.getElementById('ContentPlaceHolder1_DivNewArrival').style.display = 'none'; }
        }

        function SelectDivBestSellerValue() {
            if (document.getElementById('ContentPlaceHolder1_chkIsBestSeller') != null && document.getElementById('ContentPlaceHolder1_chkIsBestSeller').checked == true) {
                document.getElementById('ContentPlaceHolder1_DivBestSeller').style.display = '';
            }
            else { document.getElementById('ContentPlaceHolder1_DivBestSeller').style.display = 'none'; }
        }

        function CloneVisible() {
            if (document.getElementById('ContentPlaceHolder1_trCloneProductMsg') != null) {
                document.getElementById('ContentPlaceHolder1_trCloneProductMsg').style.display = 'none';
                document.getElementById('ContentPlaceHolder1_trProductData').style.display = '';
                document.getElementById('ContentPlaceHolder1_trProductData').style.display = 'block';
            }
        }
        function btnCheck(inv, btid) {
            var r = document.getElementById(inv).value;
            // alert('sam' +'---- '+r);
            if (r <= 0) {
                alert('Enter Valid Quantity!');
            }
        }

    </script>
    <script type="text/javascript">
        var myWindow;
        function openCenteredCrossSaleWindow(x) {
            createCookie('prskus', document.getElementById(x).value, 1)
            var width = 900;
            var height = 600;
            var left = parseInt((screen.availWidth / 2) - (width / 2));
            var top = parseInt((screen.availHeight / 2) - (height / 2));
            var StoreID = '<%=Request.QueryString["StoreID"]%>';
            var ProductID = '<%=Request.QueryString["ID"]%>';
            var windowFeatures = "width=" + width + ",height=" + height + ",status,resizable,scrollbars=yes,left=" + left + ",top=" + top + "screenX=" + left + ",screenY=" + top;
            myWindow = window.open('ProductSku.aspx?StoreID=' + StoreID + '&ProductID=' + ProductID + '&clientid=' + x, "subWind", windowFeatures);
        }

        function openCenteredCrossSaleWindowDouble(x) {
            createCookie('prskus', document.getElementById(x).value, 1)
            var width = 900;
            var height = 600;
            var left = parseInt((screen.availWidth / 2) - (width / 2));
            var top = parseInt((screen.availHeight / 2) - (height / 2));
            var StoreID = '<%=Request.QueryString["StoreID"]%>';
            var ProductID = '<%=Request.QueryString["ID"]%>';
            var windowFeatures = "width=" + width + ",height=" + height + ",status,resizable,scrollbars=yes,left=" + left + ",top=" + top + "screenX=" + left + ",screenY=" + top;
            myWindow = window.open('ProductSku.aspx?StoreID=' + StoreID + '&ProductID=' + ProductID + '&clientid=' + x, "subWind", windowFeatures);
        }

        function openCenteredCrossSaleWindowOptionalAcc(x) {
            createCookie('prskus', document.getElementById(x).value, 1)
            var width = 900;
            var height = 600;
            var left = parseInt((screen.availWidth / 2) - (width / 2));
            var top = parseInt((screen.availHeight / 2) - (height / 2));
            var StoreID = '<%=Request.QueryString["StoreID"]%>';
            var ProductID = '<%=Request.QueryString["ID"]%>';
            var windowFeatures = "width=" + width + ",height=" + height + ",status,resizable,scrollbars=yes,left=" + left + ",top=" + top + "screenX=" + left + ",screenY=" + top;
            myWindow = window.open('OptionalAccessoriesPopup.aspx?StoreID=' + StoreID + '&ProductID=' + ProductID + '&clientid=' + x, "subWind", windowFeatures);
        }

        function openCenteredCrossSaleWindowforProductSwatch(x) {
            createCookie('prskus', document.getElementById(x).value, 1)
            var width = 900;
            var height = 600;
            var left = parseInt((screen.availWidth / 2) - (width / 2));
            var top = parseInt((screen.availHeight / 2) - (height / 2));
            var StoreID = '<%=Request.QueryString["StoreID"]%>';
            var ProductID = '<%=Request.QueryString["ID"]%>';
            var vname = document.getElementById('ContentPlaceHolder1_txtProductSwatchId').value;
            var windowFeatures = "width=" + width + ",height=" + height + ",status,resizable,scrollbars=yes,left=" + left + ",top=" + top + "screenX=" + left + ",screenY=" + top;
            myWindow = window.open('ProductSwatchSKU.aspx?StoreID=' + StoreID + '&ProductID=' + ProductID + '&clientid=' + x + '&vname=' + vname, "subWind", windowFeatures);
        }

        function createCookie(name, value, days) {
            if (days) {
                var date = new Date();
                date.setTime(date.getTime() + (days * 24 * 60 * 60 * 1000));
                var expires = "; expires=" + date.toGMTString();
            }
            else var expires = "";
            document.cookie = name + "=" + value + expires + "; path=/";
        }

        function OpenProductOption() {
            var productid = '<%=Request["Id"] %>'
            if (productid != '' && productid != null) {
                window.location.href = 'ProductVariant.aspx?StoreId=<%=Request["StoreID"] %>&ID=<%=Request["ID"] %>';
            }
        }

        function OpenMoreImagesPopup() {
            var popupurl = "MoreImagesUpload.aspx?StoreID=<%=Request["StoreID"] %>&ID=<%=Request["ID"] %>";
            window.open(popupurl, "MoreIamgesPopup", "toolbar=no,location=no,directories=no,status=no,menubar=no,scrollbars=yes,resizable=yes,width=800,height=550,left=250,top=80");
        }

        function SaleClerance() {
            if (document.getElementById('ContentPlaceHolder1_chkSaleclearance').checked == true) {
                document.getElementById('ContentPlaceHolder1_txtSaleClearance').style.display = '';
            }
            else {
                document.getElementById('ContentPlaceHolder1_txtSaleClearance').style.display = 'none';
            }
        }

        function ShowHideButton(id, number) {
            var imgsrc = document.getElementById(id).src;

            if (imgsrc.toLowerCase().indexOf('minimize.png') > -1) {
                document.getElementById(id).src = imgsrc.toLowerCase().replace('minimize.png', 'expand.gif');
                document.getElementById(id).title = 'Show';
                document.getElementById(id).alt = 'Show';
                document.getElementById(id).ClassName = 'close';
                document.getElementById(number).style.paddingTop = "8px";
                document.getElementById(number).style.border = "none";
                document.getElementById(number).style.paddingBottom = "8px";
            }
            else if (imgsrc.toLowerCase().indexOf('expand.gif') > -1) {
                document.getElementById(id).src = imgsrc.toLowerCase().replace('expand.gif', 'minimize.png');
                document.getElementById(id).title = 'Minimize';
                document.getElementById(id).alt = 'Minimize';
                document.getElementById(id).ClassName = 'minimize';
                document.getElementById(number).style.paddingTop = "0px";
                document.getElementById(number).style.border = "none";
                document.getElementById(number).style.paddingBottom = "0px";

            }
        }

        function ChangeVendorSKU(id, controlname, controlmain) {
            document.getElementById("ContentPlaceHolder1_hdnid").value = id;
            document.getElementById("ContentPlaceHolder1_hdncontrol").value = controlname;
            document.getElementById("ContentPlaceHolder1_hdnmaincontrol").value = controlmain;
        }

        function openCenteredCrossSaleWindow1(mode) {
            var StoreID = '<%=Request.QueryString["StoreID"]%>';
            if (StoreID.value != "0") {
                var width = 700;
                var height = 500;
                var left = parseInt((screen.availWidth / 2) - (width / 2));
                var top = parseInt((screen.availHeight / 2) - (height / 2));

                var windowFeatures = "width=" + width + ",height=" + height + ",status=no,resizable=yes,scrollbars=yes,left=" + left + ",top=" + top + ",screenX=" + left + ",screenY=" + top;
                window.open('DropShipperPopUp.aspx?StoreID=' + StoreID + '&mode=' + mode, "Mywindow", windowFeatures);
                return false;
            }
            else {
                jAlert('Select Store!', 'Message', StoreID);
                return false;
            }
        }
        function openCenteredCrossSaleWindow2(mode, hdnQtyname) {
            var StoreID = '<%=Request.QueryString["StoreID"]%>';
            if (StoreID.value != "0") {
                var width = 700;
                var height = 500;
                var left = parseInt((screen.availWidth / 2) - (width / 2));
                var top = parseInt((screen.availHeight / 2) - (height / 2));

                var windowFeatures = "width=" + width + ",height=" + height + ",status=no,resizable=yes,scrollbars=yes,left=" + left + ",top=" + top + ",screenX=" + left + ",screenY=" + top;
                window.open('AssemblerProductPopUp.aspx?StoreID=' + StoreID + '&mode=' + mode + '&qtymode=' + hdnQtyname, "Mywindow", windowFeatures);
                return false;
            }
            else {
                jAlert('Select Store!', 'Message', StoreID);
                return false;
            }
        }

    </script>
    <script language="javascript" type="text/javascript">
        function ConfirmDelete() {
            jConfirm('Are you sure want to delete this Video ?', 'Confirmation', function (r) {
                if (r == true) {
                    document.getElementById('ContentPlaceHolder1_btndeleteVideoTemp').click();
                    return true;
                }
                else {

                    return false;
                }
            });
            return false;
        }
    </script>
    <script type="text/javascript">

        $(document).ready(function () {

            $(".slidingDiv").show();
            $(".show_hide").show();

            $('.show_hide').click(function () {
                $(".slidingDiv").slideToggle();
            });

        });

        $(document).ready(function () {

            $(".slidingDivImage").show();
            $(".show_hideImage").show();

            $('.show_hideImage').click(function () {
                $(".slidingDivImage").slideToggle();
            });
        });

        $(document).ready(function () {

            $(".slidingDivSEO").show();
            $(".show_hideSEO").show();

            $('.show_hideSEO').click(function () {
                $(".slidingDivSEO").slideToggle();
            });
        });

        $(document).ready(function () {

            $(".slidingDivCategory").show();
            $(".show_hideCategory").show();

            $('.show_hideCategory').click(function () {
                $(".slidingDivCategory").slideToggle();
            });
        });

        $(document).ready(function () {

            $(".slidingDivWarehouse").show();
            $(".show_hideWarehouse").show();

            $('.show_hideWarehouse').click(function () {
                $(".slidingDivWarehouse").slideToggle();
            });
        });

        $(document).ready(function () {

            $(".slidingDivDesc").show();
            $(".show_hideDesc").show();

            $('.show_hideDesc').click(function () {

                $(".slidingDivDesc").slideToggle();
            });
        });
        $(document).ready(function () {

            $(".slidingoptionDivDesc").show();
            $(".show_optionhideDesc").show();

            $('.show_optionhideDesc').click(function () {

                $(".slidingoptionDivDesc").slideToggle();
            });
        });
        $(document).ready(function () {

            $(".slidingDivMainDiv").show();
            $(".show_hideMainDiv").show();

            $('.show_hideMainDiv').click(function () {
                $(".slidingDivMainDiv").slideToggle();
            });
        });

        $(document).ready(function () {

            $(".slidingDivProductDesc").show();
            $(".show_hideProductDesc").show();

            $('.show_hideProductDesc').click(function () {
                $(".slidingDivProductDesc").slideToggle();
            });
        });

        $(document).ready(function () {

            $(".proslidingDivDesc").show();
            $(".ProslidingDivDesc_a").show();

            $('.ProslidingDivDesc_a').click(function () {
                $(".proslidingDivDesc").slideToggle();
            });
        });

    </script>
    <script type="text/javascript">
        function keyRestrictEngraving(e, validchars) {
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
    </script>
    <script type="text/javascript">
        function keyRestrictForInventory(e, validchars) {
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
    </script>
    <script type="text/javascript">
        function SetTotalWeight() {

            var totalWeight = 0;
            var Weight = 0;
            for (var i = 0; i < 20; i++) {
                if (document.getElementById('ContentPlaceHolder1_grdWarehouse_txtInventory_' + i)) {
                    if (document.getElementById('ContentPlaceHolder1_grdWarehouse_txtInventory_' + i).value == '') {
                        Weight = 0;
                    }
                    else {
                        Weight = parseInt(document.getElementById('ContentPlaceHolder1_grdWarehouse_txtInventory_' + i).value);
                    }
                    totalWeight += Weight;
                }
            }
            document.getElementById('ContentPlaceHolder1_grdWarehouse_lblTotal').innerHTML = totalWeight;
            document.getElementById('ContentPlaceHolder1_txtInventory').value = totalWeight;
        }


        function SettoggetinInventory() {

            if ((document.getElementById('ContentPlaceHolder1_txtInventory')) < (document.getElementById('ContentPlaceHolder1_txtLowInventory'))) {
                document.getElementById('ContentPlaceHolder1_divsettooltip').style.display = 'block';
            }
            else {
                document.getElementById('ContentPlaceHolder1_divsettooltip').style.display = 'none';
            }
            document.getElementById('ContentPlaceHolder1_txtInventory').focus();

        }


        //        function PrintBarcode() {
        //            if (document.getElementById('divBarcodePrint')) {
        //                var pri = document.getElementById("ifmcontentstoprint").contentWindow;
        //                pri.document.open();
        //                var contentAll = document.getElementById("divBarcodePrint").innerHTML;
        //                pri.document.write(contentAll);
        //                pri.document.close();
        //                pri.focus();
        //                pri.print();
        //            }
        //            return false;
        //        }

        function PrintBarcode() {
            if (document.getElementById('divBarcodePrint')) {
                var BrowserName = navigator.appName.toString();

                if (BrowserName.toString().toLowerCase().indexOf('internet explorer') > -1) {
                    w = window.open('', 'msg', 'directories=no, location=no, menubar=no, status=yes,titlebar=no,toolbar=no,scrollbars=no,resizable=no,Height=10,Width=10,left=0,top=0,visible=false,alwaysLowered=yes');
                    w.document.write(document.getElementById("divBarcodePrint").innerHTML);
                    w.document.close();
                    w.print();
                    w.close();
                }
                else {
                    var pri = document.getElementById("ifmcontentstoprint").contentWindow;
                    pri.document.open();
                    var contentAll = document.getElementById("divBarcodePrint").innerHTML;
                    pri.document.write(contentAll);
                    pri.document.close();
                    pri.focus();
                    pri.print();
                }
            }
            return false;
        }
        function TabdisplayProduct(id) {
            for (var i = 1; i <= 4; i++) {

                var divid = "divtab" + i.toString()
                var liid = "lip" + i.toString()
                if (document.getElementById(divid) != null && ('divtab' + id == divid)) {
                    document.getElementById(divid).style.display = '';
                }
                else {
                    if (document.getElementById(divid) != null) {
                        document.getElementById(divid).style.display = 'none';
                    }
                }
                if (document.getElementById(liid) && ('lip' + id == liid)) {
                    document.getElementById(liid).className = 'active';
                    //$("#" + liid + "").addClass("active")
                }
                else {
                    if (document.getElementById(liid) != null) {
                        document.getElementById(liid).className = '';
                        // $("#" + liid + "").removeClass("active")
                    }
                }
            }

        }
    </script>
    <asp:ScriptManager ID="sm1" runat="server">
    </asp:ScriptManager>
    <div class="content-row1">
        <div class="create-new-order">
            &nbsp;
        </div>
    </div>
    <div class="content-row2">
        <table width="100%" border="0" cellspacing="0" cellpadding="0">
            <tr>
                <td height="5" align="left" valign="top">
                    <img alt="" src="/App_Themes/<%=Page.Theme %>/images/spacer.gif" width="1" height="5" />
                </td>
            </tr>
            <tr>
                <td height="5" align="left" valign="top">
                    <img alt="" src="/App_Themes/<%=Page.Theme %>/images/spacer.gif" width="1" height="5" />
                </td>
            </tr>
            <tr id="trProductOption" runat="server" style="display: none;" align="right">
                <td>
                    <a href="#" title="Product Option" onclick="return OpenProductOption();">
                        <img src="/App_Themes/<%=Page.Theme %>/images/product-options.gif" alt="Product Options"
                            title="Product Options" class="img-right" height="23" />
                    </a>
                </td>
            </tr>
            <tr id="trProductData" runat="server">
                <td class="border-td">
                    <table width="100%" border="0" cellpadding="0" cellspacing="0" bgcolor="#FFFFFF"
                        class="content-table">
                        <tr>
                            <td class="border-td-sub">
                                <table width="100%" border="0" cellpadding="0" cellspacing="0" class="add-product">
                                    <tr style="width: 1126px;">
                                        <th>
                                            <div class="main-title-left">
                                                <img class="img-left" title="Add Product" alt="Add Product" src="/App_Themes/<%=Page.Theme %>/images/add-product-icon.png" />
                                                <h2 style="padding-top: 3px;">Add Product</h2>
                                            </div>
                                            <div class="main-title-right">
                                                <a href="javascript:void(0);" class="show_hideMainDiv" onclick="return ShowHideButton('imgMainDiv','tdMainDiv');">
                                                    <img id="imgMainDiv" class="minimize" title="Minimize" alt="Minimize" src="/App_Themes/<%=Page.Theme %>/images/minimize.png" /></a>
                                            </div>
                                        </th>
                                    </tr>
                                    <tr id="tr1" runat="server" style="background-color: #fff;">
                                        <td class="border-td">
                                            <table width="100%" border="0" cellpadding="0" cellspacing="0" bgcolor="#FFFFFF"
                                                class="content-table">
                                                <tr>
                                                    <td class="border-td-sub" align="center">
                                                        <table width="100%" border="0" cellpadding="0" cellspacing="0" class="add-product">
                                                            <tr>
                                                                <td align="left" valign="middle" colspan="3" width="100%">
                                                                    <div class="tab_box2">
                                                                        <uc1:Header runat="server" ID="Header1"></uc1:Header>
                                                                    </div>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr runat="server" id="trAddNewProduct" visible="false">
                                        <td align="center" class="border" valign="middle" style="width: 202px; text-align: center; line-height: 30px; background-color: white;"
                                            colspan="2">
                                            <asp:Label ID="pronotavailmsg" runat="server"></asp:Label>
                                            <asp:ImageButton ID="btnCloneNewProduct" runat="server" ImageUrl="../images/add.jpg"
                                                OnClick="btnCloneNewProduct_Click" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <table cellspacing="0" cellpadding="0" border="0" width="100%" id="tableproduct" runat="server">
                                                <tbody>
                                                    <tr>
                                                        <td height="5" align="left" valign="top">
                                                            <img alt="" height="5" width="1" src="/App_Themes/<%=Page.Theme %>/images/spacer.gif" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td valign="top">
                                                            <div id="tab-container-product">
                                                                <ul class="menu">
                                                                    <li class="active" id="liMenu5" onclick="Tabdisplay(5);">GENERAL</li>
                                                                    <%--<li id="li2" class="" onclick="Tabdisplay(2);iframereload('ContentPlaceHolder1_frmPO');">
                                    PURCHASE ORDER</li>--%>
                                                                    <li id="liMenu6" class="" onclick="Tabdisplay(6);">PROMOTIONS</li>
                                                                    <li id="liMenu7" class="" onclick="Tabdisplay(7);">SEARCH OPTIONS</li>
                                                                    <li id="liMenu8" class="" onclick="Tabdisplay(8);">CUSTOM CALCULATOR</li>
                                                                    <li id="liMenu9" class="" onclick="Tabdisplay(9);">SEO</li>
                                                                    <li id="liMenu10" style="display: none;" class="" onclick="Tabdisplay(10);var hgt = $(document).height();hgt = parseInt(hgt);window.parent.iframeAutoheightById('ContentPlaceHolder1_ifrmProductVariant',hgt);">PRODUCT OPTION</li>
                                                                    <%--  <li id="li3" class="" onclick="Tabdisplay(3);chkHeight();iframereload('ContentPlaceHolder1_frmPackingSlip');document.getElementById('prepage').style.display = 'none';">
                                                                        PACKING SLIP</li>--%>
                                                                    <li id="liMenu11" class="" onclick="Tabdisplay(11);" style="display: none;">SHADE CALCULATOR</li>
                                                                    <li id="liMenu12" class="" onclick="Tabdisplay(12);">REPLENISHMENT</li>
                                                                    <li onclick="Tabdisplay(13);" id="liMenu13">NAV DATA</li>
                                                                </ul>
                                                                <div class="tab-content general-tab" style="margin-top: 0px; font-size: 12px;" id="divtab5">
                                                                    <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                                                        <tr>
                                                                            <td id="tdMainDiv">
                                                                                <div id="divMain" class="slidingDivMainDiv" style="font-size: 12px;">
                                                                                    <table width="100%" border="0" cellpadding="0" cellspacing="0" class="add-product">
                                                                                        <tr class="altrow">
                                                                                            <td align="center">
                                                                                                <span id="msgid" runat="server" style="cursor: default; text-align: center; color: Red; font-weight: bold;"
                                                                                                    visible="false">Your product dose not clone until click on save
                                                                            button</span>
                                                                                                <asp:Label ID="lblMsg" ForeColor="Red" runat="server"></asp:Label>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr class="even-row">
                                                                                            <td>
                                                                                                <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                                                                                    <tr class="oddrow">
                                                                                                        <td width="12%">
                                                                                                            <span class="star">*</span>Product&nbsp;Name:
                                                                                                        </td>
                                                                                                        <td colspan="2">
                                                                                                            <asp:TextBox ID="txtProductName" runat="server" MaxLength="500" Style="width: 1000px;"
                                                                                                                class="order-textfield" TabIndex="1"></asp:TextBox>
                                                                                                        </td>
                                                                                                    </tr>
                                                                                                    <tr class="altrow">
                                                                                                        <td width="12%">
                                                                                                            <span class="star">*</span>SKU:
                                                                                                        </td>
                                                                                                        <td>
                                                                                                            <asp:TextBox ID="txtSKU" runat="server" MaxLength="500" class="order-textfield" TabIndex="2"></asp:TextBox>&nbsp;<a href="javascript:void(0);" onclick="changereadonly();" id="skuedit" runat="server" visible="false">Edit</a>
                                                                                                            <input type="hidden" id="hdnskuedit" runat="server" value="0" />
                                                                                                        </td>
                                                                                                        <td width="58%" rowspan="7">
                                                                                                            <table width="60%" border="0" cellpadding="0" cellspacing="0" style="border: 1px solid #BCC0C1;">
                                                                                                                <tr>
                                                                                                                    <th>
                                                                                                                        <div class="main-title-left">
                                                                                                                            <img class="img-left" title="Warehouses" alt="Warehouses" src="/App_Themes/<%=Page.Theme %>/icon/add-product.png">
                                                                                                                            <h2>Warehouses
                                                                                                                            </h2>
                                                                                                                        </div>
                                                                                                                        <div class="main-title-right">
                                                                                                                            <a href="javascript:void(0);" class="show_hideWarehouse" onclick="return ShowHideButton('ImgWarehouses','tdWarehouses');">
                                                                                                                                <img id="ImgWarehouses" src="/App_Themes/<%=Page.Theme %>/images/minimize.png" class="minimize"
                                                                                                                                    title="Minimize" alt="Minimize"></a>
                                                                                                                        </div>
                                                                                                                    </th>
                                                                                                                </tr>
                                                                                                                <tr>
                                                                                                                    <td id="tdWarehouses">
                                                                                                                        <div id="div3" class="slidingDivWarehouse">
                                                                                                                            <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                                                                                                                <tr>
                                                                                                                                    <td>
                                                                                                                                        <asp:GridView ID="grdWarehouse" AutoGenerateColumns="false" runat="server" BorderStyle="Solid"
                                                                                                                                            BorderColor="#E7E7E7" BorderWidth="1px" CellPadding="2" CellSpacing="1" GridLines="None"
                                                                                                                                            Width="100%" DataKeyNames="WareHouseID" EmptyDataText="No Records Found!" RowStyle-ForeColor="Black"
                                                                                                                                            HeaderStyle-ForeColor="White" HeaderStyle-Font-Bold="false" PagerSettings-Mode="NumericFirstLast"
                                                                                                                                            AllowPaging="True" PageSize="<%$ appSettings:GridPageSize %>" AllowSorting="True"
                                                                                                                                            EmptyDataRowStyle-ForeColor="Red" EmptyDataRowStyle-HorizontalAlign="Center"
                                                                                                                                            ViewStateMode="Enabled" ShowHeaderWhenEmpty="true" OnRowDataBound="grdWarehouse_RowDataBound"
                                                                                                                                            ShowFooter="True">
                                                                                                                                            <Columns>
                                                                                                                                                <asp:TemplateField>
                                                                                                                                                    <HeaderTemplate>
                                                                                                                                                        Preferred&nbsp;Location
                                                                                                                                                    </HeaderTemplate>
                                                                                                                                                    <ItemTemplate>
                                                                                                                                                        <asp:RadioButton ID="rdowarehouse" runat="server" OnClick="javascript:SelectSingleRadiobutton(this.id)"
                                                                                                                                                            GroupName="WarehouseSelect" />
                                                                                                                                                        <asp:Label ID="lblPreferredLocation" runat="server" Visible="false" Text='<%#Bind("PreferredLocation") %>'></asp:Label>
                                                                                                                                                    </ItemTemplate>
                                                                                                                                                    <ItemStyle HorizontalAlign="Center" Width="15%"></ItemStyle>
                                                                                                                                                    <FooterTemplate>
                                                                                                                                                        &nbsp;
                                                                                                                                                    </FooterTemplate>
                                                                                                                                                    <FooterStyle HorizontalAlign="Right" CssClass="footerBorder" />
                                                                                                                                                </asp:TemplateField>
                                                                                                                                                <asp:TemplateField HeaderText="Warehouse Name">
                                                                                                                                                    <ItemTemplate>
                                                                                                                                                        <asp:Label ID="lblWarehouse" runat="server" Text='<%#Bind("Name") %>'></asp:Label>
                                                                                                                                                    </ItemTemplate>
                                                                                                                                                    <HeaderStyle HorizontalAlign="Left" Width="80%"></HeaderStyle>
                                                                                                                                                    <ItemStyle HorizontalAlign="Left" Width="80%"></ItemStyle>
                                                                                                                                                    <FooterTemplate>
                                                                                                                                                        <b>Total Inventory:&nbsp;&nbsp;&nbsp;&nbsp;</b>
                                                                                                                                                    </FooterTemplate>
                                                                                                                                                    <FooterStyle HorizontalAlign="Right" CssClass="footerBorder" />
                                                                                                                                                </asp:TemplateField>
                                                                                                                                                <asp:TemplateField HeaderText="Inventory">
                                                                                                                                                    <ItemTemplate>
                                                                                                                                                        <asp:TextBox ID="txtInventory" CssClass="order-textfield" Style="width: 50px; text-align: center;"
                                                                                                                                                            runat="server" Text='<%#Bind("Inventory") %>' onKeyPress="var ret=keyRestrictForInventory(event,'0123456789');SetTotalWeight();return ret;"
                                                                                                                                                            onblur="SetTotalWeight();" MaxLength="5" TabIndex="7" onkeyup="SetTotalWeight();"></asp:TextBox>
                                                                                                                                                    </ItemTemplate>
                                                                                                                                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                                                                                                                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                                                                                                                    <FooterTemplate>
                                                                                                                                                        <asp:Label ID="lblTotal" runat="server" Font-Bold="true"></asp:Label>
                                                                                                                                                    </FooterTemplate>
                                                                                                                                                    <FooterStyle HorizontalAlign="Center" Font-Bold="true" CssClass="footerBorderinventory" />
                                                                                                                                                </asp:TemplateField>
                                                                                                                                                <asp:TemplateField HeaderText="WarehouseID" Visible="false">
                                                                                                                                                    <ItemTemplate>
                                                                                                                                                        <asp:Label ID="lblWarehouseID" runat="server" Text='<%#Bind("WarehouseID") %>'></asp:Label>
                                                                                                                                                    </ItemTemplate>
                                                                                                                                                </asp:TemplateField>
                                                                                                                                            </Columns>
                                                                                                                                            <RowStyle HorizontalAlign="Center" CssClass="oddrow" BackColor="White" />
                                                                                                                                            <AlternatingRowStyle CssClass="altrow" BackColor="#FBFBFB" />
                                                                                                                                        </asp:GridView>
                                                                                                                                    </td>
                                                                                                                                </tr>
                                                                                                                            </table>
                                                                                                                        </div>
                                                                                                                    </td>
                                                                                                                </tr>
                                                                                                            </table>
                                                                                                        </td>
                                                                                                    </tr>
                                                                                                    <tr class="altrow" style="display: none;">
                                                                                                        <td width="12%">
                                                                                                            <span class="star">&nbsp;</span>Sale Channel SKU:
                                                                                                        </td>
                                                                                                        <td>
                                                                                                            <asp:TextBox ID="txtoptionSKU" runat="server" MaxLength="500"
                                                                                                                class="order-textfield" TabIndex="1"></asp:TextBox>
                                                                                                        </td>
                                                                                                    </tr>
                                                                                                    <tr class="oddrow">
                                                                                                        <td width="12%">
                                                                                                            <span class="star">&nbsp;&nbsp;</span>UPC:
                                                                                                        </td>
                                                                                                        <td>
                                                                                                            <table border="0" cellpadding="0" cellspacing="0">
                                                                                                                <tr>
                                                                                                                    <td>
                                                                                                                        <asp:TextBox ID="txtUPC" runat="server" MaxLength="100" class="order-textfield" TabIndex="3"></asp:TextBox>
                                                                                                                    </td>
                                                                                                                    <td style="display: none">
                                                                                                                        <asp:LinkButton ID="lbtngetUPC" runat="server" OnClick="lbtngetUPC_Click" Visible="false">Get UPC</asp:LinkButton>&nbsp;
                                                                                                                    </td>
                                                                                                                </tr>
                                                                                                            </table>
                                                                                                        </td>
                                                                                                    </tr>
                                                                                                    <tr id="trBarcode" runat="server" visible="false" style="display: none;">
                                                                                                        <td valign="top" style="padding-top: 32px;">Product Barcode :
                                                                                                        </td>
                                                                                                        <td>
                                                                                                            <a style="padding-left: 174px" href="javascript:void(0);" onclick="return PrintBarcode();">Print Barcode</a>
                                                                                                            <br />
                                                                                                            <div id="divBarcodePrint">
                                                                                                                <img alt="" id="imgOrderBarcode" runat="server" />
                                                                                                            </div>
                                                                                                        </td>
                                                                                                    </tr>
                                                                                                    <tr class="altrow">
                                                                                                        <td width="12%">
                                                                                                            <span class="star">&nbsp;</span>Tax&nbsp;Class:
                                                                                                        </td>
                                                                                                        <td>
                                                                                                            <asp:DropDownList ID="ddlTaxClass" runat="server" class="product-type" TabIndex="4">
                                                                                                            </asp:DropDownList>
                                                                                                        </td>
                                                                                                    </tr>
                                                                                                    <%--  <tr class="oddrow" style="display: none;">
                                                                                <td width="12%">
                                                                                    <span class="star">*</span>Product&nbsp;Type:
                                                                                </td>
                                                                                <td>
                                                                                    <asp:UpdateProgress ID="upgrProductType" runat="server" AssociatedUpdatePanelID="upProductType">
                                                                                        <ProgressTemplate>
                                                                                            <div style="position: relative;">
                                                                                                <div style="position: absolute; bottom: -27px; left: 40%;">
                                                                                                    <img alt="" src="../images/ProductLoader.gif" />
                                                                                                    <b>Loading ... ... Please wait!</b>
                                                                                                </div>
                                                                                            </div>
                                                                                        </ProgressTemplate>
                                                                                    </asp:UpdateProgress>
                                                                                    <asp:UpdatePanel ID="upProductType" runat="server" UpdateMode="Always">
                                                                                        <ContentTemplate>
                                                                                            <asp:DropDownList ID="ddlProductType" AutoPostBack="true" runat="server" class="product-type"
                                                                                                OnSelectedIndexChanged="ddlProductType_SelectedIndexChanged" TabIndex="5">
                                                                                            </asp:DropDownList>
                                                                                        </ContentTemplate>
                                                                                    </asp:UpdatePanel>
                                                                                </td>
                                                                            </tr>--%>
                                                                                                    <tr class="oddrow">
                                                                                                        <td width="12%">
                                                                                                            <span class="star">*</span>ProductURL:
                                                                                                        </td>
                                                                                                        <td>
                                                                                                            <asp:TextBox ID="txtProductURL" runat="server" class="order-textfield" Width="450px"
                                                                                                                TabIndex="5"></asp:TextBox>
                                                                                                        </td>
                                                                                                    </tr>
                                                                                                    <tr class="altrow">
                                                                                                        <td width="12%">
                                                                                                            <span class="star">*</span>Item Type:
                                                                                                        </td>
                                                                                                        <td>
                                                                                                            <asp:DropDownList ID="ddlItemType" runat="server" class="product-type" onchange="SetItemtype();" TabIndex="6">
                                                                                                                <asp:ListItem Text="Drape" Selected="True" Value="Drape"></asp:ListItem>
                                                                                                                <asp:ListItem Text="Swatch" Value="Swatch"></asp:ListItem>
                                                                                                                <asp:ListItem Text="Roman" Value="Roman"></asp:ListItem>
                                                                                                                <asp:ListItem Text="Fabric" Value="Fabric"></asp:ListItem>
                                                                                                            </asp:DropDownList>
                                                                                                        </td>
                                                                                                    </tr>
                                                                                                    <tr class="oddrow">
                                                                                                        <td width="12%">
                                                                                                            <span class="star">*</span>Manufacturer:
                                                                                                        </td>
                                                                                                        <td>
                                                                                                            <asp:DropDownList ID="ddlManufacture" runat="server" class="product-type" TabIndex="6">
                                                                                                            </asp:DropDownList>&nbsp;&nbsp;&nbsp;<a id="Manufacturelink" runat="server" style="color: #B92127; text-decoration: underline;">Add Manufacture</a>
                                                                                                        </td>
                                                                                                    </tr>
                                                                                                    <tr class="altrow">
                                                                                                        <td width="12%">
                                                                                                            <span class="star">*</span>Inventory:
                                                                                                        </td>
                                                                                                        <td>
                                                                                                            <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Always">
                                                                                                                <ContentTemplate>
                                                                                                                    <asp:TextBox ID="txtInventory" runat="server" class="status-textfield"></asp:TextBox>
                                                                                                                    <%-- &nbsp;
                                                                                        <img  alt="Low Level" title="Low Level" src="../Images/bullet_red.png" id="ImgToggelInventory" runat="server"  visible="false" style="vertical-align:middle;"/>--%>
                                                                                            &nbsp;<span style="font-size: 28px; vertical-align: middle; color: Red; cursor: default;"
                                                                                                title="Low Level" id="ImgToggelInventory" runat="server" visible="false">• <font
                                                                                                    style="font-size: 13px; vertical-align: middle; padding-bottom: 5px;">Low Level</font></span>
                                                                                                                    &nbsp;
                                                                                                                </ContentTemplate>
                                                                                                            </asp:UpdatePanel>
                                                                                                        </td>
                                                                                                    </tr>
                                                                                                    <tr class="oddrow">
                                                                                                        <td width="12%">
                                                                                                            <span class="star">&nbsp;</span>Low Inventory:
                                                                                                        </td>
                                                                                                        <td>
                                                                                                            <asp:TextBox ID="txtLowInventory" runat="server" class="status-textfield" MaxLength="6"
                                                                                                                onKeyPress="var ret=keyRestrictForInventory(event,'0123456789');return ret;"></asp:TextBox>
                                                                                                        </td>
                                                                                                    </tr>
                                                                                                </table>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td>
                                                                                                <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                                                                                    <tr class="oddrow" style="display: none">
                                                                                                        <td>&nbsp;
                                                                                                        </td>
                                                                                                        <td>&nbsp;
                                                                                                        </td>
                                                                                                        <td></td>
                                                                                                        <td></td>
                                                                                                        <td width="13%" style="display: none">Availability:
                                                                                                        </td>
                                                                                                        <td width="44%" style="display: none">
                                                                                                            <asp:TextBox ID="txtAvailability" MaxLength="500" runat="server" class="order-textfield"></asp:TextBox>
                                                                                                        </td>
                                                                                                    </tr>
                                                                                                    <tr class="altrow">
                                                                                                        <td style="width: 12%">
                                                                                                            <span class="star">*</span>Weight:
                                                                                                        </td>
                                                                                                        <td>
                                                                                                            <asp:TextBox ID="txtWeight" runat="server" Width="80px" class="order-textfield" onkeypress="return keyRestrict(event,'0123456789.');"
                                                                                                                TabIndex="9"></asp:TextBox>
                                                                                                            lbs. Ex (12.52)
                                                                                                        </td>
                                                                                                        <td width="12%">Quantity Discount :
                                                                                                        </td>
                                                                                                        <td width="46%">
                                                                                                            <asp:DropDownList ID="ddlQuantityDiscount" runat="server" class="product-type" TabIndex="10">
                                                                                                            </asp:DropDownList>&nbsp;&nbsp;&nbsp;<a id="QuantityDiscountLink" runat="server" style="color: #B92127; text-decoration: underline;">Add Quantity Discount</a>
                                                                                                        </td>
                                                                                                        <td style="display: none">
                                                                                                            <div>
                                                                                                                width&nbsp;&nbsp;
                                                                                        <asp:TextBox ID="txtWidth" runat="server" class="order-textfield" Width="80px" onkeypress="return keyRestrict(event,'0123456789.');"></asp:TextBox>
                                                                                                                &nbsp;&nbsp;&nbsp;x&nbsp;&nbsp;&nbsp;Height
                                                                                        <asp:TextBox ID="txtHeight" runat="server" class="order-textfield" Width="80px" onkeypress="return keyRestrict(event,'0123456789.');"></asp:TextBox>
                                                                                                                &nbsp;&nbsp;&nbsp;x&nbsp;&nbsp;&nbsp;Length
                                                                                        <asp:TextBox ID="txtLength" runat="server" class="order-textfield" Width="80px" onkeypress="return keyRestrict(event,'0123456789.');"></asp:TextBox>
                                                                                                            </div>
                                                                                                        </td>
                                                                                                    </tr>
                                                                                                    <tr class="oddrow">
                                                                                                        <td>
                                                                                                            <span class="star">*</span>Price:
                                                                                                        </td>
                                                                                                        <td>$<asp:TextBox ID="txtPrice" runat="server" class="order-textfield" Width="80px" onkeypress="return keyRestrict(event,'0123456789.');"
                                                                                                            TabIndex="11"></asp:TextBox>
                                                                                                            Ex (12.00)
                                                                                                        </td>
                                                                                                        <td>Sale Price:
                                                                                                        </td>
                                                                                                        <td>$<asp:TextBox ID="txtSalePrice" runat="server" class="order-textfield" Width="80px"
                                                                                                            onkeypress="return keyRestrict(event,'0123456789.');" TabIndex="12"></asp:TextBox>
                                                                                                            Ex (8.62) &nbsp;&nbsp;
                                                                                    <asp:CompareValidator ID="cmpSalePrice" runat="server" ControlToCompare="txtPrice"
                                                                                        ControlToValidate="txtSalePrice" Display="Dynamic" ForeColor="Red" Font-Bold="true"
                                                                                        CssClass="error" ErrorMessage="Sale Price Should Be Less than Price" Operator="LessThanEqual"
                                                                                        SetFocusOnError="True" Type="Double" ValidationGroup="Product">Sale Price Should Be Less than Price</asp:CompareValidator>
                                                                                                            &nbsp;&nbsp;
                                                                                     <asp:DropDownList ID="ddlSalePriceTag" class="product-type" Width="100px" runat="server"
                                                                                         TabIndex="35">
                                                                                         <asp:ListItem Text="None" Value=""></asp:ListItem>
                                                                                         <asp:ListItem Text="Per Panel" Value="Per Panel"></asp:ListItem>
                                                                                         <asp:ListItem Text="Per Pair" Value="Per Pair"></asp:ListItem>
                                                                                     </asp:DropDownList>
                                                                                                        </td>
                                                                                                    </tr>
                                                                                                    <tr class="altrow">
                                                                                                        <td>
                                                                                                            <span class="star">&nbsp;</span>Our Cost:
                                                                                                        </td>
                                                                                                        <td>$<asp:TextBox ID="txtOurPrice" runat="server" CssClass="order-textfield" Width="80px"
                                                                                                            onkeypress="return keyRestrict(event,'0123456789.');" TabIndex="13"></asp:TextBox>
                                                                                                        </td>
                                                                                                        <td></td>
                                                                                                        <td></td>
                                                                                                    </tr>
                                                                                                    <tr class="oddrow" style="display: none;">
                                                                                                        <td>
                                                                                                            <span class="star">&nbsp;</span> Display Order:
                                                                                                        </td>
                                                                                                        <td>
                                                                                                            <asp:TextBox ID="txtDisplayOrder" runat="server" class="status-textfield" TabIndex="15"></asp:TextBox>
                                                                                                        </td>
                                                                                                        <td>SurCharge:
                                                                                                        </td>
                                                                                                        <td style="display: none;">
                                                                                                            <asp:TextBox ID="txtSurCharge" runat="server" class="order-textfield" Width="80px"
                                                                                                                onkeypress="return keyRestrict(event,'0123456789.');"></asp:TextBox>
                                                                                                        </td>
                                                                                                    </tr>
                                                                                                      <tr class="altrow">
                                                                                                        <td>
                                                                                                            <span class="star">&nbsp;</span>Is Grommet:
                                                                                                        </td>
                                                                                                        <td>
                                                                                                            <asp:CheckBox ID="chkIsGrommet" runat="server" Checked="false" TabIndex="16" />
                                                                                                        </td>
                                                                                                        <td></td>
                                                                                                        <td></td>
                                                                                                    </tr>
                                                                                                      <tr class="oddrow">
                                                                                                        <td>
                                                                                                            <span class="star">&nbsp;</span>Override Product Options:
                                                                                                        </td>
                                                                                                        <td>
                                                                                                            <asp:CheckBox ID="chkOverRide" runat="server" Checked="false" TabIndex="16" />
                                                                                                        </td>
                                                                                                        <td></td>
                                                                                                        <td></td>
                                                                                                    </tr>
                                                                                                    <tr class="altrow">
                                                                                                        <td>
                                                                                                            <span class="star">&nbsp;</span>Product Status:
                                                                                                        </td>
                                                                                                        <td>
                                                                                                            <asp:CheckBox ID="chkPublished" runat="server" Checked="true" Text=" Website Active" TabIndex="16" onchange="checkalladmin('ContentPlaceHolder1_chkPublished','ContentPlaceHolder1_chkpublishedadmin');" />&nbsp;<asp:CheckBox ID="chkpublishedadmin" runat="server" Checked="true" Text=" Admin Active" TabIndex="16" onchange="checkall('ContentPlaceHolder1_chkPublished','ContentPlaceHolder1_chkpublishedadmin');" />
                                                                                                        </td>
                                                                                                        <td></td>
                                                                                                        <td></td>
                                                                                                    </tr>
                                                                                                    <tr class="altrow" id="tdpageredirect" runat="server">
                                                                                                        <td>
                                                                                                            <span class="star">&nbsp;</span> Redirect URL:
                                                                                                        </td>
                                                                                                        <td>
                                                                                                            <asp:TextBox ID="txtpageredirecturl" runat="server" class="order-textfield" TabIndex="15" Style="width: 450px;"></asp:TextBox>
                                                                                                            (Ex: abc.html)
                                                                                                        </td>
                                                                                                    </tr>
                                                                                                    <tr class="altrow" style="display: none;">
                                                                                                        <td>
                                                                                                            <span class="star">&nbsp;</span> Hemming(%):
                                                                                                        </td>
                                                                                                        <td>
                                                                                                            <asp:TextBox ID="txtHammingQty" CssClass="order-textfield" Style="width: 50px; text-align: center;"
                                                                                                                runat="server" onkeypress="var ret=keyRestrictForInventory(event,'0123456789.');return ret;"
                                                                                                                MaxLength="5" TabIndex="7"></asp:TextBox>
                                                                                                        </td>
                                                                                                        <td></td>
                                                                                                        <td></td>
                                                                                                    </tr>
                                                                                                    <tr class="oddrow" style="display: none;">
                                                                                                        <td>
                                                                                                            <span class="star">&nbsp;</span> Is Eligible for Hemming:
                                                                                                        </td>
                                                                                                        <td colspan="2">
                                                                                                            <asp:CheckBox ID="chkIsHamming" runat="server" TabIndex="16" />
                                                                                                        </td>
                                                                                                    </tr>
                                                                                                    <tr class="altrow">
                                                                                                        <td>
                                                                                                            <span class="star">&nbsp;</span> Is Dropship Product:
                                                                                                        </td>
                                                                                                        <td>
                                                                                                            <asp:CheckBox ID="chkdropshipproduct" runat="server" Checked="false" TabIndex="16" />
                                                                                                        </td>
                                                                                                        <td></td>
                                                                                                        <td></td>
                                                                                                    </tr>
                                                                                                    <tr class="altrow">
                                                                                                        <td>
                                                                                                            <span class="star">&nbsp;</span>Is not display on custom page:
                                                                                                        </td>
                                                                                                        <td>
                                                                                                            <asp:CheckBox ID="chkIsnodisplaycpage" runat="server" Checked="false" TabIndex="16" />
                                                                                                        </td>
                                                                                                        <td></td>
                                                                                                        <td></td>
                                                                                                    </tr>
                                                                                                    <tr class="oddrow">
                                                                                                        <td width="12%">
                                                                                                            <span class="star">*</span>Stock/Dropship:
                                                                                                        </td>
                                                                                                        <td>
                                                                                                            <asp:UpdateProgress ID="upgrProductTypeDelivery" runat="server" AssociatedUpdatePanelID="upProductTypeDelivery">
                                                                                                                <ProgressTemplate>
                                                                                                                    <div style="position: relative;">
                                                                                                                        <div style="position: absolute; bottom: -27px; left: 40%;">
                                                                                                                            <img alt="" src="../images/ProductLoader.gif" />
                                                                                                                            <b>Loading ... ... Please wait!</b>
                                                                                                                        </div>
                                                                                                                    </div>
                                                                                                                </ProgressTemplate>
                                                                                                            </asp:UpdateProgress>
                                                                                                            <asp:UpdatePanel ID="upProductTypeDelivery" runat="server" UpdateMode="Always">
                                                                                                                <ContentTemplate>
                                                                                                                    <asp:DropDownList ID="ddlProductTypeDelivery" AutoPostBack="true" runat="server"
                                                                                                                        class="product-type" OnSelectedIndexChanged="ddlProductTypeDelivery_SelectedIndexChanged"
                                                                                                                        TabIndex="18">
                                                                                                                    </asp:DropDownList>
                                                                                                                </ContentTemplate>
                                                                                                            </asp:UpdatePanel>
                                                                                                        </td>
                                                                                                        <td width="12%">
                                                                                                            <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Always">
                                                                                                                <ContentTemplate>
                                                                                                                    <div id="divvendor" runat="server" visible="false">
                                                                                                                        <span class="star"></span>Drop Shipper/Vendor :
                                                                                                                    </div>
                                                                                                                </ContentTemplate>
                                                                                                            </asp:UpdatePanel>
                                                                                                        </td>
                                                                                                        <td width="12%">
                                                                                                            <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Always">
                                                                                                                <ContentTemplate>
                                                                                                                    <asp:DropDownList ID="ddlvendor" runat="server" class="product-type" AutoPostBack="true"
                                                                                                                        TabIndex="19" Visible="false">
                                                                                                                    </asp:DropDownList>
                                                                                                                </ContentTemplate>
                                                                                                            </asp:UpdatePanel>
                                                                                                        </td>
                                                                                                    </tr>
                                                                                                    <tr class="oddrow">
                                                                                                        <td width="12%">
                                                                                                            <span class="star">*</span>Product&nbsp;Type:
                                                                                                        </td>
                                                                                                        <td>
                                                                                                            <asp:UpdateProgress ID="upgrProductType" runat="server" AssociatedUpdatePanelID="upProductType">
                                                                                                                <ProgressTemplate>
                                                                                                                    <div style="position: relative;">
                                                                                                                        <div style="position: absolute; bottom: -27px; left: 40%;">
                                                                                                                            <img alt="" src="../images/ProductLoader.gif" />
                                                                                                                            <b>Loading ... ... Please wait!</b>
                                                                                                                        </div>
                                                                                                                    </div>
                                                                                                                </ProgressTemplate>
                                                                                                            </asp:UpdateProgress>
                                                                                                            <asp:UpdatePanel ID="upProductType" runat="server" UpdateMode="Always">
                                                                                                                <ContentTemplate>
                                                                                                                    <asp:DropDownList ID="ddlProductType" AutoPostBack="true" runat="server" class="product-type"
                                                                                                                        OnSelectedIndexChanged="ddlProductType_SelectedIndexChanged" TabIndex="20">
                                                                                                                    </asp:DropDownList>
                                                                                                                </ContentTemplate>
                                                                                                            </asp:UpdatePanel>
                                                                                                        </td>
                                                                                                    </tr>
                                                                                                    <tr>
                                                                                                        <td>
                                                                                                            <%-- <asp:ImageButton ID="ibtnFeaturecategory" runat="server" OnClientClick="openCenteredCrossSaleWindow1('ContentPlaceHolder1_hdnvendorAllSku');" />--%>
                                                                                                        </td>
                                                                                                        <td colspan="3">
                                                                                                            <div style="display: none;">
                                                                                                                <asp:Button ID="btnvendorlist" runat="server" Text="Savesadfasd" OnClick="btnvendorlist_click" />
                                                                                                            </div>
                                                                                                            <asp:UpdatePanel ID="updategrdDropShip" runat="server">
                                                                                                                <ContentTemplate>
                                                                                                                    <asp:HiddenField ID="hdnvendorAllSku" runat="server" />
                                                                                                                    <asp:HiddenField ID="hdnProductALLSku" runat="server" />
                                                                                                                    <asp:HiddenField ID="hdnProductALLQty" runat="server" />
                                                                                                                    <asp:GridView ID="grdDropShip" runat="server" AutoGenerateColumns="false" OnRowDataBound="grdDropShip_RowDataBound"
                                                                                                                        BorderStyle="Solid" BorderColor="#E7E7E7" BorderWidth="1px" CellPadding="2" CellSpacing="1"
                                                                                                                        GridLines="None" HeaderStyle-ForeColor="White" ShowFooter="true" Width="60%"
                                                                                                                        OnRowCommand="grdDropShip_RowCommand" OnRowCancelingEdit="cancelRecord" OnRowEditing="editRecord">
                                                                                                                        <EmptyDataTemplate>
                                                                                                                            <table width="100%" cellpadding="2" cellspacing="1" style="background-color: White;">
                                                                                                                                <tr>
                                                                                                                                    <th style="color: White">Dropshipper Name
                                                                                                                                    </th>
                                                                                                                                    <th style="color: White">Dropshipper Product Name
                                                                                                                                    </th>
                                                                                                                                    <th style="color: White">SKU
                                                                                                                                    </th>
                                                                                                                                    <th style="color: White">Priority
                                                                                                                                    </th>
                                                                                                                                    <th style="color: White">Operations
                                                                                                                                    </th>
                                                                                                                                </tr>
                                                                                                                                <tr>
                                                                                                                                    <td></td>
                                                                                                                                </tr>
                                                                                                                                <tr>
                                                                                                                                    <td colspan="5" align="right">
                                                                                                                                        <asp:ImageButton ID="ibtnFeaturecategory" runat="server" OnClientClick="return openCenteredCrossSaleWindow1('ContentPlaceHolder1_hdnvendorAllSku');" />
                                                                                                                                    </td>
                                                                                                                                </tr>
                                                                                                                            </table>
                                                                                                                        </EmptyDataTemplate>
                                                                                                                        <Columns>
                                                                                                                            <asp:TemplateField HeaderText="VendorID" HeaderStyle-HorizontalAlign="Left" Visible="false">
                                                                                                                                <ItemTemplate>
                                                                                                                                    <%--<asp:HiddenField ID="hdnVendorSKUID" runat="server" Value= '<%# Convert.ToInt32( DataBinder.Eval(Container.DataItem,"VendorSKUID")) %>'/>--%>
                                                                                                                                    <asp:Label ID="lblVendorSKUID" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"VendorSKUID") %>'
                                                                                                                                        Visible="false"></asp:Label>
                                                                                                                                    <asp:Label ID="lblVendorID" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"VendorID") %>'
                                                                                                                                        Visible="false"></asp:Label>
                                                                                                                                </ItemTemplate>
                                                                                                                                <ItemStyle HorizontalAlign="Left" />
                                                                                                                            </asp:TemplateField>
                                                                                                                            <asp:TemplateField HeaderText="Dropshipper Name" HeaderStyle-HorizontalAlign="Left">
                                                                                                                                <ItemTemplate>
                                                                                                                                    <asp:Label ID="lblDropshipperName" runat="server" Text='<%#Bind("Name") %>'></asp:Label>
                                                                                                                                </ItemTemplate>
                                                                                                                                <ItemStyle HorizontalAlign="Left" />
                                                                                                                            </asp:TemplateField>
                                                                                                                            <asp:TemplateField HeaderText="Dropshipper Product Name" HeaderStyle-HorizontalAlign="Left">
                                                                                                                                <ItemTemplate>
                                                                                                                                    <asp:Label ID="lblProductName" runat="server" Text='<%#Bind("ProductName") %>'></asp:Label>
                                                                                                                                </ItemTemplate>
                                                                                                                                <ItemStyle HorizontalAlign="Left" />
                                                                                                                            </asp:TemplateField>
                                                                                                                            <asp:TemplateField HeaderText="SKU" HeaderStyle-HorizontalAlign="Center">
                                                                                                                                <ItemTemplate>
                                                                                                                                    <asp:Label ID="lblVendorSKU" runat="server" Text='<%#Bind("VendorSKU") %>'></asp:Label>
                                                                                                                                </ItemTemplate>
                                                                                                                                <ItemStyle HorizontalAlign="Left" />
                                                                                                                            </asp:TemplateField>
                                                                                                                            <asp:TemplateField HeaderText="Priority" HeaderStyle-HorizontalAlign="Center">
                                                                                                                                <ItemTemplate>
                                                                                                                                    <asp:Label ID="lblPriority" runat="server" Text='<%#Bind("Priority") %>'> </asp:Label>
                                                                                                                                    <%--<asp:TextBox ID="txtPriority" runat="server" Text="0" Width="20px"></asp:TextBox>--%>
                                                                                                                                </ItemTemplate>
                                                                                                                                <EditItemTemplate>
                                                                                                                                    <asp:TextBox ID="txtPriority" runat="server" Visible="True" Width="40px" onkeypress="return keyRestrict(event,'0123456789');"
                                                                                                                                        Text='<%#Bind("Priority") %>' MaxLength="2" Style="text-align: center;"></asp:TextBox>
                                                                                                                                </EditItemTemplate>
                                                                                                                                <ItemStyle HorizontalAlign="Center" />
                                                                                                                            </asp:TemplateField>
                                                                                                                            <asp:TemplateField HeaderText="Operations">
                                                                                                                                <ItemTemplate>
                                                                                                                                    <asp:ImageButton runat="server" ID="_editLinkButton" ToolTip="Edit" CommandName="edit"
                                                                                                                                        CommandArgument='<%# Eval("VendorSKUID") %>' ImageUrl="~/App_Themes/Gray/images/edit-icon.png"></asp:ImageButton>
                                                                                                                                    <asp:ImageButton runat="server" ID="_deleteLinkButton" ToolTip="Delete" CommandName="del"
                                                                                                                                        CommandArgument='<%# Eval("VendorSKUID") %>' message='<%# Eval("VendorSKUID") %>'
                                                                                                                                        OnClientClick="javascript:if(confirm('Are you sure want to delete this record?')){return true;}else{return false;}"
                                                                                                                                        ImageUrl="~/App_Themes/Gray/images/delete-icon.png"></asp:ImageButton>
                                                                                                                                </ItemTemplate>
                                                                                                                                <EditItemTemplate>
                                                                                                                                    <asp:ImageButton ID="btnSave" runat="server" Visible="true" CommandArgument='<%# DataBinder.Eval(Container.DataItem,"VendorSKUID") %>'
                                                                                                                                        CommandName="Save" AlternateText="Save" ImageUrl="~/App_Themes/Gray/images/save.png" />
                                                                                                                                    <asp:ImageButton ID="btnCancel" runat="server" Visible="true" CommandName="Cancel"
                                                                                                                                        ImageUrl="~/App_Themes/Gray/images/CloseIcon.png" Height="16px" Width="16px"
                                                                                                                                        AlternateText="Cancel" CommandArgument='<%# DataBinder.Eval(Container.DataItem,"VendorSKUID") %>' />
                                                                                                                                </EditItemTemplate>
                                                                                                                                <FooterTemplate>
                                                                                                                                    <asp:ImageButton ID="ibtnFeaturecategory" runat="server" OnClientClick="return openCenteredCrossSaleWindow1('ContentPlaceHolder1_hdnvendorAllSku');" />
                                                                                                                                </FooterTemplate>
                                                                                                                            </asp:TemplateField>
                                                                                                                        </Columns>
                                                                                                                        <RowStyle HorizontalAlign="Center" CssClass="oddrow" BackColor="White" />
                                                                                                                        <AlternatingRowStyle CssClass="altrow" />
                                                                                                                        <FooterStyle HorizontalAlign="Right" />
                                                                                                                    </asp:GridView>
                                                                                                                    <asp:GridView ID="grdAssembler" runat="server" AutoGenerateColumns="false" Visible="false"
                                                                                                                        OnRowDataBound="grdAssembler_RowDataBound" BorderStyle="Solid" BorderColor="#E7E7E7"
                                                                                                                        BorderWidth="1px" CellPadding="2" CellSpacing="1" GridLines="None" HeaderStyle-ForeColor="White"
                                                                                                                        ShowFooter="true" Width="60%" OnRowCommand="grdAssembler_RowCommand" OnRowCancelingEdit="cancelProduct"
                                                                                                                        OnRowEditing="editProduct">
                                                                                                                        <EmptyDataTemplate>
                                                                                                                            <table width="100%" cellpadding="2" cellspacing="1" style="background-color: White;">
                                                                                                                                <tr>
                                                                                                                                    <th style="color: White">Product Name
                                                                                                                                    </th>
                                                                                                                                    <th style="color: White">SKU
                                                                                                                                    </th>
                                                                                                                                    <th style="color: White">Quantity
                                                                                                                                    </th>
                                                                                                                                    <th style="color: White" align="center">Operations
                                                                                                                                    </th>
                                                                                                                                </tr>
                                                                                                                                <tr>
                                                                                                                                    <td></td>
                                                                                                                                </tr>
                                                                                                                                <tr>
                                                                                                                                    <td colspan="4" align="right">
                                                                                                                                        <asp:ImageButton ID="ibtnFeatureProduct" runat="server" OnClientClick="return openCenteredCrossSaleWindow2('ContentPlaceHolder1_hdnProductALLSku','ContentPlaceHolder1_hdnProductALLQty');" />
                                                                                                                                    </td>
                                                                                                                                </tr>
                                                                                                                            </table>
                                                                                                                        </EmptyDataTemplate>
                                                                                                                        <Columns>
                                                                                                                            <asp:TemplateField HeaderText="VendorID" HeaderStyle-HorizontalAlign="Left" Visible="false">
                                                                                                                                <ItemTemplate>
                                                                                                                                    <%--<asp:HiddenField ID="hdnVendorSKUID" runat="server" Value= '<%# Convert.ToInt32( DataBinder.Eval(Container.DataItem,"VendorSKUID")) %>'/>--%>
                                                                                                                                    <asp:Label ID="lblProductID" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"ProductID") %>'
                                                                                                                                        Visible="false"></asp:Label>
                                                                                                                                    <%--  <asp:Label ID="lblVendorID" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"VendorID") %>'
                                                                                                                Visible="false"></asp:Label>--%>
                                                                                                                                </ItemTemplate>
                                                                                                                                <ItemStyle HorizontalAlign="Left" />
                                                                                                                            </asp:TemplateField>
                                                                                                                            <asp:TemplateField HeaderText="Product Name" HeaderStyle-HorizontalAlign="Left">
                                                                                                                                <ItemTemplate>
                                                                                                                                    <asp:Label ID="lblProductName" runat="server" Text='<%#Bind("Name") %>'></asp:Label>
                                                                                                                                </ItemTemplate>
                                                                                                                                <ItemStyle HorizontalAlign="Left" />
                                                                                                                            </asp:TemplateField>
                                                                                                                            <asp:TemplateField HeaderText="SKU" HeaderStyle-HorizontalAlign="Center">
                                                                                                                                <ItemTemplate>
                                                                                                                                    <asp:Label ID="lblProductSKU" runat="server" Text='<%#Bind("SKU") %>'></asp:Label>
                                                                                                                                </ItemTemplate>
                                                                                                                                <ItemStyle HorizontalAlign="Left" />
                                                                                                                            </asp:TemplateField>
                                                                                                                            <asp:TemplateField HeaderText="Quantity" HeaderStyle-HorizontalAlign="Center">
                                                                                                                                <ItemTemplate>
                                                                                                                                    <asp:Label ID="lblQuantity" runat="server" Text='<%#Bind("Quantity") %>'> </asp:Label>
                                                                                                                                    <%--<asp:TextBox ID="txtPriority" runat="server" Text="0" Width="20px"></asp:TextBox>--%>
                                                                                                                                </ItemTemplate>
                                                                                                                                <EditItemTemplate>
                                                                                                                                    <asp:TextBox ID="txtQuantity" CssClass="order-textfield" runat="server" Visible="True" Width="40px" onkeypress="return keyRestrict(event,'0123456789');"
                                                                                                                                        Text='<%#Bind("Quantity") %>' MaxLength="5" Style="text-align: center;"></asp:TextBox>
                                                                                                                                </EditItemTemplate>
                                                                                                                                <ItemStyle HorizontalAlign="Center" />
                                                                                                                            </asp:TemplateField>
                                                                                                                            <asp:TemplateField HeaderText="Operations" HeaderStyle-HorizontalAlign="Center">
                                                                                                                                <ItemTemplate>
                                                                                                                                    <asp:ImageButton runat="server" ID="_editLinkButton" ToolTip="Edit" CommandName="edit"
                                                                                                                                        CommandArgument='<%# Eval("ProductID") %>' ImageUrl="~/App_Themes/Gray/images/edit-icon.png"></asp:ImageButton>
                                                                                                                                    <asp:ImageButton runat="server" ID="_deleteLinkButton" ToolTip="Delete" CommandName="del"
                                                                                                                                        CommandArgument='<%# Eval("ProductID") %>' message='<%# Eval("ProductID") %>'
                                                                                                                                        OnClientClick="javascript:if(confirm('Are you sure want to delete this record?')){return true;}else{return false;}"
                                                                                                                                        ImageUrl="~/App_Themes/Gray/images/delete-icon.png"></asp:ImageButton>
                                                                                                                                </ItemTemplate>
                                                                                                                                <EditItemTemplate>
                                                                                                                                    <asp:ImageButton ID="btnSave" runat="server" Visible="true" CommandArgument='<%# DataBinder.Eval(Container.DataItem,"ProductID") %>'
                                                                                                                                        CommandName="Save" AlternateText="Save" ImageUrl="~/App_Themes/Gray/images/save.png" />
                                                                                                                                    <asp:ImageButton ID="btnCancel" runat="server" Visible="true" CommandName="Cancel"
                                                                                                                                        ImageUrl="~/App_Themes/Gray/images/CloseIcon.png" Height="16px" Width="16px"
                                                                                                                                        AlternateText="Cancel" CommandArgument='<%# DataBinder.Eval(Container.DataItem,"ProductID") %>' />
                                                                                                                                </EditItemTemplate>
                                                                                                                                <FooterTemplate>
                                                                                                                                    <asp:ImageButton ID="ibtnFeatureProduct" runat="server" OnClientClick="return openCenteredCrossSaleWindow2('ContentPlaceHolder1_hdnProductALLSku','ContentPlaceHolder1_hdnProductALLQty');" />
                                                                                                                                </FooterTemplate>
                                                                                                                            </asp:TemplateField>
                                                                                                                        </Columns>
                                                                                                                        <RowStyle HorizontalAlign="Center" CssClass="oddrow" BackColor="White" />
                                                                                                                        <AlternatingRowStyle CssClass="altrow" />
                                                                                                                        <FooterStyle HorizontalAlign="Right" />
                                                                                                                    </asp:GridView>
                                                                                                                </ContentTemplate>
                                                                                                            </asp:UpdatePanel>
                                                                                                        </td>
                                                                                                    </tr>

                                                                                                    <tr class="oddrow">
                                                                                                        <td width="12%">Fabric Category:
                                                                                                        </td>
                                                                                                        <td>
                                                                                                            <asp:UpdateProgress ID="UpdateProgressFabric" runat="server" AssociatedUpdatePanelID="UplFabricType">
                                                                                                                <ProgressTemplate>
                                                                                                                    <div style="position: relative;">
                                                                                                                        <div style="position: absolute; bottom: -27px; left: 40%;">
                                                                                                                            <img alt="" src="../images/ProductLoader.gif" />
                                                                                                                            <b>Loading ... ... Please wait!</b>
                                                                                                                        </div>
                                                                                                                    </div>
                                                                                                                </ProgressTemplate>
                                                                                                            </asp:UpdateProgress>
                                                                                                            <asp:UpdatePanel ID="UplFabricType" runat="server" UpdateMode="Always">
                                                                                                                <ContentTemplate>
                                                                                                                    <asp:DropDownList ID="ddlFabricType" runat="server" class="product-type" OnSelectedIndexChanged="ddlFabricType_SelectedIndexChanged"
                                                                                                                        AutoPostBack="true">
                                                                                                                    </asp:DropDownList>&nbsp;&nbsp;&nbsp;<a id="FabricTypeLink" runat="server" style="color: #B92127; text-decoration: underline;">Add Fabric Category</a>
                                                                                                                </ContentTemplate>
                                                                                                            </asp:UpdatePanel>
                                                                                                        </td>
                                                                                                        <td width="12%">Fabric Code:
                                                                                                        </td>
                                                                                                        <td width="12%">
                                                                                                            <asp:UpdatePanel ID="UplFabricCode" runat="server" UpdateMode="Always">
                                                                                                                <ContentTemplate>
                                                                                                                    <asp:DropDownList ID="ddlFabricCode" runat="server" class="product-type">
                                                                                                                    </asp:DropDownList>&nbsp;&nbsp;&nbsp;<a id="FabricCodeLink" runat="server" style="color: #B92127; text-decoration: underline;">Add Fabric Code</a>&nbsp;
                                                                                                                </ContentTemplate>
                                                                                                            </asp:UpdatePanel>
                                                                                                            <div style="display: none;">
                                                                                                                Fabric Vendor:&nbsp;<asp:CheckBoxList ID="chkFabricvendor" Height="65px" Width="100px" Style="float: right; margin-right: 176px;" runat="server" />
                                                                                                                <asp:DropDownList ID="ddlFabricvendor" runat="server" class="product-type" Visible="false">
                                                                                                                </asp:DropDownList>
                                                                                                            </div>
                                                                                                        </td>
                                                                                                    </tr>

                                                                                                    <tr class="altrow">
                                                                                                        <td colspan="5">
                                                                                                            <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                                                                                                <tr>
                                                                                                                    <td width="12%" valign="top">Color Options:
                                                                                                                    </td>
                                                                                                                    <td>
                                                                                                                        <asp:TextBox ID="txtcoloroptions" TextMode="MultiLine" runat="server" CssClass="status-textfield"
                                                                                                                            Width="98%" TabIndex="32"></asp:TextBox>
                                                                                                                    </td>
                                                                                                                    <td width="14%" align="left" valign="bottom">
                                                                                                                        <a id="acolorOption" name="acolorOpt" onclick="openCenteredCrossSaleWindow('ContentPlaceHolder1_txtcoloroptions');"
                                                                                                                            style="margin-right: 15px; font-weight: bold; cursor: pointer;" tabindex="34">Select
                                                                                                                            Product(s) </a>
                                                                                                                    </td>
                                                                                                                </tr>
                                                                                                                <tr>
                                                                                                                    <td width="12%" valign="top">Color Title:
                                                                                                                    </td>
                                                                                                                    <td>
                                                                                                                        <asp:TextBox ID="txtcolorTitle" runat="server" CssClass="status-textfield"
                                                                                                                            Width="48%" TabIndex="32"></asp:TextBox>
                                                                                                                    </td>
                                                                                                                    <td width="14%" align="left" valign="bottom"></td>
                                                                                                                </tr>
                                                                                                                <tr>
                                                                                                                    <td width="12%" valign="top">Main Color:
                                                                                                                    </td>
                                                                                                                    <td>
                                                                                                                        <asp:DropDownList ID="ddlMainColor" runat="server" class="product-type" AutoPostBack="false">
                                                                                                                        </asp:DropDownList>
                                                                                                                    </td>
                                                                                                                    <td width="14%" align="left" valign="bottom"></td>
                                                                                                                </tr>
                                                                                                                <tr>
                                                                                                                    <td width="12%" valign="top">Show On EFF :
                                                                                                                    </td>
                                                                                                                    <td>
                                                                                                                        <asp:RadioButtonList ID="rdoShowOnEffProduct" runat="server" RepeatColumns="2" RepeatDirection="Horizontal">
                                                                                                                            <asp:ListItem Value="True" Selected="True">Yes</asp:ListItem>
                                                                                                                            <asp:ListItem Value="False">No</asp:ListItem>
                                                                                                                        </asp:RadioButtonList>
                                                                                                                    </td>
                                                                                                                    <td width="14%" align="left" valign="bottom"></td>
                                                                                                                </tr>
                                                                                                            </table>
                                                                                                        </td>
                                                                                                    </tr>
                                                                                                    <tr class="oddrow">
                                                                                                        <td colspan="5">
                                                                                                            <table width="100%" cellspacing="0" cellpadding="0" border="0" class="content-table border-td">

                                                                                                                <tr>
                                                                                                                    <th>
                                                                                                                        <div class="main-title-left">
                                                                                                                            <img class="img-left" title="Images" alt="Images" src="/App_Themes/<%=Page.Theme %>/icon/add-product.png">
                                                                                                                            <h2>Product Properties</h2>
                                                                                                                        </div>
                                                                                                                        <div class="main-title-right">
                                                                                                                            <a href="javascript:void(0);" class="ProslidingDivDesc_a" onclick="return ShowHideButton('imgMainproperty','tdMainProperty');">
                                                                                                                                <img id="imgMainproperty" class="minimize" title="Minimize" alt="Minimize" src="/App_Themes/<%=Page.Theme %>/images/minimize.png" /></a>
                                                                                                                        </div>
                                                                                                                    </th>
                                                                                                                </tr>
                                                                                                                <tr>
                                                                                                                    <td id="tdMainProperty">
                                                                                                                        <div id="div33" class="proslidingDivDesc">
                                                                                                                            <table cellpadding="0" cellspacing="0" width="100%">
                                                                                                                                <tr class="altrow">
                                                                                                                                    <td style="display: none">Call Us for Price:
                                                                                                                                        <asp:CheckBox ID="chkCallusforPrice" Text="" runat="server" />
                                                                                                                                    </td>
                                                                                                                                    <%-- <td width="20%"></td><td colspan="8></td>--%>
                                                                                                                                    <%-- <td runat="server" id="Isspecialtd" "> Is Special: <asp:CheckBox ID="chkIsSpecial" runat="server" TabIndex="25" /> </td>--%>
                                                                                                                                    <td runat="server" id="Td1" visible="false">Is Free Engraving:
                                                                                                                                        <asp:CheckBox ID="chkIsFreeEngraving" runat="server" TabIndex="26" />
                                                                                                                                    </td>
                                                                                                                                </tr>
                                                                                                                                <tr>
                                                                                                                                    <td style="display: none">Gift Wrap:
                                                                                                                                        <asp:CheckBox ID="chkGiftWrap" runat="server" />
                                                                                                                                    </td>
                                                                                                                                    <td style="display: none">Is Authorize Refund:
                                                                                                                                        <asp:CheckBox ID="chkIsAuthorizeRefund" runat="server" />
                                                                                                                                    </td>
                                                                                                                                    <td></td>
                                                                                                                                </tr>
                                                                                                                                <tr class="altrow">
                                                                                                                                    <td colspan="9">
                                                                                                                                        <table width="100%">
                                                                                                                                            <tr>
                                                                                                                                                <td align="left" width="8%">Ready Made:
                                                                                                                                                    <asp:CheckBox ID="chkisreadymade" onclick="CheckboxChange(this.id)" runat="server" />
                                                                                                                                                </td>
                                                                                                                                                <td align="left" width="10%">Made To Order:
                                                                                                                                                    <asp:CheckBox ID="chkisordermade" onclick="CheckboxChange(this.id)" runat="server" /></td>
                                                                                                                                                <td align="left" width="10%">Made To Measure:
                                                                                                                                                    <asp:CheckBox ID="chkismadetomeasure" onclick="CheckboxChange(this.id)" runat="server" TabIndex="26" />
                                                                                                                                                </td>
                                                                                                                                                <td align="left">
                                                                                                                                                    <div style="float: left; padding-right: 10px;">
                                                                                                                                                        Order Swatch:
                                                                                                                                                        <asp:CheckBox ID="chkismadetoswatch" onclick="CheckboxChange(this.id)" runat="server" TabIndex="26" />
                                                                                                                                                    </div>
                                                                                                                                                    <div id="divProductSwatch" runat="server" style="display: none; float: left;">
                                                                                                                                                        Product Swatch:                                                                                            
                                                                                                                                                        <asp:TextBox ID="txtProductSwatchId" runat="server" CssClass="order-textfield"></asp:TextBox>
                                                                                                                                                        <a id="atagSwatchProduct" name="aOptAcc" onclick="openCenteredCrossSaleWindowforProductSwatch('ContentPlaceHolder1_txtProductSwatchId');"
                                                                                                                                                            style="margin-right: 15px; font-weight: bold; cursor: pointer;">Select Product(s)
                                                                                                                                                        </a>
                                                                                                                                                    </div>
                                                                                                                                                </td>
                                                                                                                                            </tr>
                                                                                                                                        </table>
                                                                                                                                    </td>
                                                                                                                                </tr>
                                                                                                                </tr>
                                                                                                                <tr class="altrow">
                                                                                                                    <td colspan="9">
                                                                                                                        <table>
                                                                                                                            <tr>
                                                                                                                                <td align="left">Ready Made Return:
                                                                                                <asp:CheckBox ID="chkReadyMadeReturn" onclick="CheckboxChangeReturn(this.id)" runat="server" />
                                                                                                                                </td>
                                                                                                                                <td align="left">Made To Order Return:
                                                                                                <asp:CheckBox ID="chkMadeToOrderReturn" onclick="CheckboxChangeReturn(this.id)" runat="server" TabIndex="26" />
                                                                                                                                </td>
                                                                                                                                <td align="left">Made To Measure Return:
                                                                                                <asp:CheckBox ID="chkMadeToMeasureReturn" runat="server" TabIndex="26" />
                                                                                                                                </td>
                                                                                                                                <td style="display: none;"></td>
                                                                                                                            </tr>
                                                                                                                        </table>
                                                                                                                    </td>
                                                                                                                </tr>
                                                                                                                <tr style="display: none;">
                                                                                                                    <td>Is Swatch:
                                                                                                <asp:CheckBox ID="chkIsfreefabricswatch" Text="" runat="server" />
                                                                                                                    </td>
                                                                                                                    <td>Is Custom:
                                                                                                <asp:CheckBox ID="chkIsCustom" runat="server" TabIndex="26" />
                                                                                                                    </td>
                                                                                                                    <td colspan="6">&nbsp;
                                                                                                                    </td>
                                                                                                                </tr>

                                                                                                                <tr style="height: 25px">
                                                                                                                    <td style="display: none;">Is Roman:
                                                                                                <asp:CheckBox ID="chkIsRoman" runat="server" TabIndex="26" onchange="SelectDivRomanYardValue();" Onclick="SelectDivRomanYardValue();" />
                                                                                                                    </td>
                                                                                                                    <td colspan="8">
                                                                                                                        <table cellpadding="0" cellspacing="0" id="divRomanShadeYard" runat="server" style="display: none;">
                                                                                                                            <tr>
                                                                                                                                <td>Roman Shade Yardage:
                                                                                                                                </td>
                                                                                                                                <td>
                                                                                                                                    <asp:DropDownList ID="ddlRomanShadeYardage" AutoPostBack="false" runat="server" class="product-type"
                                                                                                                                        TabIndex="20">
                                                                                                                                    </asp:DropDownList>&nbsp;&nbsp;<a href="javascript:void(0);" id="aromanadd" runat="server" style="color: #B92127; text-decoration: underline;">Add Roman Shade Yardage</a>
                                                                                                                                </td>
                                                                                                                            </tr>
                                                                                                                        </table>
                                                                                                                    </td>
                                                                                                                    <td runat="server" id="Isspecialtd">Is Special:
                                                                                                <asp:CheckBox ID="chkIsSpecial" runat="server" TabIndex="25" />
                                                                                                                    </td>
                                                                                                                </tr>

                                                                                                            </table>
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
    </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td>
                                                                                <table width="100%" border="0" cellpadding="0" cellspacing="0">

                                                                                    <tr>
                                                                                        <td width="60%" valign="top">
                                                                                            <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                                                                                <tr>
                                                                                                    <td>
                                                                                                        <table width="100%" cellspacing="0" cellpadding="0" border="0" class="content-table border-td">
                                                                                                            <tbody>
                                                                                                                <tr>
                                                                                                                    <th>
                                                                                                                        <div class="main-title-left">
                                                                                                                            <img class="img-left" title="Description" alt="Description" src="/App_Themes/<%=Page.Theme %>/icon/add-product.png" />
                                                                                                                            <h2>Description</h2>
                                                                                                                        </div>
                                                                                                                        <div class="main-title-right">
                                                                                                                            <a href="javascript:void(0);" class="show_hideProductDesc" onclick="return ShowHideButton('imgDescription','tdDescription');">
                                                                                                                                <img id="imgDescription" src="/App_Themes/<%=Page.Theme %>/images/minimize.png" class="minimize"
                                                                                                                                    title="Minimize" alt="Minimize" /></a>
                                                                                                                        </div>
                                                                                                                    </th>
                                                                                                                </tr>
                                                                                                                <tr>
                                                                                                                    <td id="tdDescription" style="font: 12px;">
                                                                                                                        <div id="tab-container-1" class="slidingDivProductDesc">
                                                                                                                            <ul class="menu">
                                                                                                                                <li id="lip1" onclick="TabdisplayProduct(1);" class="active">Description</li>
                                                                                                                                <li id="lip2" onclick="TabdisplayProduct(2);" class="">Features</li>
                                                                                                                                <%--<li id="lip3" onclick="TabdisplayProduct(33);" style="display: none;" class="">Shipping Time</li>--%>
                                                                                                                                <li id="lip4" onclick="TabdisplayProduct(4);" class="">Properties</li>
                                                                                                                                <li id="lip3" onclick="TabdisplayProduct(3);" class="">Video Detail</li>
                                                                                                                            </ul>
                                                                                                                            <span class="clear"></span>
                                                                                                                            <div id="divtab1" class="tab-content product-Description">
                                                                                                                                <div class="tab-content-3">
                                                                                                                                    <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                                                                                                                        <tr>
                                                                                                                                            <td>
                                                                                                                                                <div id="divDescription" runat="Server" visible="false">
                                                                                                                                                    <table width="100%" border="0" cellpadding="0" cellspacing="0" class="table">
                                                                                                                                                        <tr>
                                                                                                                                                            <td width="144px">
                                                                                                                                                                <asp:DropDownList ID="ddlDescription" runat="server" class="product-type" AutoPostBack="True"
                                                                                                                                                                    OnSelectedIndexChanged="ddlDescription_SelectedIndexChanged" Width="142px">
                                                                                                                                                                    <asp:ListItem Text="Description Tab : 01" Value="1"></asp:ListItem>
                                                                                                                                                                    <asp:ListItem Text="Description Tab : 02" Value="2"></asp:ListItem>
                                                                                                                                                                    <asp:ListItem Text="Description Tab : 03" Value="3"></asp:ListItem>
                                                                                                                                                                    <asp:ListItem Text="Description Tab : 04" Value="4"></asp:ListItem>
                                                                                                                                                                    <asp:ListItem Text="Description Tab : 05" Value="5"></asp:ListItem>
                                                                                                                                                                </asp:DropDownList>
                                                                                                                                                            </td>
                                                                                                                                                            <td style="text-align: left">Tab Title:
                                                                                                                                        <asp:TextBox ID="txtTitleDesc" runat="server" class="order-textfield" MaxLength="50"></asp:TextBox>&nbsp;&nbsp;
                                                                                                                                        <asp:LinkButton ID="lnkSaveDesc" runat="server" Font-Bold="true" Font-Size="12px"
                                                                                                                                            Font-Underline="true" Text="Save Description" OnClick="lnkSaveDesc_Click1">Save Description</asp:LinkButton>
                                                                                                                                                            </td>
                                                                                                                                                            <td align="left"></td>
                                                                                                                                                        </tr>
                                                                                                                                                        <tr style="display: none;">
                                                                                                                                                            <td>
                                                                                                                                                                <span>Is Tabbing Display</span>
                                                                                                                                                                <asp:CheckBox ID="chkIsTabbingDisplay" Checked="true" runat="server" />
                                                                                                                                                            </td>
                                                                                                                                                            <td></td>
                                                                                                                                                            <td>&nbsp;
                                                                                                                                                            </td>
                                                                                                                                                        </tr>
                                                                                                                                                    </table>
                                                                                                                                                </div>
                                                                                                                                            </td>
                                                                                                                                        </tr>
                                                                                                                                        <tr>
                                                                                                                                            <td>
                                                                                                                                                <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                                                                                                                                    <tr>
                                                                                                                                                        <td class="ckeditor-table">
                                                                                                                                                            <asp:TextBox TextMode="multiLine" class="description-textarea" ID="txtDescription"
                                                                                                                                                                TabIndex="27" Rows="10" Columns="80" runat="server" Style="border: solid 1px #e7e7e7; background: none repeat scroll 0 0 #E7E7E7;"></asp:TextBox>
                                                                                                                                                            <script type="text/javascript">
                                                                                                                                                                CKEDITOR.replace('<%= txtDescription.ClientID %>', { baseHref: '<%= ResolveUrl("~/ckeditor/") %>', height: 400 });
                                                                                                                                                                CKEDITOR.on('dialogDefinition', function (ev) {
                                                                                                                                                                    if (ev.data.name == 'image') {
                                                                                                                                                                        var btn = ev.data.definition.getContents('info').get('browse');
                                                                                                                                                                        btn.hidden = false;
                                                                                                                                                                        btn.onClick = function () { window.open(CKEDITOR.basePath + 'ImageBrowser.aspx', 'popuppage', 'scrollbars=no,width=780,height=630,left=' + ((screen.width - 780) / 2) + ',top=' + ((screen.height - 630) / 2) + ',resizable=no,toolbar=no,titlebar=no'); };
                                                                                                                                                                    }
                                                                                                                                                                    if (ev.data.name == 'link') {
                                                                                                                                                                        var btn = ev.data.definition.getContents('info').get('browse');
                                                                                                                                                                        btn.hidden = false;
                                                                                                                                                                        btn.onClick = function () { window.open(CKEDITOR.basePath + 'LinkBrowser.aspx', 'popuppage', 'scrollbars=no,width=520,height=580,left=' + ((screen.width - 520) / 2) + ',top=' + ((screen.height - 580) / 2) + ',resizable=no,toolbar=no,titlebar=no'); };
                                                                                                                                                                    }
                                                                                                                                                                });
                                                                                                                                                            </script>
                                                                                                                                                        </td>
                                                                                                                                                    </tr>
                                                                                                                                                </table>
                                                                                                                                            </td>
                                                                                                                                        </tr>
                                                                                                                                        <tr>
                                                                                                                                            <td>&nbsp;
                                                                                                                                            </td>
                                                                                                                                        </tr>
                                                                                                                                    </table>
                                                                                                                                </div>
                                                                                                                            </div>
                                                                                                                            <div id="divtab2" class="tab-content product-Description" style="display: none;">
                                                                                                                                <div class="tab-content-3">
                                                                                                                                    <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                                                                                                                        <tr>
                                                                                                                                            <td>
                                                                                                                                                <table width="100%" border="0" cellpadding="0" cellspacing="0" class="table">
                                                                                                                                                    <tr>
                                                                                                                                                        <td>
                                                                                                                                                            <asp:DropDownList ID="ddlproductfeature" runat="server" class="product-type" AutoPostBack="True"
                                                                                                                                                                OnSelectedIndexChanged="ddlproductfeature_SelectedIndexChanged" Width="160px">
                                                                                                                                                            </asp:DropDownList>
                                                                                                                                                        </td>
                                                                                                                                                    </tr>
                                                                                                                                                </table>
                                                                                                                                            </td>
                                                                                                                                        </tr>
                                                                                                                                        <tr>
                                                                                                                                            <td>
                                                                                                                                                <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                                                                                                                                    <tr>
                                                                                                                                                        <td class="ckeditor-table">
                                                                                                                                                            <asp:TextBox TextMode="multiLine" class="description-textarea" ID="txtFeatures" Rows="10"
                                                                                                                                                                Columns="80" runat="server" Style="border: solid 1px #e7e7e7; background: none repeat scroll 0 0 #E7E7E7;"></asp:TextBox>
                                                                                                                                                            <script type="text/javascript">
                                                                                                                                                                CKEDITOR.replace('<%= txtFeatures.ClientID %>', { baseHref: '<%= ResolveUrl("~/ckeditor/") %>', height: 400 });
                                                                                                                                                                CKEDITOR.on('dialogDefinition', function (ev) {
                                                                                                                                                                    if (ev.data.name == 'image') {
                                                                                                                                                                        var btn = ev.data.definition.getContents('info').get('browse');
                                                                                                                                                                        btn.hidden = false;
                                                                                                                                                                        btn.onClick = function () { window.open(CKEDITOR.basePath + 'ImageBrowser.aspx', 'popuppage', 'scrollbars=no,width=780,height=630,left=' + ((screen.width - 780) / 2) + ',top=' + ((screen.height - 630) / 2) + ',resizable=no,toolbar=no,titlebar=no'); };
                                                                                                                                                                    }
                                                                                                                                                                    if (ev.data.name == 'link') {
                                                                                                                                                                        var btn = ev.data.definition.getContents('info').get('browse');
                                                                                                                                                                        btn.hidden = false;
                                                                                                                                                                        btn.onClick = function () { window.open(CKEDITOR.basePath + 'LinkBrowser.aspx', 'popuppage', 'scrollbars=no,width=520,height=580,left=' + ((screen.width - 520) / 2) + ',top=' + ((screen.height - 580) / 2) + ',resizable=no,toolbar=no,titlebar=no'); };
                                                                                                                                                                    }
                                                                                                                                                                });
                                                                                                                                                            </script>
                                                                                                                                                        </td>
                                                                                                                                                    </tr>
                                                                                                                                                </table>
                                                                                                                                            </td>
                                                                                                                                        </tr>
                                                                                                                                        <tr>
                                                                                                                                            <td>&nbsp;
                                                                                                                                            </td>
                                                                                                                                        </tr>
                                                                                                                                    </table>
                                                                                                                                </div>
                                                                                                                            </div>
                                                                                                                            <div id="divtab3" class="tab-content product-Description" style="display: none;">
                                                                                                                                <div class="tab-content-3">
                                                                                                                                    <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                                                                                                                        <tr>
                                                                                                                                            <td>
                                                                                                                                                <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                                                                                                                                    <tr>
                                                                                                                                                        <td class="ckeditor-table">
                                                                                                                                                            <asp:TextBox TextMode="multiLine" class="description-textarea" ID="txtVideoDetail"
                                                                                                                                                                Rows="10" Columns="80" runat="server" Style="border: solid 1px #e7e7e7; background: none repeat scroll 0 0 #E7E7E7;"></asp:TextBox>
                                                                                                                                                            <script type="text/javascript">
                                                                                                                                                                CKEDITOR.replace('<%= txtVideoDetail.ClientID %>', { baseHref: '<%= ResolveUrl("~/ckeditor/") %>', height: 400 });
                                                                                                                                                                CKEDITOR.on('dialogDefinition', function (ev) {
                                                                                                                                                                    if (ev.data.name == 'image') {
                                                                                                                                                                        var btn = ev.data.definition.getContents('info').get('browse');
                                                                                                                                                                        btn.hidden = false;
                                                                                                                                                                        btn.onClick = function () { window.open(CKEDITOR.basePath + 'ImageBrowser.aspx', 'popuppage', 'scrollbars=no,width=780,height=630,left=' + ((screen.width - 780) / 2) + ',top=' + ((screen.height - 630) / 2) + ',resizable=no,toolbar=no,titlebar=no'); };
                                                                                                                                                                    }
                                                                                                                                                                    if (ev.data.name == 'link') {
                                                                                                                                                                        var btn = ev.data.definition.getContents('info').get('browse');
                                                                                                                                                                        btn.hidden = false;
                                                                                                                                                                        btn.onClick = function () { window.open(CKEDITOR.basePath + 'LinkBrowser.aspx', 'popuppage', 'scrollbars=no,width=520,height=580,left=' + ((screen.width - 520) / 2) + ',top=' + ((screen.height - 580) / 2) + ',resizable=no,toolbar=no,titlebar=no'); };
                                                                                                                                                                    }
                                                                                                                                                                });
                                                                                                                                                            </script>
                                                                                                                                                        </td>
                                                                                                                                                    </tr>
                                                                                                                                                </table>
                                                                                                                                            </td>
                                                                                                                                        </tr>
                                                                                                                                        <tr>
                                                                                                                                            <td>&nbsp;
                                                                                                                                            </td>
                                                                                                                                        </tr>
                                                                                                                                    </table>
                                                                                                                                </div>
                                                                                                                            </div>
                                                                                                                            <div id="divtab33" class="tab-content product-Description" style="display: none;">
                                                                                                                                <div class="tab-content-3">
                                                                                                                                    <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                                                                                                                        <tr>
                                                                                                                                            <td>
                                                                                                                                                <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                                                                                                                                    <tr>
                                                                                                                                                        <td class="ckeditor-table">
                                                                                                                                                            <asp:TextBox TextMode="multiLine" class="description-textarea" ID="txtShippingTime"
                                                                                                                                                                Rows="10" Columns="80" runat="server" Style="border: solid 1px #e7e7e7; background: none repeat scroll 0 0 #E7E7E7;"></asp:TextBox>
                                                                                                                                                            <script type="text/javascript">
                                                                                                                                                                CKEDITOR.replace('<%= txtShippingTime.ClientID %>', { baseHref: '<%= ResolveUrl("~/ckeditor/") %>', height: 400 });
                                                                                                                                                                CKEDITOR.on('dialogDefinition', function (ev) {
                                                                                                                                                                    if (ev.data.name == 'image') {
                                                                                                                                                                        var btn = ev.data.definition.getContents('info').get('browse');
                                                                                                                                                                        btn.hidden = false;
                                                                                                                                                                        btn.onClick = function () { window.open(CKEDITOR.basePath + 'ImageBrowser.aspx', 'popuppage', 'scrollbars=no,width=780,height=630,left=' + ((screen.width - 780) / 2) + ',top=' + ((screen.height - 630) / 2) + ',resizable=no,toolbar=no,titlebar=no'); };
                                                                                                                                                                    }
                                                                                                                                                                    if (ev.data.name == 'link') {
                                                                                                                                                                        var btn = ev.data.definition.getContents('info').get('browse');
                                                                                                                                                                        btn.hidden = false;
                                                                                                                                                                        btn.onClick = function () { window.open(CKEDITOR.basePath + 'LinkBrowser.aspx', 'popuppage', 'scrollbars=no,width=520,height=580,left=' + ((screen.width - 520) / 2) + ',top=' + ((screen.height - 580) / 2) + ',resizable=no,toolbar=no,titlebar=no'); };
                                                                                                                                                                    }
                                                                                                                                                                });
                                                                                                                                                            </script>
                                                                                                                                                        </td>
                                                                                                                                                    </tr>
                                                                                                                                                </table>
                                                                                                                                            </td>
                                                                                                                                        </tr>
                                                                                                                                        <tr>
                                                                                                                                            <td>&nbsp;
                                                                                                                                            </td>
                                                                                                                                        </tr>
                                                                                                                                    </table>
                                                                                                                                </div>
                                                                                                                            </div>
                                                                                                                            <div id="divtab4" class="tab-content product-Description" style="display: none;">
                                                                                                                                <div class="tab-content-3" style="font-size: 12px;">
                                                                                                                                    <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                                                                                                                        <tr>
                                                                                                                                            <td>
                                                                                                                                                <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                                                                                                                                    <tr>
                                                                                                                                                        <td>Is Property:
                                                                                                                        <asp:CheckBox ID="chkIsProperty" runat="server" Style="vertical-align: middle;" onchange="SelectDivValue();" Onclick="SelectDivValue();" />
                                                                                                                                                        </td>
                                                                                                                                                        <td colspan="2">
                                                                                                                                                            <table cellpadding="0" cellspacing="0" id="divproperty" runat="server" style="display: none;">
                                                                                                                                                                <tr>
                                                                                                                                                                    <td>Light Control
                                                                                                                                                                    </td>
                                                                                                                                                                    <td>
                                                                                                                                                                        <asp:DropDownList ID="ddlLightControl" runat="server" Width="45px" Height="17px"
                                                                                                                                                                            class="product-type">
                                                                                                                                                                            <asp:ListItem Text="1" Value="1"></asp:ListItem>
                                                                                                                                                                            <asp:ListItem Text="2" Value="2"></asp:ListItem>
                                                                                                                                                                            <asp:ListItem Text="3" Value="3"></asp:ListItem>
                                                                                                                                                                            <asp:ListItem Text="4" Value="4"></asp:ListItem>
                                                                                                                                                                            <asp:ListItem Text="5" Value="5"></asp:ListItem>
                                                                                                                                                                        </asp:DropDownList>
                                                                                                                                                                    </td>
                                                                                                                                                                    <td>&nbsp;&nbsp;Privacy
                                                                                                                                                                    </td>
                                                                                                                                                                    <td>
                                                                                                                                                                        <asp:DropDownList ID="ddlPrivacy" runat="server" Width="45px" Height="17px" class="product-type">
                                                                                                                                                                            <asp:ListItem Text="1" Value="1"></asp:ListItem>
                                                                                                                                                                            <asp:ListItem Text="2" Value="2"></asp:ListItem>
                                                                                                                                                                            <asp:ListItem Text="3" Value="3"></asp:ListItem>
                                                                                                                                                                            <asp:ListItem Text="4" Value="4"></asp:ListItem>
                                                                                                                                                                            <asp:ListItem Text="5" Value="5"></asp:ListItem>
                                                                                                                                                                        </asp:DropDownList>
                                                                                                                                                                    </td>
                                                                                                                                                                    <td>&nbsp;&nbsp;Efficiency
                                                                                                                                                                    </td>
                                                                                                                                                                    <td>
                                                                                                                                                                        <asp:DropDownList ID="ddlEfficiency" runat="server" Width="45px" Height="17px" class="product-type">
                                                                                                                                                                            <asp:ListItem Text="1" Value="1"></asp:ListItem>
                                                                                                                                                                            <asp:ListItem Text="2" Value="2"></asp:ListItem>
                                                                                                                                                                            <asp:ListItem Text="3" Value="3"></asp:ListItem>
                                                                                                                                                                            <asp:ListItem Text="4" Value="4"></asp:ListItem>
                                                                                                                                                                            <asp:ListItem Text="5" Value="5"></asp:ListItem>
                                                                                                                                                                        </asp:DropDownList>
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
                                                                                                                            </div>
                                                                                                                        </div>
                                                                                                                    </td>
                                                                                                                </tr>
                                                                                                            </tbody>
                                                                                                        </table>
                                                                                                    </td>
                                                                                                </tr>
                                                                                                <tr>
                                                                                                    <td>
                                                                                                        <table width="100%" cellspacing="0" cellpadding="0" border="0" class="content-table border-td">
                                                                                                            <tbody>
                                                                                                                <tr>
                                                                                                                    <th>
                                                                                                                        <div class="main-title-left">
                                                                                                                            <img class="img-left" title="Images" alt="Images" src="/App_Themes/<%=Page.Theme %>/icon/add-product.png">
                                                                                                                            <h2>Images</h2>
                                                                                                                        </div>
                                                                                                                        <div class="main-title-right">
                                                                                                                            <a href="javascript:void(0);" class="show_hideImage" title="Close" onclick="return ShowHideButton('ImgImages','tdImages');">
                                                                                                                                <img id="ImgImages" src="/App_Themes/<%=Page.Theme %>/images/minimize.png" class="minimize"
                                                                                                                                    title="Minimize" alt="Minimize"></a>
                                                                                                                        </div>
                                                                                                                    </th>
                                                                                                                </tr>
                                                                                                                <tr>
                                                                                                                    <td id="tdImages">
                                                                                                                        <div id="divImage" class="slidingDivImage">
                                                                                                                            <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                                                                                                                <tr>
                                                                                                                                    <td>
                                                                                                                                        <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                                                                                                                            <tr>
                                                                                                                                                <td width="20%" valign="top">Main Image:
                                                                                                                                                </td>
                                                                                                                                                <td valign="middle">
                                                                                                                                                    <div style="float: left;">
                                                                                                                                                        <img alt="Upload" id="ImgLarge" src="/App_Themes/<%=Page.Theme %>/images/icon-image.gif"
                                                                                                                                                            runat="server" width="150" style="margin-bottom: 5px; border: 1px solid darkgray" />
                                                                                                                                                    </div>
                                                                                                                                                    <div style="float: right;">
                                                                                                                                                        Icon Width:
                                                                                                                                <asp:TextBox ID="txtIconWidth" CssClass="order-textfield" Style="width: 70px; text-align: center;"
                                                                                                                                    runat="server" onKeyPress="rerutn keyRestrictForInventory(event,'0123456789');"
                                                                                                                                    MaxLength="4"></asp:TextBox>
                                                                                                                                                        &nbsp;&nbsp;Icon Height:
                                                                                                                                <asp:TextBox ID="txtIconHeigth" CssClass="order-textfield" Style="width: 70px; text-align: center;"
                                                                                                                                    runat="server" onKeyPress="rerutn keyRestrictForInventory(event,'0123456789');"
                                                                                                                                    MaxLength="4"></asp:TextBox>
                                                                                                                                                    </div>
                                                                                                                                                </td>
                                                                                                                                            </tr>


                                                                                                                                            <tr>
                                                                                                                                                <td valign="top">Image Description :</td>
                                                                                                                                                <td>

                                                                                                                                                    <asp:TextBox ID="txtImgDesc" CssClass="order-textfield" Style="width: 300px; height: 60px;"
                                                                                                                                                        runat="server" TextMode="MultiLine"></asp:TextBox>
                                                                                                                                                </td>
                                                                                                                                            </tr>

                                                                                                                                            <tr>
                                                                                                                                                <td>&nbsp;
                                                                                                                                                </td>
                                                                                                                                                <td>
                                                                                                                                                    <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                                                                                                                                        <tr>
                                                                                                                                                            <td width="10%">
                                                                                                                                                                <asp:FileUpload ID="fuProductIcon" runat="server" Width="220px" Style="border: 1px solid #1a1a1a; background: #f5f5f5; color: #000000;"
                                                                                                                                                                    TabIndex="25" />
                                                                                                                                                            </td>
                                                                                                                                                            <td width="9%">
                                                                                                                                                                <asp:ImageButton ID="btnUpload" runat="server" AlternateText="Upload" OnClick="btnUpload_Click"
                                                                                                                                                                    TabIndex="26" />
                                                                                                                                                            </td>
                                                                                                                                                            <td width="64%">
                                                                                                                                                                <asp:ImageButton ID="btnDelete" runat="server" Visible="false" AlternateText="Delete"
                                                                                                                                                                    OnClick="btnDelete_Click" />
                                                                                                                                                            </td>
                                                                                                                                                        </tr>
                                                                                                                                                    </table>
                                                                                                                                                </td>
                                                                                                                                            </tr>

                                                                                                                                        </table>
                                                                                                                                    </td>
                                                                                                                                    <td valign="bottom" id="trUploadFiles" runat="server">
                                                                                                                                        <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                                                                                                                            <tr>
                                                                                                                                                <td valign="bottom" style="padding: 0px 2px;">
                                                                                                                                                    <table width="100%" border="0" cellpadding="0" cellspacing="0">

                                                                                                                                                        <tr>
                                                                                                                                                            <td>
                                                                                                                                                                <table width="100%" border="0" cellpadding="0" cellspacing="0" id="tduploadMoreImages"
                                                                                                                                                                    runat="server" visible="false">
                                                                                                                                                                    <tr>
                                                                                                                                                                        <td>
                                                                                                                                                                            <a target="content" class="list_lin" style="cursor: pointer; display: none;" onclick="JavaScript:OpenMoreImagesPopup();">
                                                                                                                                                                                <img src="/App_Themes/<%=Page.Theme %>/images/Upload-More-Images.png" title="Upload More Images " />
                                                                                                                                                                            </a>
                                                                                                                                                                        </td>
                                                                                                                                                                    </tr>
                                                                                                                                                                    <tr>
                                                                                                                                                                        <td style="padding: 0px 2px;">Upload PDF File:
                                                                                                                                                                        </td>
                                                                                                                                                                    </tr>
                                                                                                                                                                </table>
                                                                                                                                                            </td>
                                                                                                                                                        </tr>
                                                                                                                                                        <tr>
                                                                                                                                                            <td id="tduploadPdf" runat="server" visible="false">
                                                                                                                                                                <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                                                                                                                                                    <tr>
                                                                                                                                                                        <td style="padding: 0px 2px;" colspan="3">Upload PDF File:
                                                                                                                                                                        </td>
                                                                                                                                                                    </tr>
                                                                                                                                                                    <tr>
                                                                                                                                                                        <td width="10%">
                                                                                                                                                                            <asp:FileUpload ID="FileUploadPdf" runat="server" Width="220px" Style="border: 1px solid #1a1a1a; background: #f5f5f5; color: #000000;" />
                                                                                                                                                                        </td>
                                                                                                                                                                        <td width="9%">
                                                                                                                                                                            <asp:ImageButton ID="btnUploadPdfFile" runat="server" AlternateText="Upload PDF file"
                                                                                                                                                                                OnClick="btnUploadPdfFile_Click" />
                                                                                                                                                                        </td>
                                                                                                                                                                        <td width="64%">
                                                                                                                                                                            <a target="_blank" href='' id="btnDownloadPDF" visible="false" runat="server" title="Download PDF File">
                                                                                                                                                                                <img src="/App_Themes/<%=Page.Theme %>/images/download-icon.jpg" />
                                                                                                                                                                            </a>
                                                                                                                                                                        </td>
                                                                                                                                                                    </tr>
                                                                                                                                                                </table>
                                                                                                                                                            </td>
                                                                                                                                                        </tr>
                                                                                                                                                    </table>
                                                                                                                                                </td>
                                                                                                                                            </tr>
                                                                                                                                            <tr>
                                                                                                                                                <td>
                                                                                                                                                    <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                                                                                                                                        <tr>
                                                                                                                                                            <td>Upload Product Video:
                                                                                                                                                            </td>
                                                                                                                                                            <td></td>
                                                                                                                                                        </tr>
                                                                                                                                                        <tr>
                                                                                                                                                            <td>Video Title :
                                                                                                                                                                <asp:TextBox ID="txtVideotitle" CssClass="order-textfield"
                                                                                                                                                                    runat="server" Style="float: right; width: 155px;"></asp:TextBox>
                                                                                                                                                            </td>
                                                                                                                                                            <td></td>
                                                                                                                                                        </tr>
                                                                                                                                                        <tr>
                                                                                                                                                            <td width="10%">
                                                                                                                                                                <asp:FileUpload ID="fuProductVideo2" runat="server" Style="width: 220px; border: 1px solid #1a1a1a;"
                                                                                                                                                                    TabIndex="27" /><span style="font-size: 10px; color: red">(Only ".mp4" files)</span>
                                                                                                                                                            </td>
                                                                                                                                                            <td width="9%">
                                                                                                                                                                <asp:ImageButton ID="btnUploadvideo2" ImageUrl="/App_Themes/Gray/images/upload.gif" runat="server" OnClick="btnUploadvideo2_Click"
                                                                                                                                                                    TabIndex="28" />
                                                                                                                                                            </td>
                                                                                                                                                            <td width="64%"></td>
                                                                                                                                                        </tr>
                                                                                                                                                        <tr runat="server" id="trvideodelete2" visible="false">
                                                                                                                                                            <td align="right" width="10%">
                                                                                                                                                                <asp:Label ID="lblVideoName2" runat="server" Text=""></asp:Label>
                                                                                                                                                            </td>
                                                                                                                                                            <td width="9%" align="center">
                                                                                                                                                                <asp:ImageButton ID="btndeleteVideo2" runat="server" ImageUrl="/App_Themes/Gray/images/delet.gif" AlternateText="Delete" ToolTip="Delete Video"
                                                                                                                                                                    OnClick="btndeleteVideo2_Click" Visible="false" OnClientClick="return ConfirmDelete2();" />
                                                                                                                                                                <div style="display: none">
                                                                                                                                                                    <asp:ImageButton ID="btndeleteVideoTemp2" runat="server" ToolTip="Delete" OnClick="btndeleteVideo2_Click" />
                                                                                                                                                                </div>
                                                                                                                                                            </td>
                                                                                                                                                            <td width="64%"></td>
                                                                                                                                                        </tr>
                                                                                                                                                    </table>
                                                                                                                                                </td>
                                                                                                                                            </tr>


                                                                                                                                            <tr>
                                                                                                                                                <td>
                                                                                                                                                    <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                                                                                                                                        <tr>
                                                                                                                                                            <td>Upload Measure Video:
                                                                                                                                                            </td>
                                                                                                                                                            <td></td>
                                                                                                                                                        </tr>
                                                                                                                                                        <tr>
                                                                                                                                                            <td width="10%">
                                                                                                                                                                <asp:FileUpload ID="fuProductVideo" runat="server" Style="width: 220px; border: 1px solid #1a1a1a;"
                                                                                                                                                                    TabIndex="27" /><span style="font-size: 10px; color: red">(Only ".mp4" files)</span>
                                                                                                                                                            </td>
                                                                                                                                                            <td width="9%">
                                                                                                                                                                <asp:ImageButton ID="btnUploadvideo" runat="server" OnClick="btnUploadvideo_Click"
                                                                                                                                                                    TabIndex="28" />
                                                                                                                                                            </td>
                                                                                                                                                            <td width="64%"></td>
                                                                                                                                                        </tr>
                                                                                                                                                        <tr runat="server" id="trvideodelete" visible="false">
                                                                                                                                                            <td align="right" width="10%">
                                                                                                                                                                <asp:Label ID="lblVideoName" runat="server" Text=""></asp:Label>
                                                                                                                                                            </td>
                                                                                                                                                            <td width="9%" align="center">
                                                                                                                                                                <asp:ImageButton ID="btndeleteVideo" runat="server" AlternateText="Delete" ToolTip="Delete Video"
                                                                                                                                                                    OnClick="btndeleteVideo_Click" Visible="false" OnClientClick="return ConfirmDelete();" />
                                                                                                                                                                <div style="display: none">
                                                                                                                                                                    <asp:ImageButton ID="btndeleteVideoTemp" runat="server" ToolTip="Delete" OnClick="btndeleteVideo_Click" />
                                                                                                                                                                </div>
                                                                                                                                                            </td>
                                                                                                                                                            <td width="64%"></td>
                                                                                                                                                        </tr>
                                                                                                                                                    </table>
                                                                                                                                                </td>
                                                                                                                                            </tr>
                                                                                                                                            <tr>
                                                                                                                                                <td>
                                                                                                                                                    <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                                                                                                                                        <tr>
                                                                                                                                                            <td>Upload Header Video:
                                                                                                                                                            </td>
                                                                                                                                                            <td></td>
                                                                                                                                                        </tr>
                                                                                                                                                        <tr>
                                                                                                                                                            <td width="10%">
                                                                                                                                                                <asp:FileUpload ID="fuProductVideo1" runat="server" Style="width: 220px; border: 1px solid #1a1a1a;"
                                                                                                                                                                    TabIndex="27" /><span style="font-size: 10px; color: red">(Only ".mp4" files)</span>
                                                                                                                                                            </td>
                                                                                                                                                            <td width="9%">
                                                                                                                                                                <asp:ImageButton ID="btnUploadvideo1" ImageUrl="/App_Themes/Gray/images/upload.gif" runat="server" OnClick="btnUploadvideo1_Click"
                                                                                                                                                                    TabIndex="28" />
                                                                                                                                                            </td>
                                                                                                                                                            <td width="64%"></td>
                                                                                                                                                        </tr>
                                                                                                                                                        <tr runat="server" id="trvideodelete1" visible="false">
                                                                                                                                                            <td align="right" width="10%">
                                                                                                                                                                <asp:Label ID="lblVideoName1" runat="server" Text=""></asp:Label>
                                                                                                                                                            </td>
                                                                                                                                                            <td width="9%" align="center">
                                                                                                                                                                <asp:ImageButton ID="btndeleteVideo1" runat="server" AlternateText="Delete" ToolTip="Delete Video"
                                                                                                                                                                    OnClick="btndeleteVideo1_Click" ImageUrl="/App_Themes/Gray/images/delet.gif" Visible="false" OnClientClick="return ConfirmDelete1();" />
                                                                                                                                                                <div style="display: none">
                                                                                                                                                                    <asp:ImageButton ID="btndeleteVideoTemp1" runat="server" ToolTip="Delete" OnClick="btndeleteVideo1_Click" />
                                                                                                                                                                </div>
                                                                                                                                                            </td>
                                                                                                                                                            <td width="64%"></td>
                                                                                                                                                        </tr>
                                                                                                                                                    </table>
                                                                                                                                                </td>
                                                                                                                                            </tr>
                                                                                                                                            <td>
                                                                                                                                                <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                                                                                                                                    <tr>
                                                                                                                                                        <td>Upload Youtube Video URL:
                                                                                                                                                        </td>
                                                                                                                                                        <td></td>
                                                                                                                                                    </tr>
                                                                                                                                                    <tr>
                                                                                                                                                        <td width="19%">
                                                                                                                                                            <asp:TextBox ID="txtyoutubevideo" runat="server" CssClass="order-textfield" Style="width: 296px;"></asp:TextBox>
                                                                                                                                                        </td>

                                                                                                                                                        <td width="64%"></td>
                                                                                                                                                    </tr>
                                                                                                                                                </table>
                                                                                                                                            </td>
                                                                                                                                        </table>
                                                                                                                                    </td>
                                                                                                                                </tr>
                                                                                                                            </table>
                                                                                                                        </div>
                                                                                                                    </td>
                                                                                                                </tr>
                                                                                                                <tr>
                                                                                                                    <th style="line-height: 30px;">&nbsp;Upload Alternate Images</th>

                                                                                                                </tr>
                                                                                                                <tr>
                                                                                                                    <td>
                                                                                                                        <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                                                                                                            <tr>
                                                                                                                                <td>
                                                                                                                                    <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                                                                                                                        <tr>
                                                                                                                                            <td width="20%" valign="top">Atl 1 Image:
                                                                                                                                            </td>
                                                                                                                                            <td valign="middle">
                                                                                                                                                <div style="float: left;">
                                                                                                                                                    <img alt="Upload" id="ImgAlt1" src="/App_Themes/<%=Page.Theme %>/images/icon-image.gif"
                                                                                                                                                        runat="server" width="150" style="margin-bottom: 5px; border: 1px solid darkgray" />
                                                                                                                                                </div>

                                                                                                                                            </td>
                                                                                                                                        </tr>


                                                                                                                                        <tr>
                                                                                                                                            <td valign="top">Image Description :</td>
                                                                                                                                            <td>

                                                                                                                                                <asp:TextBox ID="txtalt1" CssClass="order-textfield" Style="width: 300px; height: 60px;"
                                                                                                                                                    runat="server" TextMode="MultiLine"></asp:TextBox>
                                                                                                                                            </td>
                                                                                                                                        </tr>

                                                                                                                                        <tr>
                                                                                                                                            <td>&nbsp;
                                                                                                                                            </td>
                                                                                                                                            <td>
                                                                                                                                                <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                                                                                                                                    <tr>
                                                                                                                                                        <td width="10%">
                                                                                                                                                            <asp:FileUpload ID="flalt1" runat="server" Width="220px" Style="border: 1px solid #1a1a1a; background: #f5f5f5; color: #000000;"
                                                                                                                                                                TabIndex="25" />
                                                                                                                                                        </td>
                                                                                                                                                        <td width="9%">
                                                                                                                                                            <asp:ImageButton ID="imgbtnAlt1" runat="server" AlternateText="Upload" OnClick="imgbtnAlt1_Click"
                                                                                                                                                                TabIndex="26" />
                                                                                                                                                        </td>
                                                                                                                                                        <td width="64%">
                                                                                                                                                            <asp:ImageButton ID="imgbtnAlt1del" runat="server" Visible="false" AlternateText="Delete"
                                                                                                                                                                OnClick="imgbtnAlt1del_Click" />
                                                                                                                                                        </td>
                                                                                                                                                    </tr>
                                                                                                                                                </table>
                                                                                                                                            </td>
                                                                                                                                        </tr>

                                                                                                                                    </table>
                                                                                                                                </td>
                                                                                                                                <td valign="bottom" id="Td2" runat="server"></td>
                                                                                                                            </tr>
                                                                                                                        </table>
                                                                                                                        <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                                                                                                            <tr class="altrow">
                                                                                                                                <td>
                                                                                                                                    <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                                                                                                                        <tr>
                                                                                                                                            <td width="20%" valign="top">Alt 2 Image:
                                                                                                                                            </td>
                                                                                                                                            <td valign="middle">
                                                                                                                                                <div style="float: left;">
                                                                                                                                                    <img alt="Upload" id="ImgAlt2" src="/App_Themes/<%=Page.Theme %>/images/icon-image.gif"
                                                                                                                                                        runat="server" width="150" style="margin-bottom: 5px; border: 1px solid darkgray" />
                                                                                                                                                </div>

                                                                                                                                            </td>
                                                                                                                                        </tr>


                                                                                                                                        <tr>
                                                                                                                                            <td valign="top">Image Description :</td>
                                                                                                                                            <td>

                                                                                                                                                <asp:TextBox ID="txtalt2" CssClass="order-textfield" Style="width: 300px; height: 60px;"
                                                                                                                                                    runat="server" TextMode="MultiLine"></asp:TextBox>
                                                                                                                                            </td>
                                                                                                                                        </tr>

                                                                                                                                        <tr>
                                                                                                                                            <td>&nbsp;
                                                                                                                                            </td>
                                                                                                                                            <td>
                                                                                                                                                <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                                                                                                                                    <tr>
                                                                                                                                                        <td width="10%">
                                                                                                                                                            <asp:FileUpload ID="flalt2" runat="server" Width="220px" Style="border: 1px solid #1a1a1a; background: #f5f5f5; color: #000000;"
                                                                                                                                                                TabIndex="25" />
                                                                                                                                                        </td>
                                                                                                                                                        <td width="9%">
                                                                                                                                                            <asp:ImageButton ID="imgbtnAlt2" runat="server" AlternateText="Upload" OnClick="imgbtnAlt2_Click"
                                                                                                                                                                TabIndex="26" />
                                                                                                                                                        </td>
                                                                                                                                                        <td width="64%">
                                                                                                                                                            <asp:ImageButton ID="imgbtnAlt2del" runat="server" Visible="false" AlternateText="Delete"
                                                                                                                                                                OnClick="imgbtnAlt2del_Click" />
                                                                                                                                                        </td>
                                                                                                                                                    </tr>
                                                                                                                                                </table>
                                                                                                                                            </td>
                                                                                                                                        </tr>

                                                                                                                                    </table>
                                                                                                                                </td>
                                                                                                                                <td valign="bottom" id="Td4" runat="server"></td>
                                                                                                                            </tr>
                                                                                                                        </table>
                                                                                                                        <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                                                                                                            <tr>
                                                                                                                                <td>
                                                                                                                                    <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                                                                                                                        <tr>
                                                                                                                                            <td width="20%" valign="top">Alt 3 Image:
                                                                                                                                            </td>
                                                                                                                                            <td valign="middle">
                                                                                                                                                <div style="float: left;">
                                                                                                                                                    <img alt="Upload" id="ImgAlt3" src="/App_Themes/<%=Page.Theme %>/images/icon-image.gif"
                                                                                                                                                        runat="server" width="150" style="margin-bottom: 5px; border: 1px solid darkgray" />
                                                                                                                                                </div>

                                                                                                                                            </td>
                                                                                                                                        </tr>


                                                                                                                                        <tr>
                                                                                                                                            <td valign="top">Image Description :</td>
                                                                                                                                            <td>

                                                                                                                                                <asp:TextBox ID="txtalt3" CssClass="order-textfield" Style="width: 300px; height: 60px;"
                                                                                                                                                    runat="server" TextMode="MultiLine"></asp:TextBox>
                                                                                                                                            </td>
                                                                                                                                        </tr>

                                                                                                                                        <tr>
                                                                                                                                            <td>&nbsp;
                                                                                                                                            </td>
                                                                                                                                            <td>
                                                                                                                                                <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                                                                                                                                    <tr>
                                                                                                                                                        <td width="10%">
                                                                                                                                                            <asp:FileUpload ID="flalt3" runat="server" Width="220px" Style="border: 1px solid #1a1a1a; background: #f5f5f5; color: #000000;"
                                                                                                                                                                TabIndex="25" />
                                                                                                                                                        </td>
                                                                                                                                                        <td width="9%">
                                                                                                                                                            <asp:ImageButton ID="imgbtnAlt3" runat="server" AlternateText="Upload" OnClick="imgbtnAlt3_Click"
                                                                                                                                                                TabIndex="26" />
                                                                                                                                                        </td>
                                                                                                                                                        <td width="64%">
                                                                                                                                                            <asp:ImageButton ID="imgbtnAlt3del" runat="server" Visible="false" AlternateText="Delete"
                                                                                                                                                                OnClick="imgbtnAlt3del_Click" />
                                                                                                                                                        </td>
                                                                                                                                                    </tr>
                                                                                                                                                </table>
                                                                                                                                            </td>
                                                                                                                                        </tr>

                                                                                                                                    </table>
                                                                                                                                </td>
                                                                                                                                <td valign="bottom" id="Td6" runat="server"></td>
                                                                                                                            </tr>
                                                                                                                        </table>
                                                                                                                        <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                                                                                                            <tr class="altrow">
                                                                                                                                <td>
                                                                                                                                    <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                                                                                                                        <tr>
                                                                                                                                            <td width="20%" valign="top">Alt 4 Image:
                                                                                                                                            </td>
                                                                                                                                            <td valign="middle">
                                                                                                                                                <div style="float: left;">
                                                                                                                                                    <img alt="Upload" id="ImgAlt4" src="/App_Themes/<%=Page.Theme %>/images/icon-image.gif"
                                                                                                                                                        runat="server" width="150" style="margin-bottom: 5px; border: 1px solid darkgray" />
                                                                                                                                                </div>

                                                                                                                                            </td>
                                                                                                                                        </tr>


                                                                                                                                        <tr>
                                                                                                                                            <td valign="top">Image Description :</td>
                                                                                                                                            <td>

                                                                                                                                                <asp:TextBox ID="txtalt4" CssClass="order-textfield" Style="width: 300px; height: 60px;"
                                                                                                                                                    runat="server" TextMode="MultiLine"></asp:TextBox>
                                                                                                                                            </td>
                                                                                                                                        </tr>

                                                                                                                                        <tr>
                                                                                                                                            <td>&nbsp;
                                                                                                                                            </td>
                                                                                                                                            <td>
                                                                                                                                                <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                                                                                                                                    <tr>
                                                                                                                                                        <td width="10%">
                                                                                                                                                            <asp:FileUpload ID="flalt4" runat="server" Width="220px" Style="border: 1px solid #1a1a1a; background: #f5f5f5; color: #000000;"
                                                                                                                                                                TabIndex="25" />
                                                                                                                                                        </td>
                                                                                                                                                        <td width="9%">
                                                                                                                                                            <asp:ImageButton ID="imgbtnAlt4" runat="server" AlternateText="Upload" OnClick="imgbtnAlt4_Click"
                                                                                                                                                                TabIndex="26" />
                                                                                                                                                        </td>
                                                                                                                                                        <td width="64%">
                                                                                                                                                            <asp:ImageButton ID="imgbtnAlt4del" runat="server" Visible="false" AlternateText="Delete"
                                                                                                                                                                OnClick="imgbtnAlt4del_Click" />
                                                                                                                                                        </td>
                                                                                                                                                    </tr>
                                                                                                                                                </table>
                                                                                                                                            </td>
                                                                                                                                        </tr>

                                                                                                                                    </table>
                                                                                                                                </td>
                                                                                                                                <td valign="bottom" id="Td8" runat="server"></td>
                                                                                                                            </tr>
                                                                                                                        </table>
                                                                                                                        <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                                                                                                            <tr>
                                                                                                                                <td>
                                                                                                                                    <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                                                                                                                        <tr>
                                                                                                                                            <td width="20%" valign="top">Alt 5 Image:
                                                                                                                                            </td>
                                                                                                                                            <td valign="middle">
                                                                                                                                                <div style="float: left;">
                                                                                                                                                    <img alt="Upload" id="ImgAlt5" src="/App_Themes/<%=Page.Theme %>/images/icon-image.gif"
                                                                                                                                                        runat="server" width="150" style="margin-bottom: 5px; border: 1px solid darkgray" />
                                                                                                                                                </div>

                                                                                                                                            </td>
                                                                                                                                        </tr>


                                                                                                                                        <tr>
                                                                                                                                            <td valign="top">Image Description :</td>
                                                                                                                                            <td>

                                                                                                                                                <asp:TextBox ID="txtalt5" CssClass="order-textfield" Style="width: 300px; height: 60px;"
                                                                                                                                                    runat="server" TextMode="MultiLine"></asp:TextBox>
                                                                                                                                            </td>
                                                                                                                                        </tr>

                                                                                                                                        <tr>
                                                                                                                                            <td>&nbsp;
                                                                                                                                            </td>
                                                                                                                                            <td>
                                                                                                                                                <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                                                                                                                                    <tr>
                                                                                                                                                        <td width="10%">
                                                                                                                                                            <asp:FileUpload ID="flalt5" runat="server" Width="220px" Style="border: 1px solid #1a1a1a; background: #f5f5f5; color: #000000;"
                                                                                                                                                                TabIndex="25" />
                                                                                                                                                        </td>
                                                                                                                                                        <td width="9%">
                                                                                                                                                            <asp:ImageButton ID="imgbtnAlt5" runat="server" AlternateText="Upload" OnClick="imgbtnAlt5_Click"
                                                                                                                                                                TabIndex="26" />
                                                                                                                                                        </td>
                                                                                                                                                        <td width="64%">
                                                                                                                                                            <asp:ImageButton ID="imgbtnAlt5del" runat="server" Visible="false" AlternateText="Delete"
                                                                                                                                                                OnClick="imgbtnAlt5del_Click" />
                                                                                                                                                        </td>
                                                                                                                                                    </tr>
                                                                                                                                                </table>
                                                                                                                                            </td>
                                                                                                                                        </tr>

                                                                                                                                    </table>
                                                                                                                                </td>
                                                                                                                                <td valign="bottom" id="Td10" runat="server"></td>
                                                                                                                            </tr>
                                                                                                                        </table>
                                                                                                                    </td>
                                                                                                                </tr>
                                                                                                            </tbody>
                                                                                                        </table>
                                                                                                    </td>
                                                                                                </tr>

                                                                                            </table>
                                                                                        </td>
                                                                                        <td width="40%" valign="top">
                                                                                            <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                                                                                <tr>
                                                                                                    <td>
                                                                                                        <table width="100%" border="0" cellpadding="0" cellspacing="0" class="content-table border-td">
                                                                                                            <tr>
                                                                                                                <th>
                                                                                                                    <div class="main-title-left">
                                                                                                                        <img class="img-left" title="Categories" alt="Categories" src="/App_Themes/<%=Page.Theme %>/icon/add-product.png">
                                                                                                                        <h2>Categories
                                                                                                                        </h2>
                                                                                                                    </div>
                                                                                                                    <div class="main-title-right">
                                                                                                                        <a href="javascript:void(0);" class="show_hideCategory" onclick="return ShowHideButton('ImgCategories','tdCategories');">
                                                                                                                            <img id="ImgCategories" src="/App_Themes/<%=Page.Theme %>/images/minimize.png" class="minimize"
                                                                                                                                title="Minimize" alt="Minimize"></a>
                                                                                                                    </div>
                                                                                                                </th>
                                                                                                            </tr>
                                                                                                            <tr>
                                                                                                                <td id="tdCategories">
                                                                                                                    <div id="div1" class="slidingDivCategory">
                                                                                                                        <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                                                                                                            <tr style="display: none;">
                                                                                                                                <td width="18%">Main&nbsp;Category:
                                                                                                                                </td>
                                                                                                                                <td width="82%">
                                                                                                                                    <asp:TextBox ID="txtMaincategory" runat="server" CssClass="order-textfield" MaxLength="100"
                                                                                                                                        Height="19px" Width="290px"></asp:TextBox>
                                                                                                                                </td>
                                                                                                                            </tr>
                                                                                                                            <tr>
                                                                                                                                <td width="18%" valign="top" style="padding: 10px;">
                                                                                                                                    <span class="star">&nbsp;</span>Select&nbsp;Category:
                                                                                                                                </td>
                                                                                                                                <td width="82%" align="left" class="list_table_cell_link " id="TDCategory" runat="server"
                                                                                                                                    style="height: 163px; padding-top: 3px" valign="top">
                                                                                                                                    <div id="divTrvCategories">
                                                                                                                                        <asp:TreeView ID="trvCategories" runat="server" ShowCheckBoxes="Leaf" ForeColor="#212121"
                                                                                                                                            Font-Size="12px" Width="304px" PopulateNodesFromClient="True" ShowLines="true"
                                                                                                                                            TabIndex="24">
                                                                                                                                        </asp:TreeView>
                                                                                                                                        <input id="treeCatID" runat="server" type="hidden" value="0" />
                                                                                                                                    </div>
                                                                                                                                </td>
                                                                                                                            </tr>
                                                                                                                        </table>
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
                                                                            </td>
                                                                        </tr>
    <tr>
        <td>
            <table width="100%" border="0" cellpadding="0" cellspacing="0" class="content-table border-td">
                <tr>
                    <th colspan="6">
                        <div class="main-title-left">
                            <img class="img-left" title="Additional Info" alt="Additional Info" src="/App_Themes/<%=Page.Theme %>/icon/add-product.png">
                            <h2>Additional Info</h2>
                        </div>
                        <div class="main-title-right">
                            <a href="javascript:void(0);" class="show_hideDesc" onclick="return ShowHideButton('ImgDesc','tdDesc');">
                                <img id="ImgDesc" class="minimize" title="Minimize" alt="Minimize" src="/App_Themes/<%=Page.Theme %>/images/minimize.png"></a>
                        </div>
                    </th>
                </tr>
                <tr>
                    <td valign="top" id="tdDesc" colspan="6">
                        <div id="div2" class="slidingDivDesc">
                            <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td width="12%" valign="top">Optional Accessories:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtOptionalAccesories" TextMode="MultiLine" runat="server" CssClass="status-textfield"
                                            Width="98%" TabIndex="32"></asp:TextBox>
                                    </td>
                                    <td width="14%" align="left" valign="bottom">
                                        <a id="aOptAcc" name="aOptAcc" onclick="openCenteredCrossSaleWindowOptionalAcc('ContentPlaceHolder1_txtOptionalAccesories');"
                                            style="margin-right: 15px; font-weight: bold; cursor: pointer;" tabindex="34">Select Product(s) </a>
                                    </td>
                                </tr>
                                <tr>
                                    <td width="12%" valign="top">Related Products:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtRelProducts" runat="server" CssClass="status-textfield" Width="98%"
                                            TextMode="MultiLine" TabIndex="33"></asp:TextBox>
                                    </td>
                                    <td width="14%" align="left" valign="bottom">
                                        <a id="aRelated" name="aRelated" onclick="openCenteredCrossSaleWindow('ContentPlaceHolder1_txtRelProducts');"
                                            style="margin-right: 15px; font-weight: bold; cursor: pointer;" tabindex="34">Select Product(s) </a>
                                    </td>
                                </tr>
                                <tr>
                                    <td width="12%" valign="top">Product double wides:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtProductDoublewide" runat="server" CssClass="status-textfield" Width="98%"
                                            TextMode="MultiLine" TabIndex="33"></asp:TextBox>
                                    </td>
                                    <td width="14%" align="left" valign="bottom">
                                        <a id="aDobWide" name="aRelated" onclick="openCenteredCrossSaleWindowDouble('ContentPlaceHolder1_txtProductDoublewide');"
                                            style="margin-right: 15px; font-weight: bold; cursor: pointer;" tabindex="34">Select Product(s) </a>
                                    </td>
                                </tr>
                                <tr>
                                    <td width="12%" valign="top">Product Pairs:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtProductPair" runat="server" CssClass="status-textfield" Width="98%"
                                            TextMode="MultiLine" TabIndex="33"></asp:TextBox>
                                    </td>
                                    <td width="14%" align="left" valign="bottom">
                                        <a id="aProPair" name="aRelated" onclick="openCenteredCrossSaleWindowDouble('ContentPlaceHolder1_txtProductPair');"
                                            style="margin-right: 15px; font-weight: bold; cursor: pointer;" tabindex="34">Select Product(s) </a>
                                    </td>
                                </tr>
                                <tr>
                                    <td width="12%" valign="top">Product Stylist Notes:
                                    </td>
                                    <td class="ckeditor-table">
                                        <asp:TextBox TextMode="multiLine" class="description-textarea" ID="txtStylistList"
                                            TabIndex="27" Rows="10" Columns="80" runat="server" Style="border: solid 1px #e7e7e7; background: none repeat scroll 0 0 #E7E7E7;"></asp:TextBox>
                                        <script type="text/javascript">
                                            CKEDITOR.replace('<%= txtStylistList.ClientID %>', { baseHref: '<%= ResolveUrl("~/ckeditor/") %>', height: 200 });
                                            CKEDITOR.on('dialogDefinition', function (ev) {
                                                if (ev.data.name == 'image') {
                                                    var btn = ev.data.definition.getContents('info').get('browse');
                                                    btn.hidden = false;
                                                    btn.onClick = function () { window.open(CKEDITOR.basePath + 'ImageBrowser.aspx', 'popuppage', 'scrollbars=no,width=780,height=630,left=' + ((screen.width - 780) / 2) + ',top=' + ((screen.height - 630) / 2) + ',resizable=no,toolbar=no,titlebar=no'); };
                                                }
                                                if (ev.data.name == 'link') {
                                                    var btn = ev.data.definition.getContents('info').get('browse');
                                                    btn.hidden = false;
                                                    btn.onClick = function () { window.open(CKEDITOR.basePath + 'LinkBrowser.aspx', 'popuppage', 'scrollbars=no,width=520,height=580,left=' + ((screen.width - 520) / 2) + ',top=' + ((screen.height - 580) / 2) + ',resizable=no,toolbar=no,titlebar=no'); };
                                                }
                                            });
                                        </script>
                                    </td>
                                    <td width="14%" align="left" valign="bottom"></td>
                                </tr>
                                <tr>
                                    <td width="12%" valign="top">Product Why we love this:
                                    </td>
                                    <td class="ckeditor-table">
                                        <asp:TextBox TextMode="multiLine" class="description-textarea" ID="txtWhyWeLove"
                                            TabIndex="27" Rows="10" Columns="80" runat="server" Style="border: solid 1px #e7e7e7; background: none repeat scroll 0 0 #E7E7E7;"></asp:TextBox>
                                        <script type="text/javascript">
                                            CKEDITOR.replace('<%= txtWhyWeLove.ClientID %>', { baseHref: '<%= ResolveUrl("~/ckeditor/") %>', height: 200 });
                                            CKEDITOR.on('dialogDefinition', function (ev) {
                                                if (ev.data.name == 'image') {
                                                    var btn = ev.data.definition.getContents('info').get('browse');
                                                    btn.hidden = false;
                                                    btn.onClick = function () { window.open(CKEDITOR.basePath + 'ImageBrowser.aspx', 'popuppage', 'scrollbars=no,width=780,height=630,left=' + ((screen.width - 780) / 2) + ',top=' + ((screen.height - 630) / 2) + ',resizable=no,toolbar=no,titlebar=no'); };
                                                }
                                                if (ev.data.name == 'link') {
                                                    var btn = ev.data.definition.getContents('info').get('browse');
                                                    btn.hidden = false;
                                                    btn.onClick = function () { window.open(CKEDITOR.basePath + 'LinkBrowser.aspx', 'popuppage', 'scrollbars=no,width=520,height=580,left=' + ((screen.width - 520) / 2) + ',top=' + ((screen.height - 580) / 2) + ',resizable=no,toolbar=no,titlebar=no'); };
                                                }
                                            });
                                        </script>
                                    </td>
                                    <td width="14%" align="left" valign="bottom"></td>
                                </tr>
                                <tr style="display: none">
                                    <td width="12%" valign="top">Marry Products:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtmarry" runat="server" CssClass="status-textfield" Width="98%"
                                            TextMode="MultiLine" TabIndex="33"></asp:TextBox>
                                    </td>
                                    <td width="14%" align="left" valign="bottom">
                                        <a id="a1" name="aRelated" onclick="openCenteredCrossSaleWindow('ContentPlaceHolder1_txtmarry');"
                                            style="margin-right: 15px; font-weight: bold; cursor: pointer;" tabindex="34">Select Product(s) </a>
                                    </td>
                                </tr>

                                <tr>
                                    <td colspan="3">&nbsp;
                                    </td>
                                </tr>
                                <tr style="visibility: hidden; display: none">
                                    <td colspan="3">
                                        <table cellpadding="0" cellspacing="0" width="100%">
                                            <tr>
                                                <td style="width: 15%">Satisfaction Guaranteed :
                                                </td>
                                                <td>
                                                    <asp:CheckBox ID="chkSatisfactionGuaranteed" runat="Server" TabIndex="36" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
    <tr>
        <td></td>
    </tr>

    </table>
                                                                </div>

                                                                <div class="tab-content invoice-tab" id="divtab6" style="margin-top: 0px; display: none; font-size: 12px;">
                                                                    <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                                                        <tr>
                                                                            <td width="10%" valign="top">Tag:
                                                                            </td>
                                                                            <td>
                                                                                <asp:DropDownList ID="ddlTagName" class="product-type" Width="100px" runat="server"
                                                                                    TabIndex="35">
                                                                                    <asp:ListItem Text="Select One" Value=""></asp:ListItem>
                                                                                    <asp:ListItem Text="NewArrival" Value="NewArrival"></asp:ListItem>
                                                                                    <asp:ListItem Text="HotProduct" Value="HotProduct"></asp:ListItem>
                                                                                    <asp:ListItem Text="BestSeller" Value="BestSeller"></asp:ListItem>
                                                                                </asp:DropDownList>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td style="width: 10%;">Is Free Shipping: &nbsp;
                                                                      <asp:CheckBox ID="chkIsFreeShipping" runat="server" TabIndex="21" onchange="SelectDivFreeShippingValue();" Onclick="SelectDivFreeShippingValue();" />
                                                                            </td>
                                                                            <td>
                                                                                <table cellpadding="0" width="100%" cellspacing="0" id="DiveFreeShipping" runat="server" style="display: none;">
                                                                                    <tr>
                                                                                        <td>From Date:&nbsp;&nbsp;<asp:TextBox ID="txtFreeShippingFromDate" runat="server" CssClass="from-textfield" Width="70px"
                                                                                            Style="margin-right: 3px;"></asp:TextBox>&nbsp;&nbsp;&nbsp;
                                                                        To Date:&nbsp;&nbsp;&nbsp;<asp:TextBox ID="txtFreeShippingToDate" runat="server" CssClass="from-textfield" Width="70px"
                                                                            Style="margin-right: 3px;"></asp:TextBox>
                                                                                        </td>
                                                                                    </tr>
                                                                                </table>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td style="width: 10%;">Is Featured: &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                                      <asp:CheckBox ID="chkFeatured" runat="server" TabIndex="22" onchange="SelectDivFeaturedValue();" Onclick="SelectDivFeaturedValue();" />
                                                                            </td>
                                                                            <td>
                                                                                <table cellpadding="0" width="100%" cellspacing="0" id="DivFeatured" runat="server" style="display: none;">
                                                                                    <tr>
                                                                                        <td>From Date:&nbsp;&nbsp;<asp:TextBox ID="txtIsFeaturedFromDate" runat="server" CssClass="from-textfield" Width="70px"
                                                                                            Style="margin-right: 3px;"></asp:TextBox>&nbsp;&nbsp;&nbsp;
                                                                        To Date:&nbsp;&nbsp;&nbsp;<asp:TextBox ID="txtIsFeaturedToDate" runat="server" CssClass="from-textfield" Width="70px"
                                                                            Style="margin-right: 3px;"></asp:TextBox>
                                                                                        </td>
                                                                                    </tr>
                                                                                </table>
                                                                            </td>

                                                                        </tr>
                                                                        <tr>
                                                                            <td style="width: 10%;">Is New Arrival: &nbsp;&nbsp;&nbsp;&nbsp;
                                                                       <asp:CheckBox ID="chkNewArrival" runat="server" TabIndex="23" onchange="SelectDivNewArrivalValue();" Onclick="SelectDivNewArrivalValue();" />
                                                                            </td>
                                                                            <td>
                                                                                <table cellpadding="0" width="100%" cellspacing="0" id="DivNewArrival" runat="server" style="display: none;">
                                                                                    <tr>
                                                                                        <td>From Date:&nbsp;&nbsp;<asp:TextBox ID="txtNewArrivalFromDate" runat="server" CssClass="from-textfield" Width="70px"
                                                                                            Style="margin-right: 3px;"></asp:TextBox>&nbsp;&nbsp;&nbsp;
                                                                        To Date:&nbsp;&nbsp;&nbsp;<asp:TextBox ID="txtNewArrivalToDate" runat="server" CssClass="from-textfield" Width="70px"
                                                                            Style="margin-right: 3px;"></asp:TextBox>
                                                                                        </td>
                                                                                    </tr>
                                                                                </table>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td style="width: 10%;">Is Best Seller: &nbsp;&nbsp;&nbsp;&nbsp;
                                                                        <asp:CheckBox ID="chkIsBestSeller" runat="server" TabIndex="24" onchange="SelectDivBestSellerValue();" Onclick="SelectDivBestSellerValue();" />
                                                                            </td>
                                                                            <td>
                                                                                <table cellpadding="0" width="100%" cellspacing="0" id="DivBestSeller" runat="server" style="display: none;">
                                                                                    <tr>
                                                                                        <td>From Date:&nbsp;&nbsp;<asp:TextBox ID="txtBestSellerFromDate" runat="server" CssClass="from-textfield" Width="70px"
                                                                                            Style="margin-right: 3px;"></asp:TextBox>&nbsp;&nbsp;&nbsp;
                                                                        To Date:&nbsp;&nbsp;&nbsp;<asp:TextBox ID="txtBestSellerToDate" runat="server" CssClass="from-textfield" Width="70px"
                                                                            Style="margin-right: 3px;"></asp:TextBox>
                                                                                        </td>
                                                                                    </tr>
                                                                                </table>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td>Sale-Clearance: &nbsp;&nbsp;&nbsp;&nbsp;
                                                                        <asp:CheckBox ID="chkSaleclearance" runat="server" onchange="SaleClerance();" />
                                                                                &nbsp;Price $<asp:TextBox ID="txtSaleClearance" Style="display: none" runat="server" CssClass="order-textfield"
                                                                                    Width="80px" onkeypress="return keyRestrict(event,'0123456789.');" TabIndex="14"></asp:TextBox>
                                                                            </td>
                                                                            <td></td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td>Is Discontinue:&nbsp;&nbsp;&nbsp;&nbsp;
                                                                       <asp:CheckBox ID="chkIsDiscontinue" runat="server" Checked="false" TabIndex="17" />
                                                                            </td>
                                                                            <td></td>
                                                                            <td></td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td>Is Price Quote:
                                                                     &nbsp;&nbsp;&nbsp;&nbsp;<asp:CheckBox ID="chkIspriceQuote" runat="server" TabIndex="26" />
                                                                            </td>
                                                                            <td></td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td>Is Kids Collection:
                                                                     &nbsp;&nbsp;&nbsp;&nbsp;<asp:CheckBox ID="chkkidscollection" runat="server" TabIndex="26" />
                                                                            </td>
                                                                            <td></td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td>Is Show Buy1Get1:
                                                                     &nbsp;&nbsp;&nbsp;&nbsp;<asp:CheckBox ID="chkIsShowBuy1Get1" runat="server" TabIndex="27" />
                                                                            </td>
                                                                            <td></td>
                                                                        </tr>

                                                                    </table>
                                                                </div>

    <div class="tab-content invoice-tab" id="divtab7" style="margin-top: 0px; font-size: 12px; display: none;">
        <p id="pchkForAll" runat="server">
            <asp:CheckBox ID="chkForAll" runat="server" Checked="false" AutoPostBack="false" />
            <label for="ContentPlaceHolder1_chkForAll" style="font-size: 16px;">Click Checkbox to apply search options settings to All Parent Products</label>
        </p>         
        <ul class="menu">
            <li class="active" id="lisubMenu1" onclick="SubTabdisplay(1);">SEARCH COLORS</li>
            <li id="lisubMenu2" class="" onclick="SubTabdisplay(2);">SEARCH FABRICS</li>
            <li id="lisubMenu3" class="" onclick="SubTabdisplay(3);">SEARCH PATTERNS</li>
            <li id="lisubMenu4" class="" onclick="SubTabdisplay(4);">SEARCH FEATURES</li>
            <li id="lisubMenu5" class="" onclick="SubTabdisplay(5);">SEARCH STYLE</li>
            <li id="lisubMenu6" class="" onclick="SubTabdisplay(6);">SEARCH ROOM</li>
            <li onclick="SubTabdisplay(7);" id="lisubMenu7">SEARCH HEADER</li>
          <%--   <li onclick="SubTabdisplay(8);" id="lisubMenu8">SEARCH MATERIAL</li>--%>
        </ul>
        <div class="tab-content invoice-tab" id="divSubTab1">
            <table cellpadding="0" cellspacing="0" width="60%">
                <tr>
                    <%-- <td>Search Color:
                    </td>--%>
                    <td colspan="10">
                        <div style="float: right;">&nbsp;&nbsp;&nbsp;<a target="_blank" id="ColorLink" runat="server" style="color: #B92127; text-decoration: underline;">Add Color</a></div>
                        <br />
                        <br />
                        <div style="overflow: auto; height: auto; border: 1px solid rgb(231, 231, 231);">
                             <asp:CheckBoxList ID="ddlColors" RepeatLayout="Table" RepeatColumns="5" Width="700px" RepeatDirection="Horizontal" runat="server" OnDataBound="ddlColors_DataBound">
                            </asp:CheckBoxList>
                        </div>
                    </td>
                </tr>
            </table>
        </div>
        <div class="tab-content invoice-tab" id="divSubTab2" style="display: none;">
            <table cellpadding="0" cellspacing="0" width="60%">
                <tr>
                    <%-- <td>Search Fabric:
                    </td>--%>
                    <td colspan="10">
                        <div style="float: right;">
                            <a target="_blank" id="FabricLink" runat="server" style="color: #B92127; text-decoration: underline;">Add Fabric</a>
                        </div>
                        <br />
                        <br />
                        <asp:DropDownList ID="ddlFabric" runat="server" class="product-type">
                        </asp:DropDownList>
                        <div style="overflow: auto; height: auto; border: 1px solid rgb(231, 231, 231);">
                            <asp:CheckBoxList ID="ddlFab" RepeatLayout="Table" RepeatColumns="5" Width="700px" RepeatDirection="Horizontal" runat="server" OnDataBound="ddlFab_DataBound">
                            </asp:CheckBoxList>
                        </div>
                    </td>
                </tr>
            </table>
        </div>
        <div class="tab-content invoice-tab" id="divSubTab3" style="display: none;">
            <table cellpadding="0" cellspacing="0" width="60%">
                <tr>
                    <%-- <td>Search Pattern:
                    </td>--%>
                    <td colspan="10">
                        <div style="float: right;">
                            <a target="_blank" id="PatternLink" runat="server" style="color: #B92127; text-decoration: underline;">Add Pattern</a>
                        </div>
                        <br />
                        <br />
                        <asp:DropDownList ID="ddlPattern" runat="server" class="product-type">
                        </asp:DropDownList>
                        <div style="overflow: auto; height: auto; border: 1px solid rgb(231, 231, 231);">
                            <asp:CheckBoxList ID="ddlPatt" RepeatLayout="Table" RepeatColumns="5" Width="700px" RepeatDirection="Horizontal" runat="server" OnDataBound="ddlPatt_DataBound">
                            </asp:CheckBoxList>
                        </div>
                    </td>
                </tr>
            </table>
        </div>
        <div class="tab-content invoice-tab" id="divSubTab4" style="display: none;">
            <table cellpadding="0" cellspacing="0" width="60%">
                <tr>
                    <%-- <td>Search Feature:
                    </td>--%>
                    <td colspan="10">
                        <div style="float: right;">
                            <a target="_blank" id="StyleLink" runat="server" style="color: #B92127; text-decoration: underline;">Add Feature</a>
                        </div>
                        <br />
                        <br />
                        <div style="overflow: auto; height: auto; border: 1px solid rgb(231, 231, 231);">
                             <asp:CheckBoxList ID="ddlStyle" RepeatLayout="Table" RepeatColumns="5" Width="700px" RepeatDirection="Horizontal" runat="server" OnDataBound="ddlStyle_DataBound">
                            </asp:CheckBoxList>
                        </div>
                    </td>
                </tr>
            </table>
        </div>
        <div class="tab-content invoice-tab" id="divSubTab5" style="display: none;">
            <table cellpadding="0" cellspacing="0" width="60%">
                <tr>
                    <%-- <td>Search Style:
                    </td>--%>
                    <td colspan="10">
                        <div style="float: right;">
                            <a target="_blank" id="newStyleLink" runat="server" style="color: #B92127; text-decoration: underline;">Add Style</a>
                        </div>
                        <br />
                        <br />
                       <div style="overflow: auto; height: auto; border: 1px solid rgb(231, 231, 231);">
                            <asp:CheckBoxList ID="ddlNewStyle" RepeatLayout="Table" RepeatColumns="5" Width="700px" RepeatDirection="Horizontal" runat="server" OnDataBound="ddlNewStyle_DataBound">
                            </asp:CheckBoxList>
                        </div>
                    </td>
                </tr>
            </table>
        </div>
        <div class="tab-content invoice-tab" id="divSubTab6" style="display: none;">
            <table cellpadding="0" cellspacing="0" width="60%">
                <tr>
                    <%-- <td style="vertical-align: top;">Search Room:
                    </td>--%>
                    <td colspan="10">
                        <div style="float: right;">
                            <a target="_blank" id="roomLink" runat="server" style="color: #B92127; text-decoration: underline;">Add Room</a>
                        </div>
                        <br />
                        <div style="overflow: auto; height: auto; border: 1px solid rgb(231, 231, 231);">
                            <asp:CheckBoxList ID="ddlRoom" RepeatLayout="Table" RepeatColumns="5" Width="700px" RepeatDirection="Horizontal" runat="server" OnDataBound="ddlRoom_DataBound">
                            </asp:CheckBoxList>
                        </div>
                    </td>
                </tr>
            </table>
        </div>
        <div class="tab-content invoice-tab" id="divSubTab7" style="display: none;">
            <table cellpadding="0" cellspacing="0" width="60%">
                <tr>
                    <%-- <td>Header:
                    </td>--%>
                    <td colspan="10">
                        <div style="float: right;">
                            <a target="_blank" id="HeaderLink" runat="server" style="color: #B92127; text-decoration: underline;">Add Header</a>
                        </div>
                        <br />
                        <br />
                         <div style="overflow: auto; height: 450px; border: 1px solid #e7e7e7;">
                            <asp:CheckBoxList ID="chkHeader" RepeatLayout="Table" RepeatColumns="5" Width="700px" RepeatDirection="Horizontal" runat="server" OnDataBound="chkHeader_DataBound">
                            </asp:CheckBoxList>
                        </div>
                    </td>
                </tr>
            </table>
        </div>
        <%--<div class="tab-content invoice-tab" id="divSubTab8" style="display: none;">
            <table cellpadding="0" cellspacing="0" width="60%">
                <tr>
                     
                    <td colspan="10">
                        <div style="float: right;">
                            <a target="_blank" id="MaterialLink" runat="server" style="color: #B92127; text-decoration: underline;">Add Material</a>
                        </div>
                        <br />
                        <br />
                         <div style="overflow: auto; height: 450px; border: 1px solid #e7e7e7;">
                            <asp:CheckBoxList ID="chkmaterial" RepeatLayout="Table" RepeatColumns="5" Width="700px" RepeatDirection="Horizontal" runat="server" OnDataBound="chkmaterial_DataBound">
                            </asp:CheckBoxList>
                        </div>
                    </td>
                </tr>
            </table>
        </div>--%>
        <table cellpadding="0" cellspacing="0" width="60%">
            <tr id="trSecondaryColor" runat="server" visible="false">
                <td>Secondary Color(s):
                </td>
                <td colspan="10">
                    <div id="div6" class="slidingDivDesc">
                        <table width="100%" border="0" cellpadding="0" cellspacing="0">
                            <tr>
                                <td valign="top" align="left" colspan="2">
                                    <asp:GridView ID="grdSecondaryColor" runat="server" AutoGenerateColumns="False" BorderColor="#e7e7e7"
                                        BorderStyle="Solid" BorderWidth="1px" DataKeyNames="SearchID" EmptyDataText="No Record(s) Found."
                                        AllowSorting="false" EmptyDataRowStyle-ForeColor="Red" EmptyDataRowStyle-HorizontalAlign="Center"
                                        ViewStateMode="Enabled" Width="75%" OnRowDataBound="grdSecondaryColor_RowDataBound"
                                        GridLines="None" AllowPaging="True" PageSize="50" PagerSettings-Mode="NumericFirstLast">
                                        <Columns>
                                            <asp:TemplateField>
                                                <HeaderTemplate>
                                                    &nbsp;Select
                                                </HeaderTemplate>
                                                <ItemStyle HorizontalAlign="Center" Width="15%" />
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemTemplate>
                                                    &nbsp;<asp:Label ID="lblSearchID" runat="server" Visible="false" Text='<%# Bind("SearchID") %>'></asp:Label>
                                                    &nbsp;<asp:Label ID="lblActive" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container.DataItem,"Active") %>'></asp:Label>
                                                    <asp:CheckBox ID="chkSecondaryActive" runat="server" Checked='<%# Convert.ToBoolean(DataBinder.Eval(Container.DataItem,"Active")) %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <HeaderTemplate>
                                                    &nbsp;Style
                                                </HeaderTemplate>
                                                <ItemStyle HorizontalAlign="Left" />
                                                <HeaderStyle HorizontalAlign="Left" />
                                                <ItemTemplate>
                                                    &nbsp;<asp:Label ID="lblSearchvalue" runat="server" Text='<%# Bind("Searchvalue") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <HeaderTemplate>
                                                    &nbsp;Color Sku
                                                </HeaderTemplate>
                                                <ItemStyle HorizontalAlign="Left" />
                                                <HeaderStyle HorizontalAlign="Left" Width="45%" />
                                                <ItemTemplate>
                                                    &nbsp;<asp:Label ID="lblColorSku" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container.DataItem,"ColorSku") %>'></asp:Label>
                                                    <asp:TextBox ID="txtColorSku" runat="server" Width="120px" CssClass="order-textfield"
                                                        Text='<%# DataBinder.Eval(Container.DataItem,"ColorSku") %>' MaxLength="250"></asp:TextBox>
                                                    &nbsp;<asp:Literal ID="ltrSelectSku" runat="server"></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <PagerSettings Mode="NumericFirstLast" Position="TopAndBottom"></PagerSettings>
                                        <PagerStyle HorizontalAlign="Right" CssClass="grid_paging" />
                                        <RowStyle HorizontalAlign="Center" CssClass="oddrow" />
                                        <AlternatingRowStyle CssClass="altrow" />
                                        <HeaderStyle ForeColor="White" Font-Bold="false" />
                                    </asp:GridView>
                                </td>

                            </tr>
                            <tr>
                                <td width="14%" valign="top" align="right">
                                    <a id="SecondaryColorLink" runat="server" style="color: #B92127; text-decoration: underline;">Add Color</a>
                                </td>
                                <td valign="top" align="center">
                                    <asp:ImageButton ID="btnSaveSecondaryColor" runat="server" OnClientClick="return ChkSecondaryColorValidate();" OnClick="btnSaveSecondaryColor_Click" />
                                </td>
                            </tr>
                        </table>
                    </div>
                </td>
            </tr>
        </table>
    </div>

    <div class="tab-content invoice-tab" id="divtab8" style="margin-top: 0px; font-size: 12px; display: none;">
        <table width="100%" border="0" cellpadding="0" cellspacing="0" class="content-table border-td">
            <tr>
                <th colspan="6">
                    <div class="main-title-left">
                        <img class="img-left" title=" Custom Calculator" alt=" Custom Calculator" src="/App_Themes/<%=Page.Theme %>/icon/add-product.png" />
                        <h2>Custom Calculator</h2>
                    </div>
                    <div class="main-title-right">
                        <a href="javascript:void(0);" class="show_hideDesc" onclick="return ShowHideButton('ImgCustomCalc','tdCustomCalc');">
                            <img id="ImgCustomCalc" class="minimize" title="Minimize" alt="Minimize" src="/App_Themes/<%=Page.Theme %>/images/minimize.png" /></a>   <a id="CustomCalculatorLink" runat="server" style="color: #B92127; text-decoration: underline; float: right; margin-right: 12px;">Add Custom Style</a>
                    </div>
                </th>
            </tr>
            <tr>
                <td valign="top" id="tdCustomCalc" colspan="6">
                    <div id="div4" class="slidingDivDesc">
                        <table width="100%" border="0" cellpadding="0" cellspacing="0">
                            <tr>
                                <td valign="top" align="left" colspan="2">
                                    <asp:GridView ID="grdProductStyleType" runat="server" AutoGenerateColumns="False"
                                        BorderColor="#e7e7e7" BorderStyle="Solid" BorderWidth="1px" DataKeyNames="ProductStyleId"
                                        EmptyDataText="No Record(s) Found." AllowSorting="false" EmptyDataRowStyle-ForeColor="Red"
                                        EmptyDataRowStyle-HorizontalAlign="Center" ViewStateMode="Enabled" OnRowCommand="grdProductStyleType_RowCommand"
                                        Width="100%" GridLines="None" AllowPaging="True" PageSize="50" PagerSettings-Mode="NumericFirstLast"
                                        OnRowDataBound="grdProductStyleType_RowDataBound" OnRowEditing="grdProductStyleType_RowEditing"
                                        OnRowCancelingEdit="grdProductStyleType_RowCancelingEdit">
                                        <Columns>
                                            <asp:TemplateField>
                                                <HeaderTemplate>
                                                    &nbsp;Active
                                                </HeaderTemplate>
                                                <ItemStyle HorizontalAlign="Center" Width="10%" />
                                                <HeaderStyle HorizontalAlign="Center" Width="10%" />
                                                <ItemTemplate>
                                                    &nbsp;<asp:Label ID="lblProductStyleId" runat="server" Visible="false" Text='<%# Bind("ProductStyleId") %>'></asp:Label>
                                                    &nbsp;<asp:Label ID="lblActive" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container.DataItem,"Active") %>'></asp:Label>
                                                    &nbsp;<asp:CheckBox ID="chkActive" runat="server" Checked='<%# Convert.ToBoolean(DataBinder.Eval(Container.DataItem,"Active")) %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <HeaderTemplate>
                                                    &nbsp;Style
                                                </HeaderTemplate>
                                                <ItemStyle HorizontalAlign="Left" />
                                                <HeaderStyle HorizontalAlign="Left" />
                                                <ItemTemplate>
                                                    &nbsp;<asp:Label ID="lblStylename" runat="server" Text='<%# Bind("StyleName") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <HeaderTemplate>
                                                    &nbsp;(Inch) Yard header & hem
                                                </HeaderTemplate>
                                                <ItemStyle HorizontalAlign="Right" Width="20%" />
                                                <HeaderStyle HorizontalAlign="Right" Width="20%" />
                                                <ItemTemplate>
                                                    &nbsp;<asp:Label ID="lblYardHeaderandhem" runat="server" Text='<%# Math.Round(Convert.ToDecimal(DataBinder.Eval(Container.DataItem,"YardHeaderandhem")),2) %>'></asp:Label>
                                                    <asp:TextBox ID="txtYardHeaderandhem" runat="server" Width="70px" Visible="false" CssClass="order-textfield"
                                                        onkeypress="return keyRestrictforIntOnly(event,'0123456789');" Text='<%# Math.Round(Convert.ToDecimal(DataBinder.Eval(Container.DataItem,"YardHeaderandhem")),2) %>'
                                                        MaxLength="6"></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField>
                                                <HeaderTemplate>
                                                    &nbsp;(Inch) Fabric
                                                </HeaderTemplate>
                                                <ItemStyle HorizontalAlign="Right" Width="10%" />
                                                <HeaderStyle HorizontalAlign="Right" Width="10%" />
                                                <ItemTemplate>
                                                    &nbsp;<asp:Label ID="lblFabricInch" runat="server" Text='<%# Math.Round(Convert.ToDecimal(DataBinder.Eval(Container.DataItem,"FabricInch")),2) %>'></asp:Label>
                                                    <asp:TextBox ID="txtFabricInch" runat="server" Visible="false" Width="70px" MaxLength="6" CssClass="order-textfield"
                                                        onkeypress="return keyRestrictforIntOnly(event,'0123456789');" Text='<%# Math.Round(Convert.ToDecimal(DataBinder.Eval(Container.DataItem,"FabricInch")),2) %>'></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <HeaderTemplate>
                                                    &nbsp;Fabric width
                                                </HeaderTemplate>
                                                <ItemStyle HorizontalAlign="Right" Width="10%" />
                                                <HeaderStyle HorizontalAlign="Right" Width="10%" />
                                                <ItemTemplate>
                                                    <asp:Label ID="lblFabricwidth" runat="server" Text='<%# Math.Round(Convert.ToDecimal(DataBinder.Eval(Container.DataItem,"FabricWidth")),2) %>'></asp:Label>
                                                    <asp:TextBox ID="txtFabricwidth" runat="server" Width="70px" Visible="false" MaxLength="6" CssClass="order-textfield"
                                                        onkeypress="return keyRestrictforIntOnly(event,'0123456789.');" onkeyUp="Texboxfabricwidthchange(this.id);" Text='<%# Math.Round(Convert.ToDecimal(DataBinder.Eval(Container.DataItem,"FabricWidth")),2) %>'></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <HeaderTemplate>
                                                    &nbsp;Divided width
                                                </HeaderTemplate>
                                                <ItemStyle HorizontalAlign="Right" Width="10%" />
                                                <HeaderStyle HorizontalAlign="Right" Width="10%" />
                                                <ItemTemplate>
                                                    <asp:Label ID="lbldividedwidth" runat="server" Text='<%# Math.Round(Convert.ToDecimal(DataBinder.Eval(Container.DataItem,"DividedWidth")),2) %>'></asp:Label>
                                                    <asp:TextBox ID="txtdividedwidth" runat="server" Width="70px" Visible="false" MaxLength="6" CssClass="order-textfield"
                                                        onkeypress="return keyRestrictforIntOnly(event,'0123456789');" Text='<%# Math.Round(Convert.ToDecimal(DataBinder.Eval(Container.DataItem,"DividedWidth")),2) %>'></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <HeaderTemplate>
                                                    &nbsp;($) Additional Price
                                                </HeaderTemplate>
                                                <ItemStyle HorizontalAlign="Right" Width="15%" />
                                                <HeaderStyle HorizontalAlign="Right" Width="15%" />
                                                <ItemTemplate>
                                                    &nbsp;<asp:Label ID="lblPrice" runat="server" Text='<%# Math.Round(Convert.ToDecimal(DataBinder.Eval(Container.DataItem,"AdditionalPrice")),2) %>'></asp:Label>
                                                    <asp:TextBox ID="txtAdditionalPrice" runat="server" Width="70px" Visible="false" CssClass="order-textfield"
                                                        onkeypress="return keyRestrict(event,'0123456789.');" Text='<%# Math.Round(Convert.ToDecimal(DataBinder.Eval(Container.DataItem,"AdditionalPrice")),2) %>'
                                                        MaxLength="10"></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <HeaderTemplate>
                                                    &nbsp;($) Per Inch
                                                </HeaderTemplate>
                                                <ItemStyle HorizontalAlign="Right" Width="10%" />
                                                <HeaderStyle HorizontalAlign="Right" Width="10%" />
                                                <ItemTemplate>
                                                    &nbsp;<asp:Label ID="lblPerInch" runat="server" Text='<%# Math.Round(Convert.ToDecimal(DataBinder.Eval(Container.DataItem,"PerInch")),2) %>'></asp:Label>
                                                    <asp:TextBox ID="txtPerInch" runat="server" Visible="false" Width="70px" onkeypress="return keyRestrict(event,'0123456789.');"
                                                        CssClass="order-textfield" Text='<%# Math.Round(Convert.ToDecimal(DataBinder.Eval(Container.DataItem,"PerInch")),2) %>'
                                                        MaxLength="10"></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Operations">
                                                <ItemStyle HorizontalAlign="Center" Width="10%" />
                                                <HeaderStyle HorizontalAlign="Center" Width="10%" />
                                                <ItemTemplate>
                                                    <asp:ImageButton runat="server" ID="_editLinkButton" ToolTip="Edit"
                                                        CommandName="edit" CommandArgument='<%# Eval("ProductStyleId") %>'></asp:ImageButton>
                                                    <asp:ImageButton ID="btnSave" Visible="false" OnClientClick="return checkvalidationsearchtype(this.id);"
                                                        runat="server" CommandArgument='<%# DataBinder.Eval(Container.DataItem,"ProductStyleId") %>'
                                                        CommandName="Save" AlternateText="Save" />
                                                    <asp:ImageButton ID="btnCancel" runat="server" Visible="false" CommandName="Cancel"
                                                        Height="16px" Width="16px" AlternateText="Cancel" CommandArgument='<%# DataBinder.Eval(Container.DataItem,"ProductStyleId") %>' />
                                                    <asp:ImageButton runat="server" ID="_deleteLinkButton" ToolTip="Delete" CommandName="DeleteAdmin"
                                                        CommandArgument='<%# Eval("ProductStyleId") %>' message="Are you sure want to delete current Record?"
                                                        OnClientClick='return confirm(this.getAttribute("message"))'></asp:ImageButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <PagerSettings Mode="NumericFirstLast" Position="TopAndBottom"></PagerSettings>
                                        <PagerStyle HorizontalAlign="Right" CssClass="grid_paging" />
                                        <RowStyle HorizontalAlign="Center" CssClass="oddrow" />
                                        <AlternatingRowStyle CssClass="altrow" />
                                        <HeaderStyle ForeColor="White" Font-Bold="false" />
                                    </asp:GridView>
                                </td>
                            </tr>
                            <tr>
                                <td width="14%" valign="top" align="right">&nbsp;
                                </td>
                                <td valign="top" align="right">
                                    <asp:ImageButton ID="btnSaveStylePrice" runat="server" OnClientClick="return chkselectcheckbox();"
                                        OnClick="btnSaveStylePrice_Click" />
                                </td>
                            </tr>
                            <tr style="display: none">
                                <td width="14%" valign="top" align="right">$ Per Inch :
                                </td>
                                <td valign="top">$
                                                                                                <asp:TextBox ID="txtPricePerInch" MaxLength="10" runat="server" class="order-textfield"
                                                                                                    Width="80px" onkeypress="return keyRestrict(event,'0123456789.');"></asp:TextBox>
                                </td>
                            </tr>
                            <tr style="display: none">
                                <td valign="top" align="right">Additional Charges :
                                </td>
                                <td valign="top">$
                                                                                                <asp:TextBox ID="txtAdditionalCharge" MaxLength="10" runat="server" class="order-textfield"
                                                                                                    Width="80px" onkeypress="return keyRestrict(event,'0123456789.');"></asp:TextBox>
                                </td>
                            </tr>
                            <tr style="display: none">
                                <td width="14%" valign="top" align="right">Yard header & hem :
                                </td>
                                <td valign="top">&nbsp; &nbsp;<asp:TextBox ID="txtInchHeaderHeme" MaxLength="10" runat="server" class="order-textfield"
                                    Width="80px" onkeypress="return keyRestrictForInventory(event,'0123456789');"></asp:TextBox>&nbsp;Inch&nbsp;&nbsp;<b>+</b>&nbsp;&nbsp;
                                                                                                Fabric &nbsp;:&nbsp;
                                                                                                <asp:TextBox ID="txtFabric" runat="server" MaxLength="10" class="order-textfield"
                                                                                                    Width="80px" onkeypress="return keyRestrictForInventory(event,'0123456789');"></asp:TextBox>&nbsp;Inch
                                </td>
                            </tr>
                        </table>
                    </div>
                </td>
            </tr>
        </table>
        <br />
        <br />
        <table width="100%" border="0" cellpadding="0" cellspacing="0" class="content-table border-td">
            <tr>
                <th colspan="6">
                    <div class="main-title-left">
                        <img class="img-left" title=" Custom Calculator" alt=" Custom Calculator" src="/App_Themes/<%=Page.Theme %>/icon/add-product.png" />
                        <h2>Options</h2>
                    </div>
                    <div class="main-title-right">
                        <a href="javascript:void(0);" class="show_optionhideDesc" onclick="return ShowHideButton('ImgoptionsCalc','tdCustomCalc');">
                            <img id="ImgoptionsCalc" class="minimize" title="Minimize" alt="Minimize" src="/App_Themes/<%=Page.Theme %>/images/minimize.png" /></a> <a id="OptionCalculatorLink" runat="server" style="color: #B92127; text-decoration: underline; float: right; margin-right: 12px;">Add Options</a>
                    </div>
                </th>
            </tr>
            <tr>
                <td valign="top" id="tdoptionsCalc" colspan="6">
                    <div id="div7" class="slidingoptionDivDesc">
                        <table width="100%" border="0" cellpadding="0" cellspacing="0">
                            <tr>
                                <td align="center">
                                    <asp:Label ID="lblmessageoption" runat="server" ForeColor="Red"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td valign="top" align="left" colspan="2">
                                    <asp:GridView ID="grdoptions" runat="server" AutoGenerateColumns="False"
                                        BorderColor="#e7e7e7" BorderStyle="Solid" BorderWidth="1px"
                                        EmptyDataText="No Record(s) Found." AllowSorting="false" EmptyDataRowStyle-ForeColor="Red"
                                        EmptyDataRowStyle-HorizontalAlign="Center" ViewStateMode="Enabled" OnRowDataBound="grdoptions_RowDataBound"
                                        Width="100%" GridLines="None" AllowPaging="True" PageSize="50" PagerSettings-Mode="NumericFirstLast">
                                        <Columns>
                                            <asp:TemplateField>
                                                <HeaderTemplate>
                                                    &nbsp;Active
                                                </HeaderTemplate>
                                                <ItemStyle HorizontalAlign="Center" Width="10%" />
                                                <HeaderStyle HorizontalAlign="Center" Width="10%" />
                                                <ItemTemplate>
                                                    &nbsp;<asp:Label ID="lblOptionActive" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container.DataItem,"Active") %>'></asp:Label>
                                                    &nbsp;<asp:CheckBox ID="chkOptionActive" runat="server" Checked='<%# Convert.ToBoolean(DataBinder.Eval(Container.DataItem,"Active")) %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <HeaderTemplate>
                                                    &nbsp;Options
                                                </HeaderTemplate>
                                                <ItemStyle HorizontalAlign="Left" />
                                                <HeaderStyle HorizontalAlign="Left" />
                                                <ItemTemplate>
                                                    &nbsp;<asp:Label ID="lblStylename" runat="server" Text='<%# Bind("StyleName") %>'></asp:Label>
                                                    <asp:Label ID="lblProductStyleId" runat="server" Visible="false" Text='<%# Bind("ProductOptionsId") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <HeaderTemplate>
                                                    &nbsp;(%)Percentage
                                                </HeaderTemplate>
                                                <ItemStyle HorizontalAlign="Right" Width="15%" />
                                                <HeaderStyle HorizontalAlign="Right" Width="15%" />
                                                <ItemTemplate>
                                                    &nbsp;<asp:Label ID="lblPricePercentage" runat="server" Visible="false" Text='<%# Math.Round(Convert.ToDecimal(DataBinder.Eval(Container.DataItem,"AdditionalPricePercentage")),2) %>'></asp:Label>
                                                    <asp:TextBox ID="txtPricePercentage" runat="server" Width="70px" CssClass="order-textfield"
                                                        onkeypress="return keyRestrict(event,'0123456789.');" Text='<%# Math.Round(Convert.ToDecimal(DataBinder.Eval(Container.DataItem,"AdditionalPricePercentage")),2) %>'
                                                        MaxLength="10"></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <HeaderTemplate>
                                                    &nbsp;($)Price
                                                </HeaderTemplate>
                                                <ItemStyle HorizontalAlign="Right" Width="15%" />
                                                <HeaderStyle HorizontalAlign="Right" Width="15%" />
                                                <ItemTemplate>
                                                    &nbsp;<asp:Label ID="lblPrice" runat="server" Visible="false" Text='<%# Math.Round(Convert.ToDecimal(DataBinder.Eval(Container.DataItem,"AdditionalPrice")),2) %>'></asp:Label>
                                                    <asp:TextBox ID="txtAdditionalPrice" runat="server" Width="70px" CssClass="order-textfield"
                                                        onkeypress="return keyRestrict(event,'0123456789.');" Text='<%# Math.Round(Convert.ToDecimal(DataBinder.Eval(Container.DataItem,"AdditionalPrice")),2) %>'
                                                        MaxLength="10"></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <PagerSettings Mode="NumericFirstLast" Position="TopAndBottom"></PagerSettings>
                                        <PagerStyle HorizontalAlign="Right" CssClass="grid_paging" />
                                        <RowStyle HorizontalAlign="Center" CssClass="oddrow" />
                                        <AlternatingRowStyle CssClass="altrow" />
                                        <HeaderStyle ForeColor="White" Font-Bold="false" />
                                    </asp:GridView>
                                </td>
                            </tr>
                            <tr>
                                <td width="14%" valign="top" align="right">&nbsp;
                                </td>
                                <td valign="top" align="right">
                                    <asp:ImageButton ID="btnSaveOptionPrice" runat="server"
                                        OnClick="btnSaveOptionPrice_Click" />
                                </td>
                            </tr>

                        </table>
                    </div>
                </td>
            </tr>
        </table>

    </div>

    <div class="tab-content invoice-tab" id="divtab9" style="margin-top: 0px; font-size: 12px; display: none;">
        <table width="100%" cellspacing="0" cellpadding="0" border="0" class="border-td content-table">
            <tbody>
                <tr>
                    <th>
                        <div class="main-title-left">
                            <img class="img-left" title="SEO" alt="SEO" src="/App_Themes/<%=Page.Theme %>/icon/add-product.png">
                            <h2>SEO</h2>
                        </div>
                        <div class="main-title-right">
                            <a title="close" href="javascript:void(0);" class="show_hideSEO" onclick="return ShowHideButton('ImgSEOmini','tdSEO');">
                                <img id="ImgSEOmini" class="minimize" title="Minimize" alt="Minimize" src="/App_Themes/<%=Page.Theme %>/images/minimize.png"></a>
                        </div>
                    </th>
                </tr>
                <tr>
                    <td align="left">
                        <table cellpadding="0" cellspacing="0">
                            <tr>
                                <td align="left">Is Canonical
                                </td>
                                <td align="left">
                                    <asp:CheckBox ID="chkcanonical" runat="server" onchange="changecanonical();" onclick="changecanonical();" />
                                </td>
                            </tr>
                            <tr>
                                <td align="left">Canonical URL
                                </td>
                                <td align="left">
                                    <asp:TextBox ID="txtcanonical" runat="server" class="order-textfield" Width="450px" TabIndex="5"></asp:TextBox>
                                </td>
                            </tr>
                        </table>

                    </td>

                </tr>
                <tr>
                    <td id="tdSEO">
                        <div id="tab-container" class="slidingDivSEO">
                            <ul class="menu">
                                <li class="active" id="ordernotes1" onclick='$("#ordernotes1").addClass("active");$("#privatenotes1").removeClass("active");$("#giftnotes1").removeClass("active");$("#myaccount1").removeClass("active");$("div.order-notes").fadeIn();$("div.private-notes").css("display", "none");$("div.gift-notes").css("display", "none");$("div.my-account").css("display", "none");'>Page Title</li>
                                <li id="privatenotes1" onclick='$("#ordernotes1").removeClass("active");$("#privatenotes1").addClass("active"); $("#giftnotes1").removeClass("active");$("#myaccount1").removeClass("active");$("div.private-notes").fadeIn();$("div.order-notes").css("display", "none");$("div.gift-notes").css("display", "none");$("div.my-account").css("display", "none");'>Keywords</li>
                                <li id="giftnotes1" onclick='$("#giftnotes1").addClass("active");$("#privatenotes1").removeClass("active");$("#ordernotes1").removeClass("active");$("#myaccount1").removeClass("active");$("div.gift-notes").fadeIn();$("div.private-notes").css("display", "none");$("div.order-notes").css("display", "none");$("div.my-account").css("display", "none");'>Description</li>
                                <li id="myaccount1" onclick='$("#myaccount1").addClass("active");$("#privatenotes1").removeClass("active");$("#ordernotes1").removeClass("active");$("#giftnotes1").removeClass("active");$("div.my-account").fadeIn();$("div.private-notes").css("display", "none");$("div.order-notes").css("display", "none");$("div.gift-notes").css("display", "none");'>Image Tooltip</li>
                            </ul>
                            <span class="clear"></span>
                            <div class="tab-content-2 order-notes">
                                <div class="tab-content-3">
                                    <asp:TextBox ID="txtSETitle" runat="server" CssClass="order-textfield" Width="100%"
                                        Columns="6" Height="90px" BorderStyle="None" Rows="6" TextMode="MultiLine" TabIndex="29"></asp:TextBox>
                                </div>
                            </div>
                            <div class="tab-content-2 private-notes">
                                <div class="tab-content-3">
                                    <asp:TextBox ID="txtSEKeyword" runat="server" CssClass="order-textfield" Width="100%"
                                        Columns="6" Height="90px" BorderStyle="None" Rows="6" TextMode="MultiLine" TabIndex="30"></asp:TextBox>
                                </div>
                            </div>
                            <div class="tab-content-2 gift-notes">
                                <div class="tab-content-3">
                                    <asp:TextBox ID="txtSEDescription" runat="server" CssClass="order-textfield" Width="100%"
                                        Columns="6" Height="90px" BorderStyle="None" Rows="6" TextMode="MultiLine" TabIndex="31"></asp:TextBox>
                                </div>
                            </div>
                            <div class="tab-content-2 my-account">
                                <div class="tab-content-3">
                                    <asp:TextBox Height="19px" ID="txtToolTip" BorderStyle="None" runat="server" MaxLength="500"
                                        CssClass="textfild" Width="100%" TabIndex="32"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                    </td>
                </tr>
            </tbody>
        </table>
    </div>

    <div class="tab-content invoice-tab" id="divtab10" style="margin-top: 0px; font-size: 12px; display: none;">
        <table id="trProductVariant" runat="server" width="100%" border="0" cellpadding="0" cellspacing="0" class="content-table border-td">
            <tr>
                <th colspan="6">
                    <div class="main-title-left">
                        <img class="img-left" title="Additional Info" alt="Additional Info" src="/App_Themes/<%=Page.Theme %>/icon/add-product.png">
                        <h2>Product Option</h2>
                    </div>
                    <div class="main-title-right">
                        <a href="javascript:void(0);" class="show_hideDesc" onclick="return ShowHideButton('ImgPVariant','tdPVariant');">
                            <img id="ImgPVariant" class="minimize" title="Minimize" alt="Minimize" src="/App_Themes/<%=Page.Theme %>/images/minimize.png"></a>
                    </div>
                </th>
            </tr>
            <tr>
                <td valign="top" id="tdPVariant" colspan="6">
                    <div id="div5" class="slidingDivDesc">
                        <iframe id="ifrmProductVariant" scrolling="auto" runat="server" frameborder="0" marginheight="0" marginwidth="0"
                            width="100%"></iframe>
                    </div>
                </td>
            </tr>
        </table>
    </div>
    <div class="tab-content invoice-tab" id="divtab11" style="margin-top: 0px; font-size: 12px; display: none;">
        <table width="100%" cellspacing="0" cellpadding="0" border="0" class="border-td content-table">
            <tbody>
                <tr>
                    <th>
                        <div class="main-title-left">
                            <img class="img-left" title="CELLULAR SHADE" alt="CELLULAR SHADE" src="/App_Themes/<%=Page.Theme %>/icon/add-product.png">
                            <h2>CELLULAR SHADE</h2>
                        </div>
                        <div class="main-title-right">
                            <a title="close" href="javascript:void(0);" class="show_hideSEO" onclick="return ShowHideButton('ImgCELLULARmini','tdCELLULAR');">
                                <img id="ImgCELLULARmini" class="minimize" title="Minimize" alt="Minimize" src="/App_Themes/<%=Page.Theme %>/images/minimize.png"></a>
                        </div>

                    </th>
                </tr>
                <tr>
                    <td valign="top" id="tdCELLULAR">
                        <div id="div11" class="slidingoptionDivDesc">
                            <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                <tr>
                                    <td></td>
                                    <td style="vertical-align: top;">
                                        <a href="/Resources/halfpricedraps/Shade/Sampleshadedata.csv" style="float: right">Download Sample File</a>
                                    </td>
                                </tr>
                                <tr>

                                    <td colspan="2" align="left">

                                        <table border="0">

                                            <tr>
                                                <td align="left">Suggested Retail: &nbsp;
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtSuggestedRetail" MaxLength="10" runat="server" class="order-textfield" ReadOnly="true"
                                                        Width="80px" onkeypress="return keyRestrict(event,'0123456789');"></asp:TextBox>
                                                    <a href="javascript:void(0);" onclick="openCenteredCrossSaleWindowShadeSuggestedRetail();" id="Editsugegsteddetail" runat="server" visible="true">Edit</a>
                                                </td>
                                                <td align="left">Min Width: &nbsp;
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtminwidth" MaxLength="10" runat="server" class="order-textfield"
                                                        Width="80px" onkeypress="return keyRestrictForInventory(event,'0123456789');"></asp:TextBox>
                                                </td>

                                                <td align="left">Max Width: &nbsp;
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtmaxwidth" MaxLength="10" runat="server" class="order-textfield"
                                                        Width="80px" onkeypress="return keyRestrictForInventory(event,'0123456789');"></asp:TextBox>
                                                </td>

                                            </tr>
                                            <tr>
                                                <td align="left">MarkUp $: &nbsp;
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtMarkup" MaxLength="10" runat="server" class="order-textfield" ReadOnly="true"
                                                        Width="80px" onkeypress="return keyRestrict(event,'0123456789');"></asp:TextBox>
                                                    <a href="javascript:void(0);" id="Editmarkup" onclick="openCenteredCrossSaleWindowShadeMarkUp();" runat="server" visible="true">Edit</a>
                                                </td>

                                                <td align="left">Min Length: &nbsp;
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtminlength" MaxLength="10" runat="server" class="order-textfield"
                                                        Width="80px" onkeypress="return keyRestrictForInventory(event,'0123456789');"></asp:TextBox>
                                                </td>

                                                <td align="left">Max Length: &nbsp;
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtmaxlength" MaxLength="10" runat="server" class="order-textfield"
                                                        Width="80px" onkeypress="return keyRestrictForInventory(event,'0123456789');"></asp:TextBox>
                                                </td>


                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" align="left" style="width: 100%;">




                                        <div id="divshadecalculator" runat="server" class="divshadecalc">
                                        </div>


                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 100%;" colspan="2">
                                        <asp:Label ID="lblImportCSv" runat="server" Text="Import :"></asp:Label>

                                        <asp:FileUpload ID="uploadCSV" runat="server" Style="border: 1px solid #1a1a1a; background: #f5f5f5; color: #000000;" />



                                        <asp:Button ID="btnImport" OnClientClick="return validatefields();" Style="background: url(/App_Themes/gray/images/Import.gif) no-repeat transparent; width: 67px; height: 23px; border: none; cursor: pointer;" runat="server" OnClick="btnImport_Click" ValidationGroup="importfile" CausesValidation="true" />


                                        <asp:RegularExpressionValidator ID="revImage" ControlToValidate="uploadCSV" ValidationExpression="(.*?)\.(csv)$"
                                            Text="Select csv File Only (Ex.: .csv)" runat="server"
                                            ValidationGroup="importfile" Display="Dynamic" ForeColor="Red" />
                                        <asp:RequiredFieldValidator ID="reqfile" runat="server" Text="Please select.csv file" ValidationGroup="importfile" ControlToValidate="uploadCSV" Display="Dynamic" ForeColor="Red"></asp:RequiredFieldValidator>
                                        <asp:Label ID="lblMessage" runat="server" Text=""></asp:Label>
                                    </td>
                                </tr>

                            </table>

                        </div>
                    </td>

                </tr>

            </tbody>
        </table>
    </div>
    <div class="tab-content invoice-tab" id="divtab13" style="margin-top: 0px; font-size: 12px; display: none;">
        <table width="100%" cellspacing="0" cellpadding="0" border="0" class="border-td content-table">
            <tbody>
                <tr>
                    <th colspan="4">
                        <div class="main-title-left">
                            <img class="img-left" title="CELLULAR SHADE" alt="CELLULAR SHADE" src="/App_Themes/<%=Page.Theme %>/icon/add-product.png">
                            <h2>NAV Required filed</h2>
                        </div>


                    </th>
                </tr>

                <tr class="oddrow">
                    <td width="12%">Country of Origin:
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlcountryoforigin" runat="server" class="product-type">
                            <asp:ListItem Text="Select Country of Origin" Value=""></asp:ListItem>
                            <asp:ListItem Text="Canada" Value="Canada"></asp:ListItem>
                            <asp:ListItem Text="China" Value="China"></asp:ListItem>
                            <asp:ListItem Text="India" Value="India"></asp:ListItem>
                            <asp:ListItem Text="Pakistan" Value="Pakistan"></asp:ListItem>
                            <asp:ListItem Text="Taiwan" Value="Taiwan"></asp:ListItem>
                            <asp:ListItem Text="USA" Value="USA"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td width="12%">Ge. Product Posting Group:
                    </td>

                    <td>
                        <asp:DropDownList ID="ddlProductPostingGroup" runat="server" class="product-type">
                            <asp:ListItem Text="Select Product Posting Group" Value=""></asp:ListItem>
                            <asp:ListItem Text="Accessories" Value="ACESORIES"></asp:ListItem>
                            <asp:ListItem Text="Drapes" Value="DRAPE"></asp:ListItem>
                            <asp:ListItem Text="Fabric" Value="FABRIC"></asp:ListItem>
                            <asp:ListItem Text="Fabrication" Value="FABRICATION"></asp:ListItem>
                            <asp:ListItem Text="Furniture" Value="FURNITURE"></asp:ListItem>
                            <asp:ListItem Text="Hardware" Value="HARDWARE"></asp:ListItem>
                            <asp:ListItem Text="Misc" Value="MISC"></asp:ListItem>
                            <asp:ListItem Text="Roman Shade" Value="ROMAN_SHDE"></asp:ListItem>
                            <asp:ListItem Text="Services-Item Charge" Value="SERVICES"></asp:ListItem>
                            <asp:ListItem Text="Swatch" Value="SWATCH"></asp:ListItem>

                        </asp:DropDownList>
                    </td>

                </tr>
                <tr class="altrow">
                    <td>Inventory Posting Group:

                    </td>
                    <td>
                        <asp:DropDownList ID="ddlInventoryPostingGroup" runat="server" class="product-type">
                            <asp:ListItem Text="Select Inventory Posting Group" Value=""></asp:ListItem>
                            <asp:ListItem Text="Finished Goods" Value="FG"></asp:ListItem>
                            <asp:ListItem Text="Raw Material" Value="RM"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td>No. of Width:
                    </td>
                    <td>
                        <asp:TextBox ID="txtnoofwidth" runat="server" class="order-textfield" Width="80px"
                            onkeypress="return keyRestrict(event,'0123456789.');" TabIndex="12"></asp:TextBox>
                        &nbsp;&nbsp;
                                                                                   
                                                                                                            Weighted &nbsp;&nbsp;
                                                                                     <asp:DropDownList ID="ddlweighted" class="product-type" Width="100px" runat="server"
                                                                                         TabIndex="35">
                                                                                         <asp:ListItem Text="Select Weighted" Value=""></asp:ListItem>
                                                                                         <asp:ListItem Text="Yes" Value="Yes"></asp:ListItem>
                                                                                         <asp:ListItem Text="No" Value="No"></asp:ListItem>
                                                                                     </asp:DropDownList>
                    </td>

                </tr>

                <tr class="oddrow">
                    <td>NAV Header
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlnavheader" runat="server" class="product-type">
                            <asp:ListItem Text="Select NAV Header" Value=""></asp:ListItem>
                            <asp:ListItem Text="French Pleat" Value="FRENCH PLEAT"></asp:ListItem>
                            <asp:ListItem Text="Grommet" Value="GROMMET"></asp:ListItem>
                            <asp:ListItem Text="Pole Pocket" Value="POLE POCKET"></asp:ListItem>
                            <asp:ListItem Text="Pole Pocket Back Tabs" Value="POLE POCKET BACK TABS"></asp:ListItem>
                            <asp:ListItem Text="Pole Pocket With Back Tabs" Value="POLE POCKET WITH BACK TABS"></asp:ListItem>
                            <asp:ListItem Text="Pole Pocket With Hook Belt" Value="POLE POCKET WITH HOOK BELT"></asp:ListItem>
                            <asp:ListItem Text="Pole Pocket With Hook Belt & Back Tabs" Value="POLE POCKET WITH HOOK BELT & BACK TABS"></asp:ListItem>
                            <asp:ListItem Text="Ruched Header" Value="RUCHED HEADER"></asp:ListItem>
                            <asp:ListItem Text="Ruched Pole Pocket" Value="RUCHED POLE POCKET"></asp:ListItem>
                        </asp:DropDownList>
                    </td>

                </tr>
                <tr class="altrow">
                    <td width="12%">NAV Item Category
                                                                                                            
                    </td>
                    <td>




                        <asp:DropDownList ID="ddlnavitemcategory" runat="server" AutoPostBack="true" class="product-type" OnSelectedIndexChanged="ddlnavitemcategory_SelectedIndexChanged"></asp:DropDownList>



                    </td>

                    <td width="12%" runat="server" id="tditemgroup1" visible="false">NAV Prod. Group Code 
                    </td>

                    <td runat="server" id="tditemgroup" visible="false">

                        <asp:DropDownList ID="ddlProductGroupCode" runat="server" class="product-type"></asp:DropDownList>

                    </td>
                </tr>

            </tbody>
        </table>

    </div>
    <div class="tab-content invoice-tab" id="divtab12" style="margin-top: 0px; font-size: 12px; display: none;">
        <table width="100%" border="0" cellpadding="0" cellspacing="0" class="content-table border-td">

            <tr>
                <th colspan="6">
                    <div class="main-title-left">
                        <img class="img-left" title=" REPLENISHMENT" alt=" REPLENISHMENT" src="/App_Themes/<%=Page.Theme %>/icon/add-product.png" />
                        <h2>Replenishment</h2>
                    </div>
                    <div class="main-title-right">
                        <a href="javascript:void(0);" class="show_hideDesc" onclick="return ShowHideButton('ImgREPLENISHMENT','tdREPLENISHMENT');">
                            <img id="ImgREPLENISHMENT" class="minimize" title="Minimize" alt="Minimize" src="/App_Themes/<%=Page.Theme %>/images/minimize.png" /></a>
                    </div>
                </th>
            </tr>

            <tr>


                <td valign="top" id="tdREPLENISHMENT">
                    <div id="div12" class="slidingoptionDivDesc">
                        <table width="100%" border="0" cellpadding="0" cellspacing="0">
                            <tr>
                                <td></td>
                                <td style="vertical-align: top;" align="right">
                                    <a href="/resources/halfpricedraps/product/ProductCSV/ImportReplenishemntCSV/SampleReplenishmentdata.csv" style="float: right">Download Sample File</a>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 100%;" colspan="2">

                                    <asp:Button ID="btnImportReplenishment" Style="background: url(/REPLENISHMENTMANAGEMENT/Images/import-replenishment-file.gif) no-repeat transparent; border: none; cursorointer; width: 200px; height: 23px;" runat="server" OnClick="btnImportReplenishment_Click" Text="" ValidationGroup="importfile1" CausesValidation="true" />



                                    <asp:Label ID="lblReplenishmentMsg" runat="server" Text=""></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 100%;" colspan="2">
                                    <asp:Label ID="lblerrorsku" runat="server" Visible="false"></asp:Label>
                                    <asp:Label ID="lblreplenishedskucount" runat="server" Visible="false"></asp:Label>
                                    <asp:Literal ID="ltrErrors" runat="server"></asp:Literal>
                                </td>
                            </tr>

                            <tr>
                                <td valign="top" align="left" colspan="2">
                                    <asp:GridView ID="grdREPLENISHMENT1" runat="server" OnRowDataBound="grdREPLENISHMENT1_RowDataBound" AutoGenerateColumns="false" BorderColor="#e7e7e7" BorderStyle="Solid" BorderWidth="1px" EmptyDataText="No Record(s) Found." AllowSorting="false" EmptyDataRowStyle-ForeColor="Red" EmptyDataRowStyle-HorizontalAlign="Center" ViewStateMode="Enabled" Width="100%" GridLines="Vertical" AllowPaging="True" PageSize="50" PagerSettings-Mode="NumericFirstLast">

                                        <Columns>
                                            <asp:TemplateField>
                                                <HeaderTemplate>
                                                    SKU (Individual SKU)

                                                </HeaderTemplate>
                                                <ItemStyle HorizontalAlign="Left" />
                                                <HeaderStyle HorizontalAlign="Left" />
                                                <ItemTemplate>
                                                    <asp:Label ID="lblvariantsku" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"SKU") %>'></asp:Label>
                                                    <asp:TextBox ID="txtvariantsku" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"SKU") %>'></asp:TextBox>

                                                    <input type="hidden" id="hdnVariantValueID" runat="server" value='<%#Eval("VariantValueID") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <HeaderTemplate>
                                                    Purchase  Order 1
                                                </HeaderTemplate>
                                                <ItemStyle HorizontalAlign="Left" />
                                                <HeaderStyle HorizontalAlign="Left" />
                                                <ItemTemplate>
                                                    <table class="gridtable" cellpadding="0" cellspacing="0" border="0">
                                                        <tr>

                                                            <td width="130px">
                                                                <asp:Label ID="lblPO1" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"PO1") %>'></asp:Label>
                                                                <asp:TextBox ID="txtPO1" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"PO1") %>' Width="98"></asp:TextBox>

                                                            </td>
                                                            <td width="100px">
                                                                <asp:Label ID="lblqty1" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"qty1") %>'></asp:Label>
                                                                <asp:TextBox ID="txtqty1" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"qty1") %>' Width="98px" onKeyPress="var ret=keyRestrictForInventory(event,'0123456789');return ret;"></asp:TextBox>

                                                            </td>
                                                            <td width="136px">
                                                                <asp:Label ID="lblshipping1" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"Shipping1") %>'></asp:Label>
                                                                <asp:TextBox ID="txtshipping1" runat="server" Text='<%# String.IsNullOrEmpty(DataBinder.Eval(Container.DataItem,"Shipping1").ToString()) ||  DataBinder.Eval(Container.DataItem,"Shipping1").ToString().IndexOf("Shipping") > -1 ? "" : string.Format("{0:MM/dd/yyyy}",Convert.ToDateTime(DataBinder.Eval(Container.DataItem,"Shipping1"))) %>' CssClass="from-textfield"></asp:TextBox>
                                                            </td>
                                                            <td width="136px">
                                                                <asp:Label ID="lblEtadate1" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"Etadate1") %>'></asp:Label>
                                                                <asp:TextBox ID="txtEtadate1" runat="server" Text='<%# String.IsNullOrEmpty(DataBinder.Eval(Container.DataItem,"Etadate1").ToString()) ||  DataBinder.Eval(Container.DataItem,"Etadate1").ToString().IndexOf("ETA") > -1 ? "" : string.Format("{0:MM/dd/yyyy}",Convert.ToDateTime(DataBinder.Eval(Container.DataItem,"Etadate1"))) %>' CssClass="from-textfield"></asp:TextBox>
                                                            </td>
                                                        </tr>

                                                    </table>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <HeaderTemplate>
                                                    Purchase   Order 2
                                                </HeaderTemplate>
                                                <ItemStyle HorizontalAlign="Left" />
                                                <HeaderStyle HorizontalAlign="Left" />
                                                <ItemTemplate>
                                                    <table class="gridtable" cellpadding="0" cellspacing="0" border="0">
                                                        <tr>

                                                            <td style="width: 130px">
                                                                <asp:Label ID="lblPO2" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"PO2") %>'></asp:Label>
                                                                <asp:TextBox ID="txtPO2" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"PO2") %>' Width="98"></asp:TextBox>

                                                            </td>
                                                            <td style="width: 100px">
                                                                <asp:Label ID="lblqty2" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"qty2") %>'></asp:Label>
                                                                <asp:TextBox ID="txtqty2" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"qty2") %>' Width="98" onKeyPress="var ret=keyRestrictForInventory(event,'0123456789');return ret;"></asp:TextBox>

                                                            </td>
                                                            <td width="136px">
                                                                <asp:Label ID="lblshipping2" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"Shipping2") %>'></asp:Label>
                                                                <asp:TextBox ID="txtshipping2" runat="server" Text='<%# String.IsNullOrEmpty(DataBinder.Eval(Container.DataItem,"Shipping2").ToString()) ||  DataBinder.Eval(Container.DataItem,"Shipping2").ToString().IndexOf("Shipping") > -1 ? "" : string.Format("{0:MM/dd/yyyy}",Convert.ToDateTime(DataBinder.Eval(Container.DataItem,"Shipping2"))) %>' CssClass="from-textfield"></asp:TextBox>
                                                            </td>
                                                            <td style="width: 136px">
                                                                <asp:Label ID="lblEtadate2" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"Etadate2") %>'></asp:Label>
                                                                <asp:TextBox ID="txtEtadate2" runat="server" Text='<%# String.IsNullOrEmpty(DataBinder.Eval(Container.DataItem,"Etadate2").ToString()) ||  DataBinder.Eval(Container.DataItem,"Etadate2").ToString().IndexOf("ETA") > -1 ? "" : string.Format("{0:MM/dd/yyyy}",Convert.ToDateTime(DataBinder.Eval(Container.DataItem,"Etadate2"))) %>' CssClass="from-textfield"></asp:TextBox>
                                                            </td>
                                                        </tr>

                                                    </table>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <HeaderTemplate>
                                                    Purchase   Order 3
                                                </HeaderTemplate>
                                                <ItemStyle HorizontalAlign="Left" />
                                                <HeaderStyle HorizontalAlign="Left" />
                                                <ItemTemplate>
                                                    <table class="gridtable" cellpadding="0" cellspacing="0" border="0">
                                                        <tr>

                                                            <td width="130px">
                                                                <asp:Label ID="lblPO3" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"PO3") %>'></asp:Label>
                                                                <asp:TextBox ID="txtPO3" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"PO3") %>' Width="98"></asp:TextBox>

                                                            </td>
                                                            <td width="100px">
                                                                <asp:Label ID="lblqty3" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"qty3") %>'></asp:Label>
                                                                <asp:TextBox ID="txtqty3" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"qty3") %>' Width="98" onKeyPress="var ret=keyRestrictForInventory(event,'0123456789');return ret;"></asp:TextBox>

                                                            </td>
                                                            <td width="136px">
                                                                <asp:Label ID="lblshipping3" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"Shipping3") %>'></asp:Label>
                                                                <asp:TextBox ID="txtshipping3" runat="server" Text='<%# String.IsNullOrEmpty(DataBinder.Eval(Container.DataItem,"Shipping3").ToString()) ||  DataBinder.Eval(Container.DataItem,"Shipping3").ToString().IndexOf("Shipping") > -1 ? "" : string.Format("{0:MM/dd/yyyy}",Convert.ToDateTime(DataBinder.Eval(Container.DataItem,"Shipping3"))) %>' CssClass="from-textfield"></asp:TextBox>
                                                            </td>
                                                            <td width="136px">
                                                                <asp:Label ID="lblEtadate3" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"Etadate3") %>'></asp:Label>
                                                                <asp:TextBox ID="txtEtadate3" runat="server" Text='<%# String.IsNullOrEmpty(DataBinder.Eval(Container.DataItem,"Etadate3").ToString()) ||  DataBinder.Eval(Container.DataItem,"Etadate3").ToString().IndexOf("ETA") > -1 ? "" : string.Format("{0:MM/dd/yyyy}",Convert.ToDateTime(DataBinder.Eval(Container.DataItem,"Etadate3"))) %>' CssClass="from-textfield"></asp:TextBox>
                                                            </td>
                                                        </tr>

                                                    </table>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <HeaderTemplate>
                                                    Purchase    Order 4
                                                </HeaderTemplate>
                                                <ItemStyle HorizontalAlign="Left" />
                                                <HeaderStyle HorizontalAlign="Left" />
                                                <ItemTemplate>
                                                    <table class="gridtable" cellpadding="0" cellspacing="0" border="0">
                                                        <tr>

                                                            <td width="130px">
                                                                <asp:Label ID="lblPO4" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"PO4") %>'></asp:Label>
                                                                <asp:TextBox ID="txtPO4" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"PO4") %>' Width="98"></asp:TextBox>

                                                            </td>
                                                            <td width="100px">
                                                                <asp:Label ID="lblqty4" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"qty4") %>'></asp:Label>
                                                                <asp:TextBox ID="txtqty4" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"qty4") %>' Width="98" onKeyPress="var ret=keyRestrictForInventory(event,'0123456789');return ret;"></asp:TextBox>

                                                            </td>
                                                            <td width="136px">
                                                                <asp:Label ID="lblshipping4" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"Shipping4") %>'></asp:Label>
                                                                <asp:TextBox ID="txtshipping4" runat="server" Text='<%# String.IsNullOrEmpty(DataBinder.Eval(Container.DataItem,"Shipping4").ToString()) ||  DataBinder.Eval(Container.DataItem,"Shipping4").ToString().IndexOf("Shipping") > -1 ? "" : string.Format("{0:MM/dd/yyyy}",Convert.ToDateTime(DataBinder.Eval(Container.DataItem,"Shipping4"))) %>' CssClass="from-textfield"></asp:TextBox>
                                                            </td>
                                                            <td width="136px">
                                                                <asp:Label ID="lblEtadate4" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"Etadate4") %>'></asp:Label>

                                                                <asp:TextBox ID="txtEtadate4" runat="server" Text='<%# String.IsNullOrEmpty(DataBinder.Eval(Container.DataItem,"Etadate4").ToString()) ||  DataBinder.Eval(Container.DataItem,"Etadate4").ToString().IndexOf("ETA") > -1 ? "" : string.Format("{0:MM/dd/yyyy}",Convert.ToDateTime(DataBinder.Eval(Container.DataItem,"Etadate4"))) %>' CssClass="from-textfield"></asp:TextBox>
                                                            </td>
                                                        </tr>

                                                    </table>
                                                </ItemTemplate>
                                            </asp:TemplateField>


                                        </Columns>
                                        <PagerSettings Mode="NumericFirstLast" Position="TopAndBottom"></PagerSettings>
                                        <PagerStyle HorizontalAlign="Right" CssClass="grid_paging" />
                                        <RowStyle HorizontalAlign="Center" CssClass="oddrow" />
                                        <AlternatingRowStyle CssClass="altrow" />
                                        <HeaderStyle ForeColor="White" Font-Bold="false" />

                                    </asp:GridView>

                                </td>
                            </tr>

                            <tr>

                                <td valign="top" align="Center">

                                    <asp:ImageButton ID="btnRepleshmentsave1" runat="server" OnClick="btnRepleshmentsave1_Click" />
                                </td>
                            </tr>

                        </table>
                    </div>
                </td>
            </tr>


        </table>

    </div>

    </div>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="center">
                                                            <div id="divfloating" class="divfloatingcss" style="width: 300px;">
                                                                <div style="margin-bottom: 1px; margin-top: 3px;">
                                                                    <asp:ImageButton ID="btnApprove" runat="server" TabIndex="37" OnClick="btnApprove_Click" Visible="false" OnClientClick="javascript:if(confirm('Are you sure want to Approve this Item?')){return true;}else{return false;}" />
                                                                    &nbsp;<asp:ImageButton ID="btnSave" runat="server" OnClientClick="if(ValidatePage()){document.getElementById('ContentPlaceHolder1_hdnexit').value = '0'; return true;}else {return false;}"
                                                                        OnClick="btnSave_Click" TabIndex="38" />&nbsp;<asp:ImageButton ID="btnSaveandexit" runat="server" OnClientClick="if(ValidatePage()){document.getElementById('ContentPlaceHolder1_hdnexit').value = '1'; return true;}else {return false;}"
                                                                            OnClick="btnSave_Click" TabIndex="38" />&nbsp;
                                                                   
                                                        <asp:ImageButton ID="btnCancel" runat="server" OnClick="btnCancel_Click" TabIndex="39" />
                                                                </div>
                                                            </div>
                                                        </td>
                                                    </tr>
    </tbody>
                                            </table>
                                        </td>
                                    </tr>

                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td height="10" align="left" valign="top">
                    <img src="/App_Themes/<%=Page.Theme %>/images/spacer.gif" width="1" height="10">
                </td>
            </tr>
    </table>
    </div>
    <div style="visibility: hidden">
        <iframe id="ifmcontentstoprint"></iframe>
    </div>
    <!--start tab--->
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-1.2.6.min.js"></script>
    <script src="/App_Themes/<%=Page.Theme %>/js/tabs.js" type="text/javascript"></script>
    <!--end tab--->
    <div style="display: none;">
        <input type="hidden" id="hdnSubTabid" runat="server" value="1" />        
        <input type="hidden" id="hdnTabid" runat="server" value="1" />
        <input type="hidden" id="hdnexit" runat="server" value="0" />
        <input type="hidden" id="hdnredirecturl" runat="server" value="" />
    </div>
</asp:Content>
