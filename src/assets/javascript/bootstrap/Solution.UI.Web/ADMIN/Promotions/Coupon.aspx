<%@ Page Title="" Language="C#" MasterPageFile="~/ADMIN/Admin.Master" AutoEventWireup="true"
    CodeBehind="Coupon.aspx.cs" Inherits="Solution.UI.Web.ADMIN.Promotions.Coupon"
    MaintainScrollPositionOnPostback="true" ValidateRequest="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts-main.js"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts.js"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-calender-custom.js"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-calender.js"></script>
    <script type="text/javascript">

        $(function () {

            $('#ContentPlaceHolder1_txtexpire').datetimepicker({
                showButtonPanel: true, ampm: false,
                showHour: false, showMinute: false, showSecond: false, showTime: false
            });
            $('#ContentPlaceHolder1_txtfromdate').datetimepicker({
                showButtonPanel: true, ampm: false,
                showHour: false, showMinute: false, showSecond: false, showTime: false
            });
        });

        function showshippingfeedcode()
        {
            if (document.getElementById('ContentPlaceHolder1_chkfeedvisible') != null && document.getElementById('ContentPlaceHolder1_chkfeedvisible').checked == true)
            {
                 
                if (document.getElementById('ContentPlaceHolder1_trshippingfeedcode') != null)
                {
                    document.getElementById('ContentPlaceHolder1_trshippingfeedcode').style.display = '';
                }
            }
            else
            {
                if (document.getElementById('ContentPlaceHolder1_trshippingfeedcode') != null) {
                    document.getElementById('ContentPlaceHolder1_trshippingfeedcode').style.display = 'none';
                }
            }
        }
        function isNumberKey(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode
            if (charCode > 31 && (charCode < 48 || charCode > 57))
                return false;
            return true;
        }
        function inputOnlyNumbers(evt) {
            var e = window.event || evt;
            var charCode = e.which || e.keyCode;
            if (charCode < 31 || (charCode > 47 && charCode < 58) || charCode == 46) {
                return true;
            }
            return false;
        }
        function RadioValidation() {
            var listItemArray = document.getElementById('ContentPlaceHolder1_rdiocouponvalidfor').getElementsByTagName('INPUT');
            var isItemChecked = false;
            for (var i = 0; i < listItemArray.length; i++) {
                var listItem = listItemArray[i];
                if (listItem.checked) {
                    isItemChecked = true;
                }
            }
            if (isItemChecked == false) {
                jAlert('Please select a option  for Coupon Validity', 'Message');
                return false;
            }
            else {
                return true;
            }
        }
        function DisabledProductdata()
        {
            if (document.getElementById('ContentPlaceHolder1_chkcategorycoupon').checked == true)
            {
                //document.getElementById('ContentPlaceHolder1_txtvalidforprod').value = '0';
                document.getElementById('ContentPlaceHolder1_txtdiscountper').value = '1';
                $('#ContentPlaceHolder1_txtdiscountper').attr("readonly", "true");
              //  $('#ContentPlaceHolder1_txtvalidforprod').attr("readonly", "true");
              //  document.getElementById('ContentPlaceHolder1_chkvalidforprod').checked = false;
               // $('#ContentPlaceHolder1_chkvalidforprod').removeAttr("checked");
               // document.getElementById('ContentPlaceHolder1_chkvalidforprod').disabled = true;
                
                
                //document.getElementById('ContentPlaceHolder1_chkvalidforcat').checked = false;
                $('#ContentPlaceHolder1_chkvalidforcat').removeAttr("checked");
                document.getElementById('ContentPlaceHolder1_chkvalidforcat').disabled = true;
                document.getElementById('ContentPlaceHolder1_btnpro').disabled = true;
            }
            else {
                document.getElementById('ContentPlaceHolder1_txtvalidforprod').value = '';
              //  $('#ContentPlaceHolder1_txtvalidforprod').removeAttr("readonly");
                document.getElementById('ContentPlaceHolder1_txtdiscountper').value = '0';
                $('#ContentPlaceHolder1_txtdiscountper').removeAttr("readonly");
               // document.getElementById('ContentPlaceHolder1_chkvalidforprod').disabled = false;
               // document.getElementById('ContentPlaceHolder1_chkvalidforprod').checked = false;
                document.getElementById('ContentPlaceHolder1_btnpro').disabled = false;
                document.getElementById('ContentPlaceHolder1_chkvalidforcat').disabled = false;
                document.getElementById('ContentPlaceHolder1_chkvalidforcat').checked = false;
            }
            
            
        }
    </script>
    <script type="text/javascript">
        function DisableBox(chk, e) {
            if (chk.childNodes.item(0).checked) {
                document.getElementById(e).setAttribute("disabled", "disabled");
            }
            else {
                document.getElementById(e).removeAttribute("disabled");
            }
        }
    </script>
    <script language="javascript" type="text/javascript">
        function testi() {
            var bt = document.getElementById('<%=btnredirect.ClientID %>');
            if (bt) {
                bt.click();
            }
        }
    </script>
    <script type="text/javascript">
        function Checkfields() {
            var RetunrData = true;

          

            if (document.getElementById("ContentPlaceHolder1_ddlStore").selectedIndex == 0) {
                jAlert('Please select Store.', 'Message', 'ContentPlaceHolder1_ddlStore');
                RetunrData = false;
            }
            else if (document.getElementById("ContentPlaceHolder1_txtcode").value == '') {
                jAlert('Please enter Coupon Code.', 'Message', 'ContentPlaceHolder1_txtcode');
                RetunrData = false;
            }
            else if (document.getElementById("ContentPlaceHolder1_txtfromdate").value == '') {
                jAlert('Please enter start date.', 'Message', 'ContentPlaceHolder1_txtfromdate');
                RetunrData = false;
            }
            else if (document.getElementById("ContentPlaceHolder1_txtexpire").value == '') {
                jAlert('Please enter end date.', 'Message', 'ContentPlaceHolder1_txtexpire');
                RetunrData = false;
            }
            else if ((document.getElementById("ContentPlaceHolder1_txtdiscountper").value == ''))// && document.getElementById("ContentPlaceHolder1_txtdiscountamt").value == '') || (document.getElementById("ContentPlaceHolder1_txtdiscountper").value == '0' && document.getElementById("ContentPlaceHolder1_txtdiscountamt").value == '0')) 
            {
                jAlert('Please Enter Discount Percent', 'Message', 'ContentPlaceHolder1_txtdiscountper');
                RetunrData = false;
            }
            else if ((document.getElementById("ContentPlaceHolder1_txtdiscountper").value > 100)) {
                jAlert('Discount Percent should be less than 100', 'Message', 'ContentPlaceHolder1_txtdiscountper');
                RetunrData = false;
            }
            else if (RadioValidation() == false) {
                RetunrData = false;
            }
            else if (document.getElementById("ContentPlaceHolder1_txtentervalue").value == '') {
                jAlert('Please enter Value.', 'Message', 'ContentPlaceHolder1_txtentervalue');
                RetunrData = false;
            }

            var startDate = new Date(document.getElementById('ContentPlaceHolder1_txtfromdate').value);
            var endDate = new Date(document.getElementById('ContentPlaceHolder1_txtexpire').value);
            if (startDate > endDate) {
                jAlert('Please Select Valid Date.', 'Message', 'ContentPlaceHolder1_txtfromdate');
                RetunrData = false;
            }
            if (document.getElementById('ContentPlaceHolder1_chkcategorycoupon').checked == true) {

                if (document.getElementById('ContentPlaceHolder1_txtvalidforcat').value == '')
                {
                    jAlert('Please Select Category.', 'Message', 'ContentPlaceHolder1_txtvalidforcat');
                    RetunrData = false;
                }
            }
            if (document.getElementById('ContentPlaceHolder1_chkfeedvisible') != null && document.getElementById('ContentPlaceHolder1_chkfeedvisible').checked == true)
            {
                if (document.getElementById('ContentPlaceHolder1_trshippingfeedcode') != null && document.getElementById('ContentPlaceHolder1_txtshippingfeedcode') != null && document.getElementById('ContentPlaceHolder1_txtshippingfeedcode').value == '') {
                    jAlert('Please Enter Shipping Feed Code.', 'Message', 'ContentPlaceHolder1_txtshippingfeedcode');
                    RetunrData = false;
                }
            }
            return RetunrData;
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="content-row1">
        <div class="create-new-order">
            &nbsp;</div>
    </div>
    <div class="content-row2">
        <table width="100%" border="0" cellspacing="0" cellpadding="0">
            <tr>
                <td style="height: 5px" align="left" valign="top">
                    <img src="/App_Themes/<%=Page.Theme %>/images/spacer.gif" alt="" width="1" height="5" />
                </td>
            </tr>
            <tr>
                <td style="height: 5px" align="left" valign="top">
                    <img src="/App_Themes/<%=Page.Theme %>/images/spacer.gif" alt="" width="1" height="5" />
                </td>
            </tr>
            <tr>
                <td class="border-td">
                    <table width="100%" border="0" cellpadding="0" cellspacing="0" style="background-color: #FFFFFF"
                        class="content-table">
                        <tr>
                            <td class="border-td-sub">
                                <table width="100%" border="0" cellpadding="0" cellspacing="0" class="add-product">
                                    <tr>
                                        <th>
                                            <div class="main-title-left">
                                                <img class="img-left" title="Add Coupon" alt="Add Coupon" src="/App_Themes/<%=Page.Theme %>/Images/coupon-discount-icon.png" />
                                                <h2>
                                                    <asp:Label runat="server" Text="Add Coupon" ID="lblTitle"></asp:Label></h2>
                                            </div>
                                        </th>
                                    </tr>
                                    <tr class="altrow">
                                        <td align="right">
                                            <span class="star">*</span>Required Field
                                        </td>
                                    </tr>
                                    <tr class="even-row">
                                        <td>
                                            <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                                <tr class="oddrow">
                                                    <td>
                                                        <span class="star">*</span>Store Name :
                                                    </td>
                                                    <td style="width: 80%" colspan="2">
                                                        <asp:DropDownList ID="ddlStore" runat="server" CssClass="order-list" Width="226px">
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr class="altrow">
                                                    <td>
                                                        <span class="star">&nbsp;&nbsp;</span>Description :
                                                    </td>
                                                    <td colspan="2">
                                                        <asp:TextBox runat="server" ID="txtdescription" CssClass="order-textfield" TextMode="MultiLine"
                                                            Style="width: 232px; height: 60px;"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr class="oddrow">
                                                    <td>
                                                        <span class="star">*</span>Coupon Code :
                                                    </td>
                                                    <td colspan="2">
                                                        <asp:TextBox runat="server" ID="txtcode" CssClass="order-textfield" MaxLength="10"></asp:TextBox>
                                                        <asp:HiddenField ID="hdncoupon" runat="server" />
                                                    </td>
                                                </tr>
                                                <tr class="altrow">
                                                    <td>
                                                        <span class="star">*</span>Start Date :
                                                    </td>
                                                    <td colspan="2">
                                                        <asp:TextBox runat="server" ID="txtfromdate" CssClass="order-textfield" Width="86px"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr class="altrow">
                                                    <td>
                                                        <span class="star">*</span>End Date :
                                                    </td>
                                                    <td colspan="2">
                                                      
                                                        <asp:TextBox runat="server" ID="txtexpire" CssClass="order-textfield" Width="86px"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                 <tr class="altrow">
                                                    <td>
                                                        <span class="star">&nbsp;</span>Active :
                                                    </td>
                                                    <td colspan="2">
                                                        <asp:CheckBox ID="chckactive" runat="server"  />
                                                         
                                                    </td>
                                                </tr>
                                                  <tr class="altrow">
                                                    <td>
                                                        <span class="star">&nbsp;</span>Is Admin Only :
                                                    </td>
                                                    <td colspan="2">
                                                        <asp:CheckBox ID="chkadminonly" runat="server"  />
                                                         
                                                    </td>
                                                </tr>
                                                <tr class="oddrow">
                                                    <td>
                                                        <span class="star">&nbsp;&nbsp;</span>Discount Percent :
                                                    </td>
                                                    <td colspan="2">
                                                        <asp:TextBox runat="server" ID="txtdiscountper" CssClass="order-textfield" Width="86px"
                                                            onkeypress="return inputOnlyNumbers(event)" MaxLength="10"></asp:TextBox>
                                                        %
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                    </td>
                                                    <td colspan="2">
                                                      
                                                    </td>
                                                </tr>
                                                <tr class="altrow" style="display:none;">
                                                    <td>
                                                        <span class="star">&nbsp;&nbsp;</span>Discount Amount :
                                                    </td>
                                                    <td colspan="2">
                                                        <asp:TextBox runat="server" ID="txtdiscountamt" CssClass="order-textfield" Width="86px"
                                                            onkeypress="return inputOnlyNumbers(event)" MaxLength="10"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr class="oddrow">
                                                    <td>
                                                        <span class="star">&nbsp;&nbsp;</span>Is Category Coupon :
                                                    </td>
                                                    <td>
                                                        <asp:CheckBox ID="chkcategorycoupon" runat="server" onchange="DisabledProductdata();" CssClass="" />
                                                    </td>
                                                </tr>
                                                <tr class="oddrow">
                                                    <td>
                                                        <span class="star">&nbsp;&nbsp;</span>Free Shipping :
                                                    </td>
                                                    <td>
                                                        <asp:CheckBox ID="chkshipdiscnt" runat="server" CssClass="" />
                                                    </td>
                                                </tr>
                                                 <tr class="altrow">
                                                    <td>
                                                        <span class="star">&nbsp;&nbsp;</span>Buy 1 Get 1 :
                                                    </td>
                                                    <td>
                                                        <asp:CheckBox ID="chkbuy1get1" runat="server" CssClass="" />
                                                    </td>
                                                </tr>
                                                 <tr class="oddrow">
                                                    <td>
                                                        <span class="star">&nbsp;&nbsp;</span>New Arrival :
                                                    </td>
                                                    <td>
                                                        <asp:CheckBox ID="chknewarrival" runat="server" CssClass="" />
                                                    </td>
                                                </tr>
                                                 <tr class="oddrow">
                                                    <td>
                                                        <span class="star">&nbsp;&nbsp;</span>Sales Clearance :
                                                    </td>
                                                    <td>
                                                        <asp:CheckBox ID="chksalesclearance" runat="server" CssClass="" />
                                                    </td>
                                                </tr>
                                                <tr class="oddrow">
                                                    <td>
                                                        <span class="star">&nbsp;&nbsp;</span>Is in Feed :
                                                    </td>
                                                    <td>
                                                        <asp:CheckBox ID="chkfeedvisible" runat="server" CssClass="" onchange="showshippingfeedcode();" />
                                                    </td>
                                                </tr>
                                                 <tr class="oddrow"   id="trshippingfeedcode" runat="server">
                                                    <td>
                                                        <span class="star">&nbsp;&nbsp;</span>Shipping Feed Code :
                                                    </td>
                                                    <td>
                                                        <asp:TextBox runat="server" ID="txtshippingfeedcode" CssClass="order-textfield" Width="180px"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr class="oddrow">
                                                    <td>
                                                        <span class="star">*</span>Coupon Valid for :
                                                    </td>
                                                    <td colspan="2">
                                                        <asp:RadioButtonList ID="rdiocouponvalidfor" AutoPostBack="true" runat="server" OnSelectedIndexChanged="rdiocouponvalidfor_SelectedIndexChanged">
                                                            <asp:ListItem> Expires on First Use by any Customer</asp:ListItem>
                                                            <asp:ListItem> Expires after One Usage by Each Customer</asp:ListItem>
                                                            <asp:ListItem> Expires after N Uses</asp:ListItem>
                                                        </asp:RadioButtonList>
                                                    </td>
                                                </tr>
                                                <tr class="oddrow" id="usesrow" runat="server" visible="false">
                                                    <td>
                                                        <span class="star">*</span>Enter Value :
                                                    </td>
                                                    <td colspan="2">
                                                        <asp:TextBox runat="server" ID="txtentervalue" CssClass="order-textfield" onkeypress="return inputOnlyNumbers(event)"
                                                            MaxLength="10"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr class="altrow">
                                                    <td>
                                                        <span class="star">&nbsp;&nbsp;</span>Valid For Customer(s) :
                                                    </td>
                                                    <td style="width: 500px;">
                                                        <asp:TextBox runat="server" ID="txtvalidforcust" CssClass="order-textfield" TextMode="MultiLine"
                                                            Width="224px" Height="100px"></asp:TextBox>&nbsp;
                                                        <asp:CheckBox ID="chkvalidforcust" runat="server" Text=" All Customers  &nbsp;&nbsp;&nbsp;|  &nbsp;&nbsp;&nbsp;(Enter Comma seperated list of Customer ID's)  &nbsp;&nbsp;&nbsp;|"
                                                            onchange="javascript:DisableBox(this,'ContentPlaceHolder1_txtvalidforcust');" />
                                                        &nbsp;&nbsp;<asp:ImageButton ID="btncust" runat="server" ToolTip="Add Customer" OnClientClick="return openCenteredCrossSaleWindow1('customer');"
                                                            CausesValidation="true" />
                                                    </td>
                                                </tr>
                                                <tr class="oddrow">
                                                    <td>
                                                        <span class="star">&nbsp;&nbsp;</span>Valid For Product(s) :
                                                    </td>
                                                    <td>
                                                        <asp:TextBox runat="server" ID="txtvalidforprod" CssClass="order-textfield" TextMode="MultiLine"
                                                            Width="224px" Height="100px"></asp:TextBox>&nbsp;
                                                        <asp:CheckBox ID="chkvalidforprod" runat="server" Text=" All Products  &nbsp;&nbsp;&nbsp;|  &nbsp;&nbsp;&nbsp;(Enter Comma seperated list of Product ID's)  &nbsp;&nbsp;&nbsp;|"
                                                            onchange="javascript:DisableBox(this,'ContentPlaceHolder1_txtvalidforprod');" />
                                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:ImageButton
                                                            ID="btnpro" runat="server" ToolTip="Add Product" OnClientClick="return openCenteredCrossSaleWindow1('product');"
                                                            CausesValidation="true" />
                                                    </td>
                                                </tr>
                                                <tr class="altrow">
                                                    <td>
                                                        <span class="star">&nbsp;&nbsp;</span>Valid For Category(s)
                                                    </td>
                                                    <td valign="bottom">
                                                        <asp:TextBox runat="server" ID="txtvalidforcat" CssClass="order-textfield" TextMode="MultiLine"
                                                            Width="224px" Height="100px"></asp:TextBox>&nbsp;
                                                        <asp:CheckBox ID="chkvalidforcat" runat="server" Text=" All Categories  &nbsp;&nbsp;&nbsp;|  &nbsp;&nbsp;&nbsp;(Enter Comma seperated list of Category ID's)  &nbsp;&nbsp;&nbsp;|"
                                                            onchange="javascript:DisableBox(this,'ContentPlaceHolder1_txtvalidforcat');" />
                                                        &nbsp;&nbsp;&nbsp;&nbsp;<asp:ImageButton ID="btncat" runat="server" ToolTip="Add Category"
                                                            OnClientClick="return openCenteredCrossSaleWindow1category('category');" CausesValidation="true" />
                                                        <div style="display: none">
                                                            <asp:Button ID="btnredirect" runat="server" Text="Redirect" OnClick="btnredirect_Click" />
                                                        </div>
                                                    </td>
                                                </tr>
                                                <tr class="oddrow">
                                                    <td>
                                                        <span class="star">&nbsp;&nbsp;</span>Min. Order Total :
                                                    </td>
                                                    <td colspan="2">
                                                        <asp:TextBox runat="server" ID="txtminorder" CssClass="order-textfield" onkeypress="return inputOnlyNumbers(event)"
                                                            MaxLength="15"></asp:TextBox>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr class="altrow">
                                        <td>
                                            <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                                <td>
                                                </td>
                                                <td style="width: 80%">
                                                    <asp:ImageButton ID="btnSave" runat="server" AlternateText="Save" ToolTip="Save"
                                                        CausesValidation="true" OnClientClick="return Checkfields();" OnClick="btnSave_Click" />
                                                    &nbsp;<asp:ImageButton ID="btnCancel" runat="server" AlternateText="Cancel" ToolTip="Cancel"
                                                        OnClick="btnCancel_Click" />
                                                </td>
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
                <td style="height: 10px" align="left" valign="top">
                    <img src="/App_Themes/<%=Page.Theme %>/images/spacer.gif" width="1" alt="" height="10" />
                </td>
            </tr>
            <tr>
                <td>
                    &nbsp;
                </td>
            </tr>
        </table>
    </div>
    <script type="text/javascript" language="javascript">
        function openCenteredCrossSaleWindow1(mode) {

            if (document.getElementById('<%=ddlStore.ClientID %>').value != "0") {
                //var ids;
                var couponid = document.getElementById('<%=hdncoupon.ClientID %>').value; ;
                var width = 700;
                var height = 500;
                if (mode == 'product')
                {
                    width = 850;
                    height = 500;
                }
                 
                var left = parseInt((screen.availWidth / 2) - (width / 2));
                var top = parseInt((screen.availHeight / 2) - (height / 2));
                var StoreID = document.getElementById('<%=ddlStore.ClientID %>').value;
                var windowFeatures = "width=" + width + ",height=" + height + ",status=no,resizable=yes,scrollbars=yes,left=" + left + ",top=" + top + ",screenX=" + left + ",screenY=" + top;
                window.open('CouponPopup.aspx?StoreID=' + StoreID + '&mode=' + mode + '&couponid=' + couponid, "Mywindow", windowFeatures);
                return false;
            }
            else {
                jAlert('Please select Store.', 'Message', '<%=ddlStore.ClientID %>');
                return false;
            }
        }
        function openCenteredCrossSaleWindow1category(mode) {

            if (document.getElementById('<%=ddlStore.ClientID %>').value != "0") {
                //var ids;
                var couponid = document.getElementById('<%=hdncoupon.ClientID %>').value;;
                var width = 850;
                var height = 700;
                if (mode == 'product') {
                    width = 850;
                    height = 500;
                }

                var left = parseInt((screen.availWidth / 2) - (width / 2));
                var top = parseInt((screen.availHeight / 2) - (height / 2));
                var StoreID = document.getElementById('<%=ddlStore.ClientID %>').value;
                var windowFeatures = "width=" + width + ",height=" + height + ",status=no,resizable=yes,scrollbars=yes,left=" + left + ",top=" + top + ",screenX=" + left + ",screenY=" + top;
                window.open('Couponcategorypopup.aspx?StoreID=' + StoreID + '&mode=' + mode + '&couponid=' + couponid, "Mywindow", windowFeatures);
                return false;
            }
            else {
                jAlert('Please select Store.', 'Message', '<%=ddlStore.ClientID %>');
                return false;
            }
        }
    </script>
</asp:Content>
