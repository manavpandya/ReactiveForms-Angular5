<%@ Page Title="" Language="C#" MasterPageFile="~/REPLENISHMENTMANAGEMENT/AdminReplnishment.Master" AutoEventWireup="true" CodeBehind="SetupInventoryFeedtemplate.aspx.cs" Inherits="Solution.UI.Web.REPLENISHMENTMANAGEMENT.SetupInventoryFeedtemplate" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script src="https://code.jquery.com/jquery-1.11.1.min.js"></script>
 
    <script src="/REPLENISHMENTMANAGEMENT/js/drag-arrange.js"></script>
    <script src="/REPLENISHMENTMANAGEMENT/js/iCheck/jquery.icheck.js"></script> 
<script src="/REPLENISHMENTMANAGEMENT/js/icheck-init.js"></script> 
    <link href="/REPLENISHMENTMANAGEMENT/js/iCheck/skins/minimal/minimal.css" rel="stylesheet" />
<link href="/REPLENISHMENTMANAGEMENT/js/iCheck/skins/minimal/red.css" rel="stylesheet" />
<link href="/REPLENISHMENTMANAGEMENT/js/iCheck/skins/minimal/green.css" rel="stylesheet" />
<link href="/REPLENISHMENTMANAGEMENT/js/iCheck/skins/minimal/blue.css" rel="stylesheet" />
<link href="/REPLENISHMENTMANAGEMENT/js/iCheck/skins/minimal/yellow.css" rel="stylesheet" />
<link href="/REPLENISHMENTMANAGEMENT/js/iCheck/skins/minimal/purple.css" rel="stylesheet" />
<link href="/REPLENISHMENTMANAGEMENT/js/iCheck/skins/square/square.css" rel="stylesheet" />
<link href="/REPLENISHMENTMANAGEMENT/js/iCheck/skins/square/red.css" rel="stylesheet" />
<link href="/REPLENISHMENTMANAGEMENT/js/iCheck/skins/square/green.css" rel="stylesheet" />
<link href="/REPLENISHMENTMANAGEMENT/js/iCheck/skins/square/blue.css" rel="stylesheet" />
<link href="/REPLENISHMENTMANAGEMENT/js/iCheck/skins/square/yellow.css" rel="stylesheet" />
<link href="/REPLENISHMENTMANAGEMENT/js/iCheck/skins/square/purple.css" rel="stylesheet" />
<link href="/REPLENISHMENTMANAGEMENT/js/iCheck/skins/flat/grey.css" rel="stylesheet" />
<link href="/REPLENISHMENTMANAGEMENT/js/iCheck/skins/flat/red.css" rel="stylesheet" />
<link href="/REPLENISHMENTMANAGEMENT/js/iCheck/skins/flat/green.css" rel="stylesheet" />
<link href="/REPLENISHMENTMANAGEMENT/js/iCheck/skins/flat/blue.css" rel="stylesheet" />
<link href="/REPLENISHMENTMANAGEMENT/js/iCheck/skins/flat/yellow.css" rel="stylesheet" />
<link href="/REPLENISHMENTMANAGEMENT/js/iCheck/skins/flat/purple.css" rel="stylesheet" />
    <%--<script type="text/javascript" src="/REPLENISHMENTMANAGEMENT/js/jquery-1.8.2.js"></script>--%>
       <%--<script type="text/javascript" src="/REPLENISHMENTMANAGEMENT/js/jquery.elevatezoom.min.js"></script>
            <script type="text/javascript" src="/REPLENISHMENTMANAGEMENT/js/jquery.fancybox.pack.js"></script>
     <link type="text/css" href="/REPLENISHMENTMANAGEMENT/css/jQuery.fancybox.css?5767" rel="stylesheet" />   --%>
 
 <script type="text/javascript">
     function configuredatacolumn(id) {

         var divid = 'divconfigure_' + id.toString();
         var checkboxid = 'chkcolumn_' + id.toString();
         if (document.getElementById(checkboxid) != null && document.getElementById(checkboxid).checked == true) {
             if (document.getElementById(divid) != null) {
                 document.getElementById(divid).style.display = '';
             }
             document.getElementById(checkboxid).value = 'Yes';
         }
         else {
             if (document.getElementById(divid) != null) {
                 document.getElementById(divid).style.display = 'none';
             }
             document.getElementById(checkboxid).value = 'No';
         }

     }
     //$.noConflict();
     //jQuery.noConflict();
     //$ll(document).ready(function ($) {
     //    $ll('a.fancybox').fancybox();
     //});
     function windowoprnincenter(urllink, wi, he)
     {
         var w = wi;
         var h = he;
         var left = (screen.width / 2) - (w / 2);
         var top = (screen.height / 2) - (h / 2);
         window.open(urllink, '_blank', 'toolbar=no,addessbar=no, location=no, directories=no, status=no, menubar=no, scrollbars=yes, resizable=no, copyhistory=no, width=' + w + ', height=' + h + ', top=' + top + ', left=' + left);
     }
    
