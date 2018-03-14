<%@ Page Title="" Language="C#" MasterPageFile="~/ADMIN/Admin.Master" AutoEventWireup="true"
    CodeBehind="ProductHemmingList.aspx.cs" Inherits="Solution.UI.Web.ADMIN.Products.ProductHemmingList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .checklist-main th
        {
            background-color: #eee;
            line-height: 25px;
            border-top: solid 1px #999;
            border-bottom: solid 1px #999;
        }
        .checklist-main td
        {
            line-height: 25px;
        }
        .divfloatingcss
        {
            bottom: 0;
            right: 0;
            position: fixed;
            width: 10%;
            margin-right: 46%;
            background-image: url("/Admin/images/title-bg-trans.png");
            -webkit-border-top-left-radius: 15px;
            -webkit-border-top-right-radius: 15px;
            -moz-border-radius-topleft: 15px;
            -moz-border-radius-topright: 15px;
            border-top-left-radius: 15px;
            border-top-right-radius: 15px; /*-webkit-border-radius: 20px;
-moz-border-radius: 20px;
border-radius: 20px;*/
        }
    </style>
    <script src="../JS/jquery-1.3.2.js" type="text/javascript"></script>
    <script type="text/javascript">

        function chksearch() {
            var value = $('ContentPlaceHolder1_txtSearch').val();
            if (value != null && value != '') {
                return true;
            }
            else { alert('Please enter search keyword'); return false; }
        }

        function chkHeight() {
            var windowHeight = 0;
            windowHeight = $(document).height(); //window.innerHeight;
            document.getElementById('prepage').style.height = windowHeight + 'px';
            document.getElementById('prepage').style.display = '';
        }
        $(document).ready(function () {
            $('#divfloating').attr("class", "divfloatingcss");
            $(window).scroll(function () {
                if ($(window).scrollTop() == $(document).height() - $(window).height()) {
                    //   $('#divfloating').attr("class", "");
                }
                else {
                    // $('#divfloating').attr("class", "divfloatingcss");
                }
            });
        });
    </script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts-main.js"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts.js"></script>
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
        function getKeyCode(e) {
            if (window.event) return window.event.keyCode; else if (e) return e.which; else return null;
        }


        function AllowandlockQtyVariantChk(id) {

            var chkbox = id.replace(/_grdoptionmainGroup_chkallowed_/ig, '_grdoptionmainGroup_grdvaluelisting_');
            var allexistchk = document.getElementById(chkbox).getElementsByTagName('*');

            for (var i = 0; i < allexistchk.length; i++) {
                var elt1 = allexistchk[i];
                if (elt1.id.toString().toLowerCase().indexOf('_txtsafetyhand') > -1) {
                    AllowandlockQtyVariant(elt1.id);
                }
            }
        }
        function AllowandlockQtyVariant(id) {

            var txtsafetyHand = id;
            var chk = id.substring(0, id.indexOf('_txt'));
            var chkallowed = chk.replace(/_grdvaluelisting/ig, '_chkallowed');
            var txttotalinventory = id.replace(/_txtsafetyHand/ig, '_txttotalinventory');
            var txtsaleschannel = id.replace(/_txtsafetyHand/ig, '_txtsaleschannel');
            var txtsafetyHandvalue = document.getElementById(txtsafetyHand).value;
            var txttotalinventoryvalue = document.getElementById(txttotalinventory).value;
            var txtsaleschannelvalue = document.getElementById(txtsaleschannel).value;

            //
            var txthpdwebsite = id.replace(/_txtsafetyHand/ig, '_txthpdwebsite');
            var txtinventoryonHand = id.replace(/_txtsafetyHand/ig, '_txtinventoryonHand');
            var txthpdwebsitevalue = document.getElementById(txthpdwebsite).value;
            var txtinventoryonHandvalue = document.getElementById(txtinventoryonHand).value;


            if (document.getElementById(txtsafetyHand) != null && document.getElementById(txtsafetyHand).value != "") {
                // alert(document.getElementById(txtsafetyHand));alert(document.getElementById(txtsafetyHand).value);
                if (document.getElementById(chkallowed) != null && document.getElementById(chkallowed).checked == true) {

                    txtsaleschannelvalue = txttotalinventoryvalue - txtsafetyHandvalue;
                    txthpdwebsitevalue = txtinventoryonHandvalue - txtsafetyHandvalue;
                    //document.getElementById(txtsaleschannel).value = txtsaleschannelvalue;
                    //document.getElementById(txthpdwebsite).value = txthpdwebsitevalue;

                    var Qty84Org = 0; var Qty96Org = 0; var Qty108Org = 0; var Qty120Org = 0;
                    var Qty84 = 0; var Qty96 = 0; var Qty108 = 0; var Qty120 = 0;
                    var Qty84Orgdiff = 0; var Qty96Orgdiff = 0; var Qty108Orgdiff = 0; var Qty120Orgdiff = 0;

                    var Qty84Orghw = 0; var Qty96Orghw = 0; var Qty108Orghw = 0; var Qty120Orghw = 0;
                    var Qty84hw = 0; var Qty96hw = 0; var Qty108hw = 0; var Qty120hw = 0;
                    var Qty84Orgdiffhw = 0; var Qty96Orgdiffhw = 0; var Qty108Orgdiffhw = 0; var Qty120Orgdiffhw = 0;


                    var alldiv1 = id.substring(0, id.indexOf('_txt'));
                    var allexist1 = document.getElementById(alldiv1).getElementsByTagName('*');

                    for (var i = 0; i < allexist1.length; i++) {
                        var elt = allexist1[i];
                        if (elt.id.toString().toLowerCase().indexOf('_ltsku') > -1) {

                            if (elt.innerHTML.toString().toLowerCase().indexOf('-84') > -1) {

                                var Qty84id1 = elt.id.replace(/_ltSKU/ig, '_txtsaleschannel');
                                var total84id = elt.id.replace(/_ltSKU/ig, '_txttotalinventory');
                                var safty84id = elt.id.replace(/_ltSKU/ig, '_txtsafetyHand');
                                var total84value = 0; var safty84value = 0;
                                if (document.getElementById(total84id) != null && document.getElementById(total84id).value != "") { total84value = document.getElementById(total84id).value }
                                if (document.getElementById(safty84id) != null && document.getElementById(safty84id).value != "") { safty84value = document.getElementById(safty84id).value }
                                Qty84Orgdiff = parseFloat(total84value) - parseFloat(safty84value);
                                Qty84Org = document.getElementById(Qty84id1).value;


                                var Qty84id1hw = elt.id.replace(/_ltSKU/ig, '_txthpdwebsite');
                                var total84idhw = elt.id.replace(/_ltSKU/ig, '_txtinventoryonHand');
                                var safty84idhw = elt.id.replace(/_ltSKU/ig, '_txtsafetyHand');
                                var total84valuehw = 0; var safty84valuehw = 0;
                                if (document.getElementById(total84idhw) != null && document.getElementById(total84idhw).value != "") { total84valuehw = document.getElementById(total84idhw).value; }
                                if (document.getElementById(safty84idhw) != null && document.getElementById(safty84idhw).value != "") { safty84valuehw = document.getElementById(safty84idhw).value; }
                                Qty84Orgdiffhw = parseFloat(total84valuehw) - parseFloat(safty84valuehw);
                                Qty84Orghw = document.getElementById(Qty84id1hw).value;

                            }
                            else if (elt.innerHTML.toString().toLowerCase().indexOf('-96') > -1) {

                                var Qty96id1 = elt.id.replace(/_ltSKU/ig, '_txtsaleschannel');
                                var total96id = elt.id.replace(/_ltSKU/ig, '_txttotalinventory');
                                var safty96id = elt.id.replace(/_ltSKU/ig, '_txtsafetyHand');
                                var total96value = 0; var safty96value = 0;
                                if (document.getElementById(total96id) != null && document.getElementById(total96id).value != "") { total96value = document.getElementById(total96id).value; }
                                if (document.getElementById(safty96id) != null && document.getElementById(safty96id).value != "") { safty96value = document.getElementById(safty96id).value; }
                                Qty96Orgdiff = parseFloat(total96value) - parseFloat(safty96value);
                                Qty96Org = document.getElementById(Qty96id1).value;

                                var Qty96id1hw = elt.id.replace(/_ltSKU/ig, '_txthpdwebsite');
                                var total96idhw = elt.id.replace(/_ltSKU/ig, '_txtinventoryonHand');
                                var safty96idhw = elt.id.replace(/_ltSKU/ig, '_txtsafetyHand');
                                var total96valuehw = 0; var safty96valuehw = 0;
                                if (document.getElementById(total96idhw) != null && document.getElementById(total96idhw).value != "") { total96valuehw = document.getElementById(total96idhw).value; }
                                if (document.getElementById(safty96idhw) != null && document.getElementById(safty96idhw).value != "") { safty96valuehw = document.getElementById(safty96idhw).value; }
                                Qty96Orgdiffhw = parseFloat(total96valuehw) - parseFloat(safty96valuehw);
                                Qty96Orghw = document.getElementById(Qty96id1hw).value;


                            }
                            else if (elt.innerHTML.toString().toLowerCase().indexOf('-108') > -1) {

                                var Qty108id1 = elt.id.replace(/_ltSKU/ig, '_txtsaleschannel');
                                var total108id = elt.id.replace(/_ltSKU/ig, '_txttotalinventory');
                                var safty108id = elt.id.replace(/_ltSKU/ig, '_txtsafetyHand');
                                var total108value = 0; var safty108value = 0;
                                if (document.getElementById(total108id) != null && document.getElementById(total108id).value != "") { total108value = document.getElementById(total108id).value; }
                                if (document.getElementById(safty108id) != null && document.getElementById(safty108id).value != "") { safty108value = document.getElementById(safty108id).value; }
                                Qty108Orgdiff = parseFloat(total108value) - parseFloat(safty108value);
                                Qty108Org = document.getElementById(Qty108id1).value;

                                var Qty108id1hw = elt.id.replace(/_ltSKU/ig, '_txthpdwebsite');
                                var total108idhw = elt.id.replace(/_ltSKU/ig, '_txtinventoryonHand');
                                var safty108idhw = elt.id.replace(/_ltSKU/ig, '_txtsafetyHand');
                                var total108valuehw = 0; var safty108valuehw = 0;
                                if (document.getElementById(total108idhw) != null && document.getElementById(total108idhw).value != "") { total108valuehw = document.getElementById(total108idhw).value; }
                                if (document.getElementById(safty108idhw) != null && document.getElementById(safty108idhw).value != "") { safty108valuehw = document.getElementById(safty108idhw).value; }
                                Qty108Orgdiffhw = parseFloat(total108valuehw) - parseFloat(safty108valuehw);
                                Qty108Orghw = document.getElementById(Qty108id1hw).value;

                            }
                            else if (elt.innerHTML.toString().toLowerCase().indexOf('-120') > -1) {

                                var Qty120id1 = elt.id.replace(/_ltSKU/ig, '_txtsaleschannel');
                                var total120id = elt.id.replace(/_ltSKU/ig, '_txttotalinventory');
                                var total120value = 0; var safty120value = 0;
                                if (document.getElementById(total120id) != null && document.getElementById(total120id).value != "") { total120value = document.getElementById(total120id).value; }
                                var safty120id = elt.id.replace(/_ltSKU/ig, '_txtsafetyHand');
                                if (document.getElementById(safty120id) != null && document.getElementById(safty120id).value != "") { safty120value = document.getElementById(safty120id).value; }
                                Qty120Orgdiff = parseFloat(total120value) - parseFloat(safty120value);
                                Qty120Org = document.getElementById(Qty120id1).value;

                                var Qty120id1hw = elt.id.replace(/_ltSKU/ig, '_txthpdwebsite');
                                var total120idhw = elt.id.replace(/_ltSKU/ig, '_txtinventoryonHand');
                                var safty120idhw = elt.id.replace(/_ltSKU/ig, '_txtsafetyHand');
                                var total120valuehw = 0; var safty120valuehw = 0;
                                if (document.getElementById(total120idhw) != null && document.getElementById(total120idhw).value != "") { total120valuehw = document.getElementById(total120idhw).value; }
                                if (document.getElementById(safty120idhw) != null && document.getElementById(safty120idhw).value != "") { safty120valuehw = document.getElementById(safty120idhw).value; }
                                Qty120Orgdiffhw = parseFloat(total120valuehw) - parseFloat(safty120valuehw);
                                Qty120Orghw = document.getElementById(Qty120id1hw).value;

                            }


                        }
                    }

                    var Qty84id = ""; var Qty96id = ""; var Qty108id = ""; var Qty120id = "";
                    var Qty84idhw = ""; var Qty96idhw = ""; var Qty108idhw = ""; var Qty120idhw = "";
                    // alert(Qty84Org); alert(Qty96Org);alert(Qty108Org);alert(Qty120Org);

                    var alldiv = id.substring(0, id.indexOf('_txt'));
                    var allexist = document.getElementById(alldiv).getElementsByTagName('*');
                    for (var i = 0; i < allexist.length; i++) {
                        var elt = allexist[i];
                        if (elt.id.toString().toLowerCase().indexOf('_ltsku') > -1) {

                            if (elt.innerHTML.toString().toLowerCase().indexOf('-84') > -1) {

                                Qty84id = elt.id.replace(/_ltSKU/ig, '_txtsaleschannel');
                                Qty84 = parseFloat(Qty84Orgdiff) + parseFloat(Qty96Org);
                                if (document.getElementById(Qty84id) != null) { document.getElementById(Qty84id).value = Qty84; }

                                Qty84idhw = elt.id.replace(/_ltSKU/ig, '_txthpdwebsite');
                                Qty84hw = parseFloat(Qty84Orgdiffhw) + parseFloat(Qty96Orghw);
                                if (document.getElementById(Qty84idhw) != null) { document.getElementById(Qty84idhw).value = Qty84hw; }

                            }
                            else if (elt.innerHTML.toString().toLowerCase().indexOf('-96') > -1) {

                                Qty96id = elt.id.replace(/_ltSKU/ig, '_txtsaleschannel');
                                Qty96 = parseFloat(Qty96Orgdiff) + parseFloat(Qty108Org);
                                Qty84 = parseFloat(Qty84Orgdiff) + parseFloat(Qty96);
                                if (document.getElementById(Qty84id) != null) { document.getElementById(Qty84id).value = Qty84; }
                                if (document.getElementById(Qty96id) != null) { document.getElementById(Qty96id).value = Qty96; }

                                Qty96idhw = elt.id.replace(/_ltSKU/ig, '_txthpdwebsite');
                                Qty96hw = parseFloat(Qty96Orgdiffhw) + parseFloat(Qty108Orghw);
                                Qty84hw = parseFloat(Qty84Orgdiffhw) + parseFloat(Qty96hw);
                                if (document.getElementById(Qty84idhw) != null) { document.getElementById(Qty84idhw).value = Qty84hw; }
                                if (document.getElementById(Qty96idhw) != null) { document.getElementById(Qty96idhw).value = Qty96hw; }

                            }
                            else if (elt.innerHTML.toString().toLowerCase().indexOf('-108') > -1) {

                                Qty108id = elt.id.replace(/_ltSKU/ig, '_txtsaleschannel');
                                Qty108 = parseFloat(Qty108Orgdiff) + parseFloat(Qty120Org);
                                Qty96 = parseFloat(Qty96Orgdiff) + parseFloat(Qty108);
                                Qty84 = parseFloat(Qty84Orgdiff) + parseFloat(Qty96);
                                if (document.getElementById(Qty84id) != null) { document.getElementById(Qty84id).value = Qty84; }
                                if (document.getElementById(Qty96id) != null) { document.getElementById(Qty96id).value = Qty96; }
                                if (document.getElementById(Qty108id) != null) { document.getElementById(Qty108id).value = Qty108; }


                                Qty108idhw = elt.id.replace(/_ltSKU/ig, '_txthpdwebsite');
                                Qty108hw = parseFloat(Qty108Orgdiffhw) + parseFloat(Qty120Orghw);
                                Qty96hw = parseFloat(Qty96Orgdiffhw) + parseFloat(Qty108hw);
                                Qty84hw = parseFloat(Qty84Orgdiffhw) + parseFloat(Qty96hw);
                                if (document.getElementById(Qty84idhw) != null) { document.getElementById(Qty84idhw).value = Qty84hw; }
                                if (document.getElementById(Qty96idhw) != null) { document.getElementById(Qty96idhw).value = Qty96hw; }
                                if (document.getElementById(Qty108idhw) != null) { document.getElementById(Qty108idhw).value = Qty108hw; }

                            }
                            else if (elt.innerHTML.toString().toLowerCase().indexOf('-120') > -1) {

                                Qty120id = elt.id.replace(/_ltSKU/ig, '_txtsaleschannel');
                                Qty120 = parseFloat(Qty120Orgdiff);
                                Qty108 = parseFloat(Qty108Orgdiff) + parseFloat(Qty120);
                                Qty96 = parseFloat(Qty96Orgdiff) + parseFloat(Qty108);
                                Qty84 = parseFloat(Qty84Orgdiff) + parseFloat(Qty96);
                                if (document.getElementById(Qty84id) != null) { document.getElementById(Qty84id).value = Qty84; }
                                if (document.getElementById(Qty96id) != null) { document.getElementById(Qty96id).value = Qty96; }
                                if (document.getElementById(Qty108id) != null) { document.getElementById(Qty108id).value = Qty108; }
                                if (document.getElementById(Qty120id) != null) { document.getElementById(Qty120id).value = Qty120; }

                                Qty120idhw = elt.id.replace(/_ltSKU/ig, '_txthpdwebsite');
                                Qty120hw = parseFloat(Qty120Orgdiffhw);
                                Qty108hw = parseFloat(Qty108Orgdiffhw) + parseFloat(Qty120hw);
                                Qty96hw = parseFloat(Qty96Orgdiffhw) + parseFloat(Qty108hw);
                                Qty84hw = parseFloat(Qty84Orgdiffhw) + parseFloat(Qty96hw);
                                if (document.getElementById(Qty84idhw) != null) { document.getElementById(Qty84idhw).value = Qty84hw; }
                                if (document.getElementById(Qty96idhw) != null) { document.getElementById(Qty96idhw).value = Qty96hw; }
                                if (document.getElementById(Qty108idhw) != null) { document.getElementById(Qty108idhw).value = Qty108hw; }
                                if (document.getElementById(Qty120idhw) != null) { document.getElementById(Qty120idhw).value = Qty120hw; }


                            }

                        }
                    }




                }
                else if (document.getElementById(chkallowed) != null && document.getElementById(chkallowed).checked == false) {

                    txtsaleschannelvalue = txttotalinventoryvalue - txtsafetyHandvalue;
                    txthpdwebsitevalue = txtinventoryonHandvalue - txtsafetyHandvalue;

                    document.getElementById(txtsaleschannel).value = txtsaleschannelvalue;
                    document.getElementById(txthpdwebsite).value = txthpdwebsitevalue;



                }
            }


        }
           

         

         
    </script>
    <style type="text/css">
        .divfloatingcss
        {
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
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="divcontentrow1" runat="server" class="content-row1">
    </div>
    <div class="content-row2">
        <div style="background: url(/App_Themes/Gray/images/title-bg.jpg) repeat scroll left top #545454;
            border: 1px solid #999999; color: #000000; font-weight: normal; height: 26px;
            margin: 10px 0 0 0;">
            <div style="vertical-align: middle; float: left; margin: 5px 0 0 0;">
                &nbsp;Product Hemming</div>
            &nbsp;</div>
        <div style="float: left; width: 50%; line-height: 40px; display: none;">
            Select : &nbsp;
            <asp:DropDownList ID="drphamming" runat="server" AutoPostBack="true" onchange="javascript:chkHeight();"
                OnSelectedIndexChanged="drphamming_SelectedIndexChanged">
                <asp:ListItem Text="Without hemming List" Value="0"></asp:ListItem>
                <asp:ListItem Text="With hemming List" Value="1"></asp:ListItem>
            </asp:DropDownList>
        </div>
        <div style="float: right; width: 100%; line-height: 40px;">
            <div style="float: left;">
                <asp:CheckBox ID="chkonoff" runat="server" /> <b>&nbsp;Globally&nbsp;Hemming&nbsp;On&nbsp;or&nbsp;Off</b>
                 </div>
            <div style="float: right">
                Search By(SKU or UPC):
                <asp:TextBox ID="txtSearch" runat="server" CssClass="order-textfield"></asp:TextBox>&nbsp;<asp:Button
                    ID="btnSearch" runat="server" OnClick="btnSearch_Click" OnClientClick="return chksearch();chkHeight();" />
                &nbsp;
                <asp:Button ID="btnShowall" runat="server" OnClick="btnShowall_Click" OnClientClick="javascript:chkHeight();" />&nbsp;
                <asp:Button ID="btnExport" runat="server" ToolTip="Export" Style="background: url(/App_Themes/<%= Page.Theme.ToString()%>/images/export.gif) no-repeat transparent;
                    width: 67px; height: 23px; border: none; cursor: pointer;" OnClick="btnExport_Click" />
            </div>
        </div>
        <div style="float: left; width: 100%;">
            <asp:Literal ID="lthamming" runat="server"></asp:Literal>
        </div>
        <div style="float: right; width: 100%; height: 40px; padding-top: 5px; display: none;">
            <div id="divfloating" class="divfloatingcss" style="width: 300px;">
                <div style="margin-bottom: 1px; margin-top: 3px;">
                    <asp:ImageButton ID="btnSave" runat="server" TabIndex="38" Visible="false" />
                </div>
            </div>
        </div>
    </div>
    <div id="prepage" style="position: absolute; font-family: arial; font-size: 16; left: 0px;
        top: 0px; background-color: #000; opacity: 0.7; filter: alpha(opacity=70); layer-background-color: white;
        height: 100%; width: 100%; z-index: 1000; display: none;">
        <table width="100%" style="padding-top: 400px;" align="center">
            <tr>
                <td align="center" style="color: #fff;" valign="middle">
                    <img alt="" src="/App_Themes/<%=Page.Theme %>/images/loding.png" /><br />
                    <b>Loading ... ... Please wait!</b>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
