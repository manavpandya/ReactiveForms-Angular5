<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MultipleShippingPopupForLabel.aspx.cs" Inherits="Solution.UI.Web.ADMIN.Orders.MultipleShippingPopupForLabel" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <script type="text/javascript" src="/App_Themes/gray/js/jquery-alerts-main.js"></script>
    <script type="text/javascript" src="/App_Themes/gray/js/jquery-calender-custom.js"></script>
    <script type="text/javascript" src="/App_Themes/gray/js/tabs.js"></script>
    <script type="text/javascript" src="/App_Themes/gray/js/jquery-alerts.js"></script>
    <script type="text/javascript" src="/js/popup.js"></script>
    <link href="/css/general.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
 function chkHeight() {
            var windowHeight = 0;
            windowHeight = $(document).height(); //window.innerHeight;
            document.getElementById('prepage').style.height = windowHeight + 'px';
            document.getElementById('prepage').style.display = '';
          
        }
 function imggetmethodweight() {
           
            if (document.getElementById('ddlboxesall') != null && document.getElementById('ddlboxesall').selectedIndex == 0) {
                
                document.getElementById('ddlboxesall').focus();
                return false;
            }
            if (document.getElementById('txtProWeightall') != null && document.getElementById('txtProWeightall').value == '') {
                
                document.getElementById('txtProWeightall').focus();
                return false;
            }
            else if (document.getElementById('txtProWeightall') != null && parseInt(document.getElementById('txtProWeightall').value) == parseInt(0)) {
                
                document.getElementById('txtProWeightall').focus();
                return false;
            }
             chkHeight();
            document.getElementById('imggetmethod').click();
            return false;
        }
        function checkqtyvalid(id)
        {
            var orderqty = document.getElementById(id.replace('txtShippedQty', 'lblQty')).innerHTML;
            var ordershipqty = document.getElementById(id.replace('txtShippedQty', 'lblOldQty')).innerHTML;
            var ordercurrentqty = document.getElementById(id).value;
            var ordercurrentweight = document.getElementById(id.replace('txtShippedQty', 'lblweight')).innerHTML;
            orderqty = parseFloat(orderqty) - parseFloat(ordershipqty);
            if (parseFloat(ordercurrentqty) > parseFloat(orderqty))
            {
                document.getElementById(id).value = orderqty;
                //ordercurrentweight = parseFloat(orderqty) * parseFloat(ordercurrentweight);
               // document.getElementById(id.replace('txtShippedQty', 'txtProWeight')).value = parseFloat(parseFloat(document.getElementById(id).value) * parseFloat(ordercurrentweight)).toFixed(2);
            }
            else
            {
 if((document.getElementById(id).value == '') || (parseFloat(document.getElementById(id).value) == parseFloat(0)))
                {
                    document.getElementById(id).value = orderqty;
                }
                //document.getElementById(id.replace('txtShippedQty', 'txtProWeight')).value = parseFloat(parseFloat(document.getElementById(id).value) * parseFloat(ordercurrentweight)).toFixed(2);
            }
            //document.getElementById(id.replace('txtShippedQty', 'btnRebindShippingMethods')).click();
            //
            return false;
        }

        function CheckValidation(id) {
            var ids = id;
            if (document.getElementById(ids.replace('btnEdit', 'chkgeneratelbl')) != null && document.getElementById(ids.replace('btnEdit', 'chkgeneratelbl')).checked == false) {
                alert('Please select Item.');
                return false;
            }
            if (document.getElementById(ids.replace('btnEdit', 'txtShippedQty')) != null && document.getElementById(ids.replace('btnEdit', 'txtShippedQty')).value == '') {
                alert('Please enter quantity.');
                document.getElementById(ids.replace('btnEdit', 'txtShippedQty')).focus();
                return false;
            }
            else if (document.getElementById(ids.replace('btnEdit', 'txtShippedQty')) != null && parseInt(document.getElementById(ids.replace('btnEdit', 'txtShippedQty')).value) == parseInt(0)) {
                alert('Please enter valid quantity.');
                document.getElementById(ids.replace('btnEdit', 'txtShippedQty')).focus();
                return false;
            }
            else if (document.getElementById(ids.replace('btnEdit', 'ddlboxes')) != null &&  document.getElementById(ids.replace('btnEdit', 'ddlboxes')).selectedIndex == 0 ) {
                alert('Please select box.');
                document.getElementById(ids.replace('btnEdit', 'ddlboxes')).focus();
                return false;
            }
            if (document.getElementById(ids.replace('btnEdit', 'txtProWeight')) != null && document.getElementById(ids.replace('btnEdit', 'txtProWeight')).value == '') {
                alert('Please enter weight.');
                document.getElementById(ids.replace('btnEdit', 'txtProWeight')).focus();
                return false;
            }
            else if (document.getElementById(ids.replace('btnEdit', 'txtProWeight')) != null && parseFloat(document.getElementById(ids.replace('btnEdit', 'txtProWeight')).value) == parseFloat(0)) {
                alert('Please enter valid weight.');
                document.getElementById(ids.replace('btnEdit', 'txtProWeight')).focus();
                return false;
            }
            else if (document.getElementById(ids.replace('btnEdit', 'ddlShippingMethod')) != null && document.getElementById(ids.replace('btnEdit', 'ddlShippingMethod')).selectedIndex <= -1) {
                alert('Please select Shipping Method.');
                document.getElementById(ids.replace('btnEdit', 'ddlShippingMethod')).focus();
                return false;
            }
 chkHeight();
        }
        //function imggetmethod()
        //{
        //    //if (document.getElementById('txtHeightgridall') != null && document.getElementById('txtHeightgridall').value =='') {
        //    //    alert('Please Enter Height.');
        //    //    document.getElementById('txtHeightgridall').focus();
        //    //    return false;
        //    //}
        //    //else if (document.getElementById('txtHeightgridall') != null &&  parseInt(document.getElementById('txtHeightgridall').value) ==   parseInt(0)) {
        //    //    alert('Please Enter valid Height.');
        //    //    document.getElementById('txtHeightgridall').focus();
        //    //    return false;
        //    //}
        //    //if (document.getElementById('txtWidthgridall') != null && document.getElementById('txtWidthgridall').value == '') {
        //    //    alert('Please Enter Width.');
        //    //    document.getElementById('txtWidthgridall').focus();
        //    //    return false;
        //    //}
        //    //else if (document.getElementById('txtWidthgridall') != null && parseInt(document.getElementById('txtWidthgridall').value) == parseInt(0)) {
        //    //    alert('Please Enter valid Width.');
        //    //    document.getElementById('txtWidthgridall').focus();
        //    //    return false;
        //    //}
        //    //if (document.getElementById('txtLengthgridall') != null && document.getElementById('txtLengthgridall').value == '') {
        //    //    alert('Please Enter Length.');
        //    //    document.getElementById('txtLengthgridall').focus();
        //    //    return false;
        //    //}
        //    //else if (document.getElementById('txtLengthgridall') != null && parseInt(document.getElementById('txtLengthgridall').value) == parseInt(0)) {
        //    //    alert('Please Enter valid Length.');
        //    //    document.getElementById('txtLengthgridall').focus();
        //    //    return false;
        //    //}

        //    if (document.getElementById('txtProWeightall') != null && document.getElementById('txtProWeightall').value == '') {
        //        alert('Please Enter Weight.');
        //        document.getElementById('txtProWeightall').focus();
        //        return false;
        //    }
        //    else if (document.getElementById('txtProWeightall') != null && parseInt(document.getElementById('txtProWeightall').value) == parseInt(0)) {
        //        alert('Please Enter valid Weight.');
        //        document.getElementById('txtProWeightall').focus();
        //        return false;
        //    }
        //    return true;
        //}
        function imggetmethod() {
            //if (document.getElementById('txtHeightgridall') != null && document.getElementById('txtHeightgridall').value == '') {
            //    alert('Please Enter Height.');
            //    document.getElementById('txtHeightgridall').focus();
            //    return false;
            //}
            //else if (document.getElementById('txtHeightgridall') != null && parseInt(document.getElementById('txtHeightgridall').value) == parseInt(0)) {
            //    alert('Please Enter valid Height.');
            //    document.getElementById('txtHeightgridall').focus();
            //    return false;
            //}
            //if (document.getElementById('txtWidthgridall') != null && document.getElementById('txtWidthgridall').value == '') {
            //    alert('Please Enter Width.');
            //    document.getElementById('txtWidthgridall').focus();
            //    return false;
            //}
            //else if (document.getElementById('txtWidthgridall') != null && parseInt(document.getElementById('txtWidthgridall').value) == parseInt(0)) {
            //    alert('Please Enter valid Width.');
            //    document.getElementById('txtWidthgridall').focus();
            //    return false;
            //}
            //if (document.getElementById('txtLengthgridall') != null && document.getElementById('txtLengthgridall').value == '') {
            //    alert('Please Enter Length.');
            //    document.getElementById('txtLengthgridall').focus();
            //    return false;
            //}
            //else if (document.getElementById('txtLengthgridall') != null && parseInt(document.getElementById('txtLengthgridall').value) == parseInt(0)) {
            //    alert('Please Enter valid Length.');
            //    document.getElementById('txtLengthgridall').focus();
            //    return false;
            //}
            if (document.getElementById('ddlboxesall') != null && document.getElementById('ddlboxesall').selectedIndex == 0) {
            alert('Please select box.');
            document.getElementById('ddlboxesall').focus();
            return false;
        }
            if (document.getElementById('txtProWeightall') != null && document.getElementById('txtProWeightall').value == '') {
                alert('Please Enter Weight.');
                document.getElementById('txtProWeightall').focus();
                return false;
            }
            else if (document.getElementById('txtProWeightall') != null && parseInt(document.getElementById('txtProWeightall').value) == parseInt(0)) {
                alert('Please Enter valid Weight.');
                document.getElementById('txtProWeightall').focus();
                return false;
            }
            //if (document.getElementById('ddlShippingMethodall') != null && document.getElementById('ddlShippingMethodall').selectedIndex <=-1) {
            //    alert('Please select Shipping Method.');
            //    document.getElementById('ddlShippingMethodall').focus();
            //    return false;
            //}
 chkHeight();
            return true;
        }
        function btngeneratealllabelall() {
            //if (document.getElementById('txtHeightgridall') != null && document.getElementById('txtHeightgridall').value == '') {
            //    alert('Please Enter Height.');
            //    document.getElementById('txtHeightgridall').focus();
            //    return false;
            //}
            //else if (document.getElementById('txtHeightgridall') != null && parseInt(document.getElementById('txtHeightgridall').value) == parseInt(0)) {
            //    alert('Please Enter valid Height.');
            //    document.getElementById('txtHeightgridall').focus();
            //    return false;
            //}
            //if (document.getElementById('txtWidthgridall') != null && document.getElementById('txtWidthgridall').value == '') {
            //    alert('Please Enter Width.');
            //    document.getElementById('txtWidthgridall').focus();
            //    return false;
            //}
            //else if (document.getElementById('txtWidthgridall') != null && parseInt(document.getElementById('txtWidthgridall').value) == parseInt(0)) {
            //    alert('Please Enter valid Width.');
            //    document.getElementById('txtWidthgridall').focus();
            //    return false;
            //}
            //if (document.getElementById('txtLengthgridall') != null && document.getElementById('txtLengthgridall').value == '') {
            //    alert('Please Enter Length.');
            //    document.getElementById('txtLengthgridall').focus();
            //    return false;
            //}
            //else if (document.getElementById('txtLengthgridall') != null && parseInt(document.getElementById('txtLengthgridall').value) == parseInt(0)) {
            //    alert('Please Enter valid Length.');
            //    document.getElementById('txtLengthgridall').focus();
            //    return false;
            //}
            if (document.getElementById('ddlboxesall') != null && document.getElementById('ddlboxesall').selectedIndex == 0) {
                alert('Please select box.');
                document.getElementById('ddlboxesall').focus();
                return false;
            }
            if (document.getElementById('txtProWeightall') != null && document.getElementById('txtProWeightall').value == '') {
                alert('Please Enter Weight.');
                document.getElementById('txtProWeightall').focus();
                return false;
            }
            else if (document.getElementById('txtProWeightall') != null && parseInt(document.getElementById('txtProWeightall').value) == parseInt(0)) {
                alert('Please Enter valid Weight.');
                document.getElementById('txtProWeightall').focus();
                return false;
            }
            if (document.getElementById('ddlShippingMethodall') != null && document.getElementById('ddlShippingMethodall').selectedIndex <=-1) {
                alert('Please select Shipping Method.');
                document.getElementById('ddlShippingMethodall').focus();
                return false;
            }
 chkHeight();
            return true;
        }
        function DeleteTrackingNo(id, trackingno) {
            if(confirm('Are you sure want to remove? \n this shipment will be remove from amazon'))
            {
                document.getElementById('hdnAmazonCartID').value = id;
                document.getElementById('hdnAmazonTrackinNo').value = trackingno;
                document.getElementById('btnAmazonRemove').click();
            }
          
         
            return false;
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

        function getWeightall() {
            var allchk = document.getElementById('grdShipping').getElementsByTagName('input');
            var tt = 0;
            var weightall = "0.00";
            if (allchk.length > 0) {
                for (i = 0; i < allchk.length; i++) {
                    var elt = allchk[i];
                    if (elt.id.toString().indexOf('_chkgeneratelbl_') > -1 && elt.checked == false) {

                        if (document.getElementById(elt.id.toString().replace('_chkgeneratelbl_', '_txtShippedQty_')) != null && document.getElementById(elt.id.toString().replace('_chkgeneratelbl_', '_txtShippedQty_')).value != '') {
                            weightall = parseFloat(weightall) + parseFloat(parseFloat(document.getElementById(elt.id.toString().replace('_chkgeneratelbl_', '_txtShippedQty_')).value) * parseFloat(document.getElementById(elt.id.toString().replace('_chkgeneratelbl_', '_lblweight_')).innerHTML));
                        }

var tttt= checkqtyvalid(elt.id.toString().replace('_chkgeneratelbl_', '_txtShippedQty_'));
                        tt = 1;
                    }
                     
                }
            }
            if (parseInt(tt) > 0) {
                document.getElementById('allorderqty').style.display = '';
            }
            else {
                document.getElementById('allorderqty').style.display = 'none';
            }
            document.getElementById('txtProWeightall').value = weightall;
        }

        function calculateAll() {

            var allchk = document.getElementById('grdShipping').getElementsByTagName('input');
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


            var allElts = document.getElementById('grdShipping').getElementsByTagName('input')
            var i; var hdncheck;
            for (i = 0; i < allElts.length; i++) {
                var elt = allElts[i];
                if (elt.id.toString().indexOf('grdShipping_txt') > -1) {
                    var split = elt.id.toString().split('_');
                    var hdnid = split[3];
                    hdncheck = document.getElementById('grdShipping_hdnchecklblgenerate_' + hdnid);
                    if (hdncheck != null && hdncheck.value == "0") {
                        var Height = parseInt(document.getElementById("grdShipping_txtHeightgrid_" + hdnid + "").value);
                        var Width = parseInt(document.getElementById("grdShipping_txtWidthgrid_" + hdnid + "").value);
                        var Length = parseInt(document.getElementById("grdShipping_txtLengthgrid_" + hdnid + "").value);
                        var ProWeight = parseFloat(document.getElementById("grdShipping_txtProWeight_" + hdnid + "").value);

                        if (isNaN(Height) || Height <= 0) {
                            jAlert('Please provide height for shipment.', 'Message', 'grdShipping_txtHeightgrid_' + hdnid);
                            return false;
                        }
                        else if (isNaN(Width) || Width <= 0) {
                            jAlert('Please provide Width for shipment.', 'Message', 'grdShipping_txtWidthgrid_' + hdnid);
                            return false;
                        }
                        else if (isNaN(Length) || Length <= 0) {
                            jAlert('Please provide Length for shipment.', 'Message', 'grdShipping_txtLengthgrid_' + hdnid);
                            return false;
                        }
                        else if (isNaN(ProWeight) || ProWeight <= 0) {
                            jAlert('Please provide Weight for shipment.', 'Message', 'grdShipping_txtProWeight_' + hdnid);
                            return false;
                        }
                    }
                }
            }
            
            return true;
        }

        function chkHeight() {
            var windowHeight = 0;
            windowHeight = $(document).height(); //window.innerHeight;
            document.getElementById('prepage').style.height = windowHeight + 'px';
            document.getElementById('prepage').style.display = '';
            $(".removetr").hide(); window.scrollTo(0, 0);
        }
        function ShowDiv(imgid, divid) {
            if (document.getElementById(imgid) != null) {
                var src = document.getElementById(imgid).src;
                if (src.indexOf('expand.gif') > -1) {
                    document.getElementById(imgid).src = src.replace('expand.gif', 'minimize.png');
                    document.getElementById(imgid).title = 'Minimize';
                    document.getElementById(imgid).alt = 'Minimize';
                    document.getElementById(imgid).style.marginTop = "4px";
                    document.getElementById(imgid).className = 'minimize';
                    if (document.getElementById(divid)) {
                        document.getElementById(divid).style.display = '';
                    }
                }
                else if (src.indexOf('minimize.png') > -1) {
                    document.getElementById(imgid).src = src.replace('minimize.png', 'expand.gif');
                    document.getElementById(imgid).title = 'Show';
                    document.getElementById(imgid).style.marginTop = "0px";
                    document.getElementById(imgid).alt = 'Show';
                    document.getElementById(imgid).className = 'close';
                    if (document.getElementById(divid)) {
                        document.getElementById(divid).style.display = 'none';
                    }
                }
            }
        }

        function selectAll1(idname) {
            var on;

            if ('chkallgeneratelbl' == idname) {
                if (document.getElementById('grvOrderlist_chkallgeneratelbl') != null) {
                    if (document.getElementById('grvOrderlist_chkallgeneratelbl').checked == true) {
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
        }


        function Clacu(row, name, idnew) {
            var ids = name;
            if (document.getElementById(ids.replace('txtProWeight', 'chkgeneratelbl')) != null && document.getElementById(ids.replace('txtProWeight', 'chkgeneratelbl')).checked == false) {
                alert('Please select Item.');
                return false;
            }
            if (document.getElementById(ids.replace('txtProWeight', 'txtShippedQty')) != null && document.getElementById(ids.replace('txtProWeight', 'txtShippedQty')).value == '') {
                alert('Please enter quantity.');
                document.getElementById(ids.replace('txtProWeight', 'txtShippedQty')).focus();
                return false;
            }
            else if (document.getElementById(ids.replace('txtProWeight', 'txtShippedQty')) != null && parseInt(document.getElementById(ids.replace('txtProWeight', 'txtShippedQty')).value) == parseInt(0)) {
                alert('Please enter valid quantity.');
                document.getElementById(ids.replace('txtProWeight', 'txtShippedQty')).focus();
                return false;
            }
            else if (document.getElementById(ids.replace('txtProWeight', 'ddlboxes')) != null && document.getElementById(ids.replace('txtProWeight', 'ddlboxes')).selectedIndex == 0) {
                alert('Please select box.');
                document.getElementById(ids.replace('txtProWeight', 'ddlboxes')).focus();
                return false;
            }
            if (document.getElementById(ids.replace('txtProWeight', 'txtProWeight')) != null && document.getElementById(ids.replace('txtProWeight', 'txtProWeight')).value == '') {
                alert('Please enter weight.');
                document.getElementById(ids.replace('txtProWeight', 'txtProWeight')).focus();
                return false;
            }
            else if (document.getElementById(ids.replace('txtProWeight', 'txtProWeight')) != null && parseFloat(document.getElementById(ids.replace('txtProWeight', 'txtProWeight')).value) == parseFloat(0)) {
                alert('Please enter valid weight.');
                document.getElementById(ids.replace('txtProWeight', 'txtProWeight')).focus();
                return false;
            }
 if (idnew.toString().toLowerCase().indexOf('txtproweight') <=-1)
            {
                chkHeight();
            }
// chkHeight();
            return true;
            //var id = document.getElementById("grdShipping_ddlDimensions_" + row)[document.getElementById("grdShipping_ddlDimensions_" + row).selectedIndex].text;
            //if (id.toLowerCase() == "select") {
            //    jAlert('Please select dimension shipment.', 'Message');
            //    return false;
            //}

            //if (id.toLowerCase() == "other") {
            //    if (document.getElementById("grdShipping_ddlDimensions_" + row))
            //        var Height = parseFloat(document.getElementById("grdShipping_txtHeightgrid_" + row + "").value);
            //    var Width = parseFloat(document.getElementById("grdShipping_txtWidthgrid_" + row + "").value);
            //    var Length = parseFloat(document.getElementById("grdShipping_txtLengthgrid_" + row + "").value);
            //    var ProWeight = parseFloat(document.getElementById("grdShipping_txtProWeight_" + row + "").value);
            //    if (isNaN(Height) || Height <= 0) {
            //        jAlert('Please provide height for shipment.', 'Message', 'grdShipping_txtHeightgrid_' + row);
            //        return false;
            //    }
            //    else if (isNaN(Width) || Width <= 0) {
            //        jAlert('Please provide Width for shipment.', 'Message', 'grdShipping_txtWidthgrid_' + row);
            //        return false;
            //    }
            //    else if (isNaN(Length) || Length <= 0) {
            //        jAlert('Please provide Length for shipment.', 'Message', 'grdShipping_txtLengthgrid_' + row);
            //        return false;
            //    }
            //    else if (isNaN(ProWeight) || ProWeight <= 0) {
            //        jAlert('Please provide Weight for shipment.', 'Message', 'grdShipping_txtProWeight_' + row);
            //        return false;
            //    }
            //}
            //else {
            //    chkHeight();
            //    __doPostBack(name, '');
            //    return true;
            //}
            return true;
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <table border="0" width="100%" cellspacing="0" cellpadding="0">
            <tbody>
                <tr>
                    <td class="border-td" align="left" width="100%" valign="top">
                        <table border="0" width="100%" cellspacing="0" cellpadding="0">
                            <tbody>
                                <tr>
                                    <td class="border-td-sub" align="right">
                                        <table class="content-table" border="0" width="100%" cellspacing="0" cellpadding="0">
                                            <tbody>
                                                <tr>
                                                    <th>
                                                        <div class="main-title-left">
                                                            <img src="/App_Themes/Gray/icon/order-totals.png" alt="Order Totals" title="Order Totals" class="img-left">
                                                            <h2><font style="font-siz: 18px;"></font>&nbsp;&nbsp;Order#
                                                                <asp:Label ID="lblordernumer" runat="server"></asp:Label>
                                                            </h2>
                                                        </div>
                                                        <div class="main-title-right">
                                                            <a href="javascript:void(0);" onclick="ShowDiv('imgOderTotals','trOrderTotals');" title="Minimize">
                                                                <img src="/App_Themes/Gray/images/minimize.png" id="imgOderTotals" alt="Minimize" title="Minimize" class="minimize"></a>
                                                        </div>
                                                    </th>
                                                </tr>
                                                <tr id="trOrderTotals">
                                                    <td style="background: #FFFEF3;" align="right">
                                                        <table style="width: 100%; text-align: right; border-top: none;" class="order-totals-table">
                                                            <tbody>
                                                                <tr>
                                                                    <td colspan="2">
                                                                        <table width="100%" style="border: 1px solid #DFDFDF;" cellpadding="0" cellspacing="0"
                                                                            align="center">
                                                                            <tr valign="top" class="altrow">
                                                                                <td style="padding-left: 5px; width: 9%;">Ship To:
                                                                                </td>
                                                                                <td style="text-align: left;">
                                                                                    <asp:Literal ID="ltShippingTo" runat="server"></asp:Literal>
                                                                                    <br />
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                </tr>
                                                                <tr>

                                                                    <td colspan="2">
                                                                        <asp:GridView ID="grdShipping" runat="server" Width="100%" AutoGenerateColumns="false"
                                                                            OnRowDataBound="grdShipping_RowDataBound" ShowFooter="false" OnRowCommand="grdShipping_RowCommand"
                                                                            CssClass="table-noneforOrder" EmptyDataText="Order not imported to NAV." EmptyDataRowStyle-Font-Bold="true" EmptyDataRowStyle-Font-Size="Medium" EmptyDataRowStyle-HorizontalAlign="Center" EmptyDataRowStyle-ForeColor="Red" CellSpacing="0"
                                                                            CellPadding="5" border="1" Style="width: 100%; border-collapse: collapse;">
                                                                            <Columns>
                                                                                <asp:TemplateField HeaderText="Seelct">
                                                                                    <HeaderTemplate>
                                                                                        <%--  <asp:CheckBox ID="chkallgeneratelbl" onchange="selectAll1('chkallgeneratelbl');"
                                                                                    onclick="selectAll1('chkallgeneratelbl');" runat="server" Style="margin-right: 18px;"
                                                                                    CssClass="input-checkbox" /><br />--%>
                                                                                Select
                                                                                    </HeaderTemplate>
                                                                                    <HeaderStyle HorizontalAlign="Center" CssClass="center" />
                                                                                    <ItemTemplate>
                                                                                        <asp:CheckBox ID="chkgeneratelbl" runat="server" Style="margin-right: 18px;" CssClass="input-checkbox" />
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle HorizontalAlign="Center" CssClass="center" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField Visible="False">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblorderNumber" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"OrderNumber") %>'></asp:Label>
                                                                                        <asp:Label ID="lblProductID" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"RefProductID") %>'></asp:Label>
                                                                                        <asp:Label ID="lblCustomCartID" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"OrderedCustomCartID") %>'></asp:Label>
                                                                                        <asp:Label ID="lblVariantNames" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"VariantNames") %>'></asp:Label>
                                                                                        <asp:Label ID="lblVariantValues" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"VariantValues") %>'></asp:Label>
                                                                                    </ItemTemplate>
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField>
                                                                                    <HeaderTemplate>
                                                                                        Product Name
                                                                                    </HeaderTemplate>
                                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" Width="15%" />
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblProductName" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"Name") %>'></asp:Label>
                                                                                    </ItemTemplate>
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField>
                                                                                    <HeaderTemplate>
                                                                                        SKU
                                                                                    </HeaderTemplate>
                                                                                    <HeaderStyle HorizontalAlign="Center" />
                                                                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Top" Width="10%" />
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblSKU" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"SKU") %>'></asp:Label>
                                                                                    </ItemTemplate>
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField>
                                                                                    <HeaderTemplate>
                                                                                        Quantity
                                                                                    </HeaderTemplate>
                                                                                    <HeaderStyle HorizontalAlign="Center" />
                                                                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Top" Width="5%" />
                                                                                    <ItemTemplate>

                                                                                        <asp:Label ID="lblQty" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"Quantity") %>'></asp:Label>
                                                                                        <div style="display: none;">
                                                                                        <asp:Label ID="lblOldQty" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"ShippedQty") %>'></asp:Label>
                                                                                            </div>
                                                                                        <asp:Label ID="lblOldWarehouseId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container.DataItem,"WarehouseId") %>'></asp:Label>
                                                                                        <asp:Label ID="lblInventoryupdated" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container.DataItem,"Inventoryupdated") %>'></asp:Label>

                                                                                    </ItemTemplate>
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField Visible="false">
                                                                                    <HeaderTemplate>
                                                                                        Available Qty
                                                                                    </HeaderTemplate>
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblavailQuantity" runat="server"></asp:Label>
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Top" />
                                                                                    <FooterStyle HorizontalAlign="center" />
                                                                                    <HeaderStyle Width="30px" HorizontalAlign="center" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField>
                                                                                    <HeaderTemplate>
                                                                                        Shipped&nbsp;Items
                                                                                    </HeaderTemplate>
                                                                                    <HeaderStyle HorizontalAlign="Center" />
                                                                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Top" Width="5%" />
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblShippedQty" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"ShippedQty") %>'></asp:Label><br />
                                                                                        <asp:Literal ID="lblremaining" runat="server" Text="Remaining:"></asp:Literal><asp:TextBox ID="txtShippedQty" runat="server" CssClass="textfield_small" onKeyPress="var ret=keyRestrict(event,'0123456789');return ret;" onchange="return checkqtyvalid(this.id);" AutoPostBack="true" OnTextChanged="txtProWeight_TextChanged"
                                                                                            Width="50" Text='0' Style="text-align: center;"></asp:TextBox>

                                                                                    </ItemTemplate>
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="Warehouse " Visible="false">
                                                                                    <ItemTemplate>
                                                                                        <asp:DropDownList ID="ddlWareHouse" Style="padding-left: 3px; width: 100px;" AutoPostBack="false"
                                                                                            runat="server" CssClass="op">
                                                                                        </asp:DropDownList>
                                                                                        <asp:Label ID="lblddlWareHouseName" Visible="false" runat="server"></asp:Label><br />
                                                                                    </ItemTemplate>
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="Dimensions&nbsp;(L x W x H)" ItemStyle-CssClass="griditemst">
                                                                                    <ItemTemplate>
                                                                                         <asp:DropDownList ID="ddlboxes" Style="padding-left: 3px; width: auto%; height: 35px;" AutoPostBack="false"
                                                                                            runat="server" CssClass="product-type">
                                                                                             <asp:ListItem Value="">Select Box</asp:ListItem>
                                                                                             <asp:ListItem Value="30 x 15 x 15">30 x 15 x 15</asp:ListItem>
                                                                                             <asp:ListItem Value="30 x 14 x 10">30 x 14 x 10</asp:ListItem>
                                                                                             <asp:ListItem Value="30 x 14 x 7">30 x 14 x 7</asp:ListItem>
                                                                                             <asp:ListItem Value="29 x 14 x 4">29 x 14 x 4</asp:ListItem>
                                                                                             <asp:ListItem Value="16 x 12 x 8">16 x 12 x 8</asp:ListItem>
                                                                                             <asp:ListItem Value="12 x 12 x 12">12 x 12 x 12</asp:ListItem>
                                                                                             
                                                                                              <asp:ListItem Value="12 x 12 x 8">12 x 12 x 8</asp:ListItem>
