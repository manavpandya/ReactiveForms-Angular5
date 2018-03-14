<%@ Page Title="" Language="C#" MasterPageFile="~/ADMIN/Admin.Master" AutoEventWireup="true"
    CodeBehind="ProductVariant.aspx.cs" Inherits="Solution.UI.Web.ADMIN.Products.ProductVariant" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts-main.js"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts.js"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-calender-custom.js"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-calender.js"></script>

    <script type="text/javascript" src="/js/popup.js"></script>
    <link href="/css/general.css" rel="stylesheet" type="text/css" />

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <script type="text/javascript">
        var $j = jQuery.noConflict();
        $j(function () {

            //$j('#ContentPlaceHolder1_grdoptionmainGroup_grdvaluelisting_0_grdnamevaluelisting_1_txtbackorderdate_1').datetimepicker({
            //    showButtonPanel: true, ampm: false,
            //    showHour: false, showMinute: false, showSecond: false, showTime: false
            //});

        });


        $j(function () {

            $("#<%=grdoptionmainGroup.ClientID%> tr").each(function () {

                if ($(this).find('input[id*="txtbackorderdate"]').length > 0) {

                    $j($(this).find('input[id*="txtbackorderdate"]')).datetimepicker({
                        showButtonPanel: true, ampm: false,
                        showHour: false, showMinute: false, showSecond: false, showTime: false, showOn: "button",
                        buttonImage: "/App_Themes/<%=Page.Theme %>/images/date-icon.png", buttonImageOnly: true
                    });
                }


            });




        });

    </script>

    <script type="text/javascript">

 function updateactiveoption(id)
        {
            var ids = id.replace('_chkactive', '_Imageactivebn');
             
            var gridIds = id;//.replace('_chkactive', '');
             gridIds = gridIds.substring(0, gridIds.indexOf('_chkactive'));
             var allcontrol = document.getElementById(gridIds).getElementsByTagName('input');
             var icount = 0;

             if (document.getElementById(id).checked && document.getElementById(id.replace('_chkactive', '_lttitle')) != null && document.getElementById(id.replace('_chkactive', '_lttitle')).innerHTML.toString().toLowerCase() != 'custom size')
             {
                 icount++;
             }
             
             for(var i=0;i < allcontrol.length -1; i++)
             {
                 var tt = allcontrol[i];
                 if(tt.type == 'checkbox')
                 {
                     var chkId = tt.id.replace('_chkactive', '_lttitle');
                      
                     if (document.getElementById(chkId) != null && document.getElementById(chkId).innerHTML.toString().toLowerCase() != 'custom size' && id.indexOf('_grdnamevaluelisting_') > -1 && chkId.indexOf('_grdnamevaluelisting_') > -1)
                     {
                         if(tt.checked)
                         {
                             icount++;
                         }
                     }
                     else if (document.getElementById(chkId) != null && document.getElementById(chkId).innerHTML.toString().toLowerCase() != 'custom size' && id.indexOf('_grdvaluelisting_') > -1 && chkId.indexOf('_grdnamevaluelisting_') <= -1)
                     {
                         if(tt.checked)
                         {
                             icount++;
                         }
                     }

                     

                 }
             }

            

             if (document.getElementById(id).checked == false && icount == 0) {
                 document.getElementById(id).checked = true;
             }

             if(icount == 0)
             {
                  jAlert('At least one option must be active!','Message');

             }
             else {
                  document.getElementById(ids).click();
             }

            //
        }
        function AllowandlockQty(id) {
            if (keyRestrict(window.event, '0123456789')) {
                var txtQty = id.replace(/_txtlockinventory/ig, '_hdnmainqty');

                var variantnamereminder = "";
                var origqtyreminder = "";
                var lockqtyreminder = "";
                var txtAllowQty = id.replace(/_txtlockinventory/ig, '_txtAllowinventory');

                if (document.getElementById(txtQty) != null && document.getElementById(txtQty).value != "") {
                    var originalqty = document.getElementById(txtQty).value;
                    var lockqty = document.getElementById(id).value;
                    //ContentPlaceHolder1_grdoptionmainGroup_grdvaluelisting_0_grdnamevaluelisting_1
                    if (parseInt(lockqty) > parseInt(originalqty)) {
                        jAlert('Please Lock Quantity must be less than or equal to avialable quantity.', 'Message', id);
                        document.getElementById(id).value = '';
                    }
                    else {
                        var alldiv = id.substring(0, id.indexOf('_txt'));
                        var allexist = document.getElementById(alldiv).getElementsByTagName('*');

                        for (var i = 0; i < allexist.length; i++) {
                            var elt = allexist[i];
                            if (elt.id.toString().toLowerCase().indexOf('_lttitle') > -1) {
                                if (elt.innerHTML.toString().toLowerCase().indexOf('wx') > -1) {
                                    var lblTxt = elt.innerHTML.toString().toLowerCase().substring(elt.innerHTML.toString().toLowerCase().indexOf('wx') + 2);
                                    var ids = elt.id.replace(/_lttitle_/ig, '_hdnmainqty_');
                                    var ids1 = elt.id.replace(/_lttitle_/ig, '_hdnLockInventory_');
                                    var ids2 = elt.id.replace(/_lttitle_/ig, '_hdnAddiHemingQty_');

                                    var totlamain = 0;
                                    if (document.getElementById(ids2).value != "") {
                                        totlamain = parseInt(document.getElementById(ids2).value);
                                    }
                                    totlamain = parseInt(document.getElementById(ids).value.toString()) - parseInt(totlamain);

                                    //var ids2 = elt.id.replace('_lttitle_', '_ltAllowInventory_');
                                    variantnamereminder = variantnamereminder + lblTxt.substring(0, elt.innerHTML.toString().toLowerCase().indexOf('l') - 2).replace(/ /g, '').replace(/l/g, '') + ',';
                                    //origqtyreminder = origqtyreminder + document.getElementById(ids).value.toString() + ',';
                                    origqtyreminder = origqtyreminder + totlamain.toString() + ',';
                                    lockqtyreminder = lockqtyreminder + document.getElementById(ids1).value.toString().replace(/ /g, '') + ',';

                                }
                                else if (elt.innerHTML.toString().toLowerCase().replace(/  /g, ' ').indexOf('w x') > -1) {
                                    var ids = elt.id.replace(/_lttitle_/ig, '_hdnmainqty_');
                                    var ids1 = elt.id.replace(/_lttitle_/ig, '_hdnLockInventory_');
                                    var ids2 = elt.id.replace(/_lttitle_/ig, '_hdnAddiHemingQty_');
                                    var totlamain = 0;
                                    if (document.getElementById(ids2).value != "") {
                                        totlamain = parseInt(document.getElementById(ids2).value);
                                    }
                                    totlamain = parseInt(document.getElementById(ids).value.toString()) - parseInt(totlamain);

                                    //var ids2 = elt.id.replace('_lttitle_', '_ltAllowInventory_');
                                    var lblTxt = elt.innerHTML.toString().toLowerCase().substring(elt.innerHTML.toString().toLowerCase().indexOf('w x') + 3);
                                    variantnamereminder = variantnamereminder + lblTxt.substring(0, elt.innerHTML.toString().toLowerCase().indexOf('l') - 2).replace(/ /g, '').replace(/l/g, '') + ',';
                                    //origqtyreminder = origqtyreminder + document.getElementById(ids).value.toString() + ',';
                                    origqtyreminder = origqtyreminder + totlamain.toString() + ',';

                                    lockqtyreminder = lockqtyreminder + document.getElementById(ids1).value.toString().replace(/ /g, '') + ',';
                                }
                            }
                            else if (elt.id.toString().toLowerCase().indexOf('_txttitle') > -1) {
                                if (elt.value.toString().toLowerCase().indexOf('wx') > -1) {
                                    var lblTxt = elt.value.toString().toLowerCase().substring(elt.value.toString().toLowerCase().indexOf('wx') + 2);
                                    var ids = elt.id.replace(/_txttitle/ig, '_hdnmainqty');
                                    var ids1 = elt.id.replace(/_txttitle/ig, '_txtLockinventory');
                                    var ids2 = elt.id.replace(/_lttitle_/ig, '_txtAdditionalHemingQty_');

                                    var totlamain = 0;
                                    if (document.getElementById(ids2).value != "") {
                                        totlamain = parseInt(document.getElementById(ids2).value);
                                    }
                                    totlamain = parseInt(document.getElementById(ids).value.toString()) - parseInt(totlamain);

                                    //var ids2 = elt.id.replace('_txttitle_', '_txtAllowinventory_');
                                    variantnamereminder = variantnamereminder + lblTxt.substring(0, elt.value.toString().toLowerCase().indexOf('l') - 2).replace(/ /g, '').replace(/l/g, '') + ',';

                                    // origqtyreminder = origqtyreminder + document.getElementById(ids).value.toString() + ',';
                                    origqtyreminder = origqtyreminder + totlamain.toString() + ',';
                                    if (document.getElementById(ids1).value == '') {
                                        lockqtyreminder = lockqtyreminder + '0,';
                                    }
                                    else {
                                        lockqtyreminder = lockqtyreminder + document.getElementById(ids1).value.toString() + ',';
                                    }
                                }
                                else if (elt.value.toString().toLowerCase().indexOf('w x') > -1) {
                                    var lblTxt = elt.value.toString().toLowerCase().substring(elt.value.toString().toLowerCase().indexOf('w x') + 3);
                                    variantnamereminder = variantnamereminder + lblTxt.substring(0, elt.value.toString().toLowerCase().indexOf('l') - 2).replace(/ /g, '').replace(/l/g, '') + ',';
                                    var ids = elt.id.replace(/_txttitle/ig, '_hdnmainqty');
                                    var ids1 = elt.id.replace(/_txttitle/ig, '_txtLockinventory');

                                    var ids2 = elt.id.replace(/_lttitle_/ig, '_txtAdditionalHemingQty_');

                                    var totlamain = 0;
                                    if (document.getElementById(ids2).value != "") {
                                        totlamain = parseInt(document.getElementById(ids2).value);
                                    }
                                    totlamain = parseInt(document.getElementById(ids).value.toString()) - parseInt(totlamain);

                                    //origqtyreminder = origqtyreminder + document.getElementById(ids).value.toString() + ',';
                                    origqtyreminder = origqtyreminder + totlamain.toString() + ',';
                                    if (document.getElementById(ids1).value == '') {
                                        lockqtyreminder = lockqtyreminder + '0,';
                                    }
                                    else {
                                        lockqtyreminder = lockqtyreminder + document.getElementById(ids1).value.toString() + ',';
                                    }
                                    //var ids2 = elt.id.replace('_txttitle_', '_txtAllowinventory_');
                                }

                            }
                        }


                        var alldivNew = id.substring(0, id.indexOf('_txt'));
                        var allexistNew = document.getElementById(alldivNew).getElementsByTagName('*');

                        var Arraynm = variantnamereminder.split(",");
                        var Arrayorig = origqtyreminder.split(",");
                        var Arraylock = lockqtyreminder.split(",");
                        var iLock = 0;
                        var iAllow = 0;
                        var iorig = 0;
                        for (var i = 0; i < allexistNew.length; i++) {
                            var elt = allexistNew[i];
                            if (elt.id.toString().toLowerCase().indexOf('_lttitle') > -1) {
                                if (elt.innerHTML.toString().toLowerCase().indexOf('wx') > -1) {
                                    var lblTxt = elt.innerHTML.toString().toLowerCase().substring(elt.innerHTML.toString().toLowerCase().indexOf('wx') + 2);

                                    var vvv = lblTxt.substring(0, elt.innerHTML.toString().toLowerCase().indexOf('l') - 2).replace(/ /g, '').replace(/l/g, '');
                                    var iLock = 0;
                                    var iAllow = 0;
                                    var iorig = 0;
                                    for (var k = 0; k < Arraynm.length; k++) {

                                        if (parseInt(Arraynm[k]) > parseInt(vvv)) {
                                            iAllow = parseInt(iAllow) + (parseInt(Arrayorig[k]) - parseInt(Arraylock[k]))

                                        }
                                        else if (parseInt(Arraynm[k]) == parseInt(vvv)) {
                                            iAllow = parseInt(iAllow) + (parseInt(Arrayorig[k]))

                                        }
                                    }
                                    var txtAllowupdate = elt.id.replace(/_lttitle/ig, '_ltAllowInventory');
                                    var txtAllowupdate1 = elt.id.replace(/_lttitle/ig, '_hdnAllowInventory');
                                    document.getElementById(txtAllowupdate1).value = iAllow;
                                    document.getElementById(txtAllowupdate).innerHTML = iAllow;

                                }
                                else if (elt.innerHTML.toString().toLowerCase().replace(/  /g, ' ').indexOf('w x') > -1) {
                                    var lblTxt = elt.innerHTML.toString().toLowerCase().substring(elt.innerHTML.toString().toLowerCase().indexOf('w x') + 3);
                                    var vvv = lblTxt.substring(0, elt.innerHTML.toString().toLowerCase().indexOf('l')).replace(/ /g, '').replace(/l/g, '');
                                    var iLock = 0;
                                    var iAllow = 0;
                                    var iorig = 0;
                                    for (var k = 0; k < Arraynm.length; k++) {
                                        if (parseInt(Arraynm[k]) > parseInt(vvv)) {
                                            iAllow = parseInt(iAllow) + (parseInt(Arrayorig[k]) - parseInt(Arraylock[k]))

                                        }
                                        else if (parseInt(Arraynm[k]) == parseInt(vvv)) {
                                            iAllow = parseInt(iAllow) + (parseInt(Arrayorig[k]))

                                        }
                                    }
                                    var txtAllowupdate = elt.id.replace(/_lttitle/ig, '_ltAllowInventory');
                                    var txtAllowupdate1 = elt.id.replace(/_lttitle/ig, '_hdnAllowInventory');
                                    document.getElementById(txtAllowupdate1).value = iAllow;
                                    document.getElementById(txtAllowupdate).innerHTML = iAllow;
                                }
                            }
                            else if (elt.id.toString().toLowerCase().indexOf('_txttitle') > -1) {
                                if (elt.value.toString().toLowerCase().indexOf('wx') > -1) {
                                    var lblTxt = elt.value.toString().toLowerCase().substring(elt.value.toString().toLowerCase().indexOf('wx') + 2);
                                    var vvv = lblTxt.substring(0, elt.value.toString().toLowerCase().indexOf('l')).replace(/ /g, '').replace(/l/g, '');
                                    var iLock = 0;
                                    var iAllow = 0;
                                    var iorig = 0;
                                    for (var k = 0; k < Arraynm.length; k++) {
                                        if (parseInt(Arraynm[k]) > parseInt(vvv)) {
                                            iAllow = parseInt(iAllow) + (parseInt(Arrayorig[k]) - parseInt(Arraylock[k]))

                                        }
                                        else if (parseInt(Arraynm[k]) == parseInt(vvv)) {
                                            iAllow = parseInt(iAllow) + (parseInt(Arrayorig[k]))

                                        }
                                    }
                                    var txtAllowupdate = elt.id.replace(/_txttitle/ig, '_txtAllowinventory');

                                    document.getElementById(txtAllowupdate).value = iAllow;
                                }
                                else if (elt.value.toString().toLowerCase().indexOf('w x') > -1) {
                                    var lblTxt = elt.value.toString().toLowerCase().substring(elt.value.toString().toLowerCase().indexOf('w x') + 3);
                                    var vvv = lblTxt.substring(0, elt.value.toString().toLowerCase().indexOf('l')).replace(/ /g, '').replace(/l/g, '');
                                    var iLock = 0;
                                    var iAllow = 0;
                                    var iorig = 0;
                                    for (var k = 0; k < Arraynm.length; k++) {
                                        if (parseInt(Arraynm[k]) > parseInt(vvv)) {
                                            iAllow = parseInt(iAllow) + (parseInt(Arrayorig[k]) - parseInt(Arraylock[k]))

                                        }
                                        else if (parseInt(Arraynm[k]) == parseInt(vvv)) {
                                            iAllow = parseInt(iAllow) + (parseInt(Arrayorig[k]))

                                        }
                                    }
                                    var txtAllowupdate = elt.id.replace(/_txttitle/ig, '_txtAllowinventory');
                                    document.getElementById(txtAllowupdate).value = iAllow;
                                }

                            }


                        }


                    }
                }
            }

        }
        function AllowandlockQtyVariant(id) {

            var txtQty = id.replace(/_txtlockinventory/ig, '_hdnmainqty');

            var variantnamereminder = "";
            var origqtyreminder = "";
            var lockqtyreminder = "";
            var txtAllowQty = id.replace(/_txtlockinventory/ig, '_txtAllowinventory');

            if (document.getElementById(txtQty) != null && document.getElementById(txtQty).value != "") {
                var originalqty = document.getElementById(txtQty).value;
                //var lockqty = document.getElementById(id).value;
                //ContentPlaceHolder1_grdoptionmainGroup_grdvaluelisting_0_grdnamevaluelisting_1

                var alldiv = id.substring(0, id.indexOf('_txt'));
                var allexist = document.getElementById(alldiv).getElementsByTagName('*');

                for (var i = 0; i < allexist.length; i++) {
                    var elt = allexist[i];
                    if (elt.id.toString().toLowerCase().indexOf('_lttitle') > -1) {
                        if (elt.innerHTML.toString().toLowerCase().indexOf('wx') > -1) {
                            var lblTxt = elt.innerHTML.toString().toLowerCase().substring(elt.innerHTML.toString().toLowerCase().indexOf('wx') + 2);
                            var ids = elt.id.replace(/_lttitle_/ig, '_hdnmainqty_');
                            var ids1 = elt.id.replace(/_lttitle_/ig, '_hdnLockInventory_');

                            var ids2 = elt.id.replace(/_lttitle_/ig, '_hdnAddiHemingQty_');

                            var totlamain = 0;
                            if (document.getElementById(ids2).value != "") {
                                totlamain = parseInt(document.getElementById(ids2).value);
                            }
                            totlamain = parseInt(document.getElementById(ids).value.toString()) - parseInt(totlamain);
                            //var ids2 = elt.id.replace('_lttitle_', '_ltAllowInventory_');
                            variantnamereminder = variantnamereminder + lblTxt.substring(0, elt.innerHTML.toString().toLowerCase().indexOf('l') - 2).replace(/ /g, '').replace(/l/g, '') + ',';
                            if (document.getElementById(ids) != null) {

                                origqtyreminder = origqtyreminder + totlamain.toString() + ',';
                                // origqtyreminder = origqtyreminder + document.getElementById(ids).value.toString() + ',';
                            }
                            if (document.getElementById(ids1) != null) {
                                lockqtyreminder = lockqtyreminder + document.getElementById(ids1).value.toString().replace(/ /g, '') + ',';
                            }
                        }
                        else if (elt.innerHTML.toString().toLowerCase().replace(/  /g, ' ').indexOf('w x') > -1) {
                            var ids = elt.id.replace(/_lttitle_/ig, '_hdnmainqty_');
                            var ids1 = elt.id.replace(/_lttitle_/ig, '_hdnLockInventory_');

                            var ids2 = elt.id.replace(/_lttitle_/ig, '_hdnAddiHemingQty_');

                            var totlamain = 0;
                            if (document.getElementById(ids2).value != "") {
                                totlamain = parseInt(document.getElementById(ids2).value);
                            }
                            totlamain = parseInt(document.getElementById(ids).value.toString()) - parseInt(totlamain);
                            //var ids2 = elt.id.replace('_lttitle_', '_ltAllowInventory_');
                            var lblTxt = elt.innerHTML.toString().toLowerCase().substring(elt.innerHTML.toString().toLowerCase().indexOf('w x') + 3);
                            variantnamereminder = variantnamereminder + lblTxt.substring(0, elt.innerHTML.toString().toLowerCase().indexOf('l') - 2).replace(/ /g, '').replace(/l/g, '') + ',';
                            if (document.getElementById(ids) != null) {
                                //origqtyreminder = origqtyreminder + document.getElementById(ids).value.toString() + ',';
                                origqtyreminder = origqtyreminder + totlamain.toString() + ',';
                            }
                            if (document.getElementById(ids1) != null) {
                                lockqtyreminder = lockqtyreminder + document.getElementById(ids1).value.toString().replace(/ /g, '') + ',';
                            }
                        }
                    }
                    else if (elt.id.toString().toLowerCase().indexOf('_txttitle') > -1) {
                        if (elt.value.toString().toLowerCase().indexOf('wx') > -1) {
                            var lblTxt = elt.value.toString().toLowerCase().substring(elt.value.toString().toLowerCase().indexOf('wx') + 2);
                            var ids = elt.id.replace(/_txttitle/ig, '_hdnmainqty');
                            var ids1 = elt.id.replace(/_txttitle/ig, '_txtLockinventory');

                            var ids2 = elt.id.replace(/_lttitle_/ig, '_txtAdditionalHemingQty_');
                            var totlamain = 0;
                            if (document.getElementById(ids2).value != "") {
                                totlamain = parseInt(document.getElementById(ids2).value);
                            }
                            totlamain = parseInt(document.getElementById(ids).value.toString()) - parseInt(totlamain);
                            //var ids2 = elt.id.replace('_txttitle_', '_txtAllowinventory_');
                            variantnamereminder = variantnamereminder + lblTxt.substring(0, elt.value.toString().toLowerCase().indexOf('l') - 2).replace(/ /g, '').replace(/l/g, '') + ',';
                            origqtyreminder = origqtyreminder + totlamain.toString() + ',';
                            //origqtyreminder = origqtyreminder + document.getElementById(ids).value.toString() + ',';
                            if (document.getElementById(ids1).value == '') {
                                lockqtyreminder = lockqtyreminder + '0,';
                            }
                            else {
                                lockqtyreminder = lockqtyreminder + document.getElementById(ids1).value.toString() + ',';
                            }
                        }
                        else if (elt.value.toString().toLowerCase().indexOf('w x') > -1) {
                            var lblTxt = elt.value.toString().toLowerCase().substring(elt.value.toString().toLowerCase().indexOf('w x') + 3);
                            variantnamereminder = variantnamereminder + lblTxt.substring(0, elt.value.toString().toLowerCase().indexOf('l') - 2).replace(/ /g, '').replace(/l/g, '') + ',';
                            var ids = elt.id.replace(/_txttitle/ig, '_hdnmainqty');
                            var ids1 = elt.id.replace(/_txttitle/ig, '_txtLockinventory');

                            var ids2 = elt.id.replace(/_lttitle_/ig, '_txtAdditionalHemingQty_');
                            var totlamain = 0;
                            if (document.getElementById(ids2).value != "") {
                                totlamain = parseInt(document.getElementById(ids2).value);
                            }
                            totlamain = parseInt(document.getElementById(ids).value.toString()) - parseInt(totlamain);

                            origqtyreminder = origqtyreminder + totlamain.toString() + ',';
                            //origqtyreminder = origqtyreminder + document.getElementById(ids).value.toString() + ',';
                            if (document.getElementById(ids1).value == '') {
                                lockqtyreminder = lockqtyreminder + '0,';
                            }
                            else {
                                lockqtyreminder = lockqtyreminder + document.getElementById(ids1).value.toString() + ',';
                            }
                            //var ids2 = elt.id.replace('_txttitle_', '_txtAllowinventory_');
                        }

                    }
                }


                var alldivNew = id.substring(0, id.indexOf('_txt'));
                var allexistNew = document.getElementById(alldivNew).getElementsByTagName('*');

                var Arraynm = variantnamereminder.split(",");
                var Arrayorig = origqtyreminder.split(",");
                var Arraylock = lockqtyreminder.split(",");
                var iLock = 0;
                var iAllow = 0;
                var iorig = 0;
                for (var i = 0; i < allexistNew.length; i++) {
                    var elt = allexistNew[i];
                    if (elt.id.toString().toLowerCase().indexOf('_lttitle') > -1) {
                        if (elt.innerHTML.toString().toLowerCase().indexOf('wx') > -1) {
                            var lblTxt = elt.innerHTML.toString().toLowerCase().substring(elt.innerHTML.toString().toLowerCase().indexOf('wx') + 2);

                            var vvv = lblTxt.substring(0, elt.innerHTML.toString().toLowerCase().indexOf('l') - 2).replace(/ /g, '').replace(/l/g, '');
                            var iLock = 0;
                            var iAllow = 0;
                            var iorig = 0;
                            for (var k = 0; k < Arraynm.length; k++) {

                                if (parseInt(Arraynm[k]) > parseInt(vvv)) {
                                    iAllow = parseInt(iAllow) + (parseInt(Arrayorig[k]) - parseInt(Arraylock[k]))

                                }
                                else if (parseInt(Arraynm[k]) == parseInt(vvv)) {
                                    iAllow = parseInt(iAllow) + (parseInt(Arrayorig[k]))

                                }
                            }
                            var txtAllowupdate = elt.id.replace(/_lttitle/ig, '_ltAllowInventory');
                            var txtAllowupdate1 = elt.id.replace(/_lttitle/ig, '_hdnAllowInventory');
                            document.getElementById(txtAllowupdate1).value = iAllow;
                            document.getElementById(txtAllowupdate).innerHTML = iAllow;

                        }
                        else if (elt.innerHTML.toString().toLowerCase().replace(/  /g, ' ').indexOf('w x') > -1) {
                            var lblTxt = elt.innerHTML.toString().toLowerCase().substring(elt.innerHTML.toString().toLowerCase().indexOf('w x') + 3);
                            var vvv = lblTxt.substring(0, elt.innerHTML.toString().toLowerCase().indexOf('l')).replace(/ /g, '').replace(/l/g, '');
                            var iLock = 0;
                            var iAllow = 0;
                            var iorig = 0;
                            for (var k = 0; k < Arraynm.length; k++) {
                                if (parseInt(Arraynm[k]) > parseInt(vvv)) {
                                    iAllow = parseInt(iAllow) + (parseInt(Arrayorig[k]) - parseInt(Arraylock[k]))

                                }
                                else if (parseInt(Arraynm[k]) == parseInt(vvv)) {
                                    iAllow = parseInt(iAllow) + (parseInt(Arrayorig[k]))

                                }
                            }
                            var txtAllowupdate = elt.id.replace(/_lttitle/ig, '_ltAllowInventory');
                            var txtAllowupdate1 = elt.id.replace(/_lttitle/ig, '_hdnAllowInventory');
                            document.getElementById(txtAllowupdate1).value = iAllow;
                            document.getElementById(txtAllowupdate).innerHTML = iAllow;
                        }
                    }
                    else if (elt.id.toString().toLowerCase().indexOf('_txttitle') > -1) {
                        if (elt.value.toString().toLowerCase().indexOf('wx') > -1) {
                            var lblTxt = elt.value.toString().toLowerCase().substring(elt.value.toString().toLowerCase().indexOf('wx') + 2);
                            var vvv = lblTxt.substring(0, elt.value.toString().toLowerCase().indexOf('l')).replace(/ /g, '').replace(/l/g, '');
                            var iLock = 0;
                            var iAllow = 0;
                            var iorig = 0;
                            for (var k = 0; k < Arraynm.length; k++) {
                                if (parseInt(Arraynm[k]) > parseInt(vvv)) {
                                    iAllow = parseInt(iAllow) + (parseInt(Arrayorig[k]) - parseInt(Arraylock[k]))

                                }
                                else if (parseInt(Arraynm[k]) == parseInt(vvv)) {
                                    iAllow = parseInt(iAllow) + (parseInt(Arrayorig[k]))

                                }
                            }
                            var txtAllowupdate = elt.id.replace(/_txttitle/ig, '_txtAllowinventory');

                            document.getElementById(txtAllowupdate).value = iAllow;
                        }
                        else if (elt.value.toString().toLowerCase().indexOf('w x') > -1) {
                            var lblTxt = elt.value.toString().toLowerCase().substring(elt.value.toString().toLowerCase().indexOf('w x') + 3);
                            var vvv = lblTxt.substring(0, elt.value.toString().toLowerCase().indexOf('l')).replace(/ /g, '').replace(/l/g, '');
                            var iLock = 0;
                            var iAllow = 0;
                            var iorig = 0;
                            for (var k = 0; k < Arraynm.length; k++) {
                                if (parseInt(Arraynm[k]) > parseInt(vvv)) {
                                    iAllow = parseInt(iAllow) + (parseInt(Arrayorig[k]) - parseInt(Arraylock[k]))

                                }
                                else if (parseInt(Arraynm[k]) == parseInt(vvv)) {
                                    iAllow = parseInt(iAllow) + (parseInt(Arrayorig[k]))

                                }
                            }
                            var txtAllowupdate = elt.id.replace(/_txttitle/ig, '_txtAllowinventory');
                            document.getElementById(txtAllowupdate).value = iAllow;
                        }

                    }


                }

            }


        }
    </script>
    <script type="text/javascript">

        function AllowandlockQtysave(id) {

            var txtQty = id.replace(/_txtlockinventory/ig, '_hdnmainqty');
            var variantnamereminder = "";
            var origqtyreminder = "";
            var lockqtyreminder = "";
            var txtAllowQty = id.replace(/_txtlockinventory/ig, '_txtAllowinventory');
            if (document.getElementById(txtQty) != null && document.getElementById(txtQty).value != "") {
                var originalqty = document.getElementById(txtQty).value;
                var lockqty = document.getElementById(id).value;
                //ContentPlaceHolder1_grdoptionmainGroup_grdvaluelisting_0_grdnamevaluelisting_1
                if (parseInt(lockqty) > parseInt(originalqty)) {
                    jAlert('Please Lock Quantity must be less than or equal to avialable quantity.', 'Message', id);
                    document.getElementById(id).value = '';
                    return false;
                }
                else {
                    var alldiv = id.substring(0, id.indexOf('_txt'));
                    var allexist = document.getElementById(alldiv).getElementsByTagName('*');

                    for (var i = 0; i < allexist.length; i++) {
                        var elt = allexist[i];
                        if (elt.id.toString().toLowerCase().indexOf('_lttitle') > -1) {
                            if (elt.innerHTML.toString().toLowerCase().indexOf('wx') > -1) {
                                var lblTxt = elt.innerHTML.toString().toLowerCase().substring(elt.innerHTML.toString().toLowerCase().indexOf('wx') + 2);
                                var ids = elt.id.replace(/_lttitle_/ig, '_hdnmainqty_');
                                var ids1 = elt.id.replace(/_lttitle_/ig, '_hdnLockInventory_');
                                //var ids2 = elt.id.replace('_lttitle_', '_ltAllowInventory_');
                                variantnamereminder = variantnamereminder + lblTxt.substring(0, elt.innerHTML.toString().toLowerCase().indexOf('l') - 2).replace(/ /g, '').replace(/l/g, '') + ',';
                                if (document.getElementById(ids) != null) {
                                    origqtyreminder = origqtyreminder + document.getElementById(ids).value.toString() + ',';
                                }
                                if (document.getElementById(ids1) != null) {
                                    lockqtyreminder = lockqtyreminder + document.getElementById(ids1).value.toString().replace(/ /g, '') + ',';
                                }

                            }
                            else if (elt.innerHTML.toString().toLowerCase().replace(/  /g, ' ').indexOf('w x') > -1) {
                                var ids = elt.id.replace(/_lttitle_/ig, '_hdnmainqty_');
                                var ids1 = elt.id.replace(/_lttitle_/ig, '_hdnLockInventory_');
                                //var ids2 = elt.id.replace('_lttitle_', '_ltAllowInventory_');
                                var lblTxt = elt.innerHTML.toString().toLowerCase().substring(elt.innerHTML.toString().toLowerCase().indexOf('w x') + 3);
                                variantnamereminder = variantnamereminder + lblTxt.substring(0, elt.innerHTML.toString().toLowerCase().indexOf('l') - 2).replace(/ /g, '').replace(/l/g, '') + ',';
                                if (document.getElementById(ids) != null) {
                                    origqtyreminder = origqtyreminder + document.getElementById(ids).value.toString() + ',';
                                }
                                if (document.getElementById(ids1) != null) {
                                    lockqtyreminder = lockqtyreminder + document.getElementById(ids1).value.toString().replace(/ /g, '') + ',';
                                }
                            }
                        }
                        else if (elt.id.toString().toLowerCase().indexOf('_txttitle') > -1) {
                            if (elt.value.toString().toLowerCase().indexOf('wx') > -1) {
                                var lblTxt = elt.value.toString().toLowerCase().substring(elt.value.toString().toLowerCase().indexOf('wx') + 2);
                                var ids = elt.id.replace(/_txttitle/ig, '_hdnmainqty');
                                var ids1 = elt.id.replace(/_txttitle/ig, '_txtLockinventory');
                                //var ids2 = elt.id.replace('_txttitle_', '_txtAllowinventory_');
                                variantnamereminder = variantnamereminder + lblTxt.substring(0, elt.value.toString().toLowerCase().indexOf('l') - 2).replace(/ /g, '').replace(/l/g, '') + ',';
                                if (document.getElementById(ids) != null) {
                                    origqtyreminder = origqtyreminder + document.getElementById(ids).value.toString() + ',';
                                }
                                if (document.getElementById(ids1) != null) {
                                    if (document.getElementById(ids1).value == '') {
                                        lockqtyreminder = lockqtyreminder + '0,';
                                    }
                                    else {
                                        lockqtyreminder = lockqtyreminder + document.getElementById(ids1).value.toString() + ',';
                                    }
                                }
                            }
                            else if (elt.value.toString().toLowerCase().indexOf('w x') > -1) {
                                var lblTxt = elt.value.toString().toLowerCase().substring(elt.value.toString().toLowerCase().indexOf('w x') + 3);
                                variantnamereminder = variantnamereminder + lblTxt.substring(0, elt.value.toString().toLowerCase().indexOf('l') - 2).replace(/ /g, '').replace(/l/g, '') + ',';
                                var ids = elt.id.replace(/_txttitle/ig, '_hdnmainqty');
                                var ids1 = elt.id.replace(/_txttitle/ig, '_txtLockinventory');
                                if (document.getElementById(ids) != null) {
                                    origqtyreminder = origqtyreminder + document.getElementById(ids).value.toString() + ',';
                                }
                                if (document.getElementById(ids1) != null) {
                                    if (document.getElementById(ids1).value == '') {
                                        lockqtyreminder = lockqtyreminder + '0,';
                                    }
                                    else {
                                        lockqtyreminder = lockqtyreminder + document.getElementById(ids1).value.toString() + ',';
                                    }
                                }
                                //var ids2 = elt.id.replace('_txttitle_', '_txtAllowinventory_');
                            }

                        }
                    }


                    var alldivNew = id.substring(0, id.indexOf('_txt'));
                    var allexistNew = document.getElementById(alldivNew).getElementsByTagName('*');

                    var Arraynm = variantnamereminder.split(",");
                    var Arrayorig = origqtyreminder.split(",");
                    var Arraylock = lockqtyreminder.split(",");
                    var iLock = 0;
                    var iAllow = 0;
                    var iorig = 0;
                    for (var i = 0; i < allexistNew.length; i++) {
                        var elt = allexistNew[i];
                        if (elt.id.toString().toLowerCase().indexOf('_lttitle') > -1) {
                            if (elt.innerHTML.toString().toLowerCase().indexOf('wx') > -1) {
                                var lblTxt = elt.innerHTML.toString().toLowerCase().substring(elt.innerHTML.toString().toLowerCase().indexOf('wx') + 2);

                                var vvv = lblTxt.substring(0, elt.innerHTML.toString().toLowerCase().indexOf('l') - 2).replace(/ /g, '').replace(/l/g, '');
                                var iLock = 0;
                                var iAllow = 0;
                                var iorig = 0;
                                for (var k = 0; k < Arraynm.length; k++) {

                                    if (parseInt(Arraynm[k]) > parseInt(vvv)) {
                                        iAllow = parseInt(iAllow) + (parseInt(Arrayorig[k]) - parseInt(Arraylock[k]))

                                    }
                                    else if (parseInt(Arraynm[k]) == parseInt(vvv)) {
                                        iAllow = parseInt(iAllow) + (parseInt(Arrayorig[k]))

                                    }
                                }
                                var txtAllowupdate = elt.id.replace(/_lttitle/ig, '_ltAllowInventory');
                                var txtAllowupdate1 = elt.id.replace(/_lttitle/ig, '_hdnAllowInventory');
                                document.getElementById(txtAllowupdate1).value = iAllow;
                                document.getElementById(txtAllowupdate).innerHTML = iAllow;

                            }
                            else if (elt.innerHTML.toString().toLowerCase().replace(/  /g, ' ').indexOf('w x') > -1) {
                                var lblTxt = elt.innerHTML.toString().toLowerCase().substring(elt.innerHTML.toString().toLowerCase().indexOf('w x') + 3);
                                var vvv = lblTxt.substring(0, elt.innerHTML.toString().toLowerCase().indexOf('l')).replace(/ /g, '').replace(/l/g, '');
                                var iLock = 0;
                                var iAllow = 0;
                                var iorig = 0;
                                for (var k = 0; k < Arraynm.length; k++) {
                                    if (parseInt(Arraynm[k]) > parseInt(vvv)) {
                                        iAllow = parseInt(iAllow) + (parseInt(Arrayorig[k]) - parseInt(Arraylock[k]))

                                    }
                                    else if (parseInt(Arraynm[k]) == parseInt(vvv)) {
                                        iAllow = parseInt(iAllow) + (parseInt(Arrayorig[k]))

                                    }
                                }
                                var txtAllowupdate = elt.id.replace(/_lttitle/ig, '_ltAllowInventory');
                                var txtAllowupdate1 = elt.id.replace(/_lttitle/ig, '_hdnAllowInventory');
                                document.getElementById(txtAllowupdate1).value = iAllow;
                                document.getElementById(txtAllowupdate).innerHTML = iAllow;
                            }
                        }
                        else if (elt.id.toString().toLowerCase().indexOf('_txttitle') > -1) {
                            if (elt.value.toString().toLowerCase().indexOf('wx') > -1) {
                                var lblTxt = elt.value.toString().toLowerCase().substring(elt.value.toString().toLowerCase().indexOf('wx') + 2);
                                var vvv = lblTxt.substring(0, elt.value.toString().toLowerCase().indexOf('l')).replace(/ /g, '').replace(/l/g, '');
                                var iLock = 0;
                                var iAllow = 0;
                                var iorig = 0;
                                for (var k = 0; k < Arraynm.length; k++) {
                                    if (parseInt(Arraynm[k]) > parseInt(vvv)) {
                                        iAllow = parseInt(iAllow) + (parseInt(Arrayorig[k]) - parseInt(Arraylock[k]))

                                    }
                                    else if (parseInt(Arraynm[k]) == parseInt(vvv)) {
                                        iAllow = parseInt(iAllow) + (parseInt(Arrayorig[k]))

                                    }
                                }
                                var txtAllowupdate = elt.id.replace(/_txttitle/ig, '_txtAllowinventory');

                                document.getElementById(txtAllowupdate).value = iAllow;
                            }
                            else if (elt.value.toString().toLowerCase().indexOf('w x') > -1) {
                                var lblTxt = elt.value.toString().toLowerCase().substring(elt.value.toString().toLowerCase().indexOf('w x') + 3);
                                var vvv = lblTxt.substring(0, elt.value.toString().toLowerCase().indexOf('l')).replace(/ /g, '').replace(/l/g, '');
                                var iLock = 0;
                                var iAllow = 0;
                                var iorig = 0;
                                for (var k = 0; k < Arraynm.length; k++) {
                                    if (parseInt(Arraynm[k]) > parseInt(vvv)) {
                                        iAllow = parseInt(iAllow) + (parseInt(Arrayorig[k]) - parseInt(Arraylock[k]))

                                    }
                                    else if (parseInt(Arraynm[k]) == parseInt(vvv)) {
                                        iAllow = parseInt(iAllow) + (parseInt(Arrayorig[k]))

                                    }
                                }
                                var txtAllowupdate = elt.id.replace(/_txttitle/ig, '_txtAllowinventory');
                                document.getElementById(txtAllowupdate).value = iAllow;
                            }

                        }


                    }


                }
            }

            return true;
        }

    </script>
    <script type="text/javascript">


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

        function keyRestrictForOnlyNumeric(e, validchars) {

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
        function CheckValidation(id, msg) {
            if (document.getElementById(id) != null && document.getElementById(id).value == '') {
                jAlert(msg, 'Message', id);
                return false;
            }
            var txtlockid = id.replace(/_txttitle/ig, '_txtLockInventory');

            if (AllowandlockQtysave(txtlockid)) {
                return true;
            }
            else {
                return false;
            }
            return true;

        }
        function OpenPopup(VId, VariValId, RId) {
            var width = 850;
            var height = 650;
            var left = parseInt((screen.availWidth / 2) - (width / 2));
            var top = parseInt((screen.availHeight / 2) - (height / 2));
            var windowFeatures = "width=" + width + ",height=" + height + ",status=no,resizable=yes,scrollbars=yes,left=" + left + ",top=" + top + ",screenX=" + left + ",screenY=" + top;
            var popupurl = "ProductSearch.aspx?VId=" + VId + "&VariValId=" + VariValId + "&ID=<%=Request["ID"] %>&StoreID=<%=Request["StoreID"] %>&RId=" + RId + "";
            window.open(popupurl, "ProductSearch", windowFeatures);
            //window.open(popupurl, "ProductSearch", "toolbar=no,location=no,directories=no,status=no,menubar=no,scrollbars=yes,resizable=yes,width=800,height=650,left=150,top=80");

        }

        function AddOptionOpenPopup() {
            var popupurl = "ProductSearch.aspx?StoreID=<%=Request["StoreID"] %>";
            window.open(popupurl, "ProductSearch", "toolbar=no,location=no,directories=no,status=no,menubar=no,scrollbars=yes,resizable=yes,width=600,height=550,left=250,top=80");
        }

        function GotoProduct() {
            var productid = '<%=Request["Id"] %>'
            if (productid != '' && productid != null) {
                if ('<%=Request["StoreID"] %>' != null && '<%=Request["StoreID"] %>' == '7') {
                    window.location.href = 'Productebay.aspx?StoreId=<%=Request["StoreID"] %>&ID=<%=Request["ID"] %>&Mode=edit';
                }
                else if ('<%=Request["StoreID"] %>' != null && '<%=Request["StoreID"] %>' == '3') {
                    window.location.href = 'ProductAmazon.aspx?StoreId=<%=Request["StoreID"] %>&ID=<%=Request["ID"] %>&Mode=edit';
                }

                else {
                    window.location.href = 'Product.aspx?StoreId=<%=Request["StoreID"] %>&ID=<%=Request["ID"] %>&Mode=edit';
                }
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

    function SetRowIndex(rowindex) {
        if (document.getElementById('ContentPlaceHolder1_hdnGrIndex')) {
            document.getElementById('ContentPlaceHolder1_hdnGrIndex').value = rowindex;
        }
    }

    function ConfirmDeleteOption() {
        return confirm('Are you sure, you want to Delete this Option? - ' + document.getElementById('ContentPlaceHolder1_hdnOptionName').value);
    }

    </script>
    <script language="javascript" type="text/javascript">
        var myWindow;
        function openCenteredCrossSaleWindow(x, vname) {
            var width = 900;
            var height = 600;
            var left = parseInt((screen.availWidth / 2) - (width / 2));
            var top = parseInt((screen.availHeight / 2) - (height / 2));
            var StoreID = '<%=Request.QueryString["StoreID"]%>';
            var ProductID = '<%=Request.QueryString["ID"]%>';
            var windowFeatures = "width=" + width + ",height=" + height + ",status,resizable,scrollbars=yes,left=" + left + ",top=" + top + "screenX=" + left + ",screenY=" + top;
            //            if (valueid == 0) {
            //                myWindow = window.open('ProductSku.aspx?StoreID=' + StoreID + '&ProductID=' + ProductID + '&clientid=' + x, "subWind", windowFeatures);
            //            }
            //            else {
            if (x.indexOf('arelatedsku') > -1) {
                // vname = document.getElementById('ContentPlaceHolder1_hdnrelatedsku1').value;
                vname = document.getElementById(x).innerHTML;
                myWindow = window.open('ProductFabricSKU.aspx?StoreID=' + StoreID + '&ProductID=' + ProductID + '&clientid=' + x + '&vname=' + vname, "subWind", windowFeatures);
            }
            else if (x.indexOf('arelatedColor') > -1) {
                vname = document.getElementById('ContentPlaceHolder1_hdnrelatedsku1').value;
                myWindow = window.open('ProductColor.aspx?StoreID=' + StoreID + '&ProductID=' + ProductID + '&clientid=' + x + '&vname=' + vname, "subWind", windowFeatures);
            }
            else if (x.indexOf('arelatedfabric') > -1) {
                vname = document.getElementById('ContentPlaceHolder1_hdnrelatedsku1').value;
                myWindow = window.open('ProductvariantFabric.aspx?StoreID=' + StoreID + '&ProductID=' + ProductID + '&clientid=' + x + '&vname=' + vname, "subWind", windowFeatures);
            }
            else {
                vname = document.getElementById('ContentPlaceHolder1_hdnrelatedcolor').value;
                myWindow = window.open('ProductFabricSKU.aspx?StoreID=' + StoreID + '&ProductID=' + ProductID + '&clientid=' + x + '&vname=' + vname, "subWind", windowFeatures);
            }
            //}
        }


        function openCenteredCrossSaleWindowEdit(x, vname, VariValId) {
            var width = 900;
            var height = 600;
            var left = parseInt((screen.availWidth / 2) - (width / 2));
            var top = parseInt((screen.availHeight / 2) - (height / 2));
            var StoreID = '<%=Request.QueryString["StoreID"]%>';
            var ProductID = '<%=Request.QueryString["ID"]%>';
            var windowFeatures = "width=" + width + ",height=" + height + ",status,resizable,scrollbars=yes,left=" + left + ",top=" + top + "screenX=" + left + ",screenY=" + top;
            //            if (valueid == 0) {
            //                myWindow = window.open('ProductSku.aspx?StoreID=' + StoreID + '&ProductID=' + ProductID + '&clientid=' + x, "subWind", windowFeatures);
            //            }
            //            else {
            if (x.indexOf('arelatedsku1') > -1) {
                vname = document.getElementById('ContentPlaceHolder1_hdnrelatedsku1').value;
                myWindow = window.open('ProductFabricSKU.aspx?StoreID=' + StoreID + '&ProductID=' + ProductID + '&clientid=' + x + '&vname=' + vname, "subWind", windowFeatures);
            }
            else if (x.indexOf('arelatedColor') > -1) {
                vname = document.getElementById('ContentPlaceHolder1_hdnrelatedsku1').value;
                myWindow = window.open('ProductColor.aspx?StoreID=' + StoreID + '&ProductID=' + ProductID + '&clientid=' + x + '&vname=' + vname + '&VariId=' + VariValId + '&Mode=edit', "subWind", windowFeatures);
            }
            else if (x.indexOf('arelatedfabric') > -1) {
                vname = document.getElementById('ContentPlaceHolder1_hdnrelatedsku1').value;
                myWindow = window.open('ProductvariantFabric.aspx?StoreID=' + StoreID + '&ProductID=' + ProductID + '&clientid=' + x + '&vname=' + vname + '&VariId=' + VariValId + '&Mode=edit', "subWind", windowFeatures);
            }
            else {
                vname = document.getElementById('ContentPlaceHolder1_hdnrelatedcolor').value;
                myWindow = window.open('ProductFabricSKU.aspx?StoreID=' + StoreID + '&ProductID=' + ProductID + '&clientid=' + x + '&vname=' + vname, "subWind", windowFeatures);
            }
            //}
        }

        function openCenteredCrossSaleWindowOnsaleBuy1(x, VariId) {
            var width = 600;
            var height = 400;
            var left = parseInt((screen.availWidth / 2) - (width / 2));
            var top = parseInt((screen.availHeight / 2) - (height / 2));
            var StoreID = '<%=Request.QueryString["StoreID"]%>';
            var ProductID = '<%=Request.QueryString["ID"]%>';
            var windowFeatures = "width=" + width + ",height=" + height + ",status,resizable,scrollbars=yes,left=" + left + ",top=" + top + "screenX=" + left + ",screenY=" + top;
            if (x.indexOf('arelatedbuy1get1') > -1) {
                myWindow = window.open('ProductVariantBuy1OnsalePopup.aspx?StoreID=' + StoreID + '&ProductID=' + ProductID + '&clientid=' + x + '&VariId=' + VariId + '&varitype=buy1', "subWind", windowFeatures);
            }
            else if (x.indexOf('arelatedonsale') > -1) {
                myWindow = window.open('ProductVariantBuy1OnsalePopup.aspx?StoreID=' + StoreID + '&ProductID=' + ProductID + '&clientid=' + x + '&VariId=' + VariId + '&varitype=onsale', "subWind", windowFeatures);
            }
            else {
                // vname = document.getElementById('ContentPlaceHolder1_hdnrelatedcolor').value;
                myWindow = window.open('ProductVariantBuy1OnsalePopup.aspx?StoreID=' + StoreID + '&ProductID=' + ProductID + '&clientid=' + x + '&vname=' + vname, "subWind", windowFeatures);
            }
            //}
        }

        function selectAll(on) {
            var myform = document.forms[0];
            var len = myform.elements.length;
            for (var i = 0; i < len; i++) {
                if (myform.elements[i].type == 'checkbox') {
                    if (on.toString() == 'false') {

                        if (myform.elements[i].checked) {
                            myform.elements[i].checked = false;
                        }
                    }
                    else {
                        myform.elements[i].checked = true;
                    }
                }
            }
        }
    </script>
    <script type="text/javascript">
        function UpdateConformation() {
            var allElts = document.forms['form1'].elements;
            var i;
            var count = 0;
            for (i = 0; i < allElts.length; i++) {
                var elt = allElts[i];

                if (elt.type == "checkbox") {
                    if (elt.checked == true)
                        count++;
                }
            }
            if (count > 0) {
                return true;
            }
            else {
                jAlert('Please select atleast one Option Value to update.', 'Message');
                return false;
            }
        }


        function Conformation() {
            var allElts = document.forms['form1'].elements;
            var i;
            var count = 0;
            for (i = 0; i < allElts.length; i++) {
                var elt = allElts[i];

                if (elt.type == "checkbox") {
                    if (elt.checked == true)
                        count++;
                }
            }
            if (count > 0) {
                var ans = confirm('Are you sure, you want to delete the Option Value(s)?');
                if (ans)
                    return true;
                else
                    return false;
            }
            else {
                jAlert('Please select atleast one Option Value to delete.', 'Message');
                return false;
            }
        }
    </script>
    <script type="text/javascript">
        function ValidatePage() {
            if (document.getElementById('ContentPlaceHolder1_txtOptionName')) {
                if ((document.getElementById('ContentPlaceHolder1_txtOptionName').value).replace(/^\s*\s*$/g, '') == '') {
                    jAlert('Please Enter Option Name', 'Message', 'ContentPlaceHolder1_txtOptionName');
                    return false;
                }
            }
            //            if (document.getElementById('ContentPlaceHolder1_txtOptionValue')) {
            //                if ((document.getElementById('ContentPlaceHolder1_txtOptionValue').value).replace(/^\s*\s*$/g, '') == '') {
            //                    jAlert('Please enter Option Value', 'Message', 'ContentPlaceHolder1_txtOptionValue')
            //                    return false;
            //                }
            //            }
            return true;
        }

        function clearField() {
            if (document.getElementById('ContentPlaceHolder1_txtOptionName')) {
                document.getElementById('ContentPlaceHolder1_txtOptionName').value = '';
            }
            if (document.getElementById('ContentPlaceHolder1_txtDisplayOrder')) {
                document.getElementById('ContentPlaceHolder1_txtDisplayOrder').value = '';
            }
            if (document.getElementById('ContentPlaceHolder1_chkparent')) {
                document.getElementById('ContentPlaceHolder1_chkparent').checked = false;
            }
            return false;
        }

        function ShowModelUserRegisterPopup(ProductId, invId, VariantID, VariantValueID, cid) {
            document.getElementById("frmdisplay").contentWindow.document.body.innerHTML = '';
            document.getElementById('frmdisplay').height = '300px';
            document.getElementById('frmdisplay').width = '602px';
            document.getElementById('popupContact').setAttribute("style", "z-index: 1000001; top: 30px; padding: 0px;width:602px;height:270px;");
            document.getElementById('popupContact').style.width = '602px';
            window.scrollTo(0, 0);
            document.getElementById('btnreadmore').click();
            document.getElementById('frmdisplay').src = 'InventoryVariantWareHouse.aspx?PID=' + ProductId + '&invId=' + invId + '&VariantID=' + VariantID + '&VariantValueID=' + VariantValueID + '&cid=' + cid;

        }

        function chkHeight() {
            var windowHeight = 0;
            windowHeight = $(document).height(); //window.innerHeight;

            document.getElementById('prepage').style.height = windowHeight + 'px';
            document.getElementById('prepage').style.display = '';
        }
    </script>
    <style type="text/css">
        .altrowmain td {
            background: #E3E3E3 !important;
            color: #000 !important;
        }

        .altrowmainsub td {
            background: #ffffff !important;
            color: #000 !important;
        }

        .content-table th {
            background: #d7d7d7 !important;
            color: #000 !important;
        }
    </style>
    <div id="divcontentrow1" runat="server" class="content-row1">
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
                <td>
                    <a id="btnGotoProduct" runat="server" href="javascript:void(0);" title="Go To Product"
                        onclick="return GotoProduct();">
                        <img src="/App_Themes/<%=Page.Theme %>/images/go-to-product.png" alt="Go To Product"
                            title="Go To Product" class="img-right" height="23" />
                    </a>
                </td>
            </tr>
            <tr>
                <td height="5" align="left" valign="top">
                    <img alt="" src="/App_Themes/<%=Page.Theme %>/images/spacer.gif" width="1" height="5" />
                </td>
            </tr>
            <tr>
                <td class="border-td">
                    <table width="100%" border="0" cellpadding="0" cellspacing="0" bgcolor="#FFFFFF"
                        class="content-table">
                        <tr>
                            <td class="border-td-sub">
                                <table width="100%" border="0" cellpadding="0" cellspacing="0" class="add-product">
                                    <tr id="idVariantHeader" runat="server" style="width: 1126px;">
                                        <th>
                                            <div class="main-title-left">
                                                <img class="img-left" title="Product Options" alt="Product Options" src="/App_Themes/<%=Page.Theme %>/icon/add-product.png" />
                                                <h2>Product Options</h2>
                                            </div>
                                            <div class="main-title-right" style="display: none;">
                                                <a href="javascript:void(0);" class="show_hideMainDiv" onclick="return ShowHideButton('imgMainDiv','tdMainDiv');">
                                                    <img id="imgMainDiv" class="close" title="close" alt="close" src="/App_Themes/<%=Page.Theme %>/images/close.png" /></a>
                                            </div>
                                        </th>
                                    </tr>
                                    <tr>
                                        <td id="tdMainDiv">
                                            <div id="divMain" class="slidingDivMainDiv">
                                                <table width="100%" border="0" cellpadding="0" cellspacing="0" class="add-product">
                                                    <tr class="altrow">
                                                        <td align="center">
                                                            <asp:Label ID="lblMsg" runat="server" ForeColor="Red"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="padding-left: 20px">
                                                            <table cellpadding="0" cellspacing="0" width="90%" align="left">
                                                                <tr valign="top">
                                                                    <td style="width: 90px">
                                                                        <b>Product Name :</b>
                                                                    </td>
                                                                    <td>
                                                                        <asp:Label ID="lblProductName" runat="server"></asp:Label>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                    <tr class="even-row" id="trAddOption" runat="server">
                                                        <td align="center">
                                                            <fieldset>
                                                                <legend><b>Add Option Name</b></legend>
                                                                <table width="90%" border="0" cellpadding="0" cellspacing="0">
                                                                    <tr class="oddrow">
                                                                        <td width="13%">
                                                                            <span class="star">*</span>Option Name :
                                                                        </td>
                                                                        <td width="88%" align="left">
                                                                            <asp:TextBox ID="txtOptionName" runat="server" MaxLength="100" class="order-textfield"
                                                                                Width="250px"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr class="altrow">
                                                                        <td width="13%">
                                                                            <span class="star">&nbsp;</span>Display Order :
                                                                        </td>
                                                                        <td align="left">
                                                                            <asp:TextBox ID="txtDisplayOrder" runat="server" MaxLength="9" onkeypress="return keyRestrict(event,'0123456789');"
                                                                                Width="100px" CssClass="order-textfield"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr class="oddrow">
                                                                        <td width="13%">
                                                                            <span class="star">&nbsp;</span>Is Parent :
                                                                        </td>
                                                                        <td align="left">
                                                                            <asp:CheckBox ID="chkparent" runat="server" />
                                                                        </td>
                                                                    </tr>
                                                                    <tr id="trSavecancel" runat="server" class="altrow">
                                                                        <td width="13%"></td>
                                                                        <td align="left">
                                                                            <asp:ImageButton ID="btnSaveOption" runat="server" OnClientClick="return ValidatePage();"
                                                                                OnClick="btnSaveOption_Click" />
                                                                            <asp:ImageButton ID="btnCancelOption" runat="server" OnClick="btnCancelOption_Click"
                                                                                OnClientClick="return clearField();" />
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </fieldset>
                                                        </td>
                                                    </tr>
                                                    <tr class="even-row" id="tr1" runat="server">
                                                        <td align="center">
                                                            <fieldset>
                                                                <legend><b>Add Option Value</b></legend>
                                                                <table width="90%" border="0" cellpadding="0" cellspacing="0">
                                                                    <tr>
                                                                        <td width="100%">
                                                                            <asp:GridView ID="grdoptionmainGroup" runat="server" GridLines="None" AutoGenerateColumns="false"
                                                                                CssClass="checklist-main border-right-all" AllowPaging="false" DataKeyNames="VariantID"
                                                                                BorderColor="#545454" BorderWidth="1px" BorderStyle="Solid" ShowFooter="true"
                                                                                OnRowDataBound="grdoptionmainGroup_RowDataBound" OnRowCommand="grdoptionmainGroup_RowCommand"
                                                                                OnRowEditing="grdoptionmainGroup_RowEditing" Width="100%">
                                                                                <Columns>
                                                                                    <asp:TemplateField HeaderText="&nbsp;Name">
                                                                                        <ItemTemplate>
                                                                                            <%--<div style="float: left; margin-top: 3px;">
                                                                                                <img src="/Admin/images/expanded.png" alt="" border="0" />
                                                                                            </div>--%>
                                                                                            &nbsp;&nbsp;<asp:Literal ID="lttitle" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"VariantName") %>'></asp:Literal>
                                                                                            <asp:TextBox ID="txttitle" runat="server" Style="width: 600px;" Visible="false" CssClass="order-textfield"
                                                                                                Text='<%# DataBinder.Eval(Container.DataItem,"VariantName") %>'></asp:TextBox>
                                                                                            <input type="hidden" id="hdnVariantID" runat="server" value='<%# DataBinder.Eval(Container.DataItem,"VariantID") %>' />
                                                                                            <input type="hidden" id="hdnparent" runat="server" value='<%# DataBinder.Eval(Container.DataItem,"isParent") %>' />
                                                                                        </ItemTemplate>
                                                                                        <FooterStyle HorizontalAlign="Left" />
                                                                                        <HeaderStyle Width="70%" HorizontalAlign="left" VerticalAlign="Middle" CssClass="table-title" />
                                                                                        <ItemStyle HorizontalAlign="left" />
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField HeaderText="&nbsp;Is Parent">
                                                                                        <ItemTemplate>
                                                                                            &nbsp;&nbsp;<asp:Literal ID="ltparentimage" runat="server" Text=''></asp:Literal>
                                                                                            <asp:CheckBox ID="chkparent" runat="server" Visible="false" />
                                                                                        </ItemTemplate>
                                                                                        <FooterStyle HorizontalAlign="Left" />
                                                                                        <HeaderStyle Width="10%" HorizontalAlign="left" VerticalAlign="Middle" CssClass="table-title" />
                                                                                        <ItemStyle HorizontalAlign="left" />
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField HeaderText="&nbsp;Display Order">
                                                                                        <ItemTemplate>
                                                                                            &nbsp;&nbsp;<asp:Literal ID="ltdisplayorder" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"DisplayOrder") %>'></asp:Literal>
                                                                                            <asp:TextBox ID="txtdisplayorder" onkeypress="return keyRestrict(event,'0123456789');"
                                                                                                runat="server" Style="width: 60px; text-align: center;" Visible="false" CssClass="order-textfield"
                                                                                                Text='<%# DataBinder.Eval(Container.DataItem,"DisplayOrder") %>'></asp:TextBox>
                                                                                        </ItemTemplate>
                                                                                        <FooterStyle HorizontalAlign="Left" />
                                                                                        <HeaderStyle Width="12%" HorizontalAlign="Center" VerticalAlign="Middle" CssClass="table-title" />
                                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField HeaderText="&nbsp;Action">
                                                                                        <HeaderStyle HorizontalAlign="center" VerticalAlign="Middle" CssClass="table-title" />
                                                                                        <ItemTemplate>
                                                                                            <asp:ImageButton ID="imgSave" ToolTip="Save" runat="server" ImageUrl="/App_Themes/Gray/Images/save.png"
                                                                                                CommandName="Save" CommandArgument='<%# DataBinder.Eval(Container.DataItem,"VariantID") %>'
                                                                                                Visible="false" />
                                                                                            <asp:ImageButton ID="imgcancel" runat="server" ImageUrl="/admin/Images/isInactive.png"
                                                                                                CommandName="Exit" ToolTip="Cancel" CommandArgument='<%# DataBinder.Eval(Container.DataItem,"VariantID") %>'
                                                                                                Visible="false" />
                                                                                            <asp:ImageButton ID="imgEdit" ToolTip="Edit" runat="server" ImageUrl="/App_Themes/Gray/Images/Edit.gif"
                                                                                                CommandName="Edit" OnClientClick="chkHeight();" CommandArgument='<%# DataBinder.Eval(Container.DataItem,"VariantID") %>' />
                                                                                            <asp:ImageButton ID="imgDelete" ToolTip="Remove" OnClientClick="javascript:if(confirm('Are you sure want to delete this record?')){chkHeight(); return true;}else{return false;}"
                                                                                                runat="server" ImageUrl="/App_Themes/Gray/Images/delete-icon.png" CommandName="Remove"
                                                                                                CommandArgument='<%# DataBinder.Eval(Container.DataItem,"VariantID") %>' />
                                                                                        </ItemTemplate>
                                                                                        <ItemStyle VerticalAlign="Top" HorizontalAlign="Center" />
                                                                                        <HeaderStyle HorizontalAlign="Center" />
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField>
                                                                                        <ItemTemplate>
                                                                                            <%--<tr>
                                                                                                <td colspan="100%" style="background:url('/images/title-bg.jpg') repeat scroll left top #545454; line-height: 18px; border: solid 1px #d8d8d8">
                                                                                                    <b>Add Option Value</b>
                                                                                                </td>
                                                                                            </tr>--%>
                                                                                            <tr>
                                                                                                <td colspan="100%">
                                                                                                    <div id="divchild<%# Eval("VariantID") %>" style="margin: 2px 0 0 0; position: relative; left: 15px; overflow: auto; width: 98%">
                                                                                                        <asp:GridView ID="grdvaluelisting" runat="server" GridLines="None" AutoGenerateColumns="false"
                                                                                                            CssClass="checklist-main border-right-all" AllowPaging="false" ShowHeader="True"
                                                                                                            ShowFooter="true" EmptyDataText="No Record Found(s)." EditRowStyle-HorizontalAlign="Center"
                                                                                                            BorderColor="#eeeeee" BorderWidth="1px" BorderStyle="Solid" DataKeyNames="VariantID"
                                                                                                            OnRowDataBound="grdvaluelisting_RowDataBound" OnRowCommand="grdvaluelisting_RowCommand"
                                                                                                            OnRowDeleting="grdvaluelisting_RowDeleting" OnRowEditing="grdvaluelisting_RowEditing">
                                                                                                            <Columns>
                                                                                                                <asp:TemplateField HeaderText="&nbsp;Value">
                                                                                                                    <ItemTemplate>
                                                                                                                        <%-- <div style="float: left; margin-top: 3px;">
                                                                                                                            <img src="/Admin/images/expanded.png" alt="" border="0" />
                                                                                                                        </div>--%>
                                                                                                                        <asp:Label ID="lttitle" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"VariantValue") %>'></asp:Label>
                                                                                                                        <input type="hidden" id="hdnVariantValueID" runat="server" value='<%# DataBinder.Eval(Container.DataItem,"VariantValueID") %>' />
                                                                                                                        <input type="hidden" id="hdnVariantID" runat="server" value='<%# DataBinder.Eval(Container.DataItem,"VariantID") %>' />
                                                                                                                        <asp:TextBox ID="txttitle" Style="width: 310px;" runat="server" Visible="false" CssClass="order-textfield"
                                                                                                                            onkeypress="AllowandlockQty(this.id);" onchange="AllowandlockQty(this.id);" onkeyup="AllowandlockQty(this.id);"
                                                                                                                            Text='<%# DataBinder.Eval(Container.DataItem,"VariantValue") %>'></asp:TextBox>
                                                                                                                    </ItemTemplate>
                                                                                                                    <HeaderStyle Width="10%" HorizontalAlign="left" VerticalAlign="Middle" CssClass="table-title" />
                                                                                                                    <ItemStyle Width="10%" />
                                                                                                                    <ItemStyle HorizontalAlign="left" />
                                                                                                                    <FooterTemplate>
                                                                                                                        <asp:TextBox ID="txtTitle" CssClass="order-textfield" Width="320px" Text="" runat="server"></asp:TextBox>
                                                                                                                        <input type="hidden" id="hdnVariantIDSub" runat="server" value='<%# DataBinder.Eval(Container.DataItem,"VariantID") %>' />
                                                                                                                    </FooterTemplate>
                                                                                                                    <FooterStyle HorizontalAlign="Left" />
                                                                                                                </asp:TemplateField>
                                                                                                                <asp:TemplateField HeaderText="&nbsp;Price($)">
                                                                                                                    <ItemTemplate>
                                                                                                                        <asp:Literal ID="ltprice" runat="server" Text='<%#Convert.ToDecimal(DataBinder.Eval(Container.DataItem,"VariantPrice")).ToString("F2") %>'></asp:Literal>
                                                                                                                        <asp:TextBox ID="txtprice" onkeypress="return keyRestrict(event,'0123456789.');"
                                                                                                                            Style="width: 80px; text-align: right;" runat="server" Visible="false" CssClass="order-textfield"
                                                                                                                            Text='<%#Convert.ToDecimal(DataBinder.Eval(Container.DataItem,"VariantPrice")).ToString("F2") %>'></asp:TextBox>
                                                                                                                    </ItemTemplate>
                                                                                                                    <HeaderStyle Width="10%" HorizontalAlign="right" VerticalAlign="Middle" CssClass="table-title" />
                                                                                                                    <ItemStyle Width="10%" />
                                                                                                                    <ItemStyle HorizontalAlign="right" />
                                                                                                                    <FooterTemplate>
                                                                                                                        <asp:TextBox ID="txtPrice" onkeypress="return keyRestrict(event,'0123456789.');"
                                                                                                                            CssClass="order-textfield" Width="80px" Text="" Style="width: 80px; text-align: right;"
                                                                                                                            runat="server"></asp:TextBox>
                                                                                                                    </FooterTemplate>
                                                                                                                    <FooterStyle HorizontalAlign="right" />
                                                                                                                </asp:TemplateField>
                                                                                                                <asp:TemplateField HeaderText="&nbsp;Base Custom Price($)">
                                                                                                                    <ItemTemplate>
                                                                                                                        <asp:Literal ID="ltBasecustomPrice" runat="server" Text='<%#Convert.ToDecimal(DataBinder.Eval(Container.DataItem,"BasecustomPrice")).ToString("F2") %>'></asp:Literal>
                                                                                                                        <asp:TextBox ID="txtBasecustomPrice" onkeypress="return keyRestrict(event,'0123456789.');"
                                                                                                                            Style="width: 80px; text-align: right;" runat="server" Visible="false" CssClass="order-textfield"
                                                                                                                            Text='<%#Convert.ToDecimal(DataBinder.Eval(Container.DataItem,"BasecustomPrice")).ToString("F2") %>'></asp:TextBox>
                                                                                                                    </ItemTemplate>
                                                                                                                    <HeaderStyle Width="10%" HorizontalAlign="right" VerticalAlign="Middle" CssClass="table-title" />
                                                                                                                    <ItemStyle Width="10%" />
                                                                                                                    <ItemStyle HorizontalAlign="right" />
                                                                                                                    <FooterTemplate>
                                                                                                                        <asp:TextBox ID="txtBasecustomPrice" onkeypress="return keyRestrict(event,'0123456789.');"
                                                                                                                            CssClass="order-textfield" Width="80px" Text="" Style="width: 80px; text-align: right;"
                                                                                                                            runat="server"></asp:TextBox>
                                                                                                                    </FooterTemplate>
                                                                                                                    <FooterStyle HorizontalAlign="right" />
                                                                                                                </asp:TemplateField>
                                                                                                                <asp:TemplateField HeaderText="&nbsp;Quantity">
                                                                                                                    <ItemTemplate>
                                                                                                                        <input type="hidden" id="hdnmainqty" runat="server" value='<%# DataBinder.Eval(Container.DataItem,"Inventory") %>' />
                                                                                                                        <a id="lnkEditInventory" runat="server" href="javascript:void(0);" style="color: #6A6A6A; cursor: pointer;">
                                                                                                                            <asp:Label ID="ltInventory" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"Inventory") %>'></asp:Label></a>
                                                                                                                    </ItemTemplate>
                                                                                                                    <HeaderStyle Width="10%" HorizontalAlign="Center" VerticalAlign="Middle" CssClass="table-title" />
                                                                                                                    <ItemStyle Width="10%" />
                                                                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                                                                    <FooterTemplate>
                                                                                                                        <%-- <asp:TextBox ID="txtInventory" onkeypress="return keyRestrict(event,'0123456789');"
                                                                                                                            CssClass="order-textfield" Width="80px" Style="width: 80px; text-align: center;"
                                                                                                                            Text="" runat="server"></asp:TextBox>--%>
                                                                                                                        <input type="hidden" id="hdnmainqty" runat="server" value='0' />
                                                                                                                    </FooterTemplate>
                                                                                                                    <FooterStyle HorizontalAlign="Center" />
                                                                                                                </asp:TemplateField>
                                                                                                                <asp:TemplateField HeaderText="&nbsp;Allow Qty">
                                                                                                                    <ItemTemplate>
                                                                                                                        <asp:Label ID="ltAllowInventory" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"AllowQuantity") %>'></asp:Label>
                                                                                                                        <input type="hidden" id="hdnAllowInventory" runat="server" value='<%# DataBinder.Eval(Container.DataItem,"AllowQuantity") %>' />
                                                                                                                        <asp:TextBox ID="txtAllowinventory" onkeypress="return keyRestrict(event,'0123456789');"
                                                                                                                            Style="width: 70px; text-align: center;" runat="server" Visible="false" CssClass="order-textfield"
                                                                                                                            Text='<%# DataBinder.Eval(Container.DataItem,"AllowQuantity") %>'></asp:TextBox>
                                                                                                                    </ItemTemplate>
                                                                                                                    <HeaderStyle Width="10%" HorizontalAlign="Center" VerticalAlign="Middle" CssClass="table-title" />
                                                                                                                    <ItemStyle Width="10%" />
                                                                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                                                                    <FooterTemplate>
                                                                                                                        <asp:TextBox ID="txtAllowinventory" onkeypress="return keyRestrict(event,'0123456789');"
                                                                                                                            CssClass="order-textfield" Width="70px" Style="text-align: center;" Text="" runat="server"></asp:TextBox>
                                                                                                                        <input type="hidden" id="hdnAllowInventory" runat="server" value='' />
                                                                                                                    </FooterTemplate>
                                                                                                                    <FooterStyle HorizontalAlign="Center" />
                                                                                                                </asp:TemplateField>
                                                                                                                <asp:TemplateField HeaderText="&nbsp;Lock Qty">
                                                                                                                    <ItemTemplate>
                                                                                                                        <asp:Literal ID="ltLockInventory" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"LockQuantity") %>'></asp:Literal>
                                                                                                                        <input id="hdnLockInventory" type="hidden" runat="server" value='<%# DataBinder.Eval(Container.DataItem,"LockQuantity") %>' />
                                                                                                                        <asp:TextBox ID="txtLockinventory" onkeypress="AllowandlockQty(this.id);" onchange="AllowandlockQty(this.id);"
                                                                                                                            onkeyup="AllowandlockQty(this.id);" Style="width: 80px; text-align: center;"
                                                                                                                            runat="server" Visible="false" CssClass="order-textfield" Text='<%# DataBinder.Eval(Container.DataItem,"LockQuantity") %>'></asp:TextBox>
                                                                                                                    </ItemTemplate>
                                                                                                                    <HeaderStyle Width="10%" HorizontalAlign="Center" VerticalAlign="Middle" CssClass="table-title" />
                                                                                                                    <ItemStyle Width="10%" />
                                                                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                                                                    <FooterTemplate>
                                                                                                                        <asp:TextBox ID="txtLockinventory" onkeypress="return keyRestrict(event,'0123456789');"
                                                                                                                            onchange="AllowandlockQty(this.id);" onkeyup="AllowandlockQty(this.id);" CssClass="order-textfield"
                                                                                                                            Width="80px" Style="width: 80px; text-align: center;" Text="" runat="server"></asp:TextBox>
                                                                                                                    </FooterTemplate>
                                                                                                                    <FooterStyle HorizontalAlign="Center" />
                                                                                                                </asp:TemplateField>
                                                                                                                <asp:TemplateField HeaderText="&nbsp;Partner Hemming Qty">
                                                                                                                    <ItemTemplate>
                                                                                                                        <asp:Literal ID="ltAdditionalHemingQty" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"AddiHemingQty") %>'></asp:Literal>
                                                                                                                        <input id="hdnAddiHemingQty" type="hidden" runat="server" value='<%# DataBinder.Eval(Container.DataItem,"AddiHemingQty") %>' />
                                                                                                                        <asp:TextBox ID="txtAdditionalHemingQty" Style="width: 70px; text-align: center;"
                                                                                                                            runat="server" Visible="false" onkeypress="return keyRestrict(event,'0123456789');" CssClass="order-textfield" Text='<%# DataBinder.Eval(Container.DataItem,"AddiHemingQty") %>'></asp:TextBox>
                                                                                                                    </ItemTemplate>
                                                                                                                    <HeaderStyle Width="10%" HorizontalAlign="Center" VerticalAlign="Middle" CssClass="table-title" />
                                                                                                                    <ItemStyle Width="10%" />
                                                                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                                                                    <FooterTemplate>
                                                                                                                        <asp:TextBox ID="txtAdditionalHemingQty"
                                                                                                                            CssClass="order-textfield" Width="80px" onkeypress="return keyRestrict(event,'0123456789');" Style="width: 70px; text-align: center;"
                                                                                                                            Text="" runat="server"></asp:TextBox>
                                                                                                                    </FooterTemplate>
                                                                                                                    <FooterStyle HorizontalAlign="Center" />
                                                                                                                </asp:TemplateField>
                                                                                                                <asp:TemplateField HeaderText="&nbsp;SKU">
                                                                                                                    <ItemTemplate>
                                                                                                                        <asp:Literal ID="ltsku" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"SKU") %>'></asp:Literal>
                                                                                                                        <asp:TextBox ID="txtsku" Style="width: 120px;" runat="server" Visible="false" CssClass="order-textfield"
                                                                                                                            Text='<%# DataBinder.Eval(Container.DataItem,"SKU") %>'></asp:TextBox>
                                                                                                                    </ItemTemplate>
                                                                                                                    <HeaderStyle Width="10%" HorizontalAlign="left" VerticalAlign="Middle" CssClass="table-title" />
                                                                                                                    <ItemStyle Width="10%" />
                                                                                                                    <ItemStyle HorizontalAlign="left" />
                                                                                                                    <FooterTemplate>
                                                                                                                        <asp:TextBox ID="txtSku" CssClass="order-textfield" Width="120px" Text="" runat="server"></asp:TextBox>
                                                                                                                    </FooterTemplate>
                                                                                                                    <FooterStyle HorizontalAlign="Left" />
                                                                                                                </asp:TemplateField>



                                                                                                                <asp:TemplateField HeaderText="Weight">
                                                                                                                    <ItemTemplate>

                                                                                                                           <asp:Literal ID="ltweight" runat="server" Text='<%# String.Format("{0:0.00}",Convert.ToDecimal(DataBinder.Eval(Container.DataItem,"Weight")))%>'></asp:Literal>

                                                                                                                        <asp:TextBox ID="txtweightparent" Text='<%# String.Format("{0:0.00}",Convert.ToDecimal(DataBinder.Eval(Container.DataItem,"Weight")))%>'  runat="server" Visible="false"
                                                                                                                           
                                                                                                                            CssClass="order-textfield" Width="40px"></asp:TextBox>
                                                                                                                    </ItemTemplate>
                                                                                                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" CssClass="table-title" />
                                                                                                                    <ItemStyle />
                                                                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                                                                    <FooterTemplate>
                                                                                                                        <asp:TextBox ID="txtweightparent" runat="server" CssClass="order-textfield"    onkeypress="return keyRestrict(event,'0123456789.');"></asp:TextBox>
                                                                                                                    </FooterTemplate>
                                                                                                                    <FooterStyle HorizontalAlign="Center" />
                                                                                                                </asp:TemplateField>




                                                                                                                <asp:TemplateField HeaderText="&nbsp;UPC">
                                                                                                                    <ItemTemplate>
                                                                                                                        <asp:Literal ID="ltupc" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"UPC") %>'></asp:Literal>
                                                                                                                        <asp:TextBox ID="txtupc" Style="width: 120px;" runat="server" Visible="false" CssClass="order-textfield"
                                                                                                                            Text='<%# DataBinder.Eval(Container.DataItem,"UPC") %>'></asp:TextBox>
                                                                                                                    </ItemTemplate>
                                                                                                                    <HeaderStyle Width="10%" HorizontalAlign="left" VerticalAlign="Middle" CssClass="table-title" />
                                                                                                                    <ItemStyle Width="10%" />
                                                                                                                    <ItemStyle HorizontalAlign="left" />
                                                                                                                    <FooterTemplate>
                                                                                                                        <asp:TextBox ID="txtUpc" CssClass="order-textfield" Width="120px" Text="" runat="server"></asp:TextBox>
                                                                                                                    </FooterTemplate>
                                                                                                                    <FooterStyle HorizontalAlign="Left" />
                                                                                                                </asp:TemplateField>
                                                                                                                <asp:TemplateField HeaderText="&nbsp;Header">
                                                                                                                    <ItemTemplate>
                                                                                                                        <asp:Literal ID="ltheader" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"Header") %>'></asp:Literal>
                                                                                                                        <asp:TextBox ID="txtheader" Style="width: 120px;" runat="server" Visible="false"
                                                                                                                            CssClass="order-textfield" Text='<%# DataBinder.Eval(Container.DataItem,"Header") %>'></asp:TextBox>
                                                                                                                    </ItemTemplate>
                                                                                                                    <HeaderStyle Width="10%" HorizontalAlign="left" VerticalAlign="Middle" CssClass="table-title" />
                                                                                                                    <ItemStyle Width="10%" />
                                                                                                                    <ItemStyle HorizontalAlign="left" />
                                                                                                                    <FooterTemplate>
                                                                                                                        <asp:TextBox ID="txtHeader" CssClass="order-textfield" Width="120px" Text="" runat="server"></asp:TextBox>
                                                                                                                    </FooterTemplate>
                                                                                                                    <FooterStyle HorizontalAlign="Left" />
                                                                                                                </asp:TemplateField>



                                                                                                                <asp:TemplateField HeaderText="Back Order Date">
                                                                                                                    <ItemTemplate>

                                                                                                                        <asp:TextBox ID="txtbackorderdateparent" Style="width: 80px; margin-right: 3px;" runat="server"
                                                                                                                            Text='<%# String.Format("{0:MM/dd/yyyy}",DataBinder.Eval(Container.DataItem,"BackOrderdate"))%>'
                                                                                                                            CssClass="order-textfield-picker"></asp:TextBox>
                                                                                                                    </ItemTemplate>
                                                                                                                    <HeaderStyle Width="8%" HorizontalAlign="left" VerticalAlign="Middle" CssClass="table-title" />
                                                                                                                    <ItemStyle Width="8%" />
                                                                                                                    <ItemStyle HorizontalAlign="left" />
                                                                                                                    <FooterTemplate>
                                                                                                                        <asp:TextBox ID="txtbackorderdateparent" Style="width: 80px; margin-right: 3px;" runat="server"
                                                                                                                            Text=''
                                                                                                                            CssClass="order-textfield-picker"></asp:TextBox>
                                                                                                                    </FooterTemplate>
                                                                                                                    <FooterStyle HorizontalAlign="Left" />
                                                                                                                </asp:TemplateField>


                                                                                                                <asp:TemplateField HeaderText="&nbsp;Buy1&nbsp;Get1 Free">
                                                                                                                    <ItemTemplate>
                                                                                                                        <asp:Literal ID="ltrBuy1get1" runat="server"></asp:Literal>
                                                                                                                        <img border="0" style="width: 12px;" id="imgBuy1get1" runat="server" />
                                                                                                                        <a href="javascript:void(0);" id="arelatedbuy1get1" style="color: #B92127; text-decoration: underline;"
                                                                                                                            runat="server">Buy 1 Get 1 Free </a>
                                                                                                                        <input type="hidden" id="hdnBuy1Get1" runat="server" value='<%# DataBinder.Eval(Container.DataItem,"Buy1Get1") %>' />
                                                                                                                        <input type="hidden" id="hdnupdatebuy1" runat="server" value='<%#DataBinder.Eval(Container.DataItem,"updatebuy1")%>' />
                                                                                                                    </ItemTemplate>
                                                                                                                    <HeaderStyle Width="11%" HorizontalAlign="Center" VerticalAlign="Middle" CssClass="table-title" />
                                                                                                                    <ItemStyle Width="11%" />
                                                                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                                                                    <FooterTemplate>
                                                                                                                        &nbsp;
                                                                                                                    </FooterTemplate>
                                                                                                                    <FooterStyle HorizontalAlign="Center" />
                                                                                                                </asp:TemplateField>
                                                                                                                <asp:TemplateField HeaderText="&nbsp;OnSale">
                                                                                                                    <ItemTemplate>
                                                                                                                        <asp:Literal ID="ltronsale" runat="server"></asp:Literal>
                                                                                                                        <img border="0" style="width: 12px;" id="imgonsale" runat="server" />
                                                                                                                        <a href="javascript:void(0);" id="arelatedonsale" style="color: #B92127; text-decoration: underline;"
                                                                                                                            runat="server">OnSale</a>
                                                                                                                        <input type="hidden" id="hdnonsale" runat="server" value='<%# DataBinder.Eval(Container.DataItem,"OnSale") %>' />
                                                                                                                    </ItemTemplate>
                                                                                                                    <HeaderStyle Width="11%" HorizontalAlign="Center" VerticalAlign="Middle" CssClass="table-title" />
                                                                                                                    <ItemStyle Width="11%" />
                                                                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                                                                    <FooterTemplate>
                                                                                                                        &nbsp;
                                                                                                                    </FooterTemplate>
                                                                                                                    <FooterStyle HorizontalAlign="Center" />
                                                                                                                </asp:TemplateField>
                                                                                                                <asp:TemplateField HeaderText="&nbsp;Display Order">
                                                                                                                    <ItemTemplate>
                                                                                                                        <asp:Literal ID="ltdisplayorder" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"DisplayOrder") %>'></asp:Literal>
                                                                                                                        <asp:TextBox ID="txtdisplayorder" onkeypress="return keyRestrict(event,'0123456789');"
                                                                                                                            Style="width: 60px; text-align: center;" runat="server" Visible="false" CssClass="order-textfield"
                                                                                                                            Text='<%# DataBinder.Eval(Container.DataItem,"DisplayOrder") %>'></asp:TextBox>
                                                                                                                    </ItemTemplate>
                                                                                                                    <HeaderStyle Width="10%" HorizontalAlign="center" VerticalAlign="Middle" CssClass="table-title" />
                                                                                                                    <ItemStyle Width="10%" />
                                                                                                                    <ItemStyle HorizontalAlign="center" />
                                                                                                                    <FooterTemplate>
                                                                                                                        <asp:TextBox ID="txtDisplayorder" onkeypress="return keyRestrict(event,'0123456789');"
                                                                                                                            CssClass="order-textfield" Width="60px" Text="" Style="text-align: center;" runat="server"></asp:TextBox>
                                                                                                                    </FooterTemplate>
                                                                                                                    <FooterStyle HorizontalAlign="center" />
                                                                                                                </asp:TemplateField>
                                                                                                                <asp:TemplateField HeaderText="&nbsp;Related Product">
                                                                                                                    <ItemTemplate>
                                                                                                                        <a href="javascript:void(0);" id="arelatedsku" runat="server" onclick="openCenteredCrossSaleWindow(this.id);">
                                                                                                                            <%# DataBinder.Eval(Container.DataItem,"selectSKU") %>
                                                                                                                        </a>
                                                                                                                        <br />
                                                                                                                        <a href="javascript:void(0);" id="arelatedColor" title="Edit Color" runat="server"
                                                                                                                            onclick="openCenteredCrossSaleWindowEdit(this.id,'');">Select Color</a>
                                                                                                                        <a href="javascript:void(0);" id="arelatedfabric" title="Edit Fabric" runat="server"
                                                                                                                            onclick="openCenteredCrossSaleWindowEdit(this.id,'');">Select Fabric</a>
                                                                                                                        <input type="hidden" id="hdnProductColorNewImg" runat="server" value="" />
                                                                                                                        <input type="hidden" id="hdnrelatedsku" runat="server" value='<%# DataBinder.Eval(Container.DataItem,"selectSKU") %>' />
                                                                                                                        <input type="hidden" id="hdnProductfabrictype" runat="server" value='<%# DataBinder.Eval(Container.DataItem,"FabricType") %>' />
                                                                                                                        <input type="hidden" id="hdnProductfabriccode" runat="server" value='<%# DataBinder.Eval(Container.DataItem,"FabricCode") %>' /><input type="hidden" id="hdnactive" runat="server" value='<%# DataBinder.Eval(Container.DataItem,"VarActive") %>' />
                                                                                                                    </ItemTemplate>
                                                                                                                    <HeaderStyle Width="10%" HorizontalAlign="center" VerticalAlign="Middle" CssClass="table-title" />
                                                                                                                    <ItemStyle Width="10%" />
                                                                                                                    <ItemStyle HorizontalAlign="center" />
                                                                                                                    <FooterTemplate>
                                                                                                                        <a href="javascript:void(0);" id="arelatedsku" runat="server" onclick="openCenteredCrossSaleWindow(this.id,'');">select sku</a>
                                                                                                                        <br />
                                                                                                                        <a href="javascript:void(0);" id="arelatedColor" runat="server" onclick="openCenteredCrossSaleWindow(this.id,'');">Select Color</a>
                                                                                                                        <a href="javascript:void(0);" id="arelatedfabric" runat="server" onclick="openCenteredCrossSaleWindow(this.id,'');">Select Fabric</a>
                                                                                                                        <input type="hidden" id="hdnProductColorNewImg" runat="server" value="" />
                                                                                                                        <input type="hidden" id="hdnProductfabrictype" runat="server" value="" />
                                                                                                                        <input type="hidden" id="hdnProductfabriccode" runat="server" value="" />
                                                                                                                    </FooterTemplate>
                                                                                                                    <FooterStyle HorizontalAlign="center" />
                                                                                                                </asp:TemplateField>
                                                                                                                <asp:TemplateField HeaderText="&nbsp;Action">
                                                                                                                    <ItemTemplate>
 <div style="width:70px;">
                                                                                                                        <asp:ImageButton ID="imgSave" ToolTip="Save" runat="server" ImageUrl="/App_Themes/Gray/Images/save.png"
                                                                                                                            CommandName="Save" CommandArgument='<%# DataBinder.Eval(Container.DataItem,"VariantValueID") %>'
                                                                                                                            Visible="false" />
                                                                                                                        <asp:ImageButton ID="imgcancel" runat="server" ImageUrl="/admin/Images/isInactive.png"
                                                                                                                            CommandName="Exit" ToolTip="Cancel" CommandArgument='<%# DataBinder.Eval(Container.DataItem,"VariantValueID") %>'
                                                                                                                            Visible="false" />
                                                                                                                        <asp:ImageButton ID="imgEdit" ToolTip="Edit" runat="server" ImageUrl="/App_Themes/Gray/Images/Edit.gif"
                                                                                                                            CommandName="Edit" OnClientClick="chkHeight();" CommandArgument='<%# DataBinder.Eval(Container.DataItem,"VariantValueID") %>' />
                                                                                                                        <asp:ImageButton ID="imgDelete" ToolTip="Remove" OnClientClick="javascript:if(confirm('Are you sure want to delete this record?')){chkHeight(); return true;}else{return false;}"
                                                                                                                            runat="server" ImageUrl="/App_Themes/Gray/Images/delete-icon.png" CommandName="Remove"
                                                                                                                            CommandArgument='<%# DataBinder.Eval(Container.DataItem,"VariantValueID") %>' /><br /><br />
                                                                                                                        <asp:CheckBox ID="chkactive" Text=" Is Active" runat="server" ToolTip="Active/In active" />
                                                                                                                         <asp:ImageButton ID="Imageactivebn" ToolTip="update" style="display:none;"
                                                                                                                            runat="server" CommandName="updateactive" CommandArgument='<%# DataBinder.Eval(Container.DataItem,"VariantValueID") %>' /></div>
                                                                                                                    </ItemTemplate>
                                                                                                                    <HeaderStyle HorizontalAlign="Center" />
                                                                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                                                                    <FooterTemplate>
                                                                                                                        <asp:LinkButton ID="lnkAdd" runat="server" CommandName="Add" CssClass="buttons" Text="<strong><span>Add</span></strong>"
                                                                                                                            Style="float: none;"></asp:LinkButton><br /><br />
                                                                                                                        <asp:CheckBox ID="chkactive" Text=" Is Active" runat="server" Checked="true" ToolTip="Active/In active" />
                                                                                                                    </FooterTemplate>
                                                                                                                    <FooterStyle HorizontalAlign="center" />
                                                                                                                </asp:TemplateField>
                                                                                                                <asp:TemplateField>
                                                                                                                    <ItemTemplate>
                                                                                                                        <tr id="trsuboption" runat="server">
                                                                                                                            <td colspan="12" style="background-color: #d7d7d7; line-height: 18px; border: solid 1px #999999;">
                                                                                                                                <b>Sub Option</b>
                                                                                                                            </td>
                                                                                                                        </tr>
                                                                                                                        <tr>
                                                                                                                            <td colspan="100%">
                                                                                                                                <div id="divchildname" style="margin: 2px 0 0 0; position: relative; left: 15px; overflow: auto; width: 98%;">
                                                                                                                                    <asp:GridView ID="grdnamevaluelisting" runat="server" GridLines="None" AutoGenerateColumns="false"
                                                                                                                                        CssClass="checklist-main border-right-all" AllowPaging="false" ShowHeader="True"
                                                                                                                                        BorderColor="#eeeeee" BorderWidth="1px" BorderStyle="Solid" ShowFooter="true"
                                                                                                                                        DataKeyNames="VariantValueID" OnRowDataBound="grdnamevaluelisting_RowDataBound"
                                                                                                                                        OnRowCommand="grdnamevaluelisting_RowCommand" OnRowDeleting="grdnamevaluelisting_RowDeleting"
                                                                                                                                        OnRowEditing="grdnamevaluelisting_RowEditing">
                                                                                                                                        <Columns>
                                                                                                                                            <asp:TemplateField HeaderText="&nbsp;Value">
                                                                                                                                                <ItemTemplate>
                                                                                                                                                    <asp:Label ID="lttitle" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"VariantValue") %>'></asp:Label>
                                                                                                                                                    <input type="hidden" id="hdnVariantValueID" runat="server" value='<%# DataBinder.Eval(Container.DataItem,"VariantValueID") %>' />
                                                                                                                                                    <input type="hidden" id="hdnVariantID" runat="server" value='<%# DataBinder.Eval(Container.DataItem,"VariantID") %>' />
                                                                                                                                                    <asp:TextBox ID="txttitle" Style="width: 300px;" runat="server" Visible="false" CssClass="order-textfield"
                                                                                                                                                        onkeypress="AllowandlockQty(this.id);" onchange="AllowandlockQty(this.id);" onkeyup="AllowandlockQty(this.id);"
                                                                                                                                                        Text='<%# DataBinder.Eval(Container.DataItem,"VariantValue") %>'></asp:TextBox>
                                                                                                                                                </ItemTemplate>
                                                                                                                                                <HeaderStyle Width="11%" HorizontalAlign="left" VerticalAlign="Middle" CssClass="table-title" />
                                                                                                                                                <ItemStyle Width="11%" />
                                                                                                                                                <ItemStyle HorizontalAlign="left" />
                                                                                                                                                <FooterTemplate>
                                                                                                                                                    <asp:TextBox ID="txtTitle" CssClass="order-textfield" Width="300px" Text="" runat="server"></asp:TextBox>
                                                                                                                                                    <input type="hidden" id="hdnVariantIDSub" runat="server" value='<%# DataBinder.Eval(Container.DataItem,"VariantID") %>' />
                                                                                                                                                </FooterTemplate>
                                                                                                                                                <FooterStyle HorizontalAlign="Left" />
                                                                                                                                            </asp:TemplateField>
                                                                                                                                            <asp:TemplateField HeaderText="&nbsp;Price($)">
                                                                                                                                                <ItemTemplate>
                                                                                                                                                    <asp:Literal ID="ltprice" runat="server" Text='<%#Convert.ToDecimal(DataBinder.Eval(Container.DataItem,"VariantPrice")).ToString("F2") %>'></asp:Literal>
                                                                                                                                                    <asp:TextBox ID="txtprice" onkeypress="return keyRestrict(event,'0123456789.');"
                                                                                                                                                        Style="width: 80px; text-align: right;" runat="server" Visible="false" CssClass="order-textfield"
                                                                                                                                                        Text='<%#Convert.ToDecimal(DataBinder.Eval(Container.DataItem,"VariantPrice")).ToString("F2") %>'></asp:TextBox>
                                                                                                                                                </ItemTemplate>
                                                                                                                                                <HeaderStyle Width="10%" HorizontalAlign="right" VerticalAlign="Middle" CssClass="table-title" />
                                                                                                                                                <ItemStyle Width="10%" />
                                                                                                                                                <ItemStyle HorizontalAlign="right" />
                                                                                                                                                <FooterTemplate>
                                                                                                                                                    <asp:TextBox ID="txtPrice" onkeypress="return keyRestrict(event,'0123456789.');"
                                                                                                                                                        CssClass="order-textfield" Width="80px" Text="" Style="width: 80px; text-align: right;"
                                                                                                                                                        runat="server"></asp:TextBox>
                                                                                                                                                </FooterTemplate>
                                                                                                                                                <FooterStyle HorizontalAlign="right" />
                                                                                                                                            </asp:TemplateField>
                                                                                                                                            <asp:TemplateField HeaderText="&nbsp;Base Custom Price($)">
                                                                                                                                                <ItemTemplate>
                                                                                                                                                    <asp:Literal ID="ltBasecustomPrice" runat="server" Text='<%#Convert.ToDecimal(DataBinder.Eval(Container.DataItem,"BasecustomPrice")).ToString("F2") %>'></asp:Literal>
                                                                                                                                                    <asp:TextBox ID="txtBasecustomPrice" onkeypress="return keyRestrict(event,'0123456789.');"
                                                                                                                                                        Style="width: 80px; text-align: right;" runat="server" Visible="false" CssClass="order-textfield"
                                                                                                                                                        Text='<%#Convert.ToDecimal(DataBinder.Eval(Container.DataItem,"BasecustomPrice")).ToString("F2") %>'></asp:TextBox>
                                                                                                                                                </ItemTemplate>
                                                                                                                                                <HeaderStyle Width="10%" HorizontalAlign="right" VerticalAlign="Middle" CssClass="table-title" />
                                                                                                                                                <ItemStyle Width="10%" />
                                                                                                                                                <ItemStyle HorizontalAlign="right" />
                                                                                                                                                <FooterTemplate>
                                                                                                                                                    <asp:TextBox ID="txtBasecustomPrice" onkeypress="return keyRestrict(event,'0123456789.');"
                                                                                                                                                        CssClass="order-textfield" Width="80px" Text="" Style="width: 80px; text-align: right;"
                                                                                                                                                        runat="server"></asp:TextBox>
                                                                                                                                                </FooterTemplate>
                                                                                                                                                <FooterStyle HorizontalAlign="right" />
                                                                                                                                            </asp:TemplateField>
                                                                                                                                            <asp:TemplateField HeaderText="&nbsp;Quantity">
                                                                                                                                                <ItemTemplate>
                                                                                                                                                    <input type="hidden" id="hdnmainqty" runat="server" value='<%# DataBinder.Eval(Container.DataItem,"Inventory") %>' />
                                                                                                                                                    <a id="lnkEditInventory" runat="server" href="javascript:void(0);" style="color: #6A6A6A; cursor: pointer;">
                                                                                                                                                        <asp:Label ID="ltInventory" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"Inventory") %>'></asp:Label></a>
                                                                                                                                                </ItemTemplate>
                                                                                                                                                <HeaderStyle Width="11%" HorizontalAlign="Center" VerticalAlign="Middle" CssClass="table-title" />
                                                                                                                                                <ItemStyle Width="11%" />
                                                                                                                                                <ItemStyle HorizontalAlign="Center" />
                                                                                                                                                <FooterTemplate>
                                                                                                                                                    <%--<asp:TextBox ID="txtInventory" onkeypress="return keyRestrict(event,'0123456789');"
                                                                                                                                                        CssClass="order-textfield" Width="80px" Style="width: 80px; text-align: center;"
                                                                                                                                                        Text="" runat="server"></asp:TextBox>--%>
                                                                                                                                                    <input type="hidden" id="hdnmainqty" runat="server" value='0' />
                                                                                                                                                </FooterTemplate>
                                                                                                                                                <FooterStyle HorizontalAlign="Center" />
                                                                                                                                            </asp:TemplateField>
                                                                                                                                            <asp:TemplateField HeaderText="&nbsp;Allow Qty">
                                                                                                                                                <ItemTemplate>
                                                                                                                                                    <asp:Label ID="ltAllowInventory" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"AllowQuantity") %>'></asp:Label>
                                                                                                                                                    <input type="hidden" id="hdnAllowInventory" runat="server" value='<%# DataBinder.Eval(Container.DataItem,"AllowQuantity") %>' />
                                                                                                                                                    <asp:TextBox ID="txtAllowinventory" onkeypress="return keyRestrict(event,'0123456789');"
                                                                                                                                                        Style="width: 60px; text-align: center;" runat="server" Visible="false" CssClass="order-textfield"
                                                                                                                                                        Text='<%# DataBinder.Eval(Container.DataItem,"AllowQuantity") %>'></asp:TextBox>
                                                                                                                                                </ItemTemplate>
                                                                                                                                                <HeaderStyle Width="8%" HorizontalAlign="Center" VerticalAlign="Middle" CssClass="table-title" />
                                                                                                                                                <ItemStyle Width="8%" />
                                                                                                                                                <ItemStyle HorizontalAlign="Center" />
                                                                                                                                                <FooterTemplate>
                                                                                                                                                    <asp:TextBox ID="txtAllowinventory" onkeypress="return keyRestrict(event,'0123456789');"
                                                                                                                                                        CssClass="order-textfield" Width="60px" Style="text-align: center;" Text="" runat="server"></asp:TextBox>
                                                                                                                                                    <input type="hidden" id="hdnAllowInventory" runat="server" value='' />
                                                                                                                                                </FooterTemplate>
                                                                                                                                                <FooterStyle HorizontalAlign="Center" />
                                                                                                                                            </asp:TemplateField>
                                                                                                                                            <asp:TemplateField HeaderText="&nbsp;Lock Qty">
                                                                                                                                                <ItemTemplate>
                                                                                                                                                    <asp:Literal ID="ltLockInventory" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"LockQuantity") %>'></asp:Literal>
                                                                                                                                                    <input id="hdnLockInventory" type="hidden" runat="server" value='<%# DataBinder.Eval(Container.DataItem,"LockQuantity") %>' />
                                                                                                                                                    <asp:TextBox ID="txtLockinventory" onkeypress="return keyRestrict(event,'0123456789');"
                                                                                                                                                        onchange="AllowandlockQty(this.id);" onkeyup="AllowandlockQty(this.id);" Style="width: 80px; text-align: center;"
                                                                                                                                                        runat="server" Visible="false" CssClass="order-textfield"
                                                                                                                                                        Text='<%# DataBinder.Eval(Container.DataItem,"LockQuantity") %>'></asp:TextBox>
                                                                                                                                                </ItemTemplate>
                                                                                                                                                <HeaderStyle Width="10%" HorizontalAlign="Center" VerticalAlign="Middle" CssClass="table-title" />
                                                                                                                                                <ItemStyle Width="10%" />
                                                                                                                                                <ItemStyle HorizontalAlign="Center" />
                                                                                                                                                <FooterTemplate>
                                                                                                                                                    <asp:TextBox ID="txtLockinventory" onkeypress="return keyRestrict(event,'0123456789');"
                                                                                                                                                        onchange="AllowandlockQty(this.id);" onkeyup="AllowandlockQty(this.id);" CssClass="order-textfield"
                                                                                                                                                        Width="80px" Style="width: 80px; text-align: center;" Text="" runat="server"></asp:TextBox>
                                                                                                                                                </FooterTemplate>
                                                                                                                                                <FooterStyle HorizontalAlign="Center" />
                                                                                                                                            </asp:TemplateField>
                                                                                                                                            <asp:TemplateField HeaderText="&nbsp;Partner Heming Qty">
                                                                                                                                                <ItemTemplate>
                                                                                                                                                    <asp:Literal ID="ltAdditionalHemingQty" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"AddiHemingQty") %>'></asp:Literal>
                                                                                                                                                    <input id="hdnAddiHemingQty" type="hidden" runat="server" value='<%# DataBinder.Eval(Container.DataItem,"AddiHemingQty") %>' />
                                                                                                                                                    <asp:TextBox ID="txtAdditionalHemingQty" onkeypress="return keyRestrict(event,'0123456789');" Style="width: 70px; text-align: center;"
                                                                                                                                                        runat="server" Visible="false" CssClass="order-textfield" Text='<%# DataBinder.Eval(Container.DataItem,"AddiHemingQty") %>'></asp:TextBox>
                                                                                                                                                </ItemTemplate>
                                                                                                                                                <HeaderStyle Width="10%" HorizontalAlign="Center" VerticalAlign="Middle" CssClass="table-title" />
                                                                                                                                                <ItemStyle Width="10%" />
                                                                                                                                                <ItemStyle HorizontalAlign="Center" />
                                                                                                                                                <FooterTemplate>
                                                                                                                                                    <asp:TextBox ID="txtAdditionalHemingQty" onkeypress="return keyRestrict(event,'0123456789');"
                                                                                                                                                        CssClass="order-textfield" Width="70px" Style="text-align: center;" Text="" runat="server"></asp:TextBox>
                                                                                                                                                </FooterTemplate>
                                                                                                                                                <FooterStyle HorizontalAlign="Center" />
                                                                                                                                            </asp:TemplateField>
                                                                                                                                            <asp:TemplateField HeaderText="&nbsp;SKU">
                                                                                                                                                <ItemTemplate>
                                                                                                                                                    <asp:Literal ID="ltsku" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"SKU") %>'></asp:Literal>
                                                                                                                                                    <asp:TextBox ID="txtsku" Style="width: 120px;" runat="server" Visible="false" CssClass="order-textfield"
                                                                                                                                                        Text='<%# DataBinder.Eval(Container.DataItem,"SKU") %>'></asp:TextBox>
                                                                                                                                                </ItemTemplate>
                                                                                                                                                <HeaderStyle Width="10%" HorizontalAlign="left" VerticalAlign="Middle" CssClass="table-title" />
                                                                                                                                                <ItemStyle Width="10%" />
                                                                                                                                                <ItemStyle HorizontalAlign="left" />
                                                                                                                                                <FooterTemplate>
                                                                                                                                                    <asp:TextBox ID="txtSku" CssClass="order-textfield" Width="120px" Text="" runat="server"></asp:TextBox>
                                                                                                                                                </FooterTemplate>
                                                                                                                                                <FooterStyle HorizontalAlign="Left" />
                                                                                                                                            </asp:TemplateField>


                                                                                                                                            
                                                                                                                                            <asp:TemplateField HeaderText="Weight">
                                                                                                                                                <ItemTemplate>

                                                                                                                                                      <asp:Literal ID="ltweight" runat="server" Text='<%# String.Format("{0:0.00}",Convert.ToDecimal(DataBinder.Eval(Container.DataItem,"Weight")))%>'></asp:Literal>

                                                                                                                                                    <asp:TextBox ID="txtweight" runat="server" Visible="false" Text='<%# String.Format("{0:0.00}",Convert.ToDecimal(DataBinder.Eval(Container.DataItem,"Weight")))%>'
                                                                                                                                                        onkeypress="return keyRestrict(event,'0123456789.');"
                                                                                                                                                        CssClass="order-textfield" Width="40px"></asp:TextBox>
                                                                                                                                                </ItemTemplate>
                                                                                                                                                <HeaderStyle Width="8%" HorizontalAlign="left" VerticalAlign="Middle" CssClass="table-title" />
                                                                                                                                                <ItemStyle Width="8%" />
                                                                                                                                                <ItemStyle HorizontalAlign="left" />
                                                                                                                                                <FooterTemplate>
                                                                                                                                                    <asp:TextBox ID="txtweight" runat="server" CssClass="order-textfield"  Width="40px"
                                                                                                                                                         onkeypress="return keyRestrict(event,'0123456789.');"></asp:TextBox>
                                                                                                                                                </FooterTemplate>
                                                                                                                                                <FooterStyle HorizontalAlign="Left" />
                                                                                                                                            </asp:TemplateField>




                                                                                                                                            <asp:TemplateField HeaderText="&nbsp;UPC">
                                                                                                                                                <ItemTemplate>
                                                                                                                                                    <asp:Literal ID="ltupc" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"UPC") %>'></asp:Literal>
                                                                                                                                                    <asp:TextBox ID="txtupc" Style="width: 120px;" runat="server" Visible="false" CssClass="order-textfield"
                                                                                                                                                        Text='<%# DataBinder.Eval(Container.DataItem,"UPC") %>'></asp:TextBox>
                                                                                                                                                </ItemTemplate>
                                                                                                                                                <HeaderStyle Width="10%" HorizontalAlign="left" VerticalAlign="Middle" CssClass="table-title" />
                                                                                                                                                <ItemStyle Width="10%" />
                                                                                                                                                <ItemStyle HorizontalAlign="left" />
                                                                                                                                                <FooterTemplate>
                                                                                                                                                    <asp:TextBox ID="txtUpc" CssClass="order-textfield" Width="120px" Text="" runat="server"></asp:TextBox>
                                                                                                                                                </FooterTemplate>
                                                                                                                                                <FooterStyle HorizontalAlign="Left" />
                                                                                                                                            </asp:TemplateField>


                                                                                                                                            <asp:TemplateField HeaderText="&nbsp;Header">
                                                                                                                                                <ItemTemplate>
                                                                                                                                                    <asp:Literal ID="ltheader" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"Header") %>'></asp:Literal>
                                                                                                                                                    <asp:TextBox ID="txtheader" Style="width: 120px;" runat="server" Visible="false"
                                                                                                                                                        CssClass="order-textfield" Text='<%# DataBinder.Eval(Container.DataItem,"Header") %>'></asp:TextBox>
                                                                                                                                                </ItemTemplate>
                                                                                                                                                <HeaderStyle Width="8%" HorizontalAlign="left" VerticalAlign="Middle" CssClass="table-title" />
                                                                                                                                                <ItemStyle Width="8%" />
                                                                                                                                                <ItemStyle HorizontalAlign="left" />
                                                                                                                                                <FooterTemplate>
                                                                                                                                                    <asp:TextBox ID="txtHeader" CssClass="order-textfield" Width="100px" Text="" runat="server"></asp:TextBox>
                                                                                                                                                </FooterTemplate>
                                                                                                                                                <FooterStyle HorizontalAlign="Left" />
                                                                                                                                            </asp:TemplateField>



                                                                                                                                            <asp:TemplateField HeaderText="&nbsp;Back Order Date">
                                                                                                                                                <ItemTemplate>

                                                                                                                                                    <asp:TextBox ID="txtbackorderdate" Style="width: 80px; margin-right: 3px;" runat="server"
                                                                                                                                                        Text='<%# String.Format("{0:MM/dd/yyyy}",DataBinder.Eval(Container.DataItem,"BackOrderdate"))%>'
                                                                                                                                                        CssClass="order-textfield-picker"></asp:TextBox>
                                                                                                                                                </ItemTemplate>
                                                                                                                                                <HeaderStyle Width="8%" HorizontalAlign="left" VerticalAlign="Middle" CssClass="table-title" />
                                                                                                                                                <ItemStyle Width="8%" />
                                                                                                                                                <ItemStyle HorizontalAlign="left" />
                                                                                                                                                <FooterTemplate>
                                                                                                                                                    <asp:TextBox ID="txtbackorderdate" Style="width: 80px; margin-right: 3px;" runat="server"
                                                                                                                                                        Text=''
                                                                                                                                                        CssClass="order-textfield-picker"></asp:TextBox>
                                                                                                                                                </FooterTemplate>
                                                                                                                                                <FooterStyle HorizontalAlign="Left" />
                                                                                                                                            </asp:TemplateField>




                                                                                                                                            <asp:TemplateField HeaderText="&nbsp;Buy1&nbsp;Get1 Free">
                                                                                                                                                <ItemTemplate>
                                                                                                                                                    <asp:Literal ID="ltrBuy1get1" runat="server"></asp:Literal>
                                                                                                                                                    <img border="0" style="width: 12px;" id="imgBuy1get1" runat="server" />
                                                                                                                                                    <a href="javascript:void(0);" id="arelatedbuy1get1" style="color: #B92127; text-decoration: underline;"
                                                                                                                                                        runat="server">Buy 1 Get 1 Free </a>
                                                                                                                                                    <input type="hidden" id="hdnBuy1Get1" runat="server" value='<%# DataBinder.Eval(Container.DataItem,"Buy1Get1") %>' />
                                                                                                                                                     <input type="hidden" id="hdnupdatebuy1" runat="server" value='<%#DataBinder.Eval(Container.DataItem,"updatebuy1")%>' />
                                                                                                                                                </ItemTemplate>
                                                                                                                                                <HeaderStyle Width="11%" HorizontalAlign="Center" VerticalAlign="Middle" CssClass="table-title" />
                                                                                                                                                <ItemStyle Width="11%" />
                                                                                                                                                <ItemStyle HorizontalAlign="Center" />
                                                                                                                                                <FooterTemplate>
                                                                                                                                                    &nbsp;
                                                                                                                                                </FooterTemplate>
                                                                                                                                                <FooterStyle HorizontalAlign="Center" />
                                                                                                                                            </asp:TemplateField>
                                                                                                                                            <asp:TemplateField HeaderText="&nbsp;OnSale">
                                                                                                                                                <ItemTemplate>
                                                                                                                                                    <asp:Literal ID="ltronsale" runat="server"></asp:Literal>
                                                                                                                                                    <img border="0" style="width: 12px;" id="imgonsale" runat="server" />
                                                                                                                                                    <a href="javascript:void(0);" id="arelatedonsale" style="color: #B92127; text-decoration: underline;"
                                                                                                                                                        runat="server">OnSale</a>
                                                                                                                                                    <input type="hidden" id="hdnonsale" runat="server" value='<%# DataBinder.Eval(Container.DataItem,"OnSale") %>' /><input type="hidden" id="hdnactive" runat="server" value='<%# DataBinder.Eval(Container.DataItem,"VarActive") %>' />
                                                                                                                                                </ItemTemplate>
                                                                                                                                                <HeaderStyle Width="11%" HorizontalAlign="Center" VerticalAlign="Middle" CssClass="table-title" />
                                                                                                                                                <ItemStyle Width="11%" />
                                                                                                                                                <ItemStyle HorizontalAlign="Center" />
                                                                                                                                                <FooterTemplate>
                                                                                                                                                    &nbsp;
                                                                                                                                                </FooterTemplate>
                                                                                                                                                <FooterStyle HorizontalAlign="Center" />
                                                                                                                                            </asp:TemplateField>
                                                                                                                                            <asp:TemplateField HeaderText="&nbsp;Display Order">
                                                                                                                                                <ItemTemplate>
                                                                                                                                                    <asp:Literal ID="ltdisplayorder" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"DisplayOrder") %>'></asp:Literal>
                                                                                                                                                    <asp:TextBox ID="txtdisplayorder" onkeypress="return keyRestrict(event,'0123456789');"
                                                                                                                                                        Style="width: 60px; text-align: center;" runat="server" Visible="false" CssClass="order-textfield"
                                                                                                                                                        Text='<%# DataBinder.Eval(Container.DataItem,"DisplayOrder") %>'></asp:TextBox>
                                                                                                                                                </ItemTemplate>
                                                                                                                                                <HeaderStyle Width="10%" HorizontalAlign="center" VerticalAlign="Middle" CssClass="table-title" />
                                                                                                                                                <ItemStyle Width="10%" />
                                                                                                                                                <ItemStyle HorizontalAlign="center" />
                                                                                                                                                <FooterTemplate>
                                                                                                                                                    <asp:TextBox ID="txtDisplayorder" onkeypress="return keyRestrict(event,'0123456789');"
                                                                                                                                                        CssClass="order-textfield" Width="60px" Text="" Style="text-align: center;" runat="server"></asp:TextBox>
                                                                                                                                                </FooterTemplate>
                                                                                                                                                <FooterStyle HorizontalAlign="center" />
                                                                                                                                            </asp:TemplateField>
                                                                                                                                            <asp:TemplateField HeaderText="&nbsp;Related Product">
                                                                                                                                                <ItemTemplate>
                                                                                                                                                    <a href="javascript:void(0);" id="arelatedsku1" style="color: #6A6A6A;" runat="server">
                                                                                                                                                        <%# DataBinder.Eval(Container.DataItem,"selectSKU") %>
                                                                                                                                                    </a>
                                                                                                                                                    <input type="hidden" id="hdnrelatedsku" runat="server" value='<%# DataBinder.Eval(Container.DataItem,"selectSKU") %>' />

                                                                                                                                                </ItemTemplate>
                                                                                                                                                <HeaderStyle Width="10%" HorizontalAlign="center" VerticalAlign="Middle" CssClass="table-title" />
                                                                                                                                                <ItemStyle Width="10%" />
                                                                                                                                                <ItemStyle HorizontalAlign="center" />
                                                                                                                                                <FooterTemplate>
                                                                                                                                                    <a href="javascript:void(0);" id="arelatedsku1" style="color: #6A6A6A;" runat="server"
                                                                                                                                                        onclick="openCenteredCrossSaleWindow(this.id,'');">select sku</a>
                                                                                                                                                </FooterTemplate>
                                                                                                                                                <FooterStyle HorizontalAlign="center" />
                                                                                                                                            </asp:TemplateField>
                                                                                                                                            <asp:TemplateField HeaderText="&nbsp;Action">
                                                                                                                                                <ItemTemplate>