</script>
 
         <%--<link type="text/css" href="/REPLENISHMENTMANAGEMENT/css/jQuery.fancybox.css" rel="stylesheet" />   
 <script type="text/javascript" src="/REPLENISHMENTMANAGEMENT/js/jquery.elevatezoom.min.js"></script>
            <script type="text/javascript" src="/REPLENISHMENTMANAGEMENT/js/jquery.fancybox.pack.js"></script>  --%>    
    <style type="text/css">
        .dd-item {
            background: none repeat scroll 0 0 #f5f5f5;
            border: 1px solid rgb(196, 196, 196);
            font-size: 12px;
            list-style: outside none none;
            margin: 5px 0;
            cursor: move;
            font-weight:bold;
            margin:5px 0;
            padding:5px 10px;
            height: 30px;
        }

        .dd-handle {
            background:none repeat scroll 0 0 #f5f5f5;
            cursor: move;
            display: inline-block;
             
            vertical-align: middle;
            width: 90%;
            
        }
        .dd-item:hover{
            color:#4BBAE6;
        }
         .dd-handle::-moz-selection
        {
            background:none repeat scroll 0 0 #f5f5f5;
            color:#000;
        }
    </style>
    <script type="text/javascript">
        //$.noConflict();
        //$(document).ready(function ($) {
        //    $('a.fancybox').fancybox();
        //});
        //$.noConflict();
        //$(document).ready(function ($) {
        //    $('a.fancybox').fancybox();
        //});