<asp:ListItem Value="12 x 12 x 4">12 x 12 x 4</asp:ListItem>
                                                                                             </asp:DropDownList>
                                                                                         






                                                                                        <asp:Label ID="lblDimensionName" runat="server" Visible="false"></asp:Label><br />

                                                                                        <div id="dimHWL" runat="server" style="display:none">
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
                                                                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="Weight">
                                                                                    <ItemTemplate>
                                                                                        <br />
                                                                                        <asp:TextBox ID="txtProWeight" autocomplete="off" AutoPostBack="true" onKeyPress="var ret=keyRestrict(event,'0123456789.');return ret;"
                                                                                            MaxLength="5" runat="server" Width="50px" CssClass="span4" Text="0.00" OnTextChanged="txtProWeight_TextChanged"></asp:TextBox>
                                                                                        <div style="display: none;">
                                                                                            <asp:Label ID="lblweight" runat="server" Text=''></asp:Label>
                                                                                        </div>

                                                                                    </ItemTemplate>
                                                                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Top" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="Shipping Method">
                                                                                    <ItemTemplate>
                                                                                        <asp:Literal ID="ltrCustomerShipping" runat="server"></asp:Literal><br />
                                                                                        <asp:DropDownList ID="ddlShippingMethod" Style="padding-left: 3px; width: 60%; height: 35px;"
                                                                                            AutoPostBack="false" runat="server" CssClass="product-type">
                                                                                        </asp:DropDownList>

                                                                                        <asp:ImageButton ID="btnRebindShippingMethods" runat="server" ToolTip="Refresh" CommandArgument='<%# Eval("OrderNumber") %>' CommandName="RefreshShipping" ImageUrl="/App_Themes/<%=Page.Theme %>/images/refresh-icon.png" />

                                                                                        <asp:Label ID="lblShippingMethod" runat="server"></asp:Label><br />
                                                                                        <asp:Literal ID="ltrTackingNo" runat="server"></asp:Literal>
                                                                                        <input type="hidden" id="hdnShippingLabelMethod" runat="server" value='<%# Eval("ShippingLabelMethod")%>' />
                                                                                        <input type="hidden" id="hdnShippingLabelWeight" runat="server" value='<%# Eval("ShippingLabelWeight")%>' />
                                                                                        <input type="hidden" id="hdnShippingLabelPackageHeight" runat="server" value='<%# Eval("ShippingLabelPackageHeight")%>' />
                                                                                        <input type="hidden" id="hdnShippingLabelPackageWidth" runat="server" value='<%# Eval("ShippingLabelPackageWidth")%>' />
                                                                                        <input type="hidden" id="hdnShippingLabelPackageLength" runat="server" value='<%# Eval("ShippingLabelPackageLength")%>' />
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Top" Width="27%" />
                                                                                    <HeaderStyle Width="27%" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField Visible="false">
                                                                                    <HeaderTemplate>
                                                                                        Tracking Number
                                                                                    </HeaderTemplate>
                                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" Width="15%" />
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblTrackingNumber" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"TrackingNumber") %>'></asp:Label>
                                                                                        <asp:TextBox Visible="False" ID="txtTrackingNumber" runat="server" Text="0" Width="150"
                                                                                            CssClass="textfield_small"></asp:TextBox>

                                                                                    </ItemTemplate>
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField Visible="false">
                                                                                    <HeaderTemplate>
                                                                                        Shipped On
                                                                                    </HeaderTemplate>
                                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" Width="10%" />
                                                                                    <ItemTemplate>
                                                                                        <input type="hidden" id="hdnRefOrderNo" runat="server" value='<%# Eval("RefOrderID")%>' />
                                                                                        <input type="hidden" id="hdnshoppingcartid" runat="server" value='<%# Eval("ShoppingCardID")%>' />
                                                                                        <asp:Label ID="lblShippedOn" runat="server" Text='<%# SetShortDate(Convert.ToString(DataBinder.Eval(Container.DataItem,"ShippedOn"))) %>'></asp:Label>
                                                                                        <asp:TextBox Visible="False" ID="txtShippedOn1" runat="server" CssClass="textfield_small"
                                                                                            Width="100" Text="01/01/2000"></asp:TextBox>
                                                                                        <asp:TextBox ID="txtShippedOn2" runat="server" CssClass="from-textfield" Width="70px"
                                                                                            tyle="margin-right: 3px;" Visible="false"></asp:TextBox>


                                                                                    </ItemTemplate>
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField Visible="false">
                                                                                    <HeaderTemplate>
                                                                                        Shipped Note
                                                                                    </HeaderTemplate>
                                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" Width="10%" />
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblShippedNote" runat="server" Text='<%# Convert.ToString(DataBinder.Eval(Container.DataItem,"ShippedNote")) %>'></asp:Label>
                                                                                        <asp:TextBox Visible="False" ID="txtShippedNote" runat="server" CssClass="textfield_small"
                                                                                            Width="180px" Height="35px" TextMode="MultiLine" Style="resize: none"></asp:TextBox>
                                                                                    </ItemTemplate>
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField Visible="false">
                                                                                    <HeaderTemplate>
                                                                                        Shipped Via
                                                                                    </HeaderTemplate>
                                                                                    <HeaderStyle HorizontalAlign="Center" />
                                                                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Top" Width="10%" />
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblShippedVia" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"ShippedVia") %>'></asp:Label>
                                                                                        <asp:DropDownList ID="ddlShippedVIA" runat="server" Visible="false" CssClass="order-list"
                                                                                            Width="80px">
                                                                                            <asp:ListItem Value="FedEx" Selected="True" Text="FedEx"></asp:ListItem>
                                                                                            <asp:ListItem Value="UPS" Text="UPS"></asp:ListItem>
                                                                                            <asp:ListItem Value="USPS" Text="USPS"></asp:ListItem>
                                                                                            <asp:ListItem Value="Freight" Text="Freight"></asp:ListItem>
                                                                                            <asp:ListItem Value="Other" Text="Other"></asp:ListItem>
                                                                                        </asp:DropDownList>
                                                                                    </ItemTemplate>
                                                                                </asp:TemplateField>

                                                                                <asp:TemplateField>
                                                                                    <HeaderTemplate>
                                                                                        Action
                                                                                    </HeaderTemplate>
                                                                                    <HeaderStyle HorizontalAlign="Center" />
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblShippingLabelFileName" Visible="false" runat="server"></asp:Label>

                                                                                        <asp:Label ID="lblAmazonTrackingNo" runat="server"></asp:Label><br />
                                                                                        <asp:ImageButton ID="btnEdit" runat="server" ToolTip="Edit" CommandName="CustomEdit"
                                                                                            CommandArgument='<%# DataBinder.Eval(Container.DataItem,"OrderedCustomCartID") %>'
                                                                                            OnClientClick="return CheckValidation(this.id);" />
                                                                                        <asp:ImageButton ID="btnSave" runat="server" ToolTip="Save" CommandArgument='<%# DataBinder.Eval(Container.DataItem,"OrderedCustomCartID") %>'
                                                                                            CommandName="CustomSave" Visible="False" ValidationGroup="UpdateShipping" />
                                                                                        <asp:ImageButton ID="btnCancel" runat="server" ToolTip="Cancel" CommandName="CustomCancel"
                                                                                            Visible="False" CommandArgument='<%# DataBinder.Eval(Container.DataItem,"OrderedCustomCartID") %>' />
                                                                                        <input type="hidden" id="hdnshipped" runat="server" value='<%# Convert.ToBoolean(DataBinder.Eval(Container.DataItem,"Shipped")) %>' />
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Top" Width="5%" />
                                                                                </asp:TemplateField>
                                                                                <%--  <asp:TemplateField>
                                                                                      <FooterTemplate>
                                                                                           <asp:ImageButton ID="btngeneratealllabel" ToolTip="Save & Generate All Label" ImageUrl="/App_Themes/<%=Page.Theme %>/images/save-and-generate-label.gif" Style="margin-top: 5px; margin-bottom: 5px;" runat="server" OnClientClick="return calculateAll();" />
                                                                                      </FooterTemplate>
                                                                                     <FooterStyle HorizontalAlign="right" VerticalAlign="Top" Width="5%" />
                                                                                </asp:TemplateField>--%>
                                                                            </Columns>
                                                                            <RowStyle CssClass="oddrow" Height="30px" HorizontalAlign="Left" />
                                                                            <EditRowStyle CssClass="altrow" />
                                                                            <PagerStyle CssClass="altrow" HorizontalAlign="Right" />
                                                                            <HeaderStyle Height="20px" HorizontalAlign="Left" Wrap="True" VerticalAlign="middle"
                                                                                BackColor="gray" />
                                                                            <AlternatingRowStyle CssClass="altrow" />
                                                                            <PagerSettings Position="TopAndBottom" />
                                                                            <EmptyDataRowStyle ForeColor="Red" Font-Size="Medium" />
                                                                        </asp:GridView>
                                                                    </td>
                                                                </tr>
                                                                <tr id="allorderqty" runat="server">
                                                                    <td colspan="2" align="center">
                                                                        <table>
                                                                            <tr>
                                                                                <td>
                                                                                    <div id="dimHWL" runat="server">
                                                                                        <b>Dimensions&nbsp;(L x W x H):</b>
                                                                                         <asp:DropDownList ID="ddlboxesall" Style="padding-left: 3px; width: auto%; height: 35px;" AutoPostBack="false"
                                                                                            runat="server" CssClass="product-type">
                                                                                             <asp:ListItem Value="">Select Box</asp:ListItem>
                                                                                             <asp:ListItem Value="30 x 15 x 15">30 x 15 x 15</asp:ListItem>
                                                                                             <asp:ListItem Value="30 x 14 x 10">30 x 14 x 10</asp:ListItem>
                                                                                             <asp:ListItem Value="30 x 14 x 7">30 x 14 x 7</asp:ListItem>
                                                                                             <asp:ListItem Value="29 x 14 x 4">29 x 14 x 4</asp:ListItem>
                                                                                             <asp:ListItem Value="16 x 12 x 8">16 x 12 x 8</asp:ListItem>

                                                                                             <asp:ListItem Value="12 x 12 x 12">12 x 12 x 12</asp:ListItem>
                                                                                              <asp:ListItem Value="12 x 12 x 8">12 x 12 x 8</asp:ListItem>