<div style="width:70px;">
                                                                                                                                                    <asp:ImageButton ID="imgSave" ToolTip="Save" runat="server" ImageUrl="/App_Themes/Gray/Images/save.png"
                                                                                                                                                        CommandName="Save1" CommandArgument='<%# DataBinder.Eval(Container.DataItem,"VariantValueID") %>'
                                                                                                                                                        Visible="false" />
                                                                                                                                                    <asp:ImageButton ID="imgcancel" runat="server" ImageUrl="/admin/Images/isInactive.png"
                                                                                                                                                        CommandName="Exit1" ToolTip="Cancel" CommandArgument='<%# DataBinder.Eval(Container.DataItem,"VariantValueID") %>'
                                                                                                                                                        Visible="false" />
                                                                                                                                                    <asp:ImageButton ID="imgEdit" ToolTip="Edit" runat="server" ImageUrl="/App_Themes/Gray/Images/Edit.gif"
                                                                                                                                                        CommandName="Edit" OnClientClick="chkHeight();" CommandArgument='<%# DataBinder.Eval(Container.DataItem,"VariantValueID") %>' />
                                                                                                                                                    <asp:ImageButton ID="imgDelete" ToolTip="Remove" OnClientClick="javascript:if(confirm('Are you sure want to delete this record?')){chkHeight(); return true;}else{return false;}"
                                                                                                                                                        runat="server" ImageUrl="/App_Themes/Gray/Images/delete-icon.png" CommandName="Remove1"
                                                                                                                                                        CommandArgument='<%# DataBinder.Eval(Container.DataItem,"VariantValueID") %>' /><br /><br />
                                                                                                                                                     <asp:CheckBox ID="chkactive" Text=" Is Active" runat="server" ToolTip="Active/In active" />
                                                                                                                                                    <asp:ImageButton ID="Imageactivebn" ToolTip="update" style="display:none;"
                                                                                                                            runat="server" CommandName="updateactive1" CommandArgument='<%# DataBinder.Eval(Container.DataItem,"VariantValueID") %>' /></div>
                                                                                                                                                </ItemTemplate>
                                                                                                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                                                                                                <ItemStyle HorizontalAlign="Center" />
                                                                                                                                                <FooterTemplate>
                                                                                                                                                    <asp:LinkButton ID="lnkAdd" runat="server" CommandName="Add1" CssClass="buttons"
                                                                                                                                                        Text="<strong><spa>Add</span></strong>" Style="float: none;"></asp:LinkButton><br /><br />
                                                                                                                                                     <asp:CheckBox ID="chkactive" Text=" Is Active" runat="server" Checked="true" ToolTip="Active/In active" />
                                                                                                                                                </FooterTemplate>
                                                                                                                                                <FooterStyle HorizontalAlign="center" />
                                                                                                                                            </asp:TemplateField>
                                                                                                                                        </Columns>
                                                                                                                                        <AlternatingRowStyle HorizontalAlign="left" />
                                                                                                                                    </asp:GridView>
                                                                                                                                </div>
                                                                                                                            </td>
                                                                                                                        </tr>
                                                                                                                    </ItemTemplate>
                                                                                                                </asp:TemplateField>
                                                                                                            </Columns>
                                                                                                            <AlternatingRowStyle CssClass="altrowmainsub" />
                                                                                                            <RowStyle CssClass="altrowmainsub" />
                                                                                                            <FooterStyle CssClass="altrowmainsub" />
                                                                                                        </asp:GridView>
                                                                                                    </div>
                                                                                                </td>
                                                                                            </tr>
                                                                                        </ItemTemplate>
                                                                                    </asp:TemplateField>
                                                                                </Columns>
                                                                                <AlternatingRowStyle CssClass="altrowmain" HorizontalAlign="left" />
                                                                                <RowStyle CssClass="altrowmain" />
                                                                            </asp:GridView>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </fieldset>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="left">
                                                            <span style="padding-left: 20px"></span>
                                                            <input type="hidden" value="0" id="hdnVariantId" runat="server" />
                                                            <input type="hidden" value="0" id="hdnVariantIdGrid" runat="server" />
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
            <tr>
                <td height="10" align="left" valign="top">
                    <img src="/App_Themes/<%=Page.Theme %>/images/spacer.gif" width="1" height="10" />
                </td>
            </tr>
            <tr>
                <td>
                    <div style="display: none;">
                        <input type="hidden" id="hdnrelatedcolor" runat="server" value="" />
                        <input type="hidden" id="hdnrelatedsku1" runat="server" value='' />
                        <input type="hidden" id="hdnrelatedsku" runat="server" value='' />
                    </div>
                    &nbsp;
                </td>
            </tr>
        </table>
    </div>
    <div id="prepage" style="position: absolute; font-family: arial; font-size: 16; left: 0px; top: 0px; background-color: #000; opacity: 0.7; filter: alpha(opacity=70); layer-background-color: white; height: 100%; width: 100%; z-index: 1000; display: none;">
        <table width="20%" style="margin-top: 25%;" align="center">
            <tr>
                <td align="center" style="color: #fff;" valign="middle">
                    <img alt="" src="/images/loding.png" /><br />
                    <b>Loading ... ... Please wait!</b>
                </td>
            </tr>
        </table>
    </div>
    <div id="popupContact" style="z-index: 1000001; top: 30px; padding: 0px; width: 750px; height: 300px; position: fixed;">
        <table border="0" cellspacing="0" cellpadding="0" class="table_border">
            <tr>
                <td align="left">
                    <iframe id="frmdisplay" frameborder="0" height="300" scrolling="auto"></iframe>
                </td>
            </tr>
        </table>
    </div>
    <div id="backgroundPopup" style="z-index: 1000000;">
    </div>
    <div style="display: none;">
        <input type="button" id="btnreadmore" />
        <asp:Button ID="btnReBindData" runat="server" OnClick="btnReBindData_Click" OnClientClick="chkHeight();" />
        <input type="hidden" id="hdngridid" runat="server" value="" />
    </div>
    <!--start tab--->
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-1.2.6.min.js"></script>
    <script src="/App_Themes/<%=Page.Theme %>/js/tabs.js" type="text/javascript"></script>



    <%--<div id="test"> <ul id="ultest"> <li id="listest"><a id ="atest"><img id="imgtest" /></a></li></ul></div>

    <input id="Button1" type="button" value="button" onclick="test();" />
    <script type="text/javascript">

        function test()
        {
            $("#test > ul > li > a > img").css("border", "3px double red");
            //alert($("#test ul li a img").find().id);

        }

    </script>--%>
</asp:Content>
