<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="OrderProductSku.aspx.cs"
    Inherits="Solution.UI.Web.ADMIN.Orders.OrderProductSku" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Shopping Cart</title>
    <script type="text/javascript" language="javascript">

function SetpriceforReadymade(idnumb,id,sprice)
        {

            if (document.getElementById('Selectvariant-' + id.toString()) != null && document.getElementById('Selectvariant-' + id.toString()).checked)
            {
if(document.getElementById('gvListProducts_lblNewVariantPrice_'+idnumb) != null)
{
document.getElementById('gvListProducts_lblNewVariantPrice_'+idnumb).innerHTML = sprice.toString();
}
else
{
                document.getElementById('gvListProducts_lblNewVariantPrice_0').innerHTML = sprice.toString();
}
  
            }

        }

        function Showsearcklinksku(sku) {        
            document.getElementById('txtSearch').value = sku;
            document.getElementById('btnSearch').click();     
        }

        function chkAddtocartForMadetoMeasure(RowIndex) {
            var strMeasureName = "";
            var strMeasureValue = "";

            if (document.getElementById('gvListProducts_txtMadetoMeasureQty_' + RowIndex + '') != null && (document.getElementById('gvListProducts_txtMadetoMeasureQty_' + RowIndex + '').value == '' || document.getElementById('gvListProducts_txtMadetoMeasureQty_' + RowIndex + '').value == 0)) {
                alert('Please Enter Valid Quantity.');
                return false;
            }
            if (document.getElementById('gvListProducts_ddlcustomstyle_' + RowIndex + '') != null && document.getElementById('gvListProducts_ddlcustomstyle_' + RowIndex + '').selectedIndex == 0) {
                alert('Please select Header.');
                return false;
            }
            if (document.getElementById('gvListProducts_ddlcustomwidth_' + RowIndex + '') != null && document.getElementById('gvListProducts_ddlcustomwidth_' + RowIndex + '').selectedIndex == 0) {
                alert('Please select Width.');
                return false;
            }
            if (document.getElementById('gvListProducts_ddlcustomlength_' + RowIndex + '') != null && document.getElementById('gvListProducts_ddlcustomlength_' + RowIndex + '').selectedIndex == 0) {
                alert('Please select Length.');
                return false;
            }

            if (document.getElementById('gvListProducts_ddlcustomoptin_' + RowIndex + '') != null && document.getElementById('gvListProducts_ddlcustomoptin_' + RowIndex + '').selectedIndex == 0) {
                alert('Please select Options.');
                return false;
            }

            //            if (document.getElementById('gvListProducts_dlcustomqty_' + RowIndex + '') != null && document.getElementById('gvListProducts_dlcustomqty_' + RowIndex + '').selectedIndex == 0) {
            //                alert('Please select Quantity.');
            //                return false;
            //            }

            if (document.getElementById('gvListProducts_ddlcustomstyle_' + RowIndex + '') != null && document.getElementById('gvListProducts_ddlcustomstyle_' + RowIndex + '').selectedIndex > 0) {
                strMeasureName = strMeasureName + 'Header' + ',';
                strMeasureValue = strMeasureValue + document.getElementById('gvListProducts_ddlcustomstyle_' + RowIndex + '').options[document.getElementById('gvListProducts_ddlcustomstyle_' + RowIndex + '').selectedIndex].value + ',';
            }
            if (document.getElementById('gvListProducts_ddlcustomwidth_' + RowIndex + '') != null && document.getElementById('gvListProducts_ddlcustomwidth_' + RowIndex + '').selectedIndex > 0) {
                strMeasureName = strMeasureName + 'Width' + ',';
                strMeasureValue = strMeasureValue + document.getElementById('gvListProducts_ddlcustomwidth_' + RowIndex + '').options[document.getElementById('gvListProducts_ddlcustomwidth_' + RowIndex + '').selectedIndex].value.replace(' ', '') + ',';
            }
            if (document.getElementById('gvListProducts_ddlcustomlength_' + RowIndex + '') != null && document.getElementById('gvListProducts_ddlcustomlength_' + RowIndex + '').selectedIndex > 0) {
                strMeasureName = strMeasureName + 'Length' + ',';
                strMeasureValue = strMeasureValue + document.getElementById('gvListProducts_ddlcustomlength_' + RowIndex + '').options[document.getElementById('gvListProducts_ddlcustomlength_' + RowIndex + '').selectedIndex].value.replace(' ', '') + ',';
            }
            if (document.getElementById('gvListProducts_ddlcustomoptin_' + RowIndex + '') != null && document.getElementById('gvListProducts_ddlcustomoptin_' + RowIndex + '').selectedIndex > 0) {
                strMeasureName = strMeasureName + 'Options' + ',';
                strMeasureValue = strMeasureValue + document.getElementById('gvListProducts_ddlcustomoptin_' + RowIndex + '').options[document.getElementById('gvListProducts_ddlcustomoptin_' + RowIndex + '').selectedIndex].value + ',';
            }

            if (document.getElementById('gvListProducts_txtMadetoMeasureQty_' + RowIndex + '') != null && (document.getElementById('gvListProducts_txtMadetoMeasureQty_' + RowIndex + '').value == '' || document.getElementById('gvListProducts_txtMadetoMeasureQty_' + RowIndex + '').value == 0)) {
                strMeasureName = strMeasureName + 'Quantity (Panels)' + ',';
                strMeasureValue = strMeasureValue + document.getElementById('gvListProducts_txtMadetoMeasureQty_' + RowIndex + '').value.replace(' ', '') + ',';
            }

            //            if (document.getElementById('gvListProducts_dlcustomqty_' + RowIndex + '') != null && document.getElementById('gvListProducts_dlcustomqty_' + RowIndex + '').selectedIndex > 0) {
            //                strMeasureName = strMeasureName + 'Quantity (Panels)' + ',';
            //                strMeasureValue = strMeasureValue + document.getElementById('gvListProducts_dlcustomqty_' + RowIndex + '').options[document.getElementById('gvListProducts_dlcustomqty_' + RowIndex + '').selectedIndex].value.replace(' ', '') + ',';
            //            }
            if (document.getElementById('hdncustombuy1get1') != null && document.getElementById('hdncustombuy1get1').value == "1") {
                strMeasureName = strMeasureName + 'Promo' + ',';
                strMeasureValue = strMeasureValue + '(Buy 1 Get 1 Free)' + ',';
            }
            document.getElementById('hdnVariProductId').value = document.getElementById('gvListProducts_hdnproductid_' + RowIndex).value;
            document.getElementById('hdnVariQuantity').value = document.getElementById('gvListProducts_txtMadetoMeasureQty_' + RowIndex).value;
            document.getElementById('hdnforMeasureVName').value = strMeasureName;
            document.getElementById('hdnforMeasureVValue').value = strMeasureValue;
            document.getElementById('btnAddVariantMeasure').click();
            return false;
        }
        function chkAddtocartForMadetoMeasureNew(RowIndex) {
            var strMeasureName = "";
            var strMeasureValue = "";
            if (document.getElementById('gvListProducts_ddlcustomstyle_' + RowIndex + '') != null && document.getElementById('gvListProducts_ddlcustomstyle_' + RowIndex + '').selectedIndex > 0) {
                strMeasureName = strMeasureName + 'Header' + ',';
                strMeasureValue = strMeasureValue + document.getElementById('gvListProducts_ddlcustomstyle_' + RowIndex + '').options[document.getElementById('gvListProducts_ddlcustomstyle_' + RowIndex + '').selectedIndex].value + ',';
            }
            if (document.getElementById('gvListProducts_ddlcustomwidth_' + RowIndex + '') != null && document.getElementById('gvListProducts_ddlcustomwidth_' + RowIndex + '').selectedIndex > 0) {
                strMeasureName = strMeasureName + 'Width' + ',';
                strMeasureValue = strMeasureValue + document.getElementById('gvListProducts_ddlcustomwidth_' + RowIndex + '').options[document.getElementById('gvListProducts_ddlcustomwidth_' + RowIndex + '').selectedIndex].value.replace(' ', '') + ',';
            }
            if (document.getElementById('gvListProducts_ddlcustomlength_' + RowIndex + '') != null && document.getElementById('gvListProducts_ddlcustomlength_' + RowIndex + '').selectedIndex > 0) {
                strMeasureName = strMeasureName + 'Length' + ',';
                strMeasureValue = strMeasureValue + document.getElementById('gvListProducts_ddlcustomlength_' + RowIndex + '').options[document.getElementById('gvListProducts_ddlcustomlength_' + RowIndex + '').selectedIndex].value.replace(' ', '') + ',';
            }
            if (document.getElementById('gvListProducts_ddlcustomoptin_' + RowIndex + '') != null && document.getElementById('gvListProducts_ddlcustomoptin_' + RowIndex + '').selectedIndex > 0) {
                strMeasureName = strMeasureName + 'Options' + ',';
                strMeasureValue = strMeasureValue + document.getElementById('gvListProducts_ddlcustomoptin_' + RowIndex + '').options[document.getElementById('gvListProducts_ddlcustomoptin_' + RowIndex + '').selectedIndex].value + ',';
            }

            if (document.getElementById('gvListProducts_txtMadetoMeasureQty_' + RowIndex + '') != null && (document.getElementById('gvListProducts_txtMadetoMeasureQty_' + RowIndex + '').value == '' || document.getElementById('gvListProducts_txtMadetoMeasureQty_' + RowIndex + '').value == 0)) {
                strMeasureName = strMeasureName + 'Quantity (Panels)' + ',';
                strMeasureValue = strMeasureValue + document.getElementById('gvListProducts_txtMadetoMeasureQty_' + RowIndex + '').value.replace(' ', '') + ',';
            }
            //            if (document.getElementById('gvListProducts_dlcustomqty_' + RowIndex + '') != null && document.getElementById('gvListProducts_dlcustomqty_' + RowIndex + '').selectedIndex > 0) {
            //                strMeasureName = strMeasureName + 'Quantity (Panels)' + ',';
            //                strMeasureValue = strMeasureValue + document.getElementById('gvListProducts_dlcustomqty_' + RowIndex + '').options[document.getElementById('gvListProducts_dlcustomqty_' + RowIndex + '').selectedIndex].value.replace(' ', '') + ',';
            //            }
            //document.getElementById('hdnVariPrice').value = document.getElementById('gvListProducts_hdnitemprice_' + RowIndex).value;
            if (document.getElementById('hdncustombuy1get1') != null && document.getElementById('hdncustombuy1get1').value == "1") {
                strMeasureName = strMeasureName + 'Promo' + ',';
                strMeasureValue = strMeasureValue + '(Buy 1 Get 1 Free)' + ',';
            }
            document.getElementById('hdnVariProductId').value = document.getElementById('gvListProducts_hdnproductid_' + RowIndex).value;

            document.getElementById('hdnforMeasureVName').value = strMeasureName;
            document.getElementById('hdnforMeasureVValue').value = strMeasureValue;

        }

        function chkAddtocart(RowIndex, IsRoman) {
            var strname = "";
            var strvalue = "";

            var VariQty = document.getElementById('gvListProducts_TxtQty_' + RowIndex).value;

            if (VariQty <= 0) {
                jAlert("Please enter valid Quantity...", "Required information", VariQty);
                return false;
            }

            var divproductvariant = 'gvListProducts_divproductvariant_' + RowIndex;
            var allDiv = document.getElementById(divproductvariant).getElementsByTagName('div');
            document.getElementById('hdnType').value = document.getElementById('gvListProducts_hdnCurrTabvalue_' + RowIndex).value;

            for (k = 0; k < allDiv.length; k++) {

                var controlid = allDiv[k];

                if (controlid.id == "divSelectvariant-0-" + RowIndex.toString()) {
                    strname = strname + "divvariantname=";
                    strvalue = strvalue + "divvariantname=";
                    var allselect01 = document.getElementById(controlid.id).getElementsByTagName('input');
                    for (var iS = 0; iS < allselect01.length; iS++) {
                        var eltSelect = allselect01[iS];
                        if (eltSelect.type == "radio") {
                            if (eltSelect.checked == true) {
                                if (eltSelect.id.indexOf('Selectvariant-000') > -1) {
                                    Width = document.getElementById(eltSelect.id).value;
                                    strname = strname + 'Width1' + ',';
                                    strvalue = strvalue + parseInt(Width) + ',';
                                }
                                if (eltSelect.id.indexOf('SelectvariantExtraWidthValue-0') > -1) {
                                    Width2 = document.getElementById(eltSelect.id).value;
                                    strname = strname + 'Width2' + ',';
                                    strvalue = strvalue + Width2 + ',';
                                }
                            }
                        }
                    }
                } // end if
                if (controlid.id == "divSelectvariant-9999999-" + RowIndex.toString()) {
                    strname = strname + "divvariantname=";
                    strvalue = strvalue + "divvariantname=";
                    var allselect01 = document.getElementById(controlid.id).getElementsByTagName('input');
                    for (var iS = 0; iS < allselect01.length; iS++) {
                        var eltSelect = allselect01[iS];
                        if (eltSelect.type == "radio") {
                            if (eltSelect.checked == true) {
                                if (eltSelect.id.indexOf('Selectvariant-9999999') > -1) {
                                    Length = document.getElementById(eltSelect.id).value;
                                    strname = strname + 'Length1' + ',';
                                    strvalue = strvalue + parseInt(Length) + ',';
                                }
                                if (eltSelect.id.indexOf('SelectvariantExtraLengthValue-9999999') > -1) {
                                    Length2 = document.getElementById(eltSelect.id).value;
                                    strname = strname + 'Length2' + ',';
                                    strvalue = strvalue + Length2 + ',';
                                }
                            }
                        }
                    }
                }

                if (controlid.id == "divSelectvariant-200000-" + RowIndex.toString()) {
                    strname = strname + "divvariantname=";
                    strvalue = strvalue + "divvariantname=";
                    var allselect01 = document.getElementById(controlid.id).getElementsByTagName('input');
                    for (var iS = 0; iS < allselect01.length; iS++) {
                        var eltSelect = allselect01[iS];
                        if (eltSelect.type == "radio") {
                            if (eltSelect.checked == true) {
                                if (eltSelect.id.indexOf('Selectvariant-20000') > -1) {
                                    options = document.getElementById(eltSelect.id).value;
                                    strname = strname + 'Option' + ',';
                                    strvalue = strvalue + options + ',';
                                }
                            }
                        }
                    }
                }
                if (controlid.id == "divSelectvariant-9000000-" + RowIndex.toString()) {
                    strname = strname + "divvariantname=";
                    strvalue = strvalue + "divvariantname=";
                    var allselect01 = document.getElementById(controlid.id).getElementsByTagName('input');
                    for (var iS = 0; iS < allselect01.length; iS++) {
                        var eltSelect = allselect01[iS];
                        if (eltSelect.type == "radio") {
                            if (eltSelect.checked == true) {
                                if (eltSelect.id.indexOf('Selectvariant-9000000') > -1) {
                                    lift = document.getElementById(eltSelect.id).value;
                                    strname = strname + 'Lift' + ',';
                                    strvalue = strvalue + lift + ',';
                                }
                            }
                        }
                    }
                }
                if (controlid.id == "divSelectvariant-100000-" + RowIndex.toString()) {
                    strname = strname + "divvariantname=";
                    strvalue = strvalue + "divvariantname=";
                    var allselect01 = document.getElementById(controlid.id).getElementsByTagName('input');
                    for (var iS = 0; iS < allselect01.length; iS++) {
                        var eltSelect = allselect01[iS];
                        if (eltSelect.type == "radio") {
                            if (eltSelect.checked == true) {
                                if (eltSelect.id.indexOf('Selectvariant-10000') > -1) {
                                    options = document.getElementById(eltSelect.id).value;
                                    strname = strname + 'Mount' + ',';
                                    strvalue = strvalue + options + ',';
                                }
                            }
                        }
                    }
                }

                else {
                    if (controlid.id.indexOf('divvariantname-') > -1) {
                        strname = strname + "divvariantname=";
                        strvalue = strvalue + "divvariantname=";
                        var hdnval = controlid.id.replace('divvariantname-', 'hdnvariantname-');
                        var divVarival = controlid.id.replace('divvariantname-', 'divSelectvariant-');
                        if (document.getElementById(divVarival) != null) {
                            if (document.getElementById(hdnval) != null) {
                                strname = strname + document.getElementById(hdnval).value + ',';
                            }
                            var allselect = document.getElementById(divVarival).getElementsByTagName('input');
                            for (var iS = 0; iS < allselect.length; iS++) {
                                var eltSelect = allselect[iS];
                                if (eltSelect.type == "radio") {
                                    if (eltSelect.checked == true) {
                                        if (eltSelect.id.indexOf('Selectvariant-000') > -1 || eltSelect.id.indexOf('SelectvariantExtraWidthValue-0') > -1 || eltSelect.id.indexOf('Selectvariant-9999999') > -1 || eltSelect.id.indexOf('SelectvariantExtraLengthValue-9999999') > -1 || eltSelect.id.indexOf('Selectvariant-20000') > -1 || eltSelect.id.indexOf('Selectvariant-10000') > -1 || eltSelect.id.indexOf('Selectvariant-9000000') > -1) {

                                        }
                                        else {
                                            strvalue = strvalue + eltSelect.value + ',';

                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            if (IsRoman == "0") {
                document.getElementById('hdnVariPrice').value = document.getElementById('gvListProducts_lblNewVariantPrice_' + RowIndex).innerHTML;
            }
            else {
                document.getElementById('hdnVariPrice').value = document.getElementById('gvListProducts_hdnitemprice_' + RowIndex).value;
            }

            document.getElementById('hdnVariProductId').value = document.getElementById('gvListProducts_hdnproductid_' + RowIndex).value;
            document.getElementById('hdnVariQuantity').value = document.getElementById('gvListProducts_TxtQty_' + RowIndex).value;
            
            document.getElementById('hdnVariName').value = strname;
            document.getElementById('hdnVarivalue').value = strvalue;
            document.getElementById('btnAddVariant').click();

            return false;
        }

        function chkAddtocartNew(RowIndex) {
            var strname = "";
            var strvalue = "";
            var divproductvariant = 'gvListProducts_divproductvariant_' + RowIndex;
            var allDiv = document.getElementById(divproductvariant).getElementsByTagName('div');
            document.getElementById('hdnType').value = document.getElementById('gvListProducts_hdnCurrTabvalue_' + RowIndex).value;

            for (k = 0; k < allDiv.length; k++) {

                var controlid = allDiv[k];
                if (controlid.id.indexOf('divvariantname-') > -1) {

                    strname = strname + "divvariantname=";
                    strvalue = strvalue + "divvariantname=";

                    var hdnval = controlid.id.replace('divvariantname-', 'hdnvariantname-');


                    var divVarival = controlid.id.replace('divvariantname-', 'divSelectvariant-');
                    if (document.getElementById(divVarival) != null) {
                        if (document.getElementById(hdnval) != null) {
                            strname = strname + document.getElementById(hdnval).value + ',';
                        }
                        var allselect = document.getElementById(divVarival).getElementsByTagName('input');

                        for (var iS = 0; iS < allselect.length; iS++) {
                            var eltSelect = allselect[iS];
                            if (eltSelect.type == "radio") {
                                if (eltSelect.checked == true) {
                                    if (eltSelect.id.indexOf('SelectvariantExtraValue') > -1) { }
                                    else
                                        strvalue = strvalue + eltSelect.value + ',';
                                }
                            }
                        }
                    }
                }
            }
            document.getElementById('hdnVariPrice').value = document.getElementById('gvListProducts_hdnitemprice_' + RowIndex).value;
            document.getElementById('hdnVariProductId').value = document.getElementById('gvListProducts_hdnproductid_' + RowIndex).value;
            document.getElementById('hdnVariQuantity').value = document.getElementById('gvListProducts_TxtQty_' + RowIndex).value;

            document.getElementById('hdnVariName').value = strname;
            document.getElementById('hdnVarivalue').value = strvalue;

        }
        var t;
        function selectAll(on) {
            var allElts = document.forms['aspnetForm'].elements;
            var i;
            for (i = 0; i < allElts.length; i++) {
                var elt = allElts[i];
                if (elt.type == "checkbox") {
                    elt.checked = on;
                }
            }
        }

        function PriceChangeondropdownChangeAllow(RowIndex, id) {

            var hdnActual = 'gvListProducts_hdnActual_' + RowIndex;
            var hdnprice = 'gvListProducts_hdnprice_' + RowIndex;
            var NewVariantPrice = 'gvListProducts_lblNewVariantPrice_' + RowIndex;
            var PriceTag = 'gvListProducts_lblPriceTag_' + RowIndex;
            var divproductvariant = 'gvListProducts_divproductvariant_' + RowIndex;

            var divallowlockQty = 'gvListProducts_divallowlockQty_' + RowIndex;
            var lblallowlockQty = 'gvListProducts_lblallowlockQty_' + RowIndex;
            var hdnallowlockQty = 'hdnallowquantity-' + id.toString();

            if (document.getElementById(divallowlockQty) != null && document.getElementById(lblallowlockQty) != null) {

                if (document.getElementById(hdnallowlockQty) != null) {
                    document.getElementById(lblallowlockQty).innerHTML = document.getElementById(hdnallowlockQty).value;
                    document.getElementById(divallowlockQty).style.display = '';
                }
            }

        }



        function PriceChangeondropdown(RowIndex) {

            var hdnActual = 'gvListProducts_hdnActual_' + RowIndex;
            var hdnprice = 'gvListProducts_hdnprice_' + RowIndex;
            var NewVariantPrice = 'gvListProducts_lblNewVariantPrice_' + RowIndex;
            var PriceTag = 'gvListProducts_lblPriceTag_' + RowIndex;
            var divproductvariant = 'gvListProducts_divproductvariant_' + RowIndex;

            var divallowlockQty = 'gvListProducts_divallowlockQty_' + RowIndex;
            var lblallowlockQty = 'gvListProducts_lblallowlockQty_' + RowIndex;
            var hdnallowlockQty;

            var price = document.getElementById(hdnActual).value;
            var saleprice = document.getElementById(hdnprice).value;
            if (parseFloat(saleprice) == parseFloat(0)) {
                saleprice = price;
            }
            var vprice = 0;
            if (document.getElementById(divproductvariant)) {
                var allselect = document.getElementById(divproductvariant).getElementsByTagName('input');
                for (var iS = 0; iS < allselect.length; iS++) {
                    var eltSelect = allselect[iS];
                    if (eltSelect.type == "radio") {
                        if (eltSelect.checked == true) {
                            if (document.getElementById(divallowlockQty) != null && document.getElementById(lblallowlockQty) != null) {
                                hdnallowlockQty = eltSelect.id.replace('Selectvariant-', 'hdnallowquantity-');
                                if (document.getElementById(hdnallowlockQty) != null) {
                                    document.getElementById(lblallowlockQty).innerHTML = document.getElementById(hdnallowlockQty).value;
                                    document.getElementById(divallowlockQty).style.display = '';
                                }
                            }

                            if (eltSelect.value.replace(/,/g, ' ').indexOf('($') > -1) {
                                var vtemp = eltSelect.value.replace(/,/g, ' ').substring(eltSelect.value.replace(/,/g, ' ').lastIndexOf('($') + 2);
                                vtemp = vtemp.replace(/\)/g, '');
                                vprice = parseFloat(vprice) + parseFloat(vtemp);
                            }
                        }
                    }
                }
            }
            if (parseFloat(vprice) > 0) {
                saleprice = parseFloat(vprice)
            }
            else {
                saleprice = parseFloat(saleprice) + parseFloat(vprice);
            }
            price = parseFloat(price);
            //            saleprice = parseFloat(saleprice) + parseFloat(vprice);
            //            price = parseFloat(price) + parseFloat(vprice);
            if (document.getElementById(NewVariantPrice) != null && document.getElementById(PriceTag) != null) {
                document.getElementById(NewVariantPrice).innerHTML = "Half Price Drapes Price : ";
                document.getElementById(NewVariantPrice).innerHTML = saleprice.toFixed(2);
            }

            var qid = '0';
            var pid = document.getElementById('gvListProducts_hdnproductid_' + RowIndex).value;
            document.getElementById('hdnType').value = document.getElementById('gvListProducts_hdnCurrTabvalue_' + RowIndex).value;
            var ptype = document.getElementById('hdnType').value;
            if (document.getElementById('gvListProducts_TxtQty_' + RowIndex) != null && document.getElementById('gvListProducts_TxtQty_' + RowIndex).value != '') {
                qid = document.getElementById('gvListProducts_TxtQty_' + RowIndex).value;
            }
            var strMeasureName ="";
            var strMeasureValue ="";

            if (ptype == 1 || ptype == 3) {
                chkAddtocartNew(RowIndex);
                strMeasureName = document.getElementById('hdnVariName').value;
                strMeasureValue = document.getElementById('hdnVarivalue').value;
            }
            else {
                chkAddtocartForMadetoMeasureNew(RowIndex);
                  strMeasureName = document.getElementById('hdnforMeasureVName').value;
                  strMeasureValue = document.getElementById('hdnforMeasureVValue').value;
            }
           
            $.ajax(
                        {
                            type: "POST",
                            url: "/TestMail.aspx/GetDataAdminMessage",
                            data: "{ProductId: " + pid + ",ProductType: " + ptype + ",Qty: " + qid + ",vValueid: '" + strMeasureValue + "',vNameid: '" + strMeasureName + "' }",
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            async: "true",
                            cache: "false",
                            success: function (msg) {

                                //alert(msg.d);
                                if (ptype == 1 || ptype == 3) {
                                    if (document.getElementById('gvListProducts_lblEstimateddate_' + RowIndex) != null) {
                                        $('#gvListProducts_lblEstimateddate_' + RowIndex).html(msg.d);

                                    }
                                }
                                else {
                                    if (document.getElementById('gvListProducts_divestimateddate_' + RowIndex) != null) {
                                        $('#gvListProducts_divestimateddate_' + RowIndex).html(msg.d);

                                    }
                                }
                            },
                            Error: function (x, e) {
                            }
                        });

        }

        function PriceChangeondropdownforroman(RowIndex) {

            var fabricnameoptin1 = '';
            var fabricnameoptin = '';
            var fabricvalueId = '0';
            var colorname = '';
            var hdnActual = 'gvListProducts_hdnActual_' + RowIndex;
            var hdnprice = 'gvListProducts_hdnprice_' + RowIndex;
            var NewVariantPrice = 'gvListProducts_lblNewVariantPrice_' + RowIndex;
            var PriceTag = 'gvListProducts_lblPriceTag_' + RowIndex;
            var divproductvariant = 'gvListProducts_divproductvariant_' + RowIndex;

            var divallowlockQty = 'gvListProducts_divallowlockQty_' + RowIndex;
            var lblallowlockQty = 'gvListProducts_lblallowlockQty_' + RowIndex;
            var hdnallowlockQty;

            var price = document.getElementById(hdnActual).value;
            var saleprice = document.getElementById(hdnprice).value;
            if (parseFloat(saleprice) == parseFloat(0)) {
                saleprice = price;
            }
            var Width = "", Width2 = "", Length = "", Length2 = "", options = "", lift = "";

            var vprice = 0;

            if (document.getElementById(divproductvariant)) {
                var allselect = document.getElementById(divproductvariant).getElementsByTagName('input');
                for (var iS = 0; iS < allselect.length; iS++) {
                    var eltSelect = allselect[iS];
                    if (eltSelect.type == "radio") {
                        if (eltSelect.checked == true) {
                            if (document.getElementById(divallowlockQty) != null && document.getElementById(lblallowlockQty) != null) {
                                hdnallowlockQty = eltSelect.id.replace('Selectvariant-', 'hdnallowquantity-');
                                if (eltSelect.id.indexOf('Selectvariant-000') > -1 || eltSelect.id.indexOf('SelectvariantExtraWidthValue-0') > -1 || eltSelect.id.indexOf('Selectvariant-9999999') > -1 || eltSelect.id.indexOf('SelectvariantExtraLengthValue-9999999') > -1 || eltSelect.id.indexOf('Selectvariant-20000') > -1) {

                                }
                                else {
                                    if (document.getElementById(hdnallowlockQty) != null && document.getElementById(hdnallowlockQty).value != "0") {
                                        document.getElementById(lblallowlockQty).innerHTML = document.getElementById(hdnallowlockQty).value;
                                        document.getElementById(divallowlockQty).style.display = '';
                                    }
                                }
                            }
                            if (eltSelect.value.replace(/,/g, ' ').indexOf('($') > -1) {
                                var vtemp = eltSelect.value.replace(/,/g, ' ').substring(eltSelect.value.replace(/,/g, ' ').lastIndexOf('($') + 2);
                                vtemp = vtemp.replace(/\)/g, '');
                                vprice = parseFloat(vprice) + parseFloat(vtemp);
                            }

                            fabricnameoptin1 = eltSelect.value;
                            var dividnew = eltSelect.name.replace('Selectvariant-', 'divvariantname-');
                            if (document.getElementById(dividnew) != null && document.getElementById(dividnew).innerHTML.toString().toLowerCase().indexOf('color') <= -1) {
                                if (fabricnameoptin1.toString().toLowerCase().indexOf('casual') > -1 || fabricnameoptin1.toString().toLowerCase().indexOf('soft fold') > -1 || fabricnameoptin1.toString().toLowerCase().indexOf('front slat') > -1 || fabricnameoptin1.toString().toLowerCase().indexOf('relaxed') > -1) {
                                    fabricnameoptin = fabricnameoptin1;
                                }
                            }


                            if (document.getElementById(dividnew) != null && document.getElementById(dividnew).innerHTML.toString().toLowerCase().indexOf('color') > -1) {
                                fabricvalueId = eltSelect.id.replace('Selectvariant-', '');
                                colorname = eltSelect.value;
                            }

                        }
                    }
                }
            }

            if (document.getElementById(divproductvariant)) {
                var allDiv = document.getElementById(divproductvariant).getElementsByTagName('div');
                for (k = 0; k < allDiv.length; k++) {
                    var controlid = allDiv[k];
                    if (controlid.id == "divSelectvariant-0-" + RowIndex.toString()) {
                        var allselect01 = document.getElementById(controlid.id).getElementsByTagName('input');
                        for (var iS = 0; iS < allselect01.length; iS++) {
                            var eltSelect = allselect01[iS];
                            if (eltSelect.type == "radio") {
                                if (eltSelect.checked == true) {
                                    if (eltSelect.id.indexOf('Selectvariant-000') > -1) {
                                        Width = document.getElementById(eltSelect.id).value;
                                    }
                                    if (eltSelect.id.indexOf('SelectvariantExtraWidthValue-0') > -1) {
                                        Width2 = document.getElementById(eltSelect.id).value;
                                    }
                                }
                            }
                        }
                    }
                    if (controlid.id == "divSelectvariant-9999999-" + RowIndex.toString()) {
                        var allselect01 = document.getElementById(controlid.id).getElementsByTagName('input');
                        for (var iS = 0; iS < allselect01.length; iS++) {
                            var eltSelect = allselect01[iS];
                            if (eltSelect.type == "radio") {
                                if (eltSelect.checked == true) {
                                    if (eltSelect.id.indexOf('Selectvariant-9999999') > -1) {
                                        Length = document.getElementById(eltSelect.id).value;
                                    }
                                    if (eltSelect.id.indexOf('SelectvariantExtraLengthValue-9999999') > -1) {
                                        Length2 = document.getElementById(eltSelect.id).value;
                                    }
                                }
                            }
                        }
                    }

                    if (controlid.id == "divSelectvariant-200000-" + RowIndex.toString()) {
                        var allselect01 = document.getElementById(controlid.id).getElementsByTagName('input');
                        for (var iS = 0; iS < allselect01.length; iS++) {
                            var eltSelect = allselect01[iS];
                            if (eltSelect.type == "radio") {
                                if (eltSelect.checked == true) {
                                    if (eltSelect.id.indexOf('Selectvariant-20000') > -1) {
                                        options = document.getElementById(eltSelect.id).value;
                                    }
                                }
                            }
                        }
                    }
                    if (controlid.id == "divSelectvariant-9000000-" + RowIndex.toString()) {
                        var allselect01 = document.getElementById(controlid.id).getElementsByTagName('input');
                        for (var iS = 0; iS < allselect01.length; iS++) {
                            var eltSelect = allselect01[iS];
                            if (eltSelect.type == "radio") {
                                if (eltSelect.checked == true) {
                                    if (eltSelect.id.indexOf('Selectvariant-9000000') > -1) {
                                        lift = document.getElementById(eltSelect.id).value;
                                    }
                                }
                            }
                        }
                    }

                }  // For loop end
            }


            //            if (parseFloat(vprice) > 0) {
            //                saleprice = parseFloat(vprice)
            //            }
            //            else {
            //                saleprice = parseFloat(saleprice) + parseFloat(vprice);
            //            }
            //            price = parseFloat(price);

            //            saleprice = parseFloat(saleprice) + parseFloat(vprice);
            //            price = parseFloat(price) + parseFloat(vprice);

            if (document.getElementById(NewVariantPrice) != null && document.getElementById(PriceTag) != null) {
                document.getElementById(NewVariantPrice).innerHTML = "Half Price Drapes Price : ";
                document.getElementById(NewVariantPrice).innerHTML = saleprice;
            }

            var qid = '0';
            var pid = document.getElementById('gvListProducts_hdnproductid_' + RowIndex).value;
            document.getElementById('hdnType').value = document.getElementById('gvListProducts_hdnCurrTabvalue_' + RowIndex).value;
            var ptype = document.getElementById('hdnType').value;
            if (document.getElementById('gvListProducts_TxtQty_' + RowIndex) != null && document.getElementById('gvListProducts_TxtQty_' + RowIndex).value != '') {
                qid = document.getElementById('gvListProducts_TxtQty_' + RowIndex).value;
            }
            if (ptype == 1 || ptype == 3) {
                chkAddtocartNew(RowIndex);
            }
            else {
                chkAddtocartForMadetoMeasureNew(RowIndex);
            }


            if (document.getElementById('gvListProducts_hdnshadesvalue_' + RowIndex) != null && document.getElementById('gvListProducts_hdnshadesvalue_' + RowIndex).value == '1') {
                if (Width != "" && Width2 != "" && Length != "" && Length2 != "") {

                    $.ajax(
                            {
                                type: "POST",
                                url: "/TestMail.aspx/GetDataRomanshadepriceAdmin",
                                data: "{ProductId: " + pid + ",ProductType: 3,Width: '" + Width + "',Width2: '" + Width2 + "', Qty: " + qid + ",options: '" + options + "',Length: '" + Length + "',Length2: '" + Length2 + "',fabricnameoptin: '" + fabricnameoptin + "',colorvalueid: '" + fabricvalueId + "' }",
                                contentType: "application/json; charset=utf-8",
                                dataType: "json",
                                async: "true",
                                cache: "false",
                                success: function (msg) {

                                    //saleprice = parseFloat(msg.d);
                                    //saleprice = parseFloat(saleprice) + parseFloat(vprice);
                                    if (document.getElementById('gvListProducts_lblNewVariantPrice_' + RowIndex) != null) {
                                        // $('#gvListProducts_lblNewVariantPrice_' + RowIndex).html(saleprice.toFixed(2));

                                        $('#gvListProducts_lblNewVariantPrice_' + RowIndex).html(msg.d);
                                        // document.getElementById('hdnVariPrice').value = saleprice.toFixed(2);
                                    }
                                },
                                Error: function (x, e) {
                                }
                            });
                }
            }
            else {
                if (Width != "" && Width2 != "" && Length != "" && Length2 != "" && options != "" && fabricnameoptin != '' && fabricvalueId != '0') {

                    $.ajax(
                            {
                                type: "POST",
                                url: "/TestMail.aspx/GetDataRomanprice",
                                data: "{ProductId: " + pid + ",ProductType: 3,Width: '" + Width + "',Width2: '" + Width2 + "', Qty: " + qid + ",options: '" + options + "',Length: '" + Length + "',Length2: '" + Length2 + "',fabricnameoptin: '" + fabricnameoptin + "',colorvalueid: '" + fabricvalueId + "',lift:'" + lift + "' }",
                                contentType: "application/json; charset=utf-8",
                                dataType: "json",
                                async: "true",
                                cache: "false",
                                success: function (msg) {

                                    saleprice = parseFloat(msg.d);
                                    saleprice = parseFloat(saleprice) + parseFloat(vprice);
                                    if (document.getElementById('gvListProducts_lblNewVariantPrice_' + RowIndex) != null) {
                                        $('#gvListProducts_lblNewVariantPrice_' + RowIndex).html(saleprice.toFixed(2));
                                        // document.getElementById('hdnVariPrice').value = saleprice.toFixed(2);
                                    }
                                },
                                Error: function (x, e) {
                                }
                            });
                }
            }
            var strMeasureName = document.getElementById('hdnforMeasureVName').value;
            var strMeasureValue = document.getElementById('hdnforMeasureVValue').value;
            if (strMeasureName == "") {
                strMeasureName = 'Width,Width2,Length,Length2,options,Roman Shade Design,Select color,';
            }
            if (strMeasureValue == "") {
                strMeasureValue = Width + ',' + Width2 + ',' + Length + ',' + Length2 + ',' + options + ',' + fabricnameoptin + ',' + colorname + ',';
            }

            if (document.getElementById('gvListProducts_hdnshadesvalue_' + RowIndex) != null && document.getElementById('gvListProducts_hdnshadesvalue_' + RowIndex).value == '1') {
                $.ajax(
                           {
                               type: "POST",
                               url: "/TestMail.aspx/GetDataAdminMessageRomanShade",
                               data: "{ProductId: " + pid + ",ProductType: 3,Qty: " + qid + ",vValueid: '" + strMeasureValue + "',vNameid: '" + strMeasureName + "' }",
                               contentType: "application/json; charset=utf-8",
                               dataType: "json",
                               async: "true",
                               cache: "false",
                               success: function (msg) {

                                   //alert(msg.d);
                                   if (ptype == 1 || ptype == 3) {
                                       if (document.getElementById('gvListProducts_lblEstimateddate_' + RowIndex) != null) {
                                           $('#gvListProducts_lblEstimateddate_' + RowIndex).html(msg.d);

                                       }
                                   }
                                   else {
                                       if (document.getElementById('gvListProducts_divestimateddate_' + RowIndex) != null) {
                                           $('#gvListProducts_divestimateddate_' + RowIndex).html(msg.d);

                                       }
                                   }
                               },
                               Error: function (x, e) {
                               }
                           });
            }
            else {

                $.ajax(
                            {
                                type: "POST",
                                url: "/TestMail.aspx/GetDataAdminMessage",
                                data: "{ProductId: " + pid + ",ProductType: 3,Qty: " + qid + ",vValueid: '" + strMeasureValue + "',vNameid: '" + strMeasureName + "' }",
                                contentType: "application/json; charset=utf-8",
                                dataType: "json",
                                async: "true",
                                cache: "false",
                                success: function (msg) {

                                    //alert(msg.d);
                                    if (ptype == 1 || ptype == 3) {
                                        if (document.getElementById('gvListProducts_lblEstimateddate_' + RowIndex) != null) {
                                            $('#gvListProducts_lblEstimateddate_' + RowIndex).html(msg.d);

                                        }
                                    }
                                    else {
                                        if (document.getElementById('gvListProducts_divestimateddate_' + RowIndex) != null) {
                                            $('#gvListProducts_divestimateddate_' + RowIndex).html(msg.d);

                                        }
                                    }
                                },
                                Error: function (x, e) {
                                }
                            });


            }
        }
        function setoriginalprice(id, Currtab) {

            if (Currtab == "1") {
                var hdnActual = id.replace('_liready_', '_hdnActual_');
                var hdnprice = id.replace('_liready_', '_hdnprice_');
                var NewVariantPrice = id.replace('_liready_', '_lblNewVariantPrice_');
                var PriceTag = id.replace('_liready_', '_lblPriceTag_');
            }
            else {
                var hdnActual = id.replace('_licustom_', '_hdnActual_');
                var hdnprice = id.replace('_licustom_', '_hdnprice_');
                var NewVariantPrice = id.replace('_licustom_', '_lblNewVariantPrice_');
                var PriceTag = id.replace('_licustom_', '_lblPriceTag_');
            }
            var price = document.getElementById(hdnActual).value;
            var saleprice = document.getElementById(hdnprice).value;
            if (parseFloat(saleprice) == parseFloat(0)) {
                saleprice = price;
            }
            if (document.getElementById(NewVariantPrice) != null && document.getElementById(PriceTag) != null) {
                if (Currtab == "1") {
                    document.getElementById(PriceTag).innerHTML = "Half Price Drapes Price : ";
                }
                else {
                    document.getElementById(PriceTag).innerHTML = "Your Price : ";
                }

                document.getElementById(NewVariantPrice).innerHTML = saleprice;
                //document.getElementById(NewVariantPrice).innerHTML = saleprice.toFixed(2);
            }
        }

        function ChangeCustomprice(productid, RowIndex) {

            var hdnActual = 'gvListProducts_hdnActual_' + RowIndex;
            var hdnprice = 'gvListProducts_hdnprice_' + RowIndex;
            var PriceTag = 'gvListProducts_lblPriceTag_' + RowIndex;
            var divproductvariant = 'gvListProducts_divproductvariant_' + RowIndex;
            var hdnpricetemp = 'gvListProducts_hdnpricetemp_' + RowIndex;
            var lblMadetoprice = 'gvListProducts_lblMadetoprice_' + RowIndex;
            var pridediv = 'gvListProducts_divYourPrice_' + RowIndex;
            var price = document.getElementById(hdnActual).value;
            var saleprice = document.getElementById(hdnprice).value;
            if (parseFloat(saleprice) == parseFloat(0)) {
                saleprice = price;
            }
            var sid = '';
            var wid = '';
            var qid = '';
            var lid = '';
            var oid = '';
            var pid = productid;
            document.getElementById('hdnType').value = document.getElementById('gvListProducts_hdnCurrTabvalue_' + RowIndex).value;
            var ptype = document.getElementById('hdnType').value;
            if (document.getElementById(hdnpricetemp) != null && document.getElementById(hdnpricetemp).value == '') {
                if (document.getElementById(pridediv) != null) {
                    document.getElementById(hdnpricetemp).value = document.getElementById(pridediv).innerHTML;
                }
            }

            sid = document.getElementById('gvListProducts_ddlcustomstyle_' + RowIndex + '').options[document.getElementById('gvListProducts_ddlcustomstyle_' + RowIndex + '').selectedIndex].value;
            wid = document.getElementById('gvListProducts_ddlcustomwidth_' + RowIndex + '').options[document.getElementById('gvListProducts_ddlcustomwidth_' + RowIndex + '').selectedIndex].value;
            lid = document.getElementById('gvListProducts_ddlcustomlength_' + RowIndex + '').options[document.getElementById('gvListProducts_ddlcustomlength_' + RowIndex + '').selectedIndex].value;
            if (document.getElementById('gvListProducts_ddlcustomoptin_' + RowIndex + '') != null)
            {
                oid = document.getElementById('gvListProducts_ddlcustomoptin_' + RowIndex + '').options[document.getElementById('gvListProducts_ddlcustomoptin_' + RowIndex + '').selectedIndex].value;
            }
            
            qid = document.getElementById('gvListProducts_txtMadetoMeasureQty_' + RowIndex + '').value;

            $.ajax(
                        {
                            type: "POST",
                            url: "/TestMail.aspx/GetDataAdmin",
                            data: "{ProductId: " + pid + ",Width: " + wid + ",Length: " + lid + ",Qty: " + qid + ",style: '" + sid + "',options: '" + oid + "',ProductType: 1 }",
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            async: "true",
                            cache: "false",
                            success: function (msg) {
                                if (document.getElementById(pridediv) != null) {
                                    //$('#divcustomprice').html('<tt>Your Price :</tt> <strong>$' + msg.d + '</strong>');
                                    $('#' + pridediv).html('<span style="color: Black; font-weight: bold;">Your Price($) :</span> <span style="color: red; font-weight: bold;">' + msg.d + '</span>');
                                    document.getElementById(lblMadetoprice).value = msg.d;
                                    document.getElementById('hdnVariPrice').value = msg.d;
                                }
                            },
                            Error: function (x, e) {
                            }
                        });
            if (ptype == 1 || ptype == 3) {
                chkAddtocartNew(RowIndex);
            }
            else {
                chkAddtocartForMadetoMeasureNew(RowIndex);
            }
            var strMeasureName = document.getElementById('hdnforMeasureVName').value;
            var strMeasureValue = document.getElementById('hdnforMeasureVValue').value;
            $.ajax(
                        {
                            type: "POST",
                            url: "/TestMail.aspx/GetDataAdminMessage",
                            data: "{ProductId: " + pid + ",ProductType: " + ptype + ",Qty: " + qid + ",vValueid: '" + strMeasureValue + "',vNameid: '" + strMeasureName + "' }",
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            async: "true",
                            cache: "false",
                            success: function (msg) {

                                //alert(msg.d);
                                if (ptype == 1 || ptype == 3) {
                                    if (document.getElementById('gvListProducts_lblEstimateddate_' + RowIndex) != null) {
                                        $('#gvListProducts_lblEstimateddate_' + RowIndex).html(msg.d);

                                    }
                                }
                                else {
                                    if (document.getElementById('gvListProducts_divestimateddate_' + RowIndex) != null) {
                                        $('#gvListProducts_divestimateddate_' + RowIndex).html(msg.d);

                                    }
                                }
                            },
                            Error: function (x, e) {
                            }
                        });
            //            }
            //            else {
            //                if (document.getElementById(pridediv) != null) {
            //                     $('#' + pridediv).html(document.getElementById(hdnpricetemp).value);
            //                }
            //            }
        }

        function tabdisplaycart(id, tabmode) {
            var tabvalue = id;
            var licustom;
            var liReady;
            var CurrTabvalue;
            var lifabric;
            if (tabmode == "1") {
                licustom = id.replace('_liready_', '_licustom_');
                CurrTabvalue = id.replace('_liready_', '_hdnCurrTabvalue_');
                lifabric = id.replace('_liready_', '_lifabric_');
                liReady = id;
                if (document.getElementById(CurrTabvalue) != null) {
                    document.getElementById(CurrTabvalue).value = '1';
                }
            }
            if (tabmode == "2") {
                liReady = id.replace('_licustom_', '_liready_');
                CurrTabvalue = id.replace('_licustom_', '_hdnCurrTabvalue_');
                lifabric = id.replace('_licustom_', '_lifabric_');
                licustom = id;
                if (document.getElementById(CurrTabvalue) != null) {
                    document.getElementById(CurrTabvalue).value = '2';
                }
            }
            if (tabmode == "3") {
                liReady = id.replace('_lifabric_', '_liready_');
                licustom = id.replace('_lifabric_', '_licustom_');
                CurrTabvalue = id.replace('_lifabric_', '_hdnCurrTabvalue_');
                lifabric = id;
                if (document.getElementById(CurrTabvalue) != null) {
                    document.getElementById(CurrTabvalue).value = '3';
                }
            }

            //MADE TO ORDER -- gvListProducts_licustom_0
            //gvListProducts_divready_0
            //READY MADE-- gvListProducts_liready_0
            //gvListProducts_divcustom_0

            if (document.getElementById(tabvalue) != null) {

                if (document.getElementById(licustom) != null) {
                    document.getElementById(licustom).className = '';
                }
                var tabCustomdiv;
                if (tabmode == "1") {
                    tabCustomdiv = id.replace('_liready_', '_divcustom_');
                }
                else if (tabmode == "2") {
                    tabCustomdiv = id.replace('_licustom_', '_divcustom_');
                }
               else if (tabmode == "3") {
                    tabCustomdiv = id.replace('_lifabric_', '_divcustom_');
                }

                if (document.getElementById(tabCustomdiv) != null) {
                    document.getElementById(tabCustomdiv).style.display = 'none';
                }
                if (document.getElementById(lifabric) != null) {
                    document.getElementById(lifabric).className = '';
                }
                var tabfabricdiv;
                if (tabmode == "1") {
                    tabfabricdiv = id.replace('_liready_', '_divfabric_');
                }

                else if (tabmode == "2") {
                    tabfabricdiv = id.replace('_licustom_', '_divfabric_');
                }
                else if (tabmode == "3") {
                    tabfabricdiv = id.replace('_lifabric_', '_divfabric_');
                }
                if (document.getElementById(tabfabricdiv) != null) {
                    document.getElementById(tabfabricdiv).style.display = 'none';
                }


                var tabvalueDivName;
                if (document.getElementById(liReady) != null) {
                    document.getElementById(liReady).className = '';
                }
                if (tabmode == "1") {
                    tabvalueDivName = id.replace('_liready_', '_divready_');
                }
                else if (tabmode == "2") {
                    tabvalueDivName = id.replace('_licustom_', '_divready_');
                }
            else if (tabmode == "3") {
                tabvalueDivName = id.replace('_lifabric_', '_divready_');
                }

                if (document.getElementById(tabvalueDivName) != null) {
                    document.getElementById(tabvalueDivName).style.display = 'none';
                }
            }

            if (document.getElementById(id) != null) {
                document.getElementById(id).className = 'tabberactive';
            }

            if (tabvalue.indexOf('_licustom') > -1) {
                var tab01 = id.replace('_licustom_', '_divcustom_');
                if (document.getElementById(tab01) != null) {
                    document.getElementById(tab01).style.display = '';
                }
            }
            else if (tabvalue.indexOf('_lifabric') > -1) {
                var tab03 = id.replace('_lifabric_', '_divfabric_');
                if (document.getElementById(tab03) != null) {
                    document.getElementById(tab03).style.display = '';
                }
            }
            else {
                var tab02 = id.replace('_liready_', '_divready_');
                if (document.getElementById(tab02) != null) {
                    document.getElementById(tab02).style.display = '';
                }
            }
            //setoriginalprice(id, tabmode);
        }
        function Changefabricprice(productid, RowIndex) {

            var hdnfabricprice = 'gvListProducts_hdnfabricprice_' + RowIndex;


            var divproductvariant = 'gvListProducts_divproductvariant_' + RowIndex;
            var hdnpricetemp = 'gvListProducts_hdnfabricpricetemp_' + RowIndex;

            var pridediv = 'gvListProducts_divyourfabricprice_' + RowIndex;
            var price = document.getElementById(hdnfabricprice).value;
            var saleprice = document.getElementById(hdnfabricprice).value;
            if (parseFloat(saleprice) == parseFloat(0)) {
                saleprice = price;
            }
            var sid = '';
            var wid = '';
            var qid = '';
            var lid = '';
            var oid = '';
            var pid = productid;
            document.getElementById('hdnType').value = '2';
            var ptype = document.getElementById('hdnType').value;
            if (document.getElementById(hdnpricetemp) != null && document.getElementById(hdnpricetemp).value == '') {
                if (document.getElementById(pridediv) != null) {
                    document.getElementById(hdnpricetemp).value = document.getElementById(pridediv).innerHTML;
                }
            }



            qid = document.getElementById('gvListProducts_txtfabricQty_' + RowIndex + '').value;

            $.ajax(
                        {
                            type: "POST",
                            url: "/TestMail.aspx/GetDataAdminfabric",
                            data: "{ProductId: " + pid + ",Qty: " + qid + "}",
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            async: "true",
                            cache: "false",
                            success: function (msg) {
                                if (document.getElementById(pridediv) != null) {
                                    //$('#divcustomprice').html('<tt>Your Price :</tt> <strong>$' + msg.d + '</strong>');
                                    $('#' + pridediv).html('<span style="color: Black; font-weight: bold;">Your Price($) :</span> <span style="color: red; font-weight: bold;">' + msg.d + '</span>');

                                }
                            },
                            Error: function (x, e) {
                            }
                        });
            //if (ptype == 1 || ptype == 3) {
            //    chkAddtocartNew(RowIndex);
            //}
            //else {
            //    chkAddtocartForMadetoMeasureNew(RowIndex);
            //}
            var strMeasureName = document.getElementById('hdnforMeasureVName').value;
            var strMeasureValue = document.getElementById('hdnforMeasureVValue').value;
            $.ajax(
                        {
                            type: "POST",
                            url: "/TestMail.aspx/GetDataAdminMessage",
                            data: "{ProductId: " + pid + ",ProductType: " + ptype + ",Qty: " + qid + ",vValueid: '" + strMeasureValue + "',vNameid: '" + strMeasureName + "' }",
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            async: "true",
                            cache: "false",
                            success: function (msg) {

                                //alert(msg.d);


                                if (document.getElementById('gvListProducts_divestimateddatefabric_' + RowIndex) != null) {
                                    $('#gvListProducts_divestimateddatefabric_' + RowIndex).html(msg.d);

                                }

                            },
                            Error: function (x, e) {
                            }
                        });

        }

        function chkAddtocartForFabric(RowIndex) {
            var strMeasureName = "";
            var strMeasureValue = "";

            if (document.getElementById('gvListProducts_txtfabricQty_' + RowIndex + '') != null && (document.getElementById('gvListProducts_txtfabricQty_' + RowIndex + '').value == '' || document.getElementById('gvListProducts_txtfabricQty_' + RowIndex + '').value == 0)) {
                alert('Please Enter Valid Quantity.');
                return false;
            }





            if (document.getElementById('gvListProducts_txtfabricQty_' + RowIndex + '') != null && (document.getElementById('gvListProducts_txtfabricQty_' + RowIndex + '').value == '' || document.getElementById('gvListProducts_txtfabricQty_' + RowIndex + '').value == 0)) {
                strMeasureName = strMeasureName + 'Quantity (Yard)' + ',';
                strMeasureValue = strMeasureValue + document.getElementById('gvListProducts_txtfabricQty_' + RowIndex + '').value.replace(' ', '') + ',';
            }

            //            if (document.getElementById('gvListProducts_dlcustomqty_' + RowIndex + '') != null && document.getElementById('gvListProducts_dlcustomqty_' + RowIndex + '').selectedIndex > 0) {
            //                strMeasureName = strMeasureName + 'Quantity (Panels)' + ',';
            //                strMeasureValue = strMeasureValue + document.getElementById('gvListProducts_dlcustomqty_' + RowIndex + '').options[document.getElementById('gvListProducts_dlcustomqty_' + RowIndex + '').selectedIndex].value.replace(' ', '') + ',';
            //            }

            document.getElementById('hdnVariProductId').value = document.getElementById('gvListProducts_hdnproductid_' + RowIndex).value;
            document.getElementById('hdnVariQuantity').value = document.getElementById('gvListProducts_txtfabricQty_' + RowIndex).value;
            document.getElementById('hdnVariPrice').value = document.getElementById('gvListProducts_hdnfabricprice_' + RowIndex).value;

            document.getElementById('hdnforMeasureVName').value = strMeasureName;
            document.getElementById('hdnforMeasureVValue').value = strMeasureValue;

            document.getElementById('btnAddfabric').click();

            return false;
        }
        function getAllproduct(id, idbtn, hdnsatus) {
            //hdnVariantValueid
            document.getElementById('hdnVariantValueid').value = "";
            var allkeys = document.getElementById(id).getElementsByTagName('*');
            var allprice = 0;
            var i = 0;
            var flg = false;
            for (i = 0; i < allkeys.length; i++) {
                var elt = allkeys[i];
                if (elt.type == "select-one") {

                    if (elt.selectedIndex > 0) {
                        document.getElementById('hdnVariantValueid').value = document.getElementById('hdnVariantValueid').value + elt.options[elt.selectedIndex].value.toString() + ',';
                    }
                }
            }
            document.getElementById(idbtn).click();

        }

        function changeprice(id, spanid, hdnstatus, price, txtqty) {
            var originalPrice = 0;
            var idTotal = 0;
            var allkeys = document.getElementById(id).getElementsByTagName('*');
            var allprice = 0;
            var i = 0;
            var flg = false;
            for (i = 0; i < allkeys.length; i++) {
                var elt = allkeys[i];

                if (elt.type == "select-one") {
                    var iPrice = elt.options[elt.selectedIndex].text;
                    var ichk = iPrice.length;
                    if (elt.selectedIndex == 0) {
                        idTotal = 1;
                    }
                    if (iPrice.indexOf('($') > -1) {
                        iPrice = iPrice.toString().substring(iPrice.indexOf('($') + 2);
                        iPrice = iPrice.replace(')', '');
                        allprice += parseFloat(iPrice);
                        var totalprice = 0;
                    }
                }
            }
            if (idTotal == 0) {
                if (document.getElementById(hdnstatus)) { document.getElementById(hdnstatus).value = "2" };
            }
            var qty = 1;
            if (document.getElementById(txtqty) != null && document.getElementById(txtqty).value != '' && document.getElementById(txtqty).value != '0') {
                qty = document.getElementById(txtqty).value;
            }
            var pp = (parseFloat(price) + parseFloat(allprice)) * parseFloat(qty);
            if (document.getElementById(spanid)) { document.getElementById(spanid).innerHTML = pp.toFixed(2); }

        }
        function chkselect() {
            var allElts = document.forms['aspnetForm'].elements;
            var i;
            var Chktrue;
            Chktrue = 0;
            var CountQty = 0;
            for (i = 0; i < allElts.length; i++) {
                var elt = allElts[i];
                if (elt.type == "checkbox") {
                    if (elt.checked == true) {
                        Chktrue = Chktrue + 1;
                        var tQty = elt.id.toString().replace('chkSelect', 'txtProQty')
                        if (document.getElementById(tQty) != null && (document.getElementById(tQty).value == '' || document.getElementById(tQty).value == '0')) {
                            CountQty = CountQty + 1;
                        }
                    }
                }
            }

            if (Chktrue < 1) {
                jAlert("Please select at least one record...", "Required information");
                return false;
            }
            if (CountQty >= 1) {
                jAlert("Please enter valid Quantity...", "Required information");
                return false;
            }
            chkHeight();
            return true;
        }


    </script>
    <script language="javascript" type="text/javascript">
        var path;
        function OpenPopUp(e) {

            var offsetX = findPosX(e) + 90;
            var offsetY = findPosY(e);
            document.getElementById('zoom1-big').style.left = offsetX + 'px';
            document.getElementById('zoom1-big').style.top = offsetY + 'px';
            document.getElementById('zoom1-big').style.display = '';
            var temp = new Image();
            path = e.id;
            temp.src = e.id;
            //alert(temp.src);
            setTimeout(ShowImage, 1500);
        }
        function ShowImage() {
            document.getElementById('PopUpIconImage').src = path;
        }
        function ClosePopUp() {
            document.getElementById('zoom1-big').style.display = "none";
            document.getElementById('PopUpIconImage').src = "../../Client/images/ajax-loader.gif";
        }
        function findPosX(obj) {
            var curleft = 0;
            if (obj.offsetParent)
                while (1) {
                    curleft += obj.offsetLeft;
                    if (!obj.offsetParent)
                        break;
                    obj = obj.offsetParent;
                }
            else if (obj.x)
                curleft += obj.x;
            return curleft;
        }

        function findPosY(obj) {
            var curtop = 0;
            if (obj.offsetParent)
                while (1) {
                    curtop += obj.offsetTop;
                    if (!obj.offsetParent)
                        break;
                    obj = obj.offsetParent;
                }
            else if (obj.y)
                curtop += obj.y;
            return curtop;
        }
        function chkHeight() {
            var windowHeight = 0;
            windowHeight = $(document).height(); //window.innerHeight;

            document.getElementById('prepage').style.height = windowHeight + 'px';
            document.getElementById('prepage').style.display = '';
        }
        function ShowVariantdiv(id) {
            var allType = document.getElementById('divProList').getElementsByTagName('*');
            for (i = 0; i < allType.length; i++) {
                var elt = allType[i];
                if (elt.id.toString().indexOf('divvariant') > -1 && id == elt.id && elt.id.toString().indexOf('divvariantname') <= -1) {
                    elt.style.display = '';
                }
                else if (elt.id.toString().indexOf('divvariant') > -1 && elt.id.toString().indexOf('divvariantname') <= -1) {
                    elt.style.display = 'none';
                }
            }
        }
        function HideVariantdiv(id) {
            if (document.getElementById(id)) {
                document.getElementById(id).style.display = 'none';
            }
        }
        function onKeyPressBlockNumbers(e) {

            var key = window.event ? window.event.keyCode : e.which;

            if (key == 32 || key == 39 || key == 37 || key == 46 || key == 13 || key == 8 || key == 9 || key == 189 || key == 109 || key == 0) {
                return key;
            }

            var keychar = String.fromCharCode(key);

            var reg = /\d/;
            if (window.event)
                return event.returnValue = reg.test(keychar);
            else
                return reg.test(keychar);
        }

        function sendData(dropid, divid, rowindex, productid, IsRoman) {

            var pid = productid;
            //var vid = document.getElementById(dropid).value;
            var vid = document.getElementById(dropid).title;
            PriceChangeondropdown(rowindex);
            $.ajax(
                            {
                                type: "POST",
                                url: "/TestMail.aspx/Getvariantdataforrdo",
                                data: "{ProductId: " + pid + ",variantvalueId: " + vid + ",RowIndex: " + rowindex + ",Roman: " + IsRoman + "}",
                                contentType: "application/json; charset=utf-8",
                                dataType: "json",
                                async: "true",
                                cache: "false",
                                success: function (msg) {
                                    //$("#myDiv:last").append(msg.d);
                                    if (document.getElementById(divid) != null) {
                                        $('#' + divid).html('');
                                        $('#' + divid).html(msg.d);
                                        if (IsRoman == 1) {
                                            PriceChangeondropdownforroman(rowindex);
                                        }
                                        else {
                                            PriceChangeondropdown(rowindex);
                                        }
                                    }
                                },
                                Error: function (x, e) {
                                    if (IsRoman == 1) {
                                        PriceChangeondropdownforroman(rowindex);
                                    }
                                    else {
                                        PriceChangeondropdown(rowindex);
                                    }
                                }
                            });
        }


        function chksearch(e) {
          
            var code = (e.keyCode ? e.keyCode : e.which);
            if (code == 13) {
                if (document.getElementById('txtSearch').value.trim() == '') {
                    jAlert('Please Enter Search Term.','Required Information');
                    document.getElementById('txtSearch').focus(); return false;
                } else { chkHeight(); document.getElementById('btnSearch').click(); return true; }

            } else { return true;}
            return false;
        }
    </script>
    <style type="text/css">
        .readymade-detail {
            float: left;
            width: 100%;
            padding: 5px 0 0 0;
        }

        .readymade-detail-pt1 {
            float: left;
            width: 100%;
            padding: 0 0 10px 0;
        }
        /*.readymade-detail-left{float:left; width:24%; font-size:10px; color:#848383; line-height:20px; text-transform:uppercase;}*/
        .readymade-detail-left {
            float: left;
            width: 24%;
            color: #848383;
            line-height: 20px;
            font-family: Arial,Helvetica,sans-serif;
            text-transform: none;
            font-size: 12px;
        }

        .readymade-detail-right {
            float: right;
            width: 74%;
        }

        .option1 {
            float: left;
            width: 75%;
            border: 1px solid #ddd;
            padding: 1px;
            font-size: 12px;
            color: #848383;
            line-height: 20px;
            height: 20px;
        }

        .readymade-detail-right span {
            float: left;
            margin: 0 0 0 5px;
            line-height: 20px;
            font-size: 12px;
            color: #848383;
        }

            .readymade-detail-right span a {
                text-decoration: none;
                color: #848383;
            }

        .readymade-detail .price-detail {
            float: left;
            width: 100%;
            padding: 5px 0 0 0;
        }

        .readymade-detail .price-detail-left {
            float: left;
            width: 285px;
        }

            .readymade-detail .price-detail-left p {
                float: left;
                width: 100%;
                font-size: 12px;
                color: #848383;
                line-height: 15px;
            }

                .readymade-detail .price-detail-left p tt {
                    float: left;
                    font-family: 'helvetica-l',Arial,Helvetica,sans-serif;
                    font-size: 14px;
                    padding: 0 5px 0 0;
                }

                .readymade-detail .price-detail-left p span {
                    font-size: 12px;
                    color: #848383;
                    line-height: 15px;
                }

                    .readymade-detail .price-detail-left p span.per {
                        font-size: 12px;
                        color: #b92127;
                        line-height: 15px;
                        margin: 0 0 0 2px;
                    }

                .readymade-detail .price-detail-left p strong {
                    font-size: 16px;
                    color: #b92127;
                    line-height: 15px;
                    margin: 0 0 0 2px;
                }

        .readymade-detail .price-detail-right {
            float: right;
            margin: 0 0 0 0;
            text-align: left;
            padding: 0;
        }

            .readymade-detail .price-detail-right p tt {
                float: left;
                font-family: Arial,Helvetica,sans-serif;
                font-size: 12px;
                padding: 0 5px 0 0;
            }

        /* Style for Tabbing*/


        .item-left-row3 {
            float: left;
            width: 100%;
            border-bottom: 1px solid #ddd;
            padding: 20px 0 0 0;
        }

        .item-tb-left {
            float: left;
            font-family: 'helvetica-l',Arial,Helvetica,sans-serif;
            width: 100%;
            padding: 0 0px 0px 0;
        }

        .item-tb-left-tab {
            float: left;
            width: 100%;
            padding-left: 0;
        }

        .tabing {
            float: left;
            height: 30px;
        }

            .tabing ul {
                list-style: none;
                padding: 0;
                margin: 0;
                float: left;
            }

                .tabing ul li {
                    list-style: none;
                    float: left;
                    background: url(../images/tab_left.jpg) left -34px no-repeat;
                    padding-left: 8px;
                    height: 30px;
                    padding: 0 0 0 3px;
                    margin: 0 10px 0 0;
                }

                    .tabing ul li:hover {
                        background: url(/images/tab_left.jpg) left top no-repeat;
                    }

                    .tabing ul li a {
                        float: left;
                        outline: none;
                        margin-right: 0px;
                        text-decoration: none;
                        background: url(/images/tab_right.jpg) right -34px no-repeat;
                        padding: 9px 15px 5px 10px;
                        font-size: 12px;
                        text-transform: uppercase;
                        font-weight: normal;
                        color: #848383;
                        height: 17px;
                    }

                        .tabing ul li a:hover {
                            background: url(/images/tab_right.jpg) right 0 no-repeat;
                            font-size: 12px;
                            color: #848383;
                            padding: 9px 15px 5px 10px;
                            text-transform: uppercase;
                            font-weight: normal;
                        }

                    .tabing ul li:hover a {
                        background: url(/images/tab_right_active.jpg) right 0 no-repeat;
                        font-size: 12px;
                        color: #fff;
                        padding: 9px 15px 5px 10px;
                        text-transform: uppercase;
                        font-weight: normal;
                    }

                    .tabing ul li.tabberactive {
                        background: url(/images/tab_left_active.jpg) left top no-repeat;
                    }

                        .tabing ul li.tabberactive a {
                            background: url(/images/tab_right_active.jpg) right 0 no-repeat;
                            font-size: 12px;
                            color: #fff;
                            padding: 9px 15px 5px 10px;
                            text-transform: uppercase;
                            font-weight: normal;
                        }

        .tabberactive ul {
            list-style: none;
            padding: 0;
            margin: 0;
            float: left;
        }

            .tabberactive ul li {
                list-style: none;
                padding: 0;
                margin: 0;
            }

        .tabberactive a {
            background: url(/images/tab_left.jpg) no-repeat left 0px;
            padding: 0 0 0 0px;
            float: left;
            color: #848383;
            font-weight: normal;
        }

            .tabberactive a:hover {
                background: url(/images/tab_right.jpg) no-repeat right 0px;
                padding: 0 5px 0 10px;
                float: left;
                color: #848383;
                font-weight: normal;
            }

        .tabberlive .tabbertabhide {
            display: none;
        }

        .tabberlive .tabbertab {
            width: 100%;
            float: left;
            position: relative;
            padding: 5px 0;
            border: 1px solid #eeeeee;
        }

            .tabberlive .tabbertab h6 {
                display: none;
            }

        .tabberlive .tabbertabhide {
            display: none;
        }

        .tabberlive .tabbertab {
            width: 100%;
            float: left;
            position: relative;
            padding: 5px 0;
        }

            .tabberlive .tabbertab h6 {
                display: none;
            }

            .tabberlive .tabbertab p {
                font-size: 13px;
                color: #848383;
                margin: 0;
                line-height: 16px;
                padding: 0 0 5px 0;
            }

        .tabbertab ul {
            padding: 0px 0px 0px 10px;
            width: 500px;
            margin: 0px;
            float: left;
            list-style: none;
            font-size: 11px;
            color: #000;
        }

            .tabbertab ul li {
                padding: 5px 0px 3px 15px;
                float: left;
                width: 500px;
                background: url(images/left_bullet.gif) no-repeat left 9px;
            }

                .tabbertab ul li ul {
                    padding: 0px;
                    margin: 0px;
                }

                    .tabbertab ul li ul li {
                        float: left;
                        width: 300px;
                        padding-bottom: 0px;
                    }

        .checkbox {
            float: left;
        }

        .checkbox-text {
            float: left;
            width: 36%;
            line-height: 22px;
            text-align: left;
        }

        .checkout-td-text {
            border: 1px solid #ddd;
            color: #6C6D71;
            font-size: 12px;
            height: 18px;
            padding: 1px 1%;
            width: 60%;
            margin: 0;
        }
    </style>
    <style type="text/css">
        .content-table td {
            background-color: transparent !important;
        }
    </style>
</head>
<body style="background: none;">
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts-main.js"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts.js"></script>
    <script type="text/javascript" language="javascript">
        <%=strScriptVar %>

        //     function chkvariantvalues_5632()
        //     {
        //         if(document.getElementById('Selectvariant-1584') != null &&  document.getElementById('Selectvariant-1584').selectedIndex==0)
        //          {
        //            jAlert('Please Select Top Header Design','Required information','Selectvariant-1584');
        //            return false;
        //          }
        //        return true;
        //     }
    </script>
    <form id="aspnetForm" runat="server">
        <table cellspacing="0" cellpadding="0" border="0" width="100%" class="content-table border-td"
            style="padding: 2px;">
            <tbody>
                <tr>
                    <th>
                        <div class="main-title-left" style="color: #fff; text-align: left; padding-top: 8px;">
                            Search Product(s)
                        </div>
                        <div class="main-title-right">
                            <asp:HiddenField ID="hfskus" runat="server" />
                            <asp:ImageButton ID="imgMainClose" CssClass="close" OnClientClick="chkHeight();"
                                runat="server" Style="border: 0px" AlternateText="Close" ToolTip="Close" OnClick="ClearShopping" />
                        </div>
                    </th>
                </tr>
                <tr>
                    <td class="border">
                        <table cellpadding="3" cellspacing="0" style="border: 1px solid gray; width: 100%; margin-bottom: 13px;">
                            <tr runat="server" id="trSelectedproduct" visible="false">
                                <td colspan="2">
                                    <table width="100%" cellpadding="0" cellspacing="0">
                                        <tr style="background-color: #E5F0EF; height: 26px">
                                            <td style="color: #666666; background-color: #fff !important; font-size: 14px; font-family: Verdana,Arial,Helvetica,sans-serif; font-weight: bold; width: 30%; padding-left: 5px;">Selected Products:
                                            </td>
                                            <td style="color: #2A5F00; font-size: 11px; font-family: Verdana,Arial,Helvetica,sans-serif; width: 40%; text-align: center; display: none; background-color: #fff !important;">Total
                                            <asp:Label ID="lblselProductCount" runat="server" Text="0" Font-Size="11px" Font-Bold="true"></asp:Label>
                                                Products Selected Within
                                            <asp:Label ID="lblselCategoryCount" runat="server" Text="0" Font-Size="11px" Font-Bold="true"></asp:Label>
                                                Categories
                                            </td>
                                            <td align="right" style="padding-top: 5px; padding-right: 5px; background-color: #fff !important;">
                                                <asp:ImageButton ID="btnSubmit" OnClientClick="chkHeight();" runat="server" OnClick="btnSubmit_Click"
                                                    Visible="false" />
                                                <asp:ImageButton ID="ImgColse2" OnClientClick="chkHeight();" runat="server" Style="border: 0px"
                                                    OnClick="ClearShopping" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="3">&nbsp;
                                            </td>
                                        </tr>
                                    </table>
                                    <div style="border: 1px solid #C1C1C1; font-size: 11px; color: #323232; font-family: Verdana,Arial,Helvetica,sans-serif;">
                                        <div style="background-color: rgb(242, 242, 242); height: 20px; font-weight: bold; padding-top: 2px; width: 100%; border-bottom: 1px solid #C1C1C1;">
                                            <table cellpadding="0" cellspacing="0" id="divgridhead" runat="server" style="width: 100%">
                                                <tr>
                                                    <td width="35%" style="padding-left: 5px;">Product Name
                                                    </td>
                                                    <td width="12%">Product Code&nbsp;
                                                    </td>
                                                    <td width="10%" align="center">Quantity&nbsp;
                                                    </td>
                                                    <td width="10%" align="center">Price&nbsp;
                                                    </td>
                                                    <td width="10%" id="tdDiscountPrice" runat="server" visible="false" align="center">Dis.Price<asp:Label ID="lblHeaderDiscount" runat="server"></asp:Label>
                                                    </td>
                                                    <td width="10%" align="center">SubTotal&nbsp;
                                                    </td>
                                                    <td width="8%" align="center">Remove&nbsp;
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                        <div id="divselected" runat="server" style="padding-left: 5px;">
                                            <asp:GridView ID="grdSelected" runat="server" AutoGenerateColumns="False" BorderStyle="Solid"
                                                CellPadding="0" CellSpacing="2" CssClass="list-table-border" GridLines="None"
                                                BorderWidth="0" ForeColor="#323232" ShowHeader="false" Width="100%" OnRowDataBound="grdSelected_RowDataBound"
                                                OnRowCommand="grdSelected_RowCommand">
                                                <Columns>
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblName" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"Name") %>'></asp:Label>
                                                            <asp:Label ID="lblProductID" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container.DataItem,"ProductID") %>'></asp:Label>
                                                            <asp:Label ID="lblCustomerCartId" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container.DataItem,"CustomCartId") %>'></asp:Label>
                                                            <asp:Label ID="lblVariantNames" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"VariantNames") %>'
                                                                Visible="false"></asp:Label>
                                                            <asp:Label ID="lblVariantValues" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"VariantValues") %>'
                                                                Visible="false"></asp:Label>
                                                            <asp:Label ID="lblProductType" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"ProductType") %>'
                                                                Visible="false"></asp:Label>
                                                             <asp:Label ID="lblisProductType" runat="server" Visible="false" Text='<%# Eval("isProductType")%>'></asp:Label>
                                                            <asp:Label ID="lblRelatedproductID" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"RelatedproductID") %>'
                                                                Visible="false"></asp:Label>

                                                            <asp:HiddenField ID="hdnswatchqty" runat="server" Value='0' />
                                                            <asp:HiddenField ID="hdnswatchtype" runat="server" Value='' />
                                                        </ItemTemplate>
                                                        <ItemStyle Width="35%" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblSKU" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"SKU") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle Width="12%" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblQtyGrid" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"Quantity") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle Width="10%" HorizontalAlign="Center" />
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            $<asp:Label ID="lblPrice" runat="server" Text='<%# Math.Round(Convert.ToDecimal(DataBinder.Eval(Container.DataItem,"SalePrice")),2) %>'></asp:Label>
                                                            <asp:Label ID="lblSalePrice" runat="server" Text='<%#Eval("SalePrice")%>' Visible="false"></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle Width="10%" HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblDiscountPrice" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"DiscountPercent")%>'
                                                                Visible="false"></asp:Label>
                                                            $<asp:Label ID="lblOrginalDiscountPrice" runat="server"></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle Width="10%" HorizontalAlign="Right" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            $<asp:Label ID="lblGridPrice" runat="server" Text='<%# GetSubTotal(Convert.ToDecimal(DataBinder.Eval(Container.DataItem,"SalePrice")), Convert.ToInt32(DataBinder.Eval(Container.DataItem,"Quantity"))) %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle Width="10%" HorizontalAlign="Right" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <asp:ImageButton ID="btndel" runat="server" ImageUrl="~/Admin/images/btndel.gif"
                                                                OnClientClick="javascript:if(confirm('Are you sure , you want to delete this Product ?')){ chkHeight();}else{return false;}"
                                                                CommandArgument='<%# DataBinder.Eval(Container.DataItem,"CustomCartID") %>' CommandName="delMe" />
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="center" />
                                                        <ItemStyle Width="8%" />
                                                    </asp:TemplateField>
                                                </Columns>
                                                <AlternatingRowStyle CssClass="altrow" BackColor="#F2F2F2" VerticalAlign="top" />
                                                <RowStyle BackColor="White" BorderColor="White" BorderStyle="Solid" BorderWidth="1px"
                                                    Height="24px" HorizontalAlign="Left" />
                                            </asp:GridView>
                                        </div>
                                        <div style="width: 100%">
                                            <table cellpadding="0" cellspacing="0" style="width: 100%">
                                                <tr>
                                                    <td style="width: 82%; padding-top: 10px; padding-bottom: 10px; text-align: right;">
                                                        <b>Total : </b>
                                                    </td>
                                                    <td style="width: 18%; padding-top: 10px; padding-bottom: 10px;">&nbsp; <b>$<asp:Label ID="lblTotal" runat="server"></asp:Label></b>
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td class="border" style="padding-bottom: 5px;">
                        <center>
                            <b>
                                <asp:Label ID="lbMsg" runat="server" Style="font-family: arial,helvetica; color: Red; font-size: 13px;"
                                    Font-Size="X-Small"></asp:Label></b></center>
                    </td>
                </tr>
                <tr>
                    <td class="border">
                        <table cellpadding="3" cellspacing="0" style="border: 1px solid gray; width: 100%">
                            <tr>
                                <th>
                                    <div class="main-title-left" style="color: #fff; text-align: left; padding-top: 8px;">
                                        Search New Product(s)
                                    </div>
                                    <div class="main-title-right">
                                    </div>
                                </th>
                            </tr>
                            <tr>
                                <td style="height: 5px;" colspan="2"></td>
                            </tr>
                            <tr style="color: #666666; font-size: 12px; font-family: Verdana,Arial,Helvetica,sans-serif;">
                                <td colspan="2">
                                    <table width="100%">
                                        <tr>
                                            <td width="377px" style="padding-bottom: 3px;">Search :&nbsp;&nbsp;&nbsp;&nbsp;
                                            <asp:TextBox ID="txtSearch" runat="server" Style="width: 295px;" CssClass="order-textfield" onkeypress="return chksearch(event);"
                                                Height="14px"></asp:TextBox>
                                            </td>
                                            <td align="left">
                                                <asp:ImageButton ID="btnSearch" runat="server" OnClick="btnSearch_Click" OnClientClick="if(document.getElementById('txtSearch').value.trim()==''){ alert('Please Enter Search Term.');document.getElementById('txtSearch').focus();return false;}else{chkHeight(); return true;}" />
                                                <asp:ImageButton ID="btnShowAll" runat="server" OnClick="btnShowAll_Click" OnClientClick="chkHeight();" />
                                            </td>
                                            <td style="color: #666666; font-size: 12px; font-family: Verdana,Arial,Helvetica,sans-serif; display: none;"
                                                align="right">
                                                <asp:ImageButton ID="btnSelectItem" runat="server" OnClick="btnSelectItem_Click"
                                                    OnClientClick="return chkselect();" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr id="trBottom" runat="server" style="color: #666666; font-size: 12px; font-family: Verdana,Arial,Helvetica,sans-serif;">
                                <td colspan="2">
                                    <table width="100%">
                                        <tr>
                                            <td align="left" colspan="2">
                                                <span style="color: #2A5F00; font-size: 11px; font-family: Verdana,Arial,Helvetica,sans-serif; text-align: right;">Products Found:
                                                <asp:Label ID="foundPro" runat="server" Text="0" Font-Size="10px" Font-Bold="true"></asp:Label></span>
                                            </td>
                                            <%-- <td width="420px">
                                            <div class="paging" id="DivBottom" runat="server">
                                                <span id="trProduct" runat="server">
                                                    <asp:Label ID="lblPage" runat="server" Text="Page"></asp:Label>
                                                    &nbsp; </span>
                                            </div>--%>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <div style="overflow: auto; font-size: 11px; color: #323232; font-family: Verdana,Arial,Helvetica,sans-serif;">
                                        <div id="zoom1-big" style="position: absolute; left: 387px; top: 65px; z-index: 0;">
                                            <img id="PopUpIconImage" alt="" src="/Admin/images/loading.gif" />
                                        </div>
                                        <div id="divProList" runat="server">
                                            <asp:GridView ID="gvListProducts" runat="server" AutoGenerateColumns="False" BorderStyle="Solid"
                                                EnableViewState="true" CellPadding="1" CellSpacing="1" GridLines="None" BorderColor="#e7e7e7"
                                                BorderWidth="1" Width="100%" AllowPaging="true" PageSize="25" OnRowDataBound="gvListProducts_RowDataBound"
                                                OnPageIndexChanging="gvListProducts_PageIndexChanging1">
                                                <FooterStyle BorderWidth="1px" />
                                                <Columns>
                                                    <asp:TemplateField HeaderText="Select" HeaderStyle-ForeColor="White">
                                                        <ItemTemplate>
                                                            <asp:CheckBox ID="chkSelect" runat="server" />
                                                            <asp:Label ID="lblProductID" Visible="false" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"ProductID") %>'></asp:Label>
                                                            <input type="hidden" id="hdnproductid" runat="server" value='<%# DataBinder.Eval(Container.DataItem,"ProductID") %>' />
                                                        </ItemTemplate>
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                        <ItemStyle HorizontalAlign="Center" Width="50px" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Quanity" HeaderStyle-ForeColor="White">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtProQty" Width="40" onkeypress="return onKeyPressBlockNumbers(event);"
                                                                Text="1" Style="text-align: center" CssClass="order-textfield" runat="server"></asp:TextBox>
                                                        </ItemTemplate>
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                        <ItemStyle HorizontalAlign="Center" Width="50px" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Product Name" HeaderStyle-ForeColor="White">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblName1" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"Name") %>'></asp:Label>
                                                            <input type="hidden" id="hdnitemprice" runat="server" value='<%# DataBinder.Eval(Container.DataItem,"SalePrice") %>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Product Code" HeaderStyle-ForeColor="White">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblSKU1" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"SKU") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="left" Width="150px" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Price" HeaderStyle-ForeColor="White">
                                                        <ItemStyle HorizontalAlign="Right" />
                                                        <ItemTemplate>
                                                            $<%# Math.Round(Convert.ToDecimal(DataBinder.Eval(Container.DataItem,"SalePrice")),2) %>
                                                            <asp:Label ID="lblPrice" runat="server" Visible="false" Text='<%# Math.Round(Convert.ToDecimal(DataBinder.Eval(Container.DataItem,"Price")),2) %>'></asp:Label>
                                                            <asp:Label ID="lblSalePrice" runat="server" Visible="false" Text='<%# Math.Round(Convert.ToDecimal(DataBinder.Eval(Container.DataItem,"SalePrice")),2) %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle Width="10%" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Options" HeaderStyle-HorizontalAlign="Center" HeaderStyle-ForeColor="White">
                                                        <ItemStyle HorizontalAlign="center" />
                                                        <ItemTemplate>
                                                            <input type="hidden" id="hdnVariantStatus" runat="server" value='' />
                                                            <a href="javascript:void(0);" id="avariantid" runat="server" style="color: #ba2b19;">Options</a>
                                                            <div style="display: none; position: absolute; width: 70%; background-color: #fbfbfb; border: solid 2px #707070; z-index: 999; right: 70px;"
                                                                id="divvariant" runat="server">
                                                                <div style="float: left; background-color: #707070; min-height: 30px; width: 93%; text-align: left; color: #fff;">
                                                                    &nbsp;<%# DataBinder.Eval(Container.DataItem,"Name") %>
                                                                </div>
                                                                <div style="float: right; background-color: #707070; height: 25px; width: 7%; padding-top: 5px;">
                                                                    <a href="javascript:void(0);" id="divinnerclose" runat="server">
                                                                        <img id="imgMainDiv" class="close" title="close" alt="close" src="/App_Themes/<%=Page.Theme %>/images/cancel-icon.png" /></a>
                                                                </div>
                                                                <div style="float: left; margin: 10px; text-align: left; width: 100%;" id="divproductvariant"
                                                                    runat="server">
                                                                    <div class="item-tb-left2-tab">
                                                                        <div class="tabing" id="tabber1-holder">
                                                                            <ul class="tabbernav">
                                                                                <li class="tabberactive" id="liready" runat="server" onclick="tabdisplaycart(this.id,1);">
                                                                                    <a href="javascript:void(null);" title="READY MADE" id="areadymade" runat="server">READY
                                                                                    MADE</a></li>
                                                                                <li onclick="tabdisplaycart(this.id,2);" id="licustom" runat="server"><a
                                                                                    href="javascript:void(null);" title="MADE TO ORDER" id="amadetomeasure" runat="server">MADE TO ORDER</a></li>
                                                                                 <li onclick="tabdisplaycart(this.id,3);" id="lifabric" runat="server"><a
                                                                                    href="javascript:void(null);" title="FABRIC BY YARD" id="afabric" runat="server">FABRIC BY YARD</a></li>
                                                                            </ul>
                                                                        </div>
                                                                    </div>
                                                                    <input type="hidden" id="hdnCurrTabvalue" runat="server" value="1" />
                                                                    <input type="hidden" id="hdnActual" runat="server" value="0" />
                                                                    <input type="hidden" id="hdnprice" runat="server" value="0" />
                                                                    <input type="hidden" id="hdnshadesvalue" runat="server" value="" />
                                                                    <div id="content-right1" class="tabberlive">
                                                                        <div class="tabbertab" id="divready" runat="server" style="padding-top: 5px; padding-left: 5px; width: 96%; background: none repeat scroll 0 0 #FFFFFF;">
                                                                            <div class="readymade-detail-pt1" style="border-bottom: 1px dotted #E7E7E7; margin-bottom: 5px; display: none;"
                                                                                id="divallowlockQty" runat="server">
                                                                                <table align="left" cellpadding="0" cellspacing="0">
                                                                                    <tr>
                                                                                        <td align="left" style="font-weight: bold;">
                                                                                            <asp:Label ID="lblallowlockQty" Font-Size="11px" runat="server"></asp:Label>&nbsp;
                                                                                        </td>
                                                                                    </tr>
                                                                                </table>
                                                                            </div>
                                                                            <div class="readymade-detail-pt1" style="color: #000; font-weight: normal;">
                                                                                <div class="readymade-detail-left">
                                                                                    <span style="color: #848383; float: left; font-family: Arial,Helvetica,sans-serif; font-size: 12px; line-height: 25px;">Quantity :</span>
                                                                                </div>
                                                                                <div class="readymade-detail-right" style="float: left;">
                                                                                    <asp:TextBox ID="TxtQty" Width="40" onkeypress="return onKeyPressBlockNumbers(event);"
                                                                                        Text="1" Style="text-align: center" CssClass="order-textfield" runat="server"></asp:TextBox><br>
                                                                                </div>
                                                                            </div>
                                                                            <asp:Literal ID="ltvariant" runat="server" Visible="false"></asp:Literal>
                                                                            <div style="float: left; margin-bottom: 10px;">
                                                                                <table align="left" cellpadding="0" cellspacing="0">
                                                                                    <tr>
                                                                                        <td align="left" style="color: #ff0000; font-weight: bold;">
                                                                                            <asp:Label ID="lblvariantprice" Visible="false" ForeColor="Red" runat="server" Text='<%# String.Format("{0:0.00}",Convert.ToDecimal(DataBinder.Eval(Container.DataItem,"SalePrice")))%>'></asp:Label>
                                                                                            <asp:Label ID="lblPriceTag" runat="server" ForeColor="Black" Font-Bold="true">Half Price Drapes Price($) : </asp:Label><asp:Label
                                                                                                ID="lblNewVariantPrice" ForeColor="Red" runat="server" Text='<%# String.Format("{0:0.00}",Convert.ToDecimal(DataBinder.Eval(Container.DataItem,"SalePrice")))%>'></asp:Label>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td align="left" style="color: #ff0000; font-weight: bold;">
                                                                                            <asp:Label ID="lblEstimateddate" runat="server" ForeColor="Red" Font-Bold="true"></asp:Label>
                                                                                        </td>
                                                                                    </tr>
                                                                                </table>
                                                                            </div>
                                                                            <div style="float: left; width: 97%; text-align: left;">
                                                                                <asp:ImageButton ID="btnAddtocartReady" runat="server" />
                                                                            </div>
                                                                        </div>
                                                                        <div class="tabbertab" id="divcustom" runat="server" style="display: none; padding-top: 5px; padding-left: 5px; width: 96%; background: none repeat scroll 0 0 #FFFFFF;">
                                                                            <div>
                                                                                <asp:Literal ID="ltmadevariant" runat="server"></asp:Literal>
                                                                                <div class="readymade-detail">
                                                                                    <div class="readymade-detail-pt1" style="border-bottom: 1px dotted #E7E7E7; margin-bottom: 5px;">
                                                                                        <table align="left" cellpadding="0" cellspacing="0">
                                                                                            <tr>
                                                                                                <td align="left" style="font-weight: bold;">
                                                                                                    <asp:Label ID="lblQtyOnHand" Font-Size="11px" runat="server"></asp:Label>&nbsp;
                                                                                                </td>
                                                                                                <td align="left" style="font-weight: bold;">
                                                                                                    <asp:Label ID="lblNextOrderQty" Font-Size="11px" runat="server"></asp:Label>&nbsp;
                                                                                                </td>
                                                                                                <td align="left" style="font-weight: bold;">
                                                                                                    <asp:Label ID="lblAllowQty" Font-Size="11px" runat="server"></asp:Label>&nbsp;
                                                                                                </td>
                                                                                                <td align="left" style="font-weight: bold;">&nbsp;&nbsp;&nbsp;<asp:Label ID="lblAvailaDate" Font-Size="11px" runat="server"></asp:Label>
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>
                                                                                    </div>
                                                                                    <div class="readymade-detail-pt1">
                                                                                        <div class="readymade-detail-left">
                                                                                            Quantity (Panels)
                                                                                        </div>
                                                                                        <div class="readymade-detail-right" id="divSelectcustomqty" runat="server">
                                                                                            <asp:TextBox ID="txtMadetoMeasureQty" Width="40" onkeypress="return onKeyPressBlockNumbers(event);"
                                                                                                Text="1" Style="text-align: center" CssClass="order-textfield" runat="server"></asp:TextBox>
                                                                                            <%--<asp:DropDownList ID="dlcustomqty" runat="server" CssClass="option1" Style="width: 200px !important;
                                                                                            display: none;">
                                                                                            <asp:ListItem Value="" Selected="True">Quantity</asp:ListItem>
                                                                                            <asp:ListItem Value="1">1</asp:ListItem>
                                                                                            <asp:ListItem Value="2">2</asp:ListItem>
                                                                                            <asp:ListItem Value="3">3</asp:ListItem>
                                                                                            <asp:ListItem Value="4">4</asp:ListItem>
                                                                                            <asp:ListItem Value="5">5</asp:ListItem>
                                                                                            <asp:ListItem Value="6">6</asp:ListItem>
                                                                                            <asp:ListItem Value="7">7</asp:ListItem>
                                                                                            <asp:ListItem Value="8">8</asp:ListItem>
                                                                                            <asp:ListItem Value="9">9</asp:ListItem>
                                                                                            <asp:ListItem Value="10">10</asp:ListItem>
                                                                                            <asp:ListItem Value="11">11</asp:ListItem>
                                                                                            <asp:ListItem Value="12">12</asp:ListItem>
                                                                                            <asp:ListItem Value="13">13</asp:ListItem>
                                                                                            <asp:ListItem Value="14">14</asp:ListItem>
                                                                                            <asp:ListItem Value="15">15</asp:ListItem>
                                                                                            <asp:ListItem Value="16">16</asp:ListItem>
                                                                                            <asp:ListItem Value="17">17</asp:ListItem>
                                                                                            <asp:ListItem Value="18">18</asp:ListItem>
                                                                                            <asp:ListItem Value="19">19</asp:ListItem>
                                                                                            <asp:ListItem Value="20">20</asp:ListItem>
                                                                                        </asp:DropDownList>--%>
                                                                                        </div>
                                                                                    </div>
                                                                                    <div class="readymade-detail-pt1">
                                                                                        <div class="readymade-detail-left">
                                                                                            Select Header
                                                                                        </div>
                                                                                        <div class="readymade-detail-right" id="divSelectcustomstyle" runat="server">
                                                                                            <asp:Literal ID="ltrcustomstyle" runat="server" Visible="false"></asp:Literal>
                                                                                            <asp:DropDownList ID="ddlcustomstyle" runat="server" CssClass="option1" Style="width: 200px !important;">
                                                                                                <asp:ListItem Selected="True" Value="">Select One</asp:ListItem>
                                                                                                <asp:ListItem Value="Pole Pocket">Pole Pocket</asp:ListItem>
                                                                                                <asp:ListItem Value="French">French</asp:ListItem>
                                                                                                <asp:ListItem Value="Parisian">Parisian</asp:ListItem>
                                                                                                <asp:ListItem Value="Inverted">Inverted</asp:ListItem>
                                                                                                <asp:ListItem Value="Goblet">Goblet</asp:ListItem>
                                                                                                <asp:ListItem Value="Grommet">Grommet</asp:ListItem>
                                                                                            </asp:DropDownList>
                                                                                        </div>
                                                                                    </div>
                                                                                    <div class="readymade-detail-pt1">
                                                                                        <div class="readymade-detail-left">
                                                                                            Select Width
                                                                                        </div>
                                                                                        <div class="readymade-detail-right" id="divSelectcustomwidth" runat="server">
                                                                                            <asp:DropDownList ID="ddlcustomwidth" runat="server" CssClass="option1" Style="width: 200px !important;">
                                                                                                <asp:ListItem Selected="True">Width</asp:ListItem>
                                                                                                <asp:ListItem Value="25">25"</asp:ListItem>
                                                                                                <asp:ListItem Value="26">26"</asp:ListItem>
                                                                                                <asp:ListItem Value="27">27"</asp:ListItem>
                                                                                                <asp:ListItem Value="28">28"</asp:ListItem>
                                                                                                <asp:ListItem Value="29">29"</asp:ListItem>
                                                                                                <asp:ListItem Value="30">30"</asp:ListItem>
                                                                                                <asp:ListItem Value="31">31"</asp:ListItem>
                                                                                                <asp:ListItem Value="32">32"</asp:ListItem>
                                                                                                <asp:ListItem Value="33">33"</asp:ListItem>
                                                                                                <asp:ListItem Value="34">34"</asp:ListItem>
                                                                                                <asp:ListItem Value="35">35"</asp:ListItem>
                                                                                                <asp:ListItem Value="36">36"</asp:ListItem>
                                                                                                <asp:ListItem Value="37">37"</asp:ListItem>
                                                                                                <asp:ListItem Value="38">38"</asp:ListItem>
                                                                                                <asp:ListItem Value="39">39"</asp:ListItem>
                                                                                                <asp:ListItem Value="40">40"</asp:ListItem>
                                                                                                <asp:ListItem Value="41">41"</asp:ListItem>
                                                                                                <asp:ListItem Value="42">42"</asp:ListItem>
                                                                                                <asp:ListItem Value="43">43"</asp:ListItem>
                                                                                                <asp:ListItem Value="44">44"</asp:ListItem>
                                                                                                <asp:ListItem Value="45">45"</asp:ListItem>
                                                                                                <asp:ListItem Value="46">46"</asp:ListItem>
                                                                                                <asp:ListItem Value="47">47"</asp:ListItem>
                                                                                                <asp:ListItem Value="48">48"</asp:ListItem>
                                                                                                <asp:ListItem Value="49">49"</asp:ListItem>
                                                                                                <asp:ListItem Value="50">50"</asp:ListItem>
                                                                                                <asp:ListItem Value="51">51"</asp:ListItem>
                                                                                                <asp:ListItem Value="52">52"</asp:ListItem>
                                                                                                <asp:ListItem Value="53">53"</asp:ListItem>
                                                                                                <asp:ListItem Value="54">54"</asp:ListItem>
                                                                                                <asp:ListItem Value="55">55"</asp:ListItem>
                                                                                                <asp:ListItem Value="56">56"</asp:ListItem>
                                                                                                <asp:ListItem Value="57">57"</asp:ListItem>
                                                                                                <asp:ListItem Value="58">58"</asp:ListItem>
                                                                                                <asp:ListItem Value="59">59"</asp:ListItem>
                                                                                                <asp:ListItem Value="60">60"</asp:ListItem>
                                                                                                <asp:ListItem Value="61">61"</asp:ListItem>
                                                                                                <asp:ListItem Value="62">62"</asp:ListItem>
                                                                                                <asp:ListItem Value="63">63"</asp:ListItem>
                                                                                                <asp:ListItem Value="64">64"</asp:ListItem>
                                                                                                <asp:ListItem Value="65">65"</asp:ListItem>
                                                                                                <asp:ListItem Value="66">66"</asp:ListItem>
                                                                                                <asp:ListItem Value="67">67"</asp:ListItem>
                                                                                                <asp:ListItem Value="68">68"</asp:ListItem>
                                                                                                <asp:ListItem Value="69">69"</asp:ListItem>
                                                                                                <asp:ListItem Value="70">70"</asp:ListItem>
                                                                                                <asp:ListItem Value="71">71"</asp:ListItem>
                                                                                                <asp:ListItem Value="72">72"</asp:ListItem>
                                                                                                <asp:ListItem Value="73">73"</asp:ListItem>
                                                                                                <asp:ListItem Value="74">74"</asp:ListItem>
                                                                                                <asp:ListItem Value="75">75"</asp:ListItem>
                                                                                                <asp:ListItem Value="76">76"</asp:ListItem>
                                                                                                <asp:ListItem Value="77">77"</asp:ListItem>
                                                                                                <asp:ListItem Value="78">78"</asp:ListItem>
                                                                                                <asp:ListItem Value="79">79"</asp:ListItem>
                                                                                                <asp:ListItem Value="80">80"</asp:ListItem>
                                                                                                <asp:ListItem Value="81">81"</asp:ListItem>
                                                                                                <asp:ListItem Value="82">82"</asp:ListItem>
                                                                                                <asp:ListItem Value="83">83"</asp:ListItem>
                                                                                                <asp:ListItem Value="84">84"</asp:ListItem>
                                                                                                <asp:ListItem Value="85">85"</asp:ListItem>
                                                                                                <asp:ListItem Value="86">86"</asp:ListItem>
                                                                                                <asp:ListItem Value="87">87"</asp:ListItem>
                                                                                                <asp:ListItem Value="88">88"</asp:ListItem>
                                                                                                <asp:ListItem Value="89">89"</asp:ListItem>
                                                                                                <asp:ListItem Value="90">90"</asp:ListItem>
                                                                                                <asp:ListItem Value="91">91"</asp:ListItem>
                                                                                                <asp:ListItem Value="92">92"</asp:ListItem>
                                                                                                <asp:ListItem Value="93">93"</asp:ListItem>
                                                                                                <asp:ListItem Value="94">94"</asp:ListItem>
                                                                                                <asp:ListItem Value="95">95"</asp:ListItem>
                                                                                                <asp:ListItem Value="96">96"</asp:ListItem>
                                                                                                <asp:ListItem Value="97">97"</asp:ListItem>
                                                                                                <asp:ListItem Value="98">98"</asp:ListItem>
                                                                                                <asp:ListItem Value="99">99"</asp:ListItem>
                                                                                                <asp:ListItem Value="100">100"</asp:ListItem>
                                                                                                <asp:ListItem Value="101">101"</asp:ListItem>
                                                                                                <asp:ListItem Value="102">102"</asp:ListItem>
                                                                                                <asp:ListItem Value="103">103"</asp:ListItem>
                                                                                                <asp:ListItem Value="104">104"</asp:ListItem>
                                                                                                <asp:ListItem Value="105">105"</asp:ListItem>
                                                                                                <asp:ListItem Value="106">106"</asp:ListItem>
                                                                                                <asp:ListItem Value="107">107"</asp:ListItem>
                                                                                                <asp:ListItem Value="108">108"</asp:ListItem>
                                                                                                <asp:ListItem Value="109">109"</asp:ListItem>
                                                                                                <asp:ListItem Value="110">110"</asp:ListItem>
                                                                                                <asp:ListItem Value="111">111"</asp:ListItem>
                                                                                                <asp:ListItem Value="112">112"</asp:ListItem>
                                                                                                <asp:ListItem Value="113">113"</asp:ListItem>
                                                                                                <asp:ListItem Value="114">114"</asp:ListItem>
                                                                                                <asp:ListItem Value="115">115"</asp:ListItem>
                                                                                                <asp:ListItem Value="116">116"</asp:ListItem>
                                                                                                <asp:ListItem Value="117">117"</asp:ListItem>
                                                                                                <asp:ListItem Value="118">118"</asp:ListItem>
                                                                                                <asp:ListItem Value="119">119"</asp:ListItem>
                                                                                                <asp:ListItem Value="120">120"</asp:ListItem>
                                                                                                <asp:ListItem Value="121">121"</asp:ListItem>
                                                                                                <asp:ListItem Value="122">122"</asp:ListItem>
                                                                                                <asp:ListItem Value="123">123"</asp:ListItem>
                                                                                                <asp:ListItem Value="124">124"</asp:ListItem>
                                                                                                <asp:ListItem Value="125">125"</asp:ListItem>
                                                                                                <asp:ListItem Value="126">126"</asp:ListItem>
                                                                                                <asp:ListItem Value="127">127"</asp:ListItem>
                                                                                                <asp:ListItem Value="128">128"</asp:ListItem>
                                                                                                <asp:ListItem Value="129">129"</asp:ListItem>
                                                                                                <asp:ListItem Value="130">130"</asp:ListItem>
                                                                                                <asp:ListItem Value="131">131"</asp:ListItem>
                                                                                                <asp:ListItem Value="132">132"</asp:ListItem>
                                                                                                <asp:ListItem Value="133">133"</asp:ListItem>
                                                                                                <asp:ListItem Value="134">134"</asp:ListItem>
                                                                                                <asp:ListItem Value="135">135"</asp:ListItem>
                                                                                                <asp:ListItem Value="136">136"</asp:ListItem>
                                                                                                <asp:ListItem Value="137">137"</asp:ListItem>
                                                                                                <asp:ListItem Value="138">138"</asp:ListItem>
                                                                                                <asp:ListItem Value="139">139"</asp:ListItem>
                                                                                                <asp:ListItem Value="140">140"</asp:ListItem>
                                                                                                <asp:ListItem Value="141">141"</asp:ListItem>
                                                                                                <asp:ListItem Value="142">142"</asp:ListItem>
                                                                                                <asp:ListItem Value="143">143"</asp:ListItem>
                                                                                                <asp:ListItem Value="144">144"</asp:ListItem>
                                                                                                <asp:ListItem Value="145">145"</asp:ListItem>
                                                                                                <asp:ListItem Value="146">146"</asp:ListItem>
                                                                                                <asp:ListItem Value="147">147"</asp:ListItem>
                                                                                                <asp:ListItem Value="148">148"</asp:ListItem>
                                                                                                <asp:ListItem Value="149">149"</asp:ListItem>
                                                                                                <asp:ListItem Value="150">150"</asp:ListItem>
                                                                                                <asp:ListItem Value="151">151"</asp:ListItem>
                                                                                                <asp:ListItem Value="152">152"</asp:ListItem>
                                                                                                <asp:ListItem Value="153">153"</asp:ListItem>
                                                                                                <asp:ListItem Value="154">154"</asp:ListItem>
                                                                                                <asp:ListItem Value="155">155"</asp:ListItem>
                                                                                                <asp:ListItem Value="156">156"</asp:ListItem>
                                                                                                <asp:ListItem Value="157">157"</asp:ListItem>
                                                                                                <asp:ListItem Value="158">158"</asp:ListItem>
                                                                                                <asp:ListItem Value="159">159"</asp:ListItem>
                                                                                                <asp:ListItem Value="160">160"</asp:ListItem>
                                                                                                <asp:ListItem Value="161">161"</asp:ListItem>
                                                                                                <asp:ListItem Value="162">162"</asp:ListItem>
                                                                                                <asp:ListItem Value="163">163"</asp:ListItem>
                                                                                                <asp:ListItem Value="164">164"</asp:ListItem>
                                                                                                <asp:ListItem Value="165">165"</asp:ListItem>
                                                                                                <asp:ListItem Value="166">166"</asp:ListItem>
                                                                                                <asp:ListItem Value="167">167"</asp:ListItem>
                                                                                                <asp:ListItem Value="168">168"</asp:ListItem>
                                                                                                <asp:ListItem Value="169">169"</asp:ListItem>
                                                                                                <asp:ListItem Value="170">170"</asp:ListItem>
                                                                                                <asp:ListItem Value="171">171"</asp:ListItem>
                                                                                                <asp:ListItem Value="172">172"</asp:ListItem>
                                                                                                <asp:ListItem Value="173">173"</asp:ListItem>
                                                                                                <asp:ListItem Value="174">174"</asp:ListItem>
                                                                                                <asp:ListItem Value="175">175"</asp:ListItem>
                                                                                                <asp:ListItem Value="176">176"</asp:ListItem>
                                                                                                <asp:ListItem Value="177">177"</asp:ListItem>
                                                                                                <asp:ListItem Value="178">178"</asp:ListItem>
                                                                                                <asp:ListItem Value="179">179"</asp:ListItem>
                                                                                                <asp:ListItem Value="180">180"</asp:ListItem>
                                                                                                <asp:ListItem Value="181">181"</asp:ListItem>
                                                                                                <asp:ListItem Value="182">182"</asp:ListItem>
                                                                                                <asp:ListItem Value="183">183"</asp:ListItem>
                                                                                                <asp:ListItem Value="184">184"</asp:ListItem>
                                                                                                <asp:ListItem Value="185">185"</asp:ListItem>
                                                                                                <asp:ListItem Value="186">186"</asp:ListItem>
                                                                                                <asp:ListItem Value="187">187"</asp:ListItem>
                                                                                                <asp:ListItem Value="188">188"</asp:ListItem>
                                                                                                <asp:ListItem Value="189">189"</asp:ListItem>
                                                                                                <asp:ListItem Value="190">190"</asp:ListItem>
                                                                                                <asp:ListItem Value="191">191"</asp:ListItem>
                                                                                                <asp:ListItem Value="192">192"</asp:ListItem>
                                                                                                <asp:ListItem Value="193">193"</asp:ListItem>
                                                                                                <asp:ListItem Value="194">194"</asp:ListItem>
                                                                                                <asp:ListItem Value="195">195"</asp:ListItem>
                                                                                                <asp:ListItem Value="196">196"</asp:ListItem>
                                                                                                <asp:ListItem Value="197">197"</asp:ListItem>
                                                                                                <asp:ListItem Value="198">198"</asp:ListItem>
                                                                                                <asp:ListItem Value="199">199"</asp:ListItem>
                                                                                                <asp:ListItem Value="200">200"</asp:ListItem>
                                                                                            </asp:DropDownList>
                                                                                        </div>
                                                                                    </div>
                                                                                    <div class="readymade-detail-pt1">
                                                                                        <div class="readymade-detail-left">
                                                                                            Select Length
                                                                                        </div>
                                                                                        <div class="readymade-detail-right" id="divSelectcustomlength" runat="server">
                                                                                            <asp:DropDownList ID="ddlcustomlength" runat="server" CssClass="option1" Style="width: 200px !important;">
                                                                                                <asp:ListItem Selected="True" Value="">Length</asp:ListItem>
                                                                                                <asp:ListItem Value="45">45"</asp:ListItem>
                                                                                                <asp:ListItem Value="46">46"</asp:ListItem>
                                                                                                <asp:ListItem Value="47">47"</asp:ListItem>
                                                                                                <asp:ListItem Value="48">48"</asp:ListItem>
                                                                                                <asp:ListItem Value="49">49"</asp:ListItem>
                                                                                                <asp:ListItem Value="50">50"</asp:ListItem>
                                                                                                <asp:ListItem Value="51">51"</asp:ListItem>
                                                                                                <asp:ListItem Value="52">52"</asp:ListItem>
                                                                                                <asp:ListItem Value="53">53"</asp:ListItem>
                                                                                                <asp:ListItem Value="54">54"</asp:ListItem>
                                                                                                <asp:ListItem Value="55">55"</asp:ListItem>
                                                                                                <asp:ListItem Value="56">56"</asp:ListItem>
                                                                                                <asp:ListItem Value="57">57"</asp:ListItem>
                                                                                                <asp:ListItem Value="58">58"</asp:ListItem>
                                                                                                <asp:ListItem Value="59">59"</asp:ListItem>
                                                                                                <asp:ListItem Value="60">60"</asp:ListItem>
                                                                                                <asp:ListItem Value="61">61"</asp:ListItem>
                                                                                                <asp:ListItem Value="62">62"</asp:ListItem>
                                                                                                <asp:ListItem Value="63">63"</asp:ListItem>
                                                                                                <asp:ListItem Value="64">64"</asp:ListItem>
                                                                                                <asp:ListItem Value="65">65"</asp:ListItem>
                                                                                                <asp:ListItem Value="66">66"</asp:ListItem>
                                                                                                <asp:ListItem Value="67">67"</asp:ListItem>
                                                                                                <asp:ListItem Value="68">68"</asp:ListItem>
                                                                                                <asp:ListItem Value="69">69"</asp:ListItem>
                                                                                                <asp:ListItem Value="70">70"</asp:ListItem>
                                                                                                <asp:ListItem Value="71">71"</asp:ListItem>
                                                                                                <asp:ListItem Value="72">72"</asp:ListItem>
                                                                                                <asp:ListItem Value="73">73"</asp:ListItem>
                                                                                                <asp:ListItem Value="74">74"</asp:ListItem>
                                                                                                <asp:ListItem Value="75">75"</asp:ListItem>
                                                                                                <asp:ListItem Value="76">76"</asp:ListItem>
                                                                                                <asp:ListItem Value="77">77"</asp:ListItem>
                                                                                                <asp:ListItem Value="78">78"</asp:ListItem>
                                                                                                <asp:ListItem Value="79">79"</asp:ListItem>
                                                                                                <asp:ListItem Value="80">80"</asp:ListItem>
                                                                                                <asp:ListItem Value="81">81"</asp:ListItem>
                                                                                                <asp:ListItem Value="82">82"</asp:ListItem>
                                                                                                <asp:ListItem Value="83">83"</asp:ListItem>
                                                                                                <asp:ListItem Value="84">84"</asp:ListItem>
                                                                                                <asp:ListItem Value="85">85"</asp:ListItem>
                                                                                                <asp:ListItem Value="86">86"</asp:ListItem>
                                                                                                <asp:ListItem Value="87">87"</asp:ListItem>
                                                                                                <asp:ListItem Value="88">88"</asp:ListItem>
                                                                                                <asp:ListItem Value="89">89"</asp:ListItem>
                                                                                                <asp:ListItem Value="90">90"</asp:ListItem>
                                                                                                <asp:ListItem Value="91">91"</asp:ListItem>
                                                                                                <asp:ListItem Value="92">92"</asp:ListItem>
                                                                                                <asp:ListItem Value="93">93"</asp:ListItem>
                                                                                                <asp:ListItem Value="94">94"</asp:ListItem>
                                                                                                <asp:ListItem Value="95">95"</asp:ListItem>
                                                                                                <asp:ListItem Value="96">96"</asp:ListItem>
                                                                                                <asp:ListItem Value="97">97"</asp:ListItem>
                                                                                                <asp:ListItem Value="98">98"</asp:ListItem>
                                                                                                <asp:ListItem Value="99">99"</asp:ListItem>
                                                                                                <asp:ListItem Value="100">100"</asp:ListItem>
                                                                                                <asp:ListItem Value="101">101"</asp:ListItem>
                                                                                                <asp:ListItem Value="102">102"</asp:ListItem>
                                                                                                <asp:ListItem Value="103">103"</asp:ListItem>
                                                                                                <asp:ListItem Value="104">104"</asp:ListItem>
                                                                                                <asp:ListItem Value="105">105"</asp:ListItem>
                                                                                                <asp:ListItem Value="106">106"</asp:ListItem>
                                                                                                <asp:ListItem Value="107">107"</asp:ListItem>
                                                                                                <asp:ListItem Value="108">108"</asp:ListItem>
                                                                                                <asp:ListItem Value="109">109"</asp:ListItem>
                                                                                                <asp:ListItem Value="110">110"</asp:ListItem>
                                                                                                <asp:ListItem Value="111">111"</asp:ListItem>
                                                                                                <asp:ListItem Value="112">112"</asp:ListItem>
                                                                                                <asp:ListItem Value="113">113"</asp:ListItem>
                                                                                                <asp:ListItem Value="114">114"</asp:ListItem>
                                                                                                <asp:ListItem Value="115">115"</asp:ListItem>
                                                                                                <asp:ListItem Value="116">116"</asp:ListItem>
                                                                                                <asp:ListItem Value="117">117"</asp:ListItem>
                                                                                                <asp:ListItem Value="118">118"</asp:ListItem>
                                                                                                <asp:ListItem Value="119">119"</asp:ListItem>
                                                                                                <asp:ListItem Value="120">120"</asp:ListItem>
                                                                                                <asp:ListItem Value="121">121"</asp:ListItem>
                                                                                                <asp:ListItem Value="122">122"</asp:ListItem>
                                                                                                <asp:ListItem Value="123">123"</asp:ListItem>
                                                                                                <asp:ListItem Value="124">124"</asp:ListItem>
                                                                                                <asp:ListItem Value="125">125"</asp:ListItem>
                                                                                                <asp:ListItem Value="126">126"</asp:ListItem>
                                                                                                <asp:ListItem Value="127">127"</asp:ListItem>
                                                                                                <asp:ListItem Value="128">128"</asp:ListItem>
                                                                                                <asp:ListItem Value="129">129"</asp:ListItem>
                                                                                                <asp:ListItem Value="130">130"</asp:ListItem>
                                                                                                <asp:ListItem Value="131">131"</asp:ListItem>
                                                                                                <asp:ListItem Value="132">132"</asp:ListItem>
                                                                                                <asp:ListItem Value="133">133"</asp:ListItem>
                                                                                                <asp:ListItem Value="134">134"</asp:ListItem>
                                                                                                <asp:ListItem Value="135">135"</asp:ListItem>
                                                                                                <asp:ListItem Value="136">136"</asp:ListItem>
                                                                                                <asp:ListItem Value="137">137"</asp:ListItem>
                                                                                                <asp:ListItem Value="138">138"</asp:ListItem>
                                                                                                <asp:ListItem Value="139">139"</asp:ListItem>
                                                                                                <asp:ListItem Value="140">140"</asp:ListItem>
                                                                                                <asp:ListItem Value="141">141"</asp:ListItem>
                                                                                                <asp:ListItem Value="142">142"</asp:ListItem>
                                                                                                <asp:ListItem Value="143">143"</asp:ListItem>
                                                                                                <asp:ListItem Value="144">144"</asp:ListItem>
                                                                                                <asp:ListItem Value="145">145"</asp:ListItem>
                                                                                                <asp:ListItem Value="146">146"</asp:ListItem>
                                                                                                <asp:ListItem Value="147">147"</asp:ListItem>
                                                                                                <asp:ListItem Value="148">148"</asp:ListItem>
                                                                                                <asp:ListItem Value="149">149"</asp:ListItem>
                                                                                                <asp:ListItem Value="150">150"</asp:ListItem>
                                                                                                <asp:ListItem Value="151">151"</asp:ListItem>
                                                                                                <asp:ListItem Value="152">152"</asp:ListItem>
                                                                                                <asp:ListItem Value="153">153"</asp:ListItem>
                                                                                                <asp:ListItem Value="154">154"</asp:ListItem>
                                                                                                <asp:ListItem Value="155">155"</asp:ListItem>
                                                                                                <asp:ListItem Value="156">156"</asp:ListItem>
                                                                                                <asp:ListItem Value="157">157"</asp:ListItem>
                                                                                                <asp:ListItem Value="158">158"</asp:ListItem>
                                                                                                <asp:ListItem Value="159">159"</asp:ListItem>
                                                                                                <asp:ListItem Value="160">160"</asp:ListItem>
                                                                                                <asp:ListItem Value="161">161"</asp:ListItem>
                                                                                                <asp:ListItem Value="162">162"</asp:ListItem>
                                                                                                <asp:ListItem Value="163">163"</asp:ListItem>
                                                                                                <asp:ListItem Value="164">164"</asp:ListItem>
                                                                                                <asp:ListItem Value="165">165"</asp:ListItem>
                                                                                                <asp:ListItem Value="166">166"</asp:ListItem>
                                                                                                <asp:ListItem Value="167">167"</asp:ListItem>
                                                                                                <asp:ListItem Value="168">168"</asp:ListItem>
                                                                                                <asp:ListItem Value="169">169"</asp:ListItem>
                                                                                                <asp:ListItem Value="170">170"</asp:ListItem>
                                                                                                <asp:ListItem Value="171">171"</asp:ListItem>
                                                                                                <asp:ListItem Value="172">172"</asp:ListItem>
                                                                                                <asp:ListItem Value="173">173"</asp:ListItem>
                                                                                                <asp:ListItem Value="174">174"</asp:ListItem>
                                                                                                <asp:ListItem Value="175">175"</asp:ListItem>
                                                                                                <asp:ListItem Value="176">176"</asp:ListItem>
                                                                                                <asp:ListItem Value="177">177"</asp:ListItem>
                                                                                                <asp:ListItem Value="178">178"</asp:ListItem>
                                                                                                <asp:ListItem Value="179">179"</asp:ListItem>
                                                                                                <asp:ListItem Value="180">180"</asp:ListItem>
                                                                                                <asp:ListItem Value="181">181"</asp:ListItem>
                                                                                                <asp:ListItem Value="182">182"</asp:ListItem>
                                                                                                <asp:ListItem Value="183">183"</asp:ListItem>
                                                                                                <asp:ListItem Value="184">184"</asp:ListItem>
                                                                                                <asp:ListItem Value="185">185"</asp:ListItem>
                                                                                                <asp:ListItem Value="186">186"</asp:ListItem>
                                                                                                <asp:ListItem Value="187">187"</asp:ListItem>
                                                                                                <asp:ListItem Value="188">188"</asp:ListItem>
                                                                                                <asp:ListItem Value="189">189"</asp:ListItem>
                                                                                                <asp:ListItem Value="190">190"</asp:ListItem>
                                                                                                <asp:ListItem Value="191">191"</asp:ListItem>
                                                                                                <asp:ListItem Value="192">192"</asp:ListItem>
                                                                                                <asp:ListItem Value="193">193"</asp:ListItem>
                                                                                                <asp:ListItem Value="194">194"</asp:ListItem>
                                                                                                <asp:ListItem Value="195">195"</asp:ListItem>
                                                                                                <asp:ListItem Value="196">196"</asp:ListItem>
                                                                                                <asp:ListItem Value="197">197"</asp:ListItem>
                                                                                                <asp:ListItem Value="198">198"</asp:ListItem>
                                                                                                <asp:ListItem Value="199">199"</asp:ListItem>
                                                                                                <asp:ListItem Value="200">200"</asp:ListItem>
                                                                                                <asp:ListItem Value="201">201"</asp:ListItem>
                                                                                                <asp:ListItem Value="202">202"</asp:ListItem>
                                                                                                <asp:ListItem Value="203">203"</asp:ListItem>
                                                                                                <asp:ListItem Value="204">204"</asp:ListItem>
                                                                                                <asp:ListItem Value="205">205"</asp:ListItem>
                                                                                                <asp:ListItem Value="206">206"</asp:ListItem>
                                                                                                <asp:ListItem Value="207">207"</asp:ListItem>
                                                                                                <asp:ListItem Value="208">208"</asp:ListItem>
                                                                                                <asp:ListItem Value="209">209"</asp:ListItem>
                                                                                                <asp:ListItem Value="210">210"</asp:ListItem>
                                                                                                <asp:ListItem Value="211">211"</asp:ListItem>
                                                                                                <asp:ListItem Value="212">212"</asp:ListItem>
                                                                                                <asp:ListItem Value="213">213"</asp:ListItem>
                                                                                                <asp:ListItem Value="214">214"</asp:ListItem>
                                                                                                <asp:ListItem Value="215">215"</asp:ListItem>
                                                                                                <asp:ListItem Value="216">216"</asp:ListItem>
                                                                                                <asp:ListItem Value="217">217"</asp:ListItem>
                                                                                                <asp:ListItem Value="218">218"</asp:ListItem>
                                                                                                <asp:ListItem Value="219">219"</asp:ListItem>
                                                                                                <asp:ListItem Value="220">220"</asp:ListItem>
                                                                                                <asp:ListItem Value="221">221"</asp:ListItem>
                                                                                                <asp:ListItem Value="222">222"</asp:ListItem>
                                                                                                <asp:ListItem Value="223">223"</asp:ListItem>
                                                                                                <asp:ListItem Value="224">224"</asp:ListItem>
                                                                                                <asp:ListItem Value="225">225"</asp:ListItem>
                                                                                                <asp:ListItem Value="226">226"</asp:ListItem>
                                                                                                <asp:ListItem Value="227">227"</asp:ListItem>
                                                                                                <asp:ListItem Value="228">228"</asp:ListItem>
                                                                                                <asp:ListItem Value="229">229"</asp:ListItem>
                                                                                                <asp:ListItem Value="230">230"</asp:ListItem>
                                                                                                <asp:ListItem Value="231">231"</asp:ListItem>
                                                                                                <asp:ListItem Value="232">232"</asp:ListItem>
                                                                                                <asp:ListItem Value="233">233"</asp:ListItem>
                                                                                                <asp:ListItem Value="234">234"</asp:ListItem>
                                                                                                <asp:ListItem Value="235">235"</asp:ListItem>
                                                                                                <asp:ListItem Value="236">236"</asp:ListItem>
                                                                                                <asp:ListItem Value="237">237"</asp:ListItem>
                                                                                                <asp:ListItem Value="238">238"</asp:ListItem>
                                                                                                <asp:ListItem Value="239">239"</asp:ListItem>
                                                                                                <asp:ListItem Value="240">240"</asp:ListItem>
                                                                                                <asp:ListItem Value="241">241"</asp:ListItem>
                                                                                                <asp:ListItem Value="242">242"</asp:ListItem>
                                                                                                <asp:ListItem Value="243">243"</asp:ListItem>
                                                                                                <asp:ListItem Value="244">244"</asp:ListItem>
                                                                                                <asp:ListItem Value="245">245"</asp:ListItem>
                                                                                                <asp:ListItem Value="246">246"</asp:ListItem>
                                                                                                <asp:ListItem Value="247">247"</asp:ListItem>
                                                                                                <asp:ListItem Value="248">248"</asp:ListItem>
                                                                                                <asp:ListItem Value="249">249"</asp:ListItem>
                                                                                                <asp:ListItem Value="250">250"</asp:ListItem>
                                                                                                <asp:ListItem Value="251">251"</asp:ListItem>
                                                                                                <asp:ListItem Value="252">252"</asp:ListItem>
                                                                                                <asp:ListItem Value="253">253"</asp:ListItem>
                                                                                                <asp:ListItem Value="254">254"</asp:ListItem>
                                                                                                <asp:ListItem Value="255">255"</asp:ListItem>
                                                                                                <asp:ListItem Value="256">256"</asp:ListItem>
                                                                                                <asp:ListItem Value="257">257"</asp:ListItem>
                                                                                                <asp:ListItem Value="258">258"</asp:ListItem>
                                                                                                <asp:ListItem Value="259">259"</asp:ListItem>
                                                                                                <asp:ListItem Value="260">260"</asp:ListItem>
                                                                                                <asp:ListItem Value="261">261"</asp:ListItem>
                                                                                                <asp:ListItem Value="262">262"</asp:ListItem>
                                                                                                <asp:ListItem Value="263">263"</asp:ListItem>
                                                                                                <asp:ListItem Value="264">264"</asp:ListItem>
                                                                                                <asp:ListItem Value="265">265"</asp:ListItem>
                                                                                                <asp:ListItem Value="266">266"</asp:ListItem>
                                                                                                <asp:ListItem Value="267">267"</asp:ListItem>
                                                                                                <asp:ListItem Value="268">268"</asp:ListItem>
                                                                                                <asp:ListItem Value="269">269"</asp:ListItem>
                                                                                                <asp:ListItem Value="270">270"</asp:ListItem>
                                                                                                <asp:ListItem Value="271">271"</asp:ListItem>
                                                                                                <asp:ListItem Value="272">272"</asp:ListItem>
                                                                                                <asp:ListItem Value="273">273"</asp:ListItem>
                                                                                                <asp:ListItem Value="274">274"</asp:ListItem>
                                                                                                <asp:ListItem Value="275">275"</asp:ListItem>
                                                                                            </asp:DropDownList>
                                                                                        </div>
                                                                                    </div>
                                                                                    <div class="readymade-detail-pt1" id="divoptioncustomhide" runat="server">
                                                                                        <div class="readymade-detail-left">
                                                                                            Select Options
                                                                                        </div>
                                                                                        <div class="readymade-detail-right" id="divSelectcustomoptin" runat="server">
                                                                                            <asp:Literal ID="ltroptions" runat="server" Visible="false"></asp:Literal>
                                                                                            <asp:DropDownList ID="ddlcustomoptin" runat="server" CssClass="option1" Style="width: 200px !important;">
                                                                                                <asp:ListItem Value="" Selected="True">Options</asp:ListItem>
                                                                                                <asp:ListItem Value="Lined">Lined</asp:ListItem>
                                                                                                <asp:ListItem Value="Lined &amp; Interlined">Lined &amp; Interlined</asp:ListItem>
                                                                                                <asp:ListItem Value="Blackout Lining">Blackout Lining</asp:ListItem>
                                                                                            </asp:DropDownList>
                                                                                        </div>
                                                                                    </div>
                                                                                    <div class="readymade-detail-pt1">
                                                                                        <div class="price-detail-left" style="width: 345px;">
                                                                                            <p id="divcustomprice" style="width: 48%;">
                                                                                                <asp:Literal ID="ltcustomPrice" Visible="false" runat="server"></asp:Literal>
                                                                                                <input type="hidden" id="hdncustomprice" runat="server" value="0" />
                                                                                                <input type="hidden" id="hdnpricetemp" value="" />
                                                                                                <asp:Label ID="lblMadetoprice" runat="server"></asp:Label>
                                                                                            </p>
                                                                                            <%--<asp:ImageButton CssClass="price-detail-right" ID="btnAddTocartMade" runat="server"
                                                                                                ToolTip="ADD TO CART" ImageUrl="/images/item-add-to-cart.jpg" />--%>
                                                                                        </div>
                                                                                    </div>
                                                                                    <asp:Literal ID="ltmadeSmallbanner" runat="server"></asp:Literal>
                                                                                </div>
                                                                                <div style="float: left; margin-bottom: 10px;">
                                                                                    <table align="left" cellpadding="0" cellspacing="0">
                                                                                        <tr>
                                                                                            <td align="left" style="color: #ff0000; font-weight: bold;">
                                                                                                <p id="divYourPrice" runat="server">
                                                                                                    <span style='color: Black; font-weight: bold;'>Your Price($) :</span>
                                                                                                    <asp:Literal ID="ltYourPrice" runat="server" Text=''></asp:Literal>
                                                                                                    <div id="spnYourPrice" style="display: none">
                                                                                                        <asp:Literal ID="ltYourPriceforshiopop" runat="server"></asp:Literal>
                                                                                                    </div>
                                                                                                </p>
                                                                                                <p>
                                                                                                    <div id="divestimateddate" runat="server">
                                                                                                    </div>
                                                                                                </p>
                                                                                            </td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </div>
                                                                                <div style="float: left; width: 97%; text-align: left;">
                                                                                    <asp:ImageButton ID="btnAddtocartcustom" runat="server" />
                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                        <div class="tabbertab" id="divfabric" runat="server" style="display: none; padding-top: 5px; padding-left: 5px; width: 96%; background: none repeat scroll 0 0 #FFFFFF;">
                                                                            <div>
                                                                                <asp:Literal ID="Literal1" runat="server"></asp:Literal>
                                                                                <div class="readymade-detail">
                                                                                    <div class="readymade-detail-pt1" id="divfabricdesc" runat="server" style="border-bottom: 1px dotted #E7E7E7; margin-bottom: 5px;">
                                                                                        <table align="left" cellpadding="0" cellspacing="0">
                                                                                            <tr>
                                                                                                <td align="left" style="font-weight: bold;"><asp:Label ID="Label1" Text="Description:" Font-Size="11px" runat="server"></asp:Label>
                                                                                                </td>
                                                                                                <td align="left" style="font-weight:normal;">
                                                                                                    <asp:Label ID="lblfabricDesc"  runat="server"></asp:Label>
                                                                                                </td>

                                                                                            </tr>
                                                                                        </table>
                                                                                    </div>
                                                                                    <div class="readymade-detail-pt1">
                                                                                        <div class="readymade-detail-left">
                                                                                            Quantity (Yard)
                                                                                        </div>
                                                                                        <div class="readymade-detail-right" id="divSelectfabricqty" runat="server">
                                                                                            <asp:TextBox ID="txtfabricQty" Width="40" onkeypress="return onKeyPressBlockNumbers(event);"
                                                                                                Text="1" Style="text-align: center" CssClass="order-textfield" runat="server"></asp:TextBox>

                                                                                        </div>
                                                                                    </div>

                                                                                    <div class="readymade-detail-pt1">
                                                                                        <div class="price-detail-left" style="width: 345px;">
                                                                                            <p id="divfabricprice" style="width: 48%;">
                                                                                                <asp:Literal ID="ltfabricPrice" Visible="false" runat="server"></asp:Literal>
                                                                                                <input type="hidden" id="hdnfabricprice" runat="server" value="0" />
                                                                                                <input type="hidden" id="hdnfabricpricetemp" value="" />

                                                                                            </p>

                                                                                        </div>
                                                                                    </div>

                                                                                </div>
                                                                                <div style="float: left; margin-bottom: 10px;">
                                                                                    <table align="left" cellpadding="0" cellspacing="0">
                                                                                        <tr>
                                                                                            <td align="left" style="color: #ff0000; font-weight: bold;">
                                                                                                <p id="divyourfabricprice" runat="server">
                                                                                                    <span style='color: Black; font-weight: bold;'>Your Price($) :</span>
                                                                                                    <asp:Literal ID="ltYourfabricPrice" runat="server" Text=''></asp:Literal>
                                                                                                    <div id="spnYourfabricPrice" style="display: none">
                                                                                                        <asp:Literal ID="ltYourfabricPriceforshiopop" runat="server"></asp:Literal>
                                                                                                    </div>
                                                                                                </p>
                                                                                                <p>
                                                                                                    <div id="divestimateddatefabric" runat="server">
                                                                                                    </div>
                                                                                                </p>
                                                                                            </td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </div>
                                                                                <div style="float: left; width: 97%; text-align: left;">
                                                                                    <asp:ImageButton ID="btnAddtocartfabric" runat="server" />
                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                                <div style="float: left; margin: 10px; text-align: left; width: 100%;" id="divAttributes"
                                                                    runat="server">
                                                                </div>
                                                                <div style="float: left; width: 97%; text-align: left; padding: 3%;">
                                                                    <asp:ImageButton ID="btnSelectGrid" runat="server" CommandName="Edit" Visible="false"
                                                                        CommandArgument='<%#DataBinder.Eval(Container.DataItem,"ProductID") %>' />
                                                                </div>
                                                                <div style="display: none;">
                                                                    <asp:Button ID="btntempClick" runat="server" OnClick="btntempClick_Click" />
                                                                </div>
                                                            </div>
                                                            <div style="display: none;" id="divshow">
                                                            </div>
                                                        </ItemTemplate>
                                                        <ItemStyle Width="10%" />
                                                    </asp:TemplateField>
                                                </Columns>
                                                <PagerSettings Mode="NumericFirstLast" Position="TopAndBottom"></PagerSettings>
                                                <PagerStyle HorizontalAlign="Right" CssClass="grid_paging" />
                                                <RowStyle HorizontalAlign="Center" CssClass="oddrow" />
                                                <AlternatingRowStyle CssClass="altrow" />
                                                <RowStyle BackColor="White" BorderColor="White" BorderStyle="Solid" BorderWidth="1px"
                                                    Height="24px" HorizontalAlign="Left" />
                                                <EditRowStyle CssClass="list_table_cell_link" />
                                                <HeaderStyle BackColor="#F2F2F2" CssClass="list-table-title" Height="30px" HorizontalAlign="Left"
                                                    Wrap="True" />
                                                <AlternatingRowStyle BackColor="#F2F2F2" BorderColor="White" BorderStyle="Solid"
                                                    CssClass="list_table_cell_link" ForeColor="#444545" />
                                                <PagerSettings Position="TopAndBottom" />
                                            </asp:GridView>
                                        </div>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: left; color: #696A6A; font-family: Verdana,Arial,Helvetica,sans-serif; font-size: 11px; padding-top: 10px;"
                                    colspan="2" id="cleartdid" runat="server">
                                    <a style="color: #696A6A; text-decoration: none;" id="lkbAllowAll" class="list_table_cell_link"
                                        href="javascript:selectAll(true);">Check All</a>&nbsp; | <a id="lkbClearAll" style="color: #696A6A; text-decoration: none;"
                                            class="list_table_cell_link" href="javascript:selectAll(false);">Clear All</a>
                                </td>
                            </tr>
                            <tr>
                                <td align="left">
                                    <asp:TextBox Visible="false" ID="txtProductIDs" runat="server" TextMode="multiline"
                                        Rows="5" Columns="30"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </tbody>
        </table>
        <asp:HiddenField ID="HdnCustID" runat="server" Value="0" />
        <asp:HiddenField ID="HdnProductSKu" runat="server" Value="" />
          <asp:HiddenField ID="hdnsearchlinksku" runat="server" />
        <div style="display: none;">
            <asp:Button ID="btnAddtocartwithvariant" runat="server" OnClick="btnAddtocartwithvariant_Click" />
            <asp:Button ID="btnAddVariant" runat="server" OnClick="btnAddVariant_Click" />
            <asp:Button ID="btnAddVariantMeasure" runat="server" OnClick="btnAddVariantMeasure_Click" />
                 <asp:Button ID="btnAddfabric" runat="server" OnClick="btnAddfabric_Click" />
            <input type="hidden" id="hdnVariProductId" runat="server" value="" />
            <input type="hidden" id="hdnVariantValueid" runat="server" value="" />
            <input type="hidden" id="hdnVariQuantity" runat="server" value="" />
            <input type="hidden" id="hdnVariPrice" runat="server" value="" />
            <input type="hidden" id="hdnType" runat="server" value="0" />
            <input type="hidden" value="" id="hdnVariName" runat="server" />
            <input type="hidden" value="" id="hdnVarivalue" runat="server" />
            <input type="hidden" value="" id="hdnforMeasureVName" runat="server" />
            <input type="hidden" value="" id="hdnforMeasureVValue" runat="server" />
            <input type="hidden" value="" id="hdnrowidss" />
            <input type="hidden" id="hdncustombuy1get1" runat="server" value="0" />
        </div>
         <div style="display:none;">
        <asp:Button ID="btndefaultclick" runat="server" OnClientClick="return false;" />
    </div>
    </form>
    <script language="javascript" type="text/javascript">
        document.getElementById('zoom1-big').style.display = 'none';

        //alert();
    </script>
    <div id="prepage" style="position: absolute; font-family: arial; font-size: 16; left: 0px; top: 0px; background-color: #000; opacity: 0.7; filter: alpha(opacity=70); layer-background-color: white; height: 100%; width: 100%; z-index: 1000; display: none;">
        <table width="100%" style="padding-top: 400px;" align="center">
            <tr>
                <td align="center" style="color: #fff;" valign="middle">
                    <img alt="" src="/images/loding.png" /><br />
                    <b>Loading ... ... Please wait!</b>
                </td>
            </tr>
        </table>
    </div>
   
</body>
</html>