<asp:ListItem Value="12 x 12 x 4">12 x 12 x 4</asp:ListItem>
                                                                                             </asp:DropDownList>
                                                                                        <div style="display:none;">
                                                                                        <asp:TextBox ID="txtHeightgridall" onKeyPress="return keyRestrict(event,'0123456789.')" CssClass="span4"
                                                                                            MaxLength="5" runat="server" Text='1' Columns="3"></asp:TextBox>
                                                                                        X
                                            <asp:TextBox ID="txtWidthgridall"  onKeyPress="return keyRestrict(event,'0123456789.')" CssClass="span4"
                                                MaxLength="5" runat="server" Columns="3" Text='11'></asp:TextBox>
                                                                                        X
                                            <asp:TextBox ID="txtLengthgridall" onKeyPress="return keyRestrict(event,'0123456789.')" CssClass="span4"
                                                MaxLength="5" runat="server" Text='13'
                                                Columns="3"></asp:TextBox></div>
                                                                                    </div>
                                                                                </td>
                                                                                <td><b>Weight:</b>
                                                                                    <asp:TextBox ID="txtProWeightall" autocomplete="off" onchange="return imggetmethodweight();" onKeyPress="var ret=keyRestrict(event,'0123456789.');return ret;"
                                                                                        MaxLength="5" runat="server" Width="50px" CssClass="span4" Text="0.00"></asp:TextBox>
                                                                                </td>
                                                                                <td>
                                                                                    <asp:DropDownList ID="ddlShippingMethodall" Style="padding-left: 3px; width: auto%; height: 35px;"
                                                                                        AutoPostBack="false" runat="server" CssClass="product-type">
                                                                                    </asp:DropDownList>
                                                                                   <asp:ImageButton ID="imggetmethod" OnClientClick="return imggetmethod();"  ToolTip="Generate Method" ImageUrl="/App_Themes/gray/images/refresh-icon.png"  runat="server" OnClick="imggetmethod_Click" />
                                                                                </td>
                                                                                <td>
                                                                                    
                                                                                </td>
                                                                                <td>
                                                                                    <asp:ImageButton ID="btngeneratealllabelall"  OnClientClick="return btngeneratealllabelall();"   ToolTip="Save & Generate All Label" OnClick="btngeneratealllabelall_Click" ImageUrl="/App_Themes/gray/images/generate-label.gif" Style="margin-top: 5px; margin-bottom: 5px;" runat="server" />
                                                                                </td>
                                                                            </tr>
                                                                        </table>

                                                                    </td>
                                                                </tr>
                                                            </tbody>
                                                        </table>


                                                    </td>
                                                </tr>
                                            </tbody>
                                        </table>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </td>
                </tr>
            </tbody>
        </table>
        <div id="divAmazonCancel" style="display: none">
            <input type="hidden" id="hdnAmazonCartID" runat="server" />
            <input type="hidden" id="hdnAmazonTrackinNo" runat="server" />
            <asp:Button ID="btnAmazonRemove" runat="server" OnClick="btnAmazonRemove_Click" />
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
    </form>
</body>
</html>
