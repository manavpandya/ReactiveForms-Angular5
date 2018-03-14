<%@ Page Title="" Language="C#" MasterPageFile="~/REPLENISHMENTMANAGEMENT/AdminReplnishment.Master" AutoEventWireup="true" CodeBehind="SetupInventoryFeedSchedular.aspx.cs" Inherits="Solution.UI.Web.REPLENISHMENTMANAGEMENT.SetupInventoryFeedSchedular" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <%--<script src="/REPLENISHMENTMANAGEMENT/js/jquery.timepicker.min.js"></script>
     <script src="/REPLENISHMENTMANAGEMENT/js/jquery.timepicker.js"></script>
    
     <link href="/REPLENISHMENTMANAGEMENT/js/jquery.timepicker.min.css" rel="stylesheet">--%>


    <script type="text/javascript" src="/REPLENISHMENTMANAGEMENT/js/jquery.min.js"></script>
    <%--<script type="text/javascript" src="http://timepicker.co/resources/bootstrap/js/bootstrap.min.js"></script>--%>
    <script src="/REPLENISHMENTMANAGEMENT/js/bootstrap.min.js"></script>
    <%--  <script type="text/javascript" src="/REPLENISHMENTMANAGEMENT/js/jquery.timepicker.min.js"></script>
        <script type="text/javascript" src="/REPLENISHMENTMANAGEMENT/js/timepicker.page.js"></script>--%>

    <link href="/REPLENISHMENTMANAGEMENT/css/bootstrap.min.css" rel="stylesheet">
    <link href="/REPLENISHMENTMANAGEMENT/js/jquery-ui/jquery-ui-1.10.1.custom.min.css" rel="stylesheet">
    <link href="/REPLENISHMENTMANAGEMENT/css/bootstrap-reset.css" rel="stylesheet">
    <link href="/REPLENISHMENTMANAGEMENT/font-awesome/css/font-awesome.css" rel="stylesheet">
    <link href="css/table-responsive.css" rel="stylesheet" />
    <!-- Custom styles for this template -->
    <link href="/REPLENISHMENTMANAGEMENT/css/style.css" rel="stylesheet">
    <link href="/REPLENISHMENTMANAGEMENT/css/style-responsive.css" rel="stylesheet" />

    <!--icheck-->
    <link href="/REPLENISHMENTMANAGEMENT/js/iCheck/skins/minimal/minimal.css" rel="stylesheet">
    <link href="/REPLENISHMENTMANAGEMENT/js/iCheck/skins/minimal/red.css" rel="stylesheet">
    <link href="/REPLENISHMENTMANAGEMENT/js/iCheck/skins/minimal/green.css" rel="stylesheet">
    <link href="/REPLENISHMENTMANAGEMENT/js/iCheck/skins/minimal/blue.css" rel="stylesheet">
    <link href="/REPLENISHMENTMANAGEMENT/js/iCheck/skins/minimal/yellow.css" rel="stylesheet">
    <link href="/REPLENISHMENTMANAGEMENT/js/iCheck/skins/minimal/purple.css" rel="stylesheet">
    <link href="/REPLENISHMENTMANAGEMENT/js/iCheck/skins/square/square.css" rel="stylesheet">
    <link href="/REPLENISHMENTMANAGEMENT/js/iCheck/skins/square/red.css" rel="stylesheet">
    <link href="/REPLENISHMENTMANAGEMENT/js/iCheck/skins/square/green.css" rel="stylesheet">
    <link href="/REPLENISHMENTMANAGEMENT/js/iCheck/skins/square/blue.css" rel="stylesheet">
    <link href="/REPLENISHMENTMANAGEMENT/js/iCheck/skins/square/yellow.css" rel="stylesheet">
    <link href="/REPLENISHMENTMANAGEMENT/js/iCheck/skins/square/purple.css" rel="stylesheet">
    <link href="/REPLENISHMENTMANAGEMENT/js/iCheck/skins/flat/grey.css" rel="stylesheet">
    <link href="/REPLENISHMENTMANAGEMENT/js/iCheck/skins/flat/red.css" rel="stylesheet">
    <link href="/REPLENISHMENTMANAGEMENT/js/iCheck/skins/flat/green.css" rel="stylesheet">
    <link href="/REPLENISHMENTMANAGEMENT/js/iCheck/skins/flat/blue.css" rel="stylesheet">
    <link href="/REPLENISHMENTMANAGEMENT/js/iCheck/skins/flat/yellow.css" rel="stylesheet">
    <link href="/REPLENISHMENTMANAGEMENT/js/iCheck/skins/flat/purple.css" rel="stylesheet">

    <style type="text/css">
        .zindexcheckbox {
            z-index: 100;
        }

        .zindexactive {
            z-index: -1;
        }
    </style>
    <script type="text/javascript">

        function validatetime(str) {
            var strval = str;
            var strval1;

            //minimum lenght is 6. example 1:2 AM
            if (strval.length < 8) {
                alert("Invalid time. Time format should be hh:mm:ss AM or PM.");
                return false;
            }

            //Maximum length is 8. example 10:45 AM
            if (strval.length > 11) {
                alert("Invalid time. Time format should be hh:mm:ss AM or PM.");
                return false;
            }

            //Removing all space
            strval = trimAllSpace(strval);

            //Checking AM/PM
            if (strval.charAt(strval.length - 1) != "M" && strval.charAt(
                strval.length - 1) != "m") {
                alert("Invalid time. Time shoule be end with AM or PM.");
                return false;

            }
            else if (strval.charAt(strval.length - 2) != 'A' && strval.charAt(
                strval.length - 2) != 'a' && strval.charAt(
                strval.length - 2) != 'p' && strval.charAt(strval.length - 2) != 'P') {
                alert("Invalid time. Time shoule be end with AM or PM.");
                return false;

            }

            //Give one space before AM/PM

            strval1 = strval.substring(0, strval.length - 2);
            strval1 = strval1 + ' ' + strval.substring(strval.length - 2, strval.length)

            strval = strval1;

            var pos1 = strval.indexOf(':');
            //document.Form1.TextBox1.value = strval;

            if (pos1 < 0) {
                alert("Invlalid time. A color(:) is missing between hour and minute.");
                return false;
            }
            else if (pos1 > 2 || pos1 < 1) {
                alert("Invalid time. Time format should be hh:mm:ss AM or PM.");
                return false;
            }

            //Checking hours
            var horval = trimString(strval.substring(0, pos1));

            if (horval == -100) {
                alert("Invalid time. Hour should contain only integer value (01-12).");
                return false;
            }
            if (horval.toString().length <= 1) {
                alert("Invalid time. Hour should contain only integer value (01-12).");
                return false;
            }
            if (horval > 12) {
                alert("Invalid time. Hour can not be greater that 12.");
                return false;
            }
            else if (horval < 0) {
                alert("Invalid time. Hour can not be hours less than 0.");
                return false;
            }
            //Completes checking hours.

            //Checking minutes.
            var minval = trimString(strval.substring(pos1 + 1, pos1 + 3).replace(':', ''));

            if (minval == -100) {
                alert("Invalid time. Minute should have only integer value (00-59).");
                return false;
            }
            if (minval.toString().length <= 1) {
                alert("Invalid time. Minute should have only integer value (00-59).");
                return false;
            }
            if (minval > 59) {
                alert("Invalid time. Minute can not be more than 59.");
                return false;
            }
            else if (minval < 0) {
                alert("Invalid time. Minute can not be less than 0.");
                return false;
            }

            //Checking minutes completed.

            //Checking seconds.
            var secval = trimString(strval.substring(pos1 + 4, pos1 + 6));

            if (secval == -100) {
                alert("Invalid time. Second should have only integer value (00-59).");
                return false;
            }
            if (secval.toString().length <= 1) {
                alert("Invalid time. Second should have only integer value (00-59).");
                return false;
            }
            if (secval > 59) {
                alert("Invalid time. Second can not be more than 59.");
                return false;
            }
            else if (secval < 0) {
                alert("Invalid time. Second can not be less than 0.");
                return false;
            }

            //Checking seconds completed.


            //Checking one space after the mintues 
            minpos = pos1 + secval.length + 4;

            if (strval.charAt(minpos) != ' ') {
                alert("Invalid time. Space missing after second. Time should have hh:mm:ss AM or PM format.");
                return false;
            }

            return true;
        }
        function trimString(str) {
            var str1 = '';
            var i = 0;
            while (i != str.length) {
                if (str.charAt(i) != ' ') str1 = str1 + str.charAt(i); i++;
            }

            var retval = IsNumeric(str1);
            if (retval == false)
                return -100;
            else
                return str1;
        }
        function trimAllSpace(str) {
            var str1 = '';
            var i = 0;
            while (i != str.length) {
                if (str.charAt(i) != ' ')
                    str1 = str1 + str.charAt(i); i++;
            }
            return str1;
        }

        function IsNumeric(strString) {
            var strValidChars = "0123456789";
            var strChar;
            var blnResult = true;
            //var strSequence = document.frmQuestionDetail.txtSequence.value; 
            //test strString consists of valid characters listed above 
            if (strString.length == 0)
                return false;
            for (i = 0; i < strString.length && blnResult == true; i++) {
                strChar = strString.charAt(i);
                if (strValidChars.indexOf(strChar) == -1) {
                    blnResult = false;
                }
            }
            return blnResult;
        }

        function checkparent(chkid) {

            var iddd = $(chkid).parent().attr('class');





            if (iddd.toString().toLowerCase() == "icheckbox_minimal") {

                $(chkid).parent().attr('class', 'icheckbox_minimal checked');


            }
            else {
                $(chkid).parent().attr('class', 'icheckbox_minimal');
            }

        }
        function removecolumn(id) {
            if (confirm('Are you sure want to remove?')) {
                var timestr = $("#txttime_" + id.toString()).val();

                $("#div_" + id.toString()).remove();
                $.ajax({
                    type: "POST",
                    url: "/REPLENISHMENTMANAGEMENT/Webmethodcalling.aspx/RemoveSedhuler",
                    data: "{timestr: '" + timestr + "',StoreId: " + document.getElementById('ContentPlaceHolder1_ddlstore').value + "}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    async: "true",
                    cache: "false",
                    success: function (msg) {
                    },
                    Error: function (x, e) {
                    }
                });
            }

            // $("#divgroup_" + id.toString()).remove();
        }

        function checkavebutton() {
            if (document.getElementById('ContentPlaceHolder1_ddlstore') != null && document.getElementById('ContentPlaceHolder1_ddlstore').selectedIndex == 0) {
                alert("Please Select Channel Partner.");
                document.getElementById('ContentPlaceHolder1_ddlstore').focus();
                return false;
            }
            var allinput = document.getElementById('maindiv').getElementsByTagName('input');

            var tt = 0;
            var tt1 = 0;
            var tt2 = 0;
            var tt3 = 0;
            var tt4 = 0;
            var tt5 = 0;
            var tt6 = 0;
            var tt7 = 0;



            var textid = '';
            var vall = '';
            var tcolumn = 0;

            if (allinput.length > 0) {
                for (var i = 0; i < allinput.length; i++) {
                    var cntrlid = allinput[i];


                    if (cntrlid.id.toString().toLowerCase().indexOf('txttime_') > -1) {
                        textid = cntrlid.id.toString();
                        vall = cntrlid.value;
                    }

                    if (cntrlid.id.toString().toLowerCase().indexOf('txttime_') > -1 && cntrlid.value.replace(/^\s*\s*$/g, '') == '') {
                        tt = 1;
                        alert('Please enter time value');
                        cntrlid.focus();
                        return false;
                    }
                    else if (cntrlid.id.toString().toLowerCase().indexOf('txttime_') > -1 && !validatetime(cntrlid.value)) {
                        tt = 1;
                        //alert('Please enter valid value');
                        cntrlid.focus();
                        return false;
                    }


                    else if (cntrlid.id.toString().toLowerCase().indexOf('chk_mon_') > -1 && cntrlid.checked == false) {
                        tt1 = 1;

                    }
                    else if (cntrlid.id.toString().toLowerCase().indexOf('chk_tues_') > -1 && cntrlid.checked == false) {
                        tt2 = 1;
                    }
                    else if (cntrlid.id.toString().toLowerCase().indexOf('chk_wed_') > -1 && cntrlid.checked == false) {
                        tt3 = 1;
                    }
                    else if (cntrlid.id.toString().toLowerCase().indexOf('chk_thur_') > -1 && cntrlid.checked == false) {
                        tt4 = 1;
                    }
                    else if (cntrlid.id.toString().toLowerCase().indexOf('chk_fri_') > -1 && cntrlid.checked == false) {
                        tt5 = 1;
                    }
                    else if (cntrlid.id.toString().toLowerCase().indexOf('chk_sat_') > -1 && cntrlid.checked == false) {
                        tt6 = 1;
                    }
                    else if (cntrlid.id.toString().toLowerCase().indexOf('chk_sun_') > -1 && cntrlid.checked == false) {

                        tt7 = 1;
                    }
                    if (cntrlid.id.toString().toLowerCase().indexOf('txttime_') > -1) {


                        for (var j = 0; j < allinput.length; j++) {

                            if (cntrlid.id.toString().toLowerCase() != allinput[j].id.toString().toLowerCase()) {

                                if ((cntrlid.value.toString().toLocaleLowerCase() == allinput[j].value.toString().toLowerCase()) && allinput[j].value.toString() != '') {

                                    alert('Scheduler already exist!');
                                    allinput[j].focus();
                                    return false;
                                }
                            }
                        }
                    }
                    if (cntrlid.id.toString().toLowerCase().indexOf('chk_sun_') > -1) {
                        if (tt2 == 1 && tt1 == 1 && tt3 == 1 && tt4 == 1 && tt5 == 1 && tt6 == 1 && tt7 == 1) {
                            alert('Please select Day.');
                            return false;

                        }
                        else {

                            tt1 = 0;
                            tt2 = 0;
                            tt3 = 0;
                            tt4 = 0;
                            tt5 = 0;
                            tt6 = 0;
                            tt7 = 0;
                        }
                    }


                }
            }
            return true;
        }
        function addmorecolumn(id) {

            if (document.getElementById('ContentPlaceHolder1_ddlstore') != null && document.getElementById('ContentPlaceHolder1_ddlstore').selectedIndex == 0) {
                alert("Please Select Channel Partner.");
                document.getElementById('ContentPlaceHolder1_ddlstore').focus();
                return false;
            }
            if (document.getElementById('ContentPlaceHolder1_divcolumndata') != null) {
                var ahtml = document.getElementById('ContentPlaceHolder1_divcolumndata').innerHTML;
                var icount = parseInt(id) + parseInt(1);
                var icountminus = parseInt(id) - parseInt(1);


                var allinput = document.getElementById('maindiv').getElementsByTagName('input');

                var tt = 0;
                var tt1 = 0;
                var tt2 = 0;
                var tt3 = 0;
                var tt4 = 0;
                var tt5 = 0;
                var tt6 = 0;
                var tt7 = 0;



                var textid = '';
                var vall = '';
                var tcolumn = 0;

                if (allinput.length > 0) {
                    for (var i = 0; i < allinput.length; i++) {
                        var cntrlid = allinput[i];


                        if (cntrlid.id.toString().toLowerCase().indexOf('txttime_') > -1) {
                            textid = cntrlid.id.toString();
                            vall = cntrlid.value;
                        }

                        if (cntrlid.id.toString().toLowerCase().indexOf('txttime_') > -1 && cntrlid.value.replace(/^\s*\s*$/g, '') == '') {
                            tt = 1;
                            alert('Please enter time value');
                            cntrlid.focus();
                            return;
                        }
                        else if (cntrlid.id.toString().toLowerCase().indexOf('txttime_') > -1 && !validatetime(cntrlid.value)) {
                            tt = 1;
                            //alert('Please enter valid value');
                            cntrlid.focus();
                            return false;
                        }

                        else if (cntrlid.id.toString().toLowerCase().indexOf('chk_mon_') > -1 && cntrlid.checked == false) {
                            tt1 = 1;

                        }
                        else if (cntrlid.id.toString().toLowerCase().indexOf('chk_tues_') > -1 && cntrlid.checked == false) {
                            tt2 = 1;
                        }
                        else if (cntrlid.id.toString().toLowerCase().indexOf('chk_wed_') > -1 && cntrlid.checked == false) {
                            tt3 = 1;
                        }
                        else if (cntrlid.id.toString().toLowerCase().indexOf('chk_thur_') > -1 && cntrlid.checked == false) {
                            tt4 = 1;
                        }
                        else if (cntrlid.id.toString().toLowerCase().indexOf('chk_fri_') > -1 && cntrlid.checked == false) {
                            tt5 = 1;
                        }
                        else if (cntrlid.id.toString().toLowerCase().indexOf('chk_sat_') > -1 && cntrlid.checked == false) {
                            tt6 = 1;
                        }
                        else if (cntrlid.id.toString().toLowerCase().indexOf('chk_sun_') > -1 && cntrlid.checked == false) {

                            tt7 = 1;
                        }
                        if (cntrlid.id.toString().toLowerCase().indexOf('txttime_') > -1) {


                            for (var j = 0; j < allinput.length; j++) {

                                if (cntrlid.id.toString().toLowerCase() != allinput[j].id.toString().toLowerCase()) {

                                    if ((cntrlid.value.toString().toLocaleLowerCase() == allinput[j].value.toString().toLowerCase()) && allinput[j].value.toString() != '') {

                                        alert('Scheduler already exist!');
                                        allinput[j].focus();
                                        return false;
                                    }
                                }
                            }
                        }


                        if (cntrlid.id.toString().toLowerCase().indexOf('chk_sun_') > -1) {
                            if (tt2 == 1 && tt1 == 1 && tt3 == 1 && tt4 == 1 && tt5 == 1 && tt6 == 1 && tt7 == 1) {
                                alert('Please select Day.');
                                return;

                            }
                            else {

                                tt1 = 0;
                                tt2 = 0;
                                tt3 = 0;
                                tt4 = 0;
                                tt5 = 0;
                                tt6 = 0;
                                tt7 = 0;
                            }
                        }


                    }
                }


                if (tt == 0 && (tt2 == 0 || tt1 == 0 || tt3 == 0 || tt4 == 0 || tt5 == 0 || tt6 == 0 || tt7 == 0)) {



                    ahtml = ahtml.replace(/##kau###/g, id.toString()).replace(/##kau_new###/g, icount.toString());
                    // alert(ahtml);
                    ahtml = ahtml.replace(/style="position: relative;/g, 'style="z-index:100; position: relative;')
                    ahtml = ahtml.replace(/name=\"chk/g, ' class="zindexcheckbox" name="chk')
                    ahtml = ahtml.replace(/iCheck-helper/g, 'iCheck-helper zindexactive')
                    $('#backbtnnext2').before(ahtml);
                    //alert(icount);
                    document.getElementById('scheduler_' + (icount - 1).toString()).innerHTML = icount;
                    $("#btnmore_" + icountminus.toString()).remove();
                    document.getElementById('btnremove_' + icountminus.toString()).style.display = '';

                }



            }
        }
    </script>

    <%-- <script type="text/javascript">
        var $k = jQuery.noConflict();
        $k(function () {

           
            $k('#txttime').datetimepicker({
                ampm: true,

                timeOnly: true,

                showSecond: true,
            });
        });

</script>--%>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="wrapper">
        <div class="row">
            <div class="col-md-12">
                <!--breadcrumbs start -->
                <ul class="breadcrumb">
                    <li>
                        <p class="hd-title">Setup Inventory Feed Scheduler</p>
                    </li>
                </ul>
                <!--breadcrumbs end -->
            </div>
        </div>
        <div class="row">
            <div class="col-md-12">
                <div class="panel">
                    <div class="panel-heading">Setup Auto Feed Scheduler</div>
                    <div class="panel-body" id="selectfile-pro">
                        <div class="form-horizontal">
                            <div class="form-group">
                                <label class="col-md-3 control-label">Select Channel Partner</label>
                                <div class="col-md-4">
                                    <asp:DropDownList runat="server" AutoPostBack="true" CssClass="form-control m-bot15" ID="ddlstore" OnSelectedIndexChanged="ddlstore_SelectedIndexChanged"></asp:DropDownList>

                                </div>
                            </div>


                            <%--   <button type="button" class="btn btn-orang" id="btnmore_##kau###" onclick="addmorecolumn(##kau_new###);" ><i class="fa fa-plus"></i>Add Scheduler</button>--%>


                            <div id="maindiv">
                                <asp:Literal ID="Literal1" runat="server"></asp:Literal>
                                <div id="backbtnnext2">
                                </div>

                            </div>
                            <asp:Literal ID="ltrgroupdata" runat="server"></asp:Literal>





                            <div class="form-group" id="divsavebtn">
                                <label class="control-label col-md-3">
                                    <%-- <button type="button" class="btn btn-orang">Cancel</button>--%>
                                </label>
                                <div class="col-md-6">
                                    <div class="form-control-static">
                                        <asp:Button ID="btncancel" runat="server" Text="Cancel" CssClass="btn btn-orang" OnClick="btncancel_Click" />&nbsp;&nbsp;<asp:Button ID="btnsave" runat="server" Text="Save" CssClass="btn btn-orang" OnClick="btnsave_Click" OnClientClick="return checkavebutton();" />
                                        <%--<button type="button" class="btn btn-orang">Save</button>--%>
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
    <script src="/REPLENISHMENTMANAGEMENT/js/jquery.js"></script>
    <script src="/REPLENISHMENTMANAGEMENT/js/jquery-ui/jquery-ui-1.10.1.custom.min.js"></script>
    <script src="/REPLENISHMENTMANAGEMENT/js/bootstrap.min.js"></script>
    <script src="/REPLENISHMENTMANAGEMENT/js/jquery.dcjqaccordion.2.7.js"></script>
    <script src="/REPLENISHMENTMANAGEMENT/js/jquery.scrollTo.min.js"></script>
    <script src="/REPLENISHMENTMANAGEMENT/js/jQuery-slimScroll-1.3.0/jquery.slimscroll.js"></script>
    <script src="/REPLENISHMENTMANAGEMENT/js/jquery.nicescroll.js"></script>
    <!--[if lte IE 8]><script language="javascript" type="text/javascript" src="js/flot-chart/excanvas.min.js"></script><![endif]-->
    <script src="/REPLENISHMENTMANAGEMENT/js/jquery.scrollTo/jquery.scrollTo.js"></script>

    <script src="/REPLENISHMENTMANAGEMENT/js/dashboard.js"></script>
    <script src="/REPLENISHMENTMANAGEMENT/js/jquery.customSelect.min.js"></script>
    <!--common script init for all pages-->
    <script src="/REPLENISHMENTMANAGEMENT/js/scripts.js"></script>
    <!--script for this page-->
    <script src="/REPLENISHMENTMANAGEMENT/js/iCheck/jquery.icheck.js"></script>
    <script src="/REPLENISHMENTMANAGEMENT/js/icheck-init.js"></script>


</asp:Content>