</script>
    <script type="text/javascript">
        function checkparent(chkid) {

            var iddd = $(chkid).parent().attr('class');



            var id2 = '';

            if (chkid.id.toLowerCase().indexOf('demo-radio_1') > -1) {

                id2 = chkid.id.replace("demo-radio_1", "demo-radio_2");

            }

            else {



                id2 = chkid.id.replace("demo-radio_2", "demo-radio_1");



            }





            if (iddd.toString().toLowerCase() == "iradio_minimal") {

                $(chkid).parent().attr('class', 'iradio_minimal checked');

                var iddd1 = $('#' + id2).parent().attr('class');

                if (iddd1.toString().toLowerCase() == "iradio_minimal checked") {

                    $('#' + id2).parent().attr('class', 'iradio_minimal');

                }



            }







            //if (iddd.toString().toLowerCase() == "iradio_minimal") {

            //    $(id).parent().attr('class', 'iradio_minimal checked');

            //}

        }
        function backnext2() {
            $('#setup-probox').fadeIn('slow');
            $('#mapping-probox').hide();
            $('#sequence-probox').hide();
            $('#generate-probox').hide();

            $('#setup-wizard').removeClass();
            $('#setup-wizard').addClass('current active-trail');
            $('#mapping-wizard').removeClass();
            $('#mapping-wizard').addClass('active-trail');
            $('#sequence-wizard').removeClass();
            $('#sequence-wizard').addClass('active-trail');
            $('#generate-wizard').removeClass();
            $('#generate-wizard').addClass('active-trail');

            $('#setup-wizard').parent().addClass('active');
            $('#mapping-wizard').parent().removeClass('active');
        }
        function backnext3() {
            
            //document.getElementById('allhtmlfile').innerHTML = document.getElementById('ContentPlaceHolder1_hdnallcolumndata').value;

            $('#setup-probox').hide();
            $('#mapping-probox').fadeIn('slow');
            $('#sequence-probox').hide();
            $('#generate-probox').hide();

            $('#setup-wizard').removeClass();
            $('#setup-wizard').addClass('currentpro active-trail');
            $('#mapping-wizard').removeClass();
            $('#mapping-wizard').addClass('current active-trail');
            $('#sequence-wizard').removeClass();
            $('#sequence-wizard').addClass('active-trail');
            $('#generate-wizard').removeClass();
            $('#generate-wizard').addClass('active-trail');

            $('#mapping-wizard').parent().addClass('active');
            $('#sequence-wizard').parent().removeClass('active');
        }
        function backnext4() {
            if (document.getElementById('ContentPlaceHolder1_btndone') == null) {
                backnext2();
                clearefiled();
            }
            else {
                document.getElementById('nestable_list_1').innerHTML = document.getElementById('ContentPlaceHolder1_hdndisplayorderlist').value;
                $("li").arrangeable({ dragSelector: '.dd-handle' });
                $('#setup-probox').hide();
                $('#mapping-probox').hide();
                $('#sequence-probox').fadeIn('slow');
                $('#generate-probox').hide();

                $('#setup-wizard').removeClass();
                $('#setup-wizard').addClass('currentpro active-trail');
                $('#mapping-wizard').removeClass();
                $('#mapping-wizard').addClass('currentpro active-trail');
                $('#sequence-wizard').removeClass();
                $('#sequence-wizard').addClass('current active-trail');
                $('#generate-wizard').removeClass();
                $('#generate-wizard').addClass('active-trail');

                $('#sequence-wizard').parent().addClass('active');
                $('#generate-wizard').parent().removeClass('active');
            }

        }
        function checkfilename() {
            var value = document.getElementById('ContentPlaceHolder1_txtfilename').value;
            if (value.toString().indexOf('.') > -1) {
                var ext = value.substring(value.lastIndexOf('.'));
                document.getElementById('ContentPlaceHolder1_txtfilename').value = document.getElementById('ContentPlaceHolder1_txtfilename').value.toString().replace(ext, '');
            }


        }
        function checkallfiledvalue() {
            var alldivgroup = document.getElementById('allhtmlfile').getElementsByTagName('div');
            var allcouont = 0;
            if (alldivgroup.length > 0) {
                for (var j = 0; j < alldivgroup.length; j++) {
                    var cntrldivid = alldivgroup[j].id;


                    if (cntrldivid != undefined && cntrldivid.toString().indexOf("div_") > -1) {
                        allcouont++;
                        var allinput = document.getElementById(cntrldivid).getElementsByTagName('input');

                        var tt = 0;
                        var tt1 = 0;
                        var tt2 = 0;
                        var textid = '';
                        var vall = '';
                        var tcolumn = 0;
                        if (allinput.length > 0) {
                            for (var i = 0; i < allinput.length; i++) {
                                var cntrlid = allinput[i];
                                if (cntrlid.id.toString().toLowerCase().indexOf('column_name_') > -1) {
                                    textid = cntrlid.id.toString();
                                    vall = cntrlid.value;
                                }
                                if (cntrlid.id.toString().toLowerCase().indexOf('column_name_') > -1 && cntrlid.value.replace(/^\s*\s*$/g, '') == '') {
                                    tt = 1;

                                }
                                else if (cntrlid.id.toString().toLowerCase().indexOf('demo-radio_1') > -1 && cntrlid.checked == false) {
                                    tt1 = 1;

                                }
                                else if (cntrlid.id.toString().toLowerCase().indexOf('demo-radio_2') > -1 && cntrlid.checked == false) {
                                    tt2 = 1;
                                }
                                if (document.getElementById(textid.replace('column_name_', 'select_')) != null && document.getElementById(textid.replace('column_name_', 'select_')).selectedIndex == 0 && document.getElementById(textid.replace('column_name_', 'txtstatic_')) != null && document.getElementById(textid.replace('column_name_', 'txtstatic_')).value.replace(/^\s*\s*$/g, '') == '') {
                                    tcolumn = 1;
                                }


                            }
                        }
                        if (allcouont == 1) {
                            if (((tt2 == 0 || tt1 == 0 || tcolumn == 0) && tt == 1)) {
                                alert('Please enter column name');
                                document.getElementById(textid).focus();
                                return false;
                            }
                            else if ((tt2 == 1 && tt1 == 1 && tt == 1)) {
                                alert('Please enter column name');
                                document.getElementById(textid).focus();
                                return false;
                            }
                            else if ((tt2 == 1 && tt1 == 1 && tt == 0)) {
                                alert('Please select attributes.');
                                document.getElementById(textid.replace('column_name_', 'demo-radio_1_')).focus();
                                return false;
                            }
                            else if (tcolumn== 1) {
                                alert('Please select column data.');
                                document.getElementById(textid.replace('column_name_', 'select_')).focus();
                                return false;
                            }
                        }

                        else {
                            if (((tt2 == 0 || tt1 == 0) && tt == 1)) {
                                alert('Please enter column name');
                               // 
                                $('html, body').animate({ scrollTop: $('#' + textid).offset().top - 80 }, 'slow');
                                document.getElementById(textid).focus();
                                return false;
                            }
                            else if ((tt2 == 1 && tt1 == 1 && tt == 0)) {
                                alert('Please select attributes.');
                                $('html, body').animate({ scrollTop: $('#' + textid.replace('column_name_', 'demo-radio_1_')).offset().top - 80 }, 'slow');
                                
                                return false;
                            }
                            else if (tcolumn == 1 && (tt2 == 0 || tt1 == 0 || tt == 0)) {
                                alert('Please select column data.');
                               // 
                                $('html, body').animate({ scrollTop: $('#' + textid.replace('column_name_', 'select_')).offset().top - 80 }, 'slow');
                                document.getElementById(textid.replace('column_name_', 'select_')).focus();
                                return false;
                            }
                        }


                    }
                }
            }
            //document.getElementById('ContentPlaceHolder1_hdnallcolumndata').value = document.getElementById('allhtmlfile').innerHTML;
           
            return true;
        }
        function changecolumndata(id) {
            var dlid = id.replace('txtstatic_', 'select_');
            if (document.getElementById(dlid) != null) {
                document.getElementById(dlid).selectedIndex = 0;
            }
        }
        function getdisplayorderlist() {

            document.getElementById('ContentPlaceHolder1_hdndisplayorderlist').value = document.getElementById('nestable_list_1').innerHTML;
        }
        function clearefiled() {

            if (document.getElementById('ContentPlaceHolder1_txtstorename') != null) {
                document.getElementById('ContentPlaceHolder1_txtstorename').value = '';
            }
            if (document.getElementById('ContentPlaceHolder1_ddlstore') != null) {
                document.getElementById('ContentPlaceHolder1_ddlstore').selectedIndex = 0;
            }
            if (document.getElementById('ContentPlaceHolder1_ddlexiststore') != null) {
                document.getElementById('ContentPlaceHolder1_ddlexiststore').selectedIndex = 0;
            }
            document.getElementById('ContentPlaceHolder1_ddlstore').focus();

        }
        function nextvisiblewhenimput() {
            document.getElementById('ContentPlaceHolder1_ddlstore').selectedIndex = 0;
           //  document.getElementById('ContentPlaceHolder1_ddlexiststore').selectedIndex = 0;
            if (document.getElementById('ContentPlaceHolder1_txtstorename') != null && document.getElementById('ContentPlaceHolder1_txtstorename').value.replace(/^\s*\s*$/g, '') != '') {
                document.getElementById('ContentPlaceHolder1_mapping_btn_next').disabled = false;
                //document.getElementById().dis = true;
            }
            else {
                document.getElementById('ContentPlaceHolder1_mapping_btn_next').disabled = true;
            }

        }
        function nextvisiblewhenimput1() {
         //   document.getElementById('ContentPlaceHolder1_ddlexiststore').selectedIndex = 0;
            document.getElementById('ContentPlaceHolder1_txtstorename').value = '';
            if (document.getElementById('ContentPlaceHolder1_ddlstore') != null && document.getElementById('ContentPlaceHolder1_ddlstore').selectedIndex > 0) {
                document.getElementById('ContentPlaceHolder1_mapping_btn_next').disabled = false;
            }
            else {

                document.getElementById('ContentPlaceHolder1_mapping_btn_next').disabled = true;

            }

        }
        function nextvisiblewhenimput2() {

            document.getElementById('ContentPlaceHolder1_ddlstore').selectedIndex = 0;
            document.getElementById('ContentPlaceHolder1_txtstorename').value = '';
            if (document.getElementById('ContentPlaceHolder1_ddlexiststore') != null && document.getElementById('ContentPlaceHolder1_ddlexiststore').selectedIndex > 0) {
                document.getElementById('ContentPlaceHolder1_mapping_btn_next').disabled = false;
            }
            else {
                document.getElementById('ContentPlaceHolder1_mapping_btn_next').disabled = true;
            }

        }
        function validatincheck() {
            if (document.getElementById('ContentPlaceHolder1_txtstorename') != null && document.getElementById('ContentPlaceHolder1_txtstorename').value.replace(/^\s*\s*$/g, '') != '') {
            }
            else if (document.getElementById('ContentPlaceHolder1_ddlstore') != null && document.getElementById('ContentPlaceHolder1_ddlstore').selectedIndex > 0) {
            }
           // else if (document.getElementById('ContentPlaceHolder1_ddlexiststore') != null && document.getElementById('ContentPlaceHolder1_ddlexiststore').selectedIndex > 0) {
          //  }
            else {
                // alert('Select Channel Partner. \n Or Add New Channel Partner. \n Or Select Existing Feed Template.');
                alert('Select Channel Partner. \n Or Add New Channel Partner. \n ');
                return false;
            }

            return true;    
        }
        function addmorecolumn(id) {
            if (document.getElementById('ContentPlaceHolder1_divcolumndata') != null) {
                var ahtml = document.getElementById('ContentPlaceHolder1_divcolumndata').innerHTML;
                var icount = parseInt(id) + parseInt(1);
                var icountminus = parseInt(id) - parseInt(1);


                var allinput = document.getElementById('div_' + icountminus.toString()).getElementsByTagName('input');

                var tt = 0;
                var tt1 = 0;
                var tt2 = 0;
                
                var textid = '';
                var vall = '';
                var tcolumn = 0;
                if (allinput.length > 0) {
                    for (var i = 0; i < allinput.length; i++) {
                        var cntrlid = allinput[i];


                        if (cntrlid.id.toString().toLowerCase().indexOf('column_name_') > -1) {
                            textid = cntrlid.id.toString();
                            vall = cntrlid.value;
                        }
                        if (cntrlid.id.toString().toLowerCase().indexOf('column_name_') > -1 && cntrlid.value.replace(/^\s*\s*$/g, '') == '') {
                            tt = 1;
                            alert('Please enter column name');
                            cntrlid.focus();
                            return;
                        }
                        else if (cntrlid.id.toString().toLowerCase().indexOf('demo-radio_1') > -1 && cntrlid.checked == false) {
                            tt1 = 1;

                        }
                        else if (cntrlid.id.toString().toLowerCase().indexOf('demo-radio_2') > -1 && cntrlid.checked == false) {
                            tt2 = 1;
                        }
                        
                        if (document.getElementById(textid.replace('column_name_', 'select_')) != null && document.getElementById(textid.replace('column_name_', 'select_')).selectedIndex == 0 && document.getElementById(textid.replace('column_name_', 'txtstatic_')) != null && document.getElementById(textid.replace('column_name_', 'txtstatic_')).value.replace(/^\s*\s*$/g, '') == '')
                        {
                            tcolumn = 1;
                        }



                    }
                }
                if ((tt2 == 1 && tt1 == 1)) {
                    alert('Please select attributes.');
                    return;
                }
                if (tcolumn==1) {
                    alert('Please select column data.');
                    return;
                }

                var allolumnname = document.getElementById('allhtmlfile').getElementsByTagName('input');
                if (allolumnname.length > 0) {
                    for (var i = 0; i < allolumnname.length; i++) {
                        var cnid = allolumnname[i];
                        if (textid.toString().toLowerCase() != cnid.id.toString().toLowerCase() && vall != '') {
                            if (vall.toString().toLowerCase() == cnid.value.toString().toLowerCase()) {
                                alert('Column name already exists!!!');
                                cnid.focus();
                                return;
                            }
                        }
                    }
                }


                if (tt == 0 && ((tt1 == 1 && tt2 == 0) || (tt2 == 1 && tt1 == 0))) {



                    ahtml = ahtml.replace(/##kau###/g, id.toString()).replace(/##kau_new###/g, icount.toString());

                    $('#backbtnnext2').before(ahtml);
                    //document.getElementById('allhtmlfile').innerHTML = document.getElementById('allhtmlfile').innerHTML + ahtml;
                    document.getElementById('btnremove_' + icountminus.toString()).style.display = '';
                    //var backbtnhtml = document.getElementById('backbtnnext2').innerHTML;
                    // $("#backbtnnext2").remove();
                    // var atmll = "<div class=\"form-group\" id=\"backbtnnext2\">" + backbtnhtml + '</div>';
                    // document.getElementById('allhtmlfile').innerHTML = document.getElementById('allhtmlfile').innerHTML + atmll;
                    $("#btnmore_" + icountminus.toString()).remove();
                }



            }
        }
        function removecolumn(id) {
            $("#div_" + id.toString()).remove();
            $("#divgroup_" + id.toString()).remove();
        }
        function checkfileformat() {

            if (document.getElementById('ContentPlaceHolder1_txtfilename') != null && document.getElementById('ContentPlaceHolder1_txtfilename').value.replace(/^\s*\s*$/g, '') == '') {
                alert('Please enter filename.');
                return false;
            }
            else if (document.getElementById('ContentPlaceHolder1_rdocsv') != null && document.getElementById('ContentPlaceHolder1_rdocsv').checked == false && document.getElementById('ContentPlaceHolder1_rdoexcel') != null && document.getElementById('ContentPlaceHolder1_rdoexcel').checked == false) {
                alert('Please select file format.');
                return false;
            }
            return true;
        }

    </script>
    <div class="wrapper">
        <div class="row">
            <div class="col-md-12">
                <!--breadcrumbs start -->
                <ul class="breadcrumb">
                    <li>
                        <p class="hd-title">Setup Inventory Feed Template</p>
                    </li>
                </ul>
                <!--breadcrumbs end -->
            </div>
        </div>
        <div class="row">
            <div class="col-md-12">
                <div class="panel">
                    <div class="panel-body">
                        <div class="full-width wizardpro-4">
                            <ul class="wizardpro-alt">
                                <li class="active"><span id="setup-wizard" class="current">Select Channel Partner</span> </li>
                                <li><span id="mapping-wizard" class="active-trail">Channel Partner Field Mapping</span> </li>
                                <li><span id="sequence-wizard" class="active-trail">Configure Column Sequence</span> </li>
                                <li><span id="generate-wizard" class="active-trail">Generate Feed Template</span></li>
                            </ul>
                        </div>
                        <div id="setup-probox" class="panel fa-border" style="display:none;">
                            <header class="panel-heading">Select Channel Partner</header>
                            <div class="panel-body">
                                <p class="help-block m-bot20">Select or add a Channel Partner to setup a new inventory feed template or make updates to an existing one. </p>
                                <div class="form-horizontal">
                                    <%-- <asp:ValidationSummary DisplayMode="BulletList" ForeColor="Red" EnableClientScript="false" ID="validation_sum" runat="server" HeaderText="Errors list" ValidationGroup="Registration" BorderColor="Red" BorderStyle="Dotted" BorderWidth="1" BackColor="#ffff66" />--%>
                                    <div class="form-group">


                                        <label class="control-label col-md-3">Select Channel Partner</label>
                                        <div class="col-md-4">

                                            <asp:DropDownList ID="ddlstore" runat="server" CssClass="form-control" onchange="nextvisiblewhenimput1();"></asp:DropDownList>
                                            <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="select store name please <br/> Or Enter store name please <br/> Or select store name please" Text="" ControlToValidate="ddlstore" EnableClientScript="False" Display="Dynamic" InitialValue="-1" ValidationGroup="Registration" CssClass="display-none" />--%>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="control-label col-md-3">&nbsp;</label>
                                        <div class="col-md-3">
                                            <div class="form-control-static text-center"><strong>OR</strong> </div>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="control-label col-md-3">Add New Channel Partner</label>
                                        <div class="col-md-4">
                                            <div class="form-control-static">
                                                <asp:TextBox ID="txtstorename" runat="server" CssClass="form-control" onchange="nextvisiblewhenimput();" onkeyup="nextvisiblewhenimput();"></asp:TextBox>
                                                <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="select store name please <br/> Or Enter store name please <br/> Or select store name please" Text="" ControlToValidate="txtstorename" EnableClientScript="False" Display="Dynamic" ValidationGroup="Registration" CssClass="display-none" />--%>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="form-group" style="display:none;">
                                        <label class="control-label col-md-3">&nbsp;</label>
                                        <div class="col-md-3">
                                            <div class="form-control-static text-center"><strong>OR</strong> </div>
                                        </div>
                                    </div>
                                    <div class="form-group" style="display:none;">
                                        <label class="control-label col-md-3">Select Existing Feed Template</label>
                                        <div class="col-md-4">
                                            <asp:DropDownList ID="ddlexiststore" runat="server" CssClass="form-control" onchange="nextvisiblewhenimput2();"></asp:DropDownList>
                                            <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="select store name please <br/> Or Enter store name please <br/> Or select store name please" Text="" ControlToValidate="ddlexiststore" EnableClientScript="False" Display="Dynamic" InitialValue="-1" ValidationGroup="Registration" CssClass="display-none" />--%>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="control-label col-md-3">&nbsp;</label>
                                        <div class="col-md-9">
                                            <div class="form-control-static">

                                                <button type="button" class="btn btn-orang" onclick="clearefiled();">Cancel</button>
                                                &nbsp;&nbsp; &nbsp;&nbsp;
                                                <asp:Button ID="mapping_btn_next" runat="server" CssClass="btn btn-orang" Text="Next" OnClientClick="return validatincheck();" OnClick="mapping_btn_next_Click" />

                                            </div>
                                        </div>
                                    </div>
                                </div>

                            </div>
                        </div>
                        <div class="panel fa-border" style="display: none" id="mapping-probox">
                            <header class="panel-heading">
                                Field Mapping for <strong>
                                    <asp:Literal ID="ltrstorename" runat="server"></asp:Literal>
                                </strong>
                            </header>
                            <div class="panel-body">
                                <p class="help-block m-bot20">Setup or modify columns and their data for the inventory feed template.</p>
                                <div class="form-horizontal" id="allhtmlfile">
                                    <asp:Literal ID="ltrgroupdata" runat="server"></asp:Literal>
                                    <div class="form-group" id="backbtnnext2">
                                        <label class="control-label col-md-3">&nbsp;</label>
                                        <div class="col-md-9">
                                            <div class="form-control-static">
                                                <button type="button" class="btn btn-orang" id="setup-btn-back" onclick="backnext2();">Back</button>
                                                &nbsp;&nbsp; &nbsp;&nbsp;<asp:Button ID="sequence_btn_next" runat="server" CssClass="btn btn-orang" Text="Next" OnClick="sequence_btn_next_Click" OnClientClick="return checkallfiledvalue();" />
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div id="sequence-probox" style="display: none" class="panel fa-border">
                            <header class="panel-heading">CONFIGURE FEED’S COLUMN SEQUENCE FOR <strong><asp:Literal ID="ltrstorename1" runat="server"></asp:Literal></strong> </header>
                            <div class="panel-body">
                                <p class="help-block m-bot20">
                                    You may change the sequence of the columns by dragging a column up or down with your mouse. 
                    The final inventory feed template will have the sequence of columns setup here.
                                </p>
                                <div class="form-horizontal">
                                    <div class="form-group">
                                        <label class="control-label col-md-3">&nbsp; </label>
                                        <div class="col-md-6">
                                            <div id="nestable_list_1" class="dd">
                                                <ol class="dd-list" style="padding:0;">

                                                    <asp:Literal ID="ltcolumn" runat="server"></asp:Literal>
                                                    <%-- <li  class="dd-item" >
                                                        <div class="dd-handle">Column 1</div>
                                                    </li>--%>
                                                </ol>
                                            </div>

                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="control-label col-md-3">&nbsp; </label>
                                        <div class="col-md-6">
                                            <div class="form-control-static">
                                                <button id="mapping-btn-back" class="btn btn-orang" type="button" onclick="backnext3();">Back</button>
                                                &nbsp;&nbsp; &nbsp;&nbsp;
                          <asp:Button ID="generate_btn_next" runat="server" CssClass="btn btn-orang" Text="Next" OnClick="generate_btn_next_Click" OnClientClick="getdisplayorderlist();" />
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="panel fa-border" style="display: none;" id="generate-probox">
                            <header class="panel-heading">Generate Feed Template For <strong><asp:Literal ID="ltrstorename2" runat="server"></asp:Literal></strong> </header>
                            <div class="panel-body">
                                <p class="help-block m-bot20">Specify file name and format for the inventory feed you are about to generate.Please keep in mind that the feed template name specified here will over-ride any previously generated templates for this Channel Partner.</p>
                                <div class="form-horizontal">
                                    <div class="form-group">
                                        <label class="col-md-3 control-label">File Name</label>
                                        <div class="col-md-3">
                                            <div class="form-control-static">

                                                <asp:TextBox ID="txtfilename" runat="server" CssClass="form-control" onchange="checkfilename();"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="col-md-3 control-label">Inventory Feed File Format</label>
                                        <div class="col-md-6 icheck minimal">
                                            <div class="row">
                                                <label class="checkbox-inline">
                                                    <div class="row">
                                                        <div class="radio single-row">

                                                            <asp:RadioButton ID="rdocsv" runat="server" GroupName="demo-radio" />
                                                            <label>CSV</label>
                                                        </div>
                                                    </div>
                                                </label>
                                                <label class="checkbox-inline">
                                                    <div class="row">
                                                        <div class="radio single-row">

                                                            <asp:RadioButton ID="rdoexcel" runat="server" GroupName="demo-radio" />
                                                            <label>Excel </label>
                                                        </div>
                                                    </div>
                                                </label>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="col-md-3 control-label">&nbsp;</label>
                                        <div class="col-md-6">

                                            <asp:Button ID="btngeneratefeed" runat="server" CssClass="btn btn-orang" Text="Generate Feed Template" Visible="false" OnClick="btngeneratefeed_Click" OnClientClick="if(document.getElementById('ContentPlaceHolder1_btndownloadnow') != null){document.getElementById('ContentPlaceHolder1_btndownloadnow').style.display='none';}" />
                                             <asp:Button runat="server" CssClass="btn btn-orang" ID="btndownloadnow" Visible="false" Text="Download Now" OnClick="btndownloadnow_Click" />
                                        </div>
                                    </div>
                                    
                                   <div class="form-group">
                                <label class="col-md-3 control-label">&nbsp;</label>
                              
                            </div>
                                    <div class="form-group">
                                        <label class="control-label col-md-3">&nbsp; </label>
                                        <div class="col-md-9">
                                            <div class="form-control-static">
                                                
                                                <asp:Button ID="final_btn_back" runat="server" Text="Back" CssClass="btn btn-orang" OnClientClick="backnext4(); return false;" />
                                                &nbsp;&nbsp; &nbsp;&nbsp;
                                                <asp:Button ID="btnsave" runat="server" Text="Save" CssClass="btn btn-orang" OnClick="btnsave_Click" OnClientClick="return checkfileformat()" />
                                                <asp:Button ID="btndone" Visible="false" runat="server" CssClass="btn btn-orang" Text="Done" OnClick="btndone_Click"  />
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div style="display: none;" id="divcolumndata" runat="server">
    </div>
    <div style="display: none;">
        <input type="hidden" id="hdndisplayorderlist" value="" runat="server" />
         <input type="hidden" id="hdnallcolumndata" value="" runat="server" />
    </div>
</asp:Content>
