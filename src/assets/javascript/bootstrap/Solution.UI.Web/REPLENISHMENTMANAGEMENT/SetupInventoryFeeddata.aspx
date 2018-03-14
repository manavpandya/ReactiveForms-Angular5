<%@ Page Title="" Language="C#" MasterPageFile="~/REPLENISHMENTMANAGEMENT/AdminReplnishment.Master" AutoEventWireup="true" CodeBehind="SetupInventoryFeeddata.aspx.cs" Inherits="Solution.UI.Web.REPLENISHMENTMANAGEMENT.SetupInventoryFeeddata" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%--    <script type="text/javascript" src="js/bootstrap-datepicker/js/bootstrap-datepicker.js"></script>
<script type="text/javascript" src="/REPLENISHMENTMANAGEMENT/js/bootstrap-datetimepicker/js/bootstrap-datetimepicker.js"></script>--%>
    <script src="/REPLENISHMENTMANAGEMENT/js/advanced-form.js"></script>
    <%--<script src="/REPLENISHMENTMANAGEMENT/js/iCheck/jquery.icheck.js"></script>--%>
    <%--<script src="/REPLENISHMENTMANAGEMENT/js/icheck-init.js"></script>--%>
    <%--<link href="/REPLENISHMENTMANAGEMENT/css/bootstrap.min.css" rel="stylesheet" />--%>
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
    <link rel="stylesheet" type="text/css" href="js/nestable/jquery.nestable.css" />
    <script type="text/javascript">
        function getallcheckboxvalue() {

            var allcheckbox = document.getElementById('ContentPlaceHolder1_divallcheckbox').getElementsByTagName('input');
            if (allcheckbox.length > 0) {
                document.getElementById('divloader').style.display = '';
                for (var i = 0; i < allcheckbox.length; i++) {

                    if (allcheckbox[i].id.toString().toLowerCase().indexOf('chk_') > -1) {
                        var skuid = allcheckbox[i].id.toString().replace('chk_', 'hdnsku_');
                        var storeid = allcheckbox[i].id.toString().replace('chk_', 'hdnstoreid_');
                        var chkchecked = 0;
                        //var ff = document.getElementById(allcheckbox[i].id.replace('chk_', 'chkdiv_')).getElementsByTagName('div')[0].className;
                        var ff = document.getElementById(allcheckbox[i].id.replace('chk_', 'chkdiv_')).className;
                        if (ff == 'icheckbox_minimal checked' || ff == 'icheckbox_minimal hover checked' || allcheckbox[i].checked == true) {
                            chkchecked = 1;
                        }
                        if (document.getElementById(skuid) != null) {
                            $.ajax({
                                type: "POST",
                                url: "/REPLENISHMENTMANAGEMENT/Webmethodcalling.aspx/GetData",
                                data: "{SKU: '" + document.getElementById(skuid).value + "',StoreId: '" + document.getElementById(storeid).value + "',IsActive: " + chkchecked + " }",
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

                    }
                }
                document.getElementById('divloader').style.display = 'none';
                alert('Record Inserted/Updated Successfully.');
            }
            return false;
        }
        function checkallcheckbox(id) {

            //document.getElementById('chkdiv_0_0').getElementsByTagName('div')[0].className = 'icheckbox_minimal checked';
            //$('div[id="chkdiv_0_0"]').attr('class', 'icheckbox_minimal checked');
            //if (document.getElementById(id).checked == true) { // check select status

            //    //$('input:checkbox[id^="' + id + '_"]').each(function () { //loop through each checkbox
            //    //    this.checked = true;  //select all checkboxes with class "checkbox1"              
            //    //});
            //    $('div[id^=chkdiv_0_]').attr('class', true);

            //} else {
            //    //$('input[id^="' + id + '_"]').each(function () { //loop through each checkbox
            //    //    this.checked = false; //deselect all checkboxes with class "checkbox1"                      
            //    //});
            //    //$('input:checkbox[id^="' + id + '_"]').attr('checked', false);
            //}
            ////$('input:checkbox[id^="' + id + '_"]')
            ////alert('test');
            //alert(document.getElementById(id.replace('chk_', 'chkdiv_')).getElementsByTagName('div')[0].className);

            if (id.toString().indexOf('chk_0_') <= -1) {
                //var ff = document.getElementById(id.replace('chk_', 'chkdiv_')).getElementsByTagName('div')[0].className;
                var ff = document.getElementById(id.replace('chk_', 'chkdiv_')).className;

                var allcheckbox = document.getElementById('ContentPlaceHolder1_divallcheckbox').getElementsByTagName('input');
                if (allcheckbox.length > 0) {

                    for (var i = 0; i < allcheckbox.length; i++) {

                        if (allcheckbox[i].id.toString().toLowerCase().indexOf(id.toLowerCase()) > -1) {

                            if (ff == 'icheckbox_minimal checked' || ff == 'icheckbox_minimal hover checked' || document.getElementById(id).checked == true) {

                                //allcheckbox[i].checked = true;

                                if (allcheckbox[i].id.replace('chk_', 'chkdiv_') != id.replace('chk_', 'chkdiv_') && allcheckbox[i].id.toString().toLowerCase().indexOf('chk_') > -1) {
                                    allcheckbox[i].checked = true;
                                    if (document.getElementById(allcheckbox[i].id.replace('chk_', 'chkdiv_'))!= null) {
                                       // document.getElementById(allcheckbox[i].id.replace('chk_', 'chkdiv_')).className = 'icheckbox_minimal checked';
                                    }
                                    // allsubchkchecked(allcheckbox[i].id);
                                }

                                //icheckbox_minimal checked
                            }
                            else {


                                if (allcheckbox[i].id.replace('chk_', 'chkdiv_') != id.replace('chk_', 'chkdiv_') && allcheckbox[i].id.toString().toLowerCase().indexOf('chk_') > -1) {
                                    allcheckbox[i].checked = false;

                                    if (document.getElementById(allcheckbox[i].id.replace('chk_', 'chkdiv_')) != null) {
                                       // document.getElementById(allcheckbox[i].id.replace('chk_', 'chkdiv_')).className = 'icheckbox_minimal';
                                    }
                                    if (document.getElementById('chkdiv_0') != null && document.getElementById('chkdiv_0') != null) {
                                       // document.getElementById('chkdiv_0').className = 'icheckbox_minimal';
                                    }
                                    //allsubchkchecked(allcheckbox[i].id);
                                }
                            }

                        }
                        else {
                            if (ff == 'icheckbox_minimal checked' || ff == 'icheckbox_minimal hover checked' || document.getElementById(id).checked == true) {

                                //allcheckbox[i].checked = true;

                                if (allcheckbox[i].id.replace('chk_', 'chkdiv_') != id.replace('chk_', 'chkdiv_') && allcheckbox[i].id.toString().toLowerCase().indexOf('chk_') > -1) {
                                    allcheckbox[i].checked = true;
                                    if (document.getElementById(allcheckbox[i].id.replace('chk_', 'chkdiv_')) != null) {
                                       // document.getElementById(allcheckbox[i].id.replace('chk_', 'chkdiv_')).className = 'icheckbox_minimal checked';
                                    }

                                }


                                //icheckbox_minimal checked
                            }
                            else {


                                if (allcheckbox[i].id.replace('chk_', 'chkdiv_') != id.replace('chk_', 'chkdiv_') && allcheckbox[i].id.toString().toLowerCase().indexOf('chk_') > -1) {
                                    allcheckbox[i].checked = false;
                                    if (document.getElementById(allcheckbox[i].id.replace('chk_', 'chkdiv_')) != null) {
                                        //document.getElementById(allcheckbox[i].id.replace('chk_', 'chkdiv_')).className = 'icheckbox_minimal';
                                    }

                                    if (document.getElementById('chkdiv_0') != null && document.getElementById('chkdiv_0') != null) {
                                        //document.getElementById('chkdiv_0').className = 'icheckbox_minimal';
                                    }

                                    //document.getElementById(allcheckbox[i].id.replace(id, 'chk_0')).getElementsByTagName('div')[0].className = 'icheckbox_minimal';

                                }
                            }
                        }
                    }

                }
            }
            else {

                allsubchkchecked(id);
            }


        }
        function checkallcheckboxchild(id, id2) {

            //document.getElementById('chkdiv_0_0').getElementsByTagName('div')[0].className = 'icheckbox_minimal checked';
            //$('div[id="chkdiv_0_0"]').attr('class', 'icheckbox_minimal checked');
            //if (document.getElementById(id).checked == true) { // check select status

            //    //$('input:checkbox[id^="' + id + '_"]').each(function () { //loop through each checkbox
            //    //    this.checked = true;  //select all checkboxes with class "checkbox1"              
            //    //});
            //    $('div[id^=chkdiv_0_]').attr('class', true);

            //} else {
            //    //$('input[id^="' + id + '_"]').each(function () { //loop through each checkbox
            //    //    this.checked = false; //deselect all checkboxes with class "checkbox1"                      
            //    //});
            //    //$('input:checkbox[id^="' + id + '_"]').attr('checked', false);
            //}
            ////$('input:checkbox[id^="' + id + '_"]')
            ////alert('test');
            //alert(document.getElementById(id.replace('chk_', 'chkdiv_')).getElementsByTagName('div')[0].className);
            if (id.toString().indexOf('chk_0_') <= -1) {
                var ff = document.getElementById(id.replace('chk_', 'chkdiv_')).className;
                if (ff == 'icheckbox_minimal checked' || ff == 'icheckbox_minimal hover checked') {
                }
                else {
                   // document.getElementById(id.replace('chk_', 'chkdiv_')).className = 'icheckbox_minimal';
                   // document.getElementById(id2.replace('chk_', 'chkdiv_')).className = 'icheckbox_minimal';
                   // document.getElementById(id.replace(id2, 'chkdiv_0')).className = 'icheckbox_minimal';
                  //  document.getElementById('chkdiv_0').className = 'icheckbox_minimal';
                }
                var allcheckbox = document.getElementById('ContentPlaceHolder1_divallcheckbox').getElementsByTagName('input');
                if (allcheckbox.length > 0) {

                    for (var i = 0; i < allcheckbox.length; i++) {

                        if (allcheckbox[i].id.toString().toLowerCase().indexOf(id.toLowerCase()) > -1) {

                            if (ff == 'icheckbox_minimal checked' || ff == 'icheckbox_minimal hover checked' || document.getElementById(id).checked == true) {

                                //allcheckbox[i].checked = true;

                                if (allcheckbox[i].id.replace('chk_', 'chkdiv_') != id.replace('chk_', 'chkdiv_') && allcheckbox[i].id.toString().toLowerCase().indexOf('chk_') > -1) {
                                    allcheckbox[i].checked = true;
                                   // document.getElementById(allcheckbox[i].id.replace('chk_', 'chkdiv_')).className = 'icheckbox_minimal checked';
                                    //allsubchkchecked(allcheckbox[i].id);
                                }

                                //icheckbox_minimal checked
                            }
                            else {


                                if (allcheckbox[i].id.replace('chk_', 'chkdiv_') != id.replace('chk_', 'chkdiv_') && allcheckbox[i].id.toString().toLowerCase().indexOf('chk_') > -1) {
                                    allcheckbox[i].checked = false;

                                   // document.getElementById(allcheckbox[i].id.replace('chk_', 'chkdiv_')).className = 'icheckbox_minimal';
                                   // document.getElementById('chkdiv_0').className = 'icheckbox_minimal';
                                    // allsubchkchecked(allcheckbox[i].id);

                                }
                            }

                        }
                        else {

                        }
                    }

                }
            }



        }
        function testtt(id) {
            //document.getElementById('chkdiv_0_0').getElementsByTagName('div')[0].className = 'icheckbox_minimal checked';
            //$('div[id="chkdiv_0_0"]').attr('class', 'icheckbox_minimal checked');
            //if (document.getElementById(id).checked == true) { // check select status

            //    //$('input:checkbox[id^="' + id + '_"]').each(function () { //loop through each checkbox
            //    //    this.checked = true;  //select all checkboxes with class "checkbox1"              
            //    //});
            //    $('div[id^=chkdiv_0_]').attr('class', true);

            //} else {
            //    //$('input[id^="' + id + '_"]').each(function () { //loop through each checkbox
            //    //    this.checked = false; //deselect all checkboxes with class "checkbox1"                      
            //    //});
            //    //$('input:checkbox[id^="' + id + '_"]').attr('checked', false);
            //}
            ////$('input:checkbox[id^="' + id + '_"]')
            ////alert('test');
            //alert(document.getElementById(id.replace('chk_', 'chkdiv_')).getElementsByTagName('div')[0].className);

            if (id.toString().indexOf('chk_0_') <= -1) {
                var ff = document.getElementById(id.replace('chk_', 'chkdiv_')).className;
                if (ff == 'icheckbox_minimal checked' || ff == 'icheckbox_minimal hover checked') {
                    //document.getElementById(id.replace('chk_', 'chkdiv_')).className = 'icheckbox_minimal checked';

                }
                else {
                    //document.getElementById(id.replace('chk_', 'chkdiv_')).className = 'icheckbox_minimal';

                }
                var allcheckbox = document.getElementById('ContentPlaceHolder1_divallcheckbox').getElementsByTagName('input');
                if (allcheckbox.length > 0) {

                    for (var i = 0; i < allcheckbox.length; i++) {

                        if (allcheckbox[i].id.toString().toLowerCase().indexOf(id.toLowerCase() + "_") > -1) {

                            if (ff == 'icheckbox_minimal checked' || ff == 'icheckbox_minimal hover checked' || document.getElementById(id).checked == true) {

                                //allcheckbox[i].checked = true;

                                if (allcheckbox[i].id.replace('chk_', 'chkdiv_') != id.replace('chk_', 'chkdiv_') && allcheckbox[i].id.toString().toLowerCase().indexOf('chk_') > -1) {
                                    allcheckbox[i].checked = true;
                                    // document.getElementById(allcheckbox[i].id.replace('chk_', 'chkdiv_')).className = 'icheckbox_minimal checked';
                                    // allsubchkchecked(allcheckbox[i].id);
                                }

                                //icheckbox_minimal checked
                            }
                            else {


                                if (allcheckbox[i].id.replace('chk_', 'chkdiv_') != id.replace('chk_', 'chkdiv_') && allcheckbox[i].id.toString().toLowerCase().indexOf('chk_') > -1) {
                                    allcheckbox[i].checked = false;

                                   // document.getElementById(allcheckbox[i].id.replace('chk_', 'chkdiv_')).className = 'icheckbox_minimal';
                                   // document.getElementById('chkdiv_0').className = 'icheckbox_minimal';

                                   // document.getElementById(allcheckbox[i].id.replace(id, 'chk_0').replace('chk_0', 'chkdiv_0')).className = 'icheckbox_minimal';

                                    //allsubchkchecked(allcheckbox[i].id);
                                }
                            }

                        }
                        else {
                            if (allcheckbox[i].id.toString().toLowerCase().indexOf('chk_0_') > -1) {

                                if (ff == 'icheckbox_minimal checked' || ff == 'icheckbox_minimal hover checked' || document.getElementById(id).checked == true) {

                                }
                                else {

                                   // document.getElementById(allcheckbox[i].id.replace('chk_', 'chkdiv_')).className = 'icheckbox_minimal';
                                    allcheckbox[i].checked = false;

                                }

                            }
                        }

                    }

                }
            }



        }

        function allsubchkchecked(id) {
            var ff = document.getElementById(id.replace('chk_', 'chkdiv_')).className;

            var alldiv = document.getElementById(id.replace('chk_0_', 'trchk_')).getElementsByTagName('div');

            if (alldiv.length > 0) {
                for (var q = 0; q < alldiv.length; q++) {
                    var cntrldivid = alldiv[q].id;

                    if (cntrldivid != undefined && cntrldivid != '') {


                        var allcheckbox = document.getElementById(cntrldivid).getElementsByTagName('input');
                        if (allcheckbox.length > 0) {
                            for (var i = 0; i < allcheckbox.length; i++) {
                                //if (allcheckbox[i].id.toString().toLowerCase().indexOf(id.toLowerCase()) > -1) {

                                if (ff == 'icheckbox_minimal checked' || ff == 'icheckbox_minimal hover checked' || document.getElementById(id).checked == true) {
                                    //if (document.getElementById(id).checked == true) {

                                    //allcheckbox[i].checked = true;

                                    if (allcheckbox[i].id.replace('chk_', 'chkdiv_') != id.replace('chk_', 'chkdiv_') && allcheckbox[i].id.toString().toLowerCase().indexOf('chk_') > -1) {
                                        allcheckbox[i].checked = true;

                                       // document.getElementById(allcheckbox[i].id.replace('chk_', 'chkdiv_')).className = 'icheckbox_minimal checked';
                                    }

                                    //icheckbox_minimal checked
                                }
                                else {

                                    if (allcheckbox[i].id.replace('chk_', 'chkdiv_') != id.replace('chk_', 'chkdiv_') && allcheckbox[i].id.toString().toLowerCase().indexOf('chk_') > -1) {
                                        allcheckbox[i].checked = false;
                                      //  document.getElementById(id.replace('chk_', 'chkdiv_')).className = 'icheckbox_minimal';
                                      //  document.getElementById('chkdiv_0').className = 'icheckbox_minimal';
                                      //  document.getElementById(allcheckbox[i].id.replace('chk_', 'chkdiv_')).className = 'icheckbox_minimal';

                                        var parentid = allcheckbox[i].id.substring(0, allcheckbox[i].id.toString().lastIndexOf('_'));
                                        if (document.getElementById(parentid) != null) {
                                            document.getElementById(parentid).checked = false;
                                        }
                                        if (document.getElementById(parentid.replace('chk_', 'chkdiv_')) != null) {
                                          //  document.getElementById(parentid.replace('chk_', 'chkdiv_')).className = 'icheckbox_minimal';
                                        }

                                    }
                                }

                            }
                            //}

                        }
                    }
                }
            }
        }
        function SearchValidation() {



            //if (document.getElementById('ContentPlaceHolder1_ddlsubcategory').selectedIndex == 0 && document.getElementById('ContentPlaceHolder1_txtfabriccode').value.replace(/^\s*\s*$/g, '') == '' && document.getElementById('ContentPlaceHolder1_txtsku_upc').value.replace(/^\s*\s*$/g, '') == '' && document.getElementById('ContentPlaceHolder1_from').value.replace(/^\s*\s*$/g, '') == '' && document.getElementById('ContentPlaceHolder1_to').value.replace(/^\s*\s*$/g, '') == '') {
            //    alert('Please Select/Enter search criteria.');
            //    document.getElementById('ContentPlaceHolder1_ddlsubcategory').focus();
            //    return false;
            //}
            if (document.getElementById('ContentPlaceHolder1_txtsku_upc').value.replace(/^\s*\s*$/g, '') == '' && document.getElementById('ContentPlaceHolder1_from').value.replace(/^\s*\s*$/g, '') == '' && document.getElementById('ContentPlaceHolder1_to').value.replace(/^\s*\s*$/g, '') == '') {
                alert('Please Select/Enter search criteria.');
                document.getElementById('ContentPlaceHolder1_txtsku_upc').focus();
                return false;
            }


            if (document.getElementById('ContentPlaceHolder1_from').value.replace(/^\s*\s*$/g, '') != '' && document.getElementById('ContentPlaceHolder1_to').value.replace(/^\s*\s*$/g, '') == '') {
                //alert('Please Enter To Date.');
                //document.getElementById('ContentPlaceHolder1_to').focus();
                //return false;
                var d = new Date();
                var curr_date = d.getDate();
                var curr_month = d.getMonth();
                curr_month = parseInt(curr_month) + parseInt(1);
                var curr_year = d.getFullYear();
                document.getElementById('ContentPlaceHolder1_to').value = curr_month + "/" + curr_date + "/" + curr_year;
            }
            else if (document.getElementById('ContentPlaceHolder1_from').value.replace(/^\s*\s*$/g, '') == '' && document.getElementById('ContentPlaceHolder1_to').value.replace(/^\s*\s*$/g, '') != '') {
                //alert('Please Enter From Date.');
                //document.getElementById('ContentPlaceHolder1_from').focus();
                //return false;
                var d = new Date();
                var curr_date = d.getDate();
                var curr_month = d.getMonth();
                curr_month = parseInt(curr_month) + parseInt(1);
                var curr_year = d.getFullYear();
                document.getElementById('ContentPlaceHolder1_from').value = curr_month + "/" + curr_date + "/" + curr_year;


            }
            if (document.getElementById('ContentPlaceHolder1_from').value.replace(/^\s*\s*$/g, '') != '' && document.getElementById('ContentPlaceHolder1_to').value.replace(/^\s*\s*$/g, '') != '') {
                var startDate = new Date(document.getElementById('ContentPlaceHolder1_from').value);
                var endDate = new Date(document.getElementById('ContentPlaceHolder1_to').value);
                if (startDate > endDate) {
                    alert('Please Select Valid Date.');
                    document.getElementById('ContentPlaceHolder1_to').focus();
                    return false;
                }
            }
            document.getElementById('divloader').style.display = '';
            return true;
        }
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="wrapper">
        <div class="row">
            <div class="col-md-12">
                <!--breadcrumbs start -->
                <ul class="breadcrumb">
                    <li>
                        <p class="hd-title">Setup Inventory Feed Data</p>
                    </li>
                </ul>
                <!--breadcrumbs end -->
            </div>
        </div>
        <div class="row">
            <div class="col-md-12">
                <div class="panel">
                    <header class="panel-heading" style="text-transform: none !important;">Select SKUs by</header>
                    <div class="panel-body" id="selectfile-pro">
                        <div class="form-horizontal">
                            <div class="form-group">
                                <label class="col-md-3 control-label">&nbsp;</label>
                                <div class="col-md-3">
                                    <asp:Label ID="lblMsg" runat="server" ForeColor="Red"></asp:Label>
                                </div>
                            </div>
                            <div class="form-group" style="display:none;">
                                <label class="col-md-3 control-label">Product Class or Group</label>
                                <div class="col-md-3">
                                    <asp:DropDownList ID="ddlsubcategory" runat="server" CssClass="form-control"></asp:DropDownList>

                                </div>
                            </div>
                            <div class="form-group">
                                <label class="col-md-3 control-label">Product SKU or UPC</label>
                                <div class="col-md-3">

                                    <asp:TextBox ID="txtsku_upc" runat="server" CssClass="form-control"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group" style="display:none;">
                                <label class="col-md-3 control-label">Fabric Code</label>
                                <div class="col-md-3">

                                    <asp:TextBox ID="txtfabriccode" runat="server" CssClass="form-control"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="col-md-3 control-label">Product Create Date Range</label>
                                <div class="col-md-3">
                                    <div class="input-group input-large" data-date="13/07/2013" data-date-format="mm/dd/yyyy">

                                        <asp:TextBox ID="from" runat="server" CssClass="form-control dpd1"></asp:TextBox>
                                        <div class="input-spacer"></div>
                                        <span class="input-group-addon" style="border: none !important;">To</span>
                                        <div class="input-spacer"></div>
                                        <asp:TextBox ID="to" runat="server" CssClass="form-control dpd2"></asp:TextBox>
                                    </div>

                                </div>
                            </div>


                            <div class="form-group">
                                <label class="col-md-3 control-label">&nbsp;</label>
                                <div class="col-md-6">

                                    <asp:Button ID="btnsubmit" runat="server" CssClass="btn btn-orang" Text="Submit" OnClick="btnsubmit_Click" OnClientClick="return SearchValidation();" />
                                </div>
                                <div class="col-md-6">
                                </div>
                            </div>
                            <div class="form-group" id="divloader" style="display: none;">
                                <label class="col-md-3 control-label">&nbsp;</label>
                                <div class="col-md-6">

                                    <img src="images/downloadloader.png" border="0" />
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="col-md-12">
                                    <div id="no-more-tables">
                                       <%-- <asp:Literal ID="ltrgrid" runat="server"></asp:Literal>--%>
                                           <div id="divallcheckbox" runat="server" style="max-width: 100%; overflow-x: auto;">
                                    </div>
                                    </div>
                                 
                                </div>
                            </div>
                            <div class="form-group" id="divbutton" runat="server" visible="false">
                                <label class="col-md-3 control-label">
                                    <button type="button" class="btn btn-orang" onclick="javascript:window.location.href='dashboard.aspx';">Cancel</button></label>
                                <div class="col-md-6">
                                    <div class="form-control-static">
                                        <asp:Button ID="btnSave" runat="server" CssClass="btn btn-orang" Text="Save" OnClick="btnSave_Click" OnClientClick="return getallcheckboxvalue();" />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
