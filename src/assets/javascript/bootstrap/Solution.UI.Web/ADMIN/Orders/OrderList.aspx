<%@ Page Title="" Language="C#" MasterPageFile="~/ADMIN/Admin.Master" AutoEventWireup="true"
    CodeBehind="OrderList.aspx.cs" Inherits="Solution.UI.Web.ADMIN.Orders.OrderList"
    ValidateRequest="false" %>

<asp:Content ID="cntOrderList" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts-main.js"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-calender-custom.js"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-calender.js"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts.js"></script>
    <script type="text/javascript" src="/js/popup.js"></script>
    <link href="/css/general.css" rel="stylesheet" type="text/css" />
    
    <script language="javascript" type="text/javascript">
        function checkCountCapture() {

            var myform = document.forms[0];
            var len = myform.elements.length;
            var count = 0;
            for (var i = 0; i < len; i++) {
                if (myform.elements[i].type == 'checkbox' && myform.elements[i].id.toString().toLowerCase().indexOf('_chkuploadorderstock_') > -1) {
                    if (myform.elements[i].checked == true) {
                        count += 1;
                    }
                }
            }
            if (count == 0) {
                $(document).ready(function () { jAlert('Select at least One Rocord(s)!', 'Message', ''); });
                return false;
            }
            else {
                MultiCaptureClick();
            }
            // return true;
        }
       function KeyPress(e) {
            var evtobj = window.event ? event : e
            if (evtobj.ctrlKey) { if (document.getElementById('prepage') != null && document.getElementById('prepage').style.display=='') { document.getElementById('prepage').style.display = 'none'; } }
       }
        document.onkeydown = KeyPress;
       
        function MultiCaptureClick() {

            jConfirm('Are you sure you want to Capture selected Order(s)?', 'Confirmation', function (r) {
                if (r == true) {
                    chkHeight();
                    document.getElementById('ContentPlaceHolder1_btnMultipleCapture').click();
                    return true;
                }
                else {
                    return false;
                }
            });
            return false;
        }
        function checkCount() {
            var myform = document.forms[0];
            var len = myform.elements.length;
            var count = 0;
            for (var i = 0; i < len; i++) {
                if (myform.elements[i].type == 'checkbox' && myform.elements[i].id.toString().toLowerCase().indexOf('_chkuploadorderstock_') > -1) {
                    if (myform.elements[i].checked == true) {
                        count += 1;
                    }
                }
            }
            if (count == 0) {
                $(document).ready(function () { jAlert('Select at least One Rocord(s)!', 'Message', ''); });
                return false;
            }
            return true;
        }
        function checkCountPrint() {
            var myform = document.forms[0];
            var len = myform.elements.length;
            var count = 0;
            for (var i = 0; i < len; i++) {
                if (myform.elements[i].type == 'checkbox' && myform.elements[i].id.toString().toLowerCase().indexOf('_chkprintsalesorder_') > -1) {
                    if (myform.elements[i].checked == true) {
                        count += 1;
                    }
                }
            }
            if (count == 0) {
                $(document).ready(function () { jAlert('Select at least One Rocord(s)!', 'Message', ''); });
                return false;
            }
            else {
                chkHeight();
            }
            return true;
        }
        function selectAllPrint(on) {
            var myform = document.forms[0];
            var len = myform.elements.length;
            for (var i = 0; i < len; i++) {
                if (myform.elements[i].type == 'checkbox' && myform.elements[i].id.toString().toLowerCase().indexOf('_chkprintsalesorder_') > -1) {
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
        function selectAll(on) {
            var myform = document.forms[0];
            var len = myform.elements.length;
            for (var i = 0; i < len; i++) {
                if (myform.elements[i].type == 'checkbox' && myform.elements[i].id.toString().toLowerCase().indexOf('_chkuploadorderstock_') > -1) {
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
        $(function () {

            $('#ContentPlaceHolder1_grvOrderlist_txtOrderFrom_0').datetimepicker({
                showButtonPanel: true, ampm: false,
                showHour: false, showMinute: false, showSecond: false, showTime: false
            });
            $('#ContentPlaceHolder1_grvOrderlist_txtOrderTo_0').datetimepicker({
                showButtonPanel: true, ampm: false,
                showHour: false, showMinute: false, showSecond: false, showTime: false
            });
        });

        function isNumberKey(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode

            if (charCode > 31 && (charCode < 48 || charCode > 57)) {
                if (charCode == 46) {
                }
                else {
                    return false;
                }
            }

            return true;
        }
        function isNumberKeyonlynumber(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode

            if ((charCode == 8 || charCode > 31) && (charCode < 48 || charCode > 57))
                return false;

            return true;
        }
        function isEnter(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode

            if (charCode == 13) {
                document.getElementById('ContentPlaceHolder1_btnGo').click();
            }
            else {
                return true;
            }

            return true;
        }
        function pagenumber() {


            var num = document.getElementById('ContentPlaceHolder1_hdnpages').value;
            var num1 = document.getElementById('ContentPlaceHolder1_txtPagenumber').value;

            if (num1 > num) {

                document.getElementById('ContentPlaceHolder1_txtPagenumber').value = num;

            }
            if (num1 == 0) {
                document.getElementById('ContentPlaceHolder1_txtPagenumber').value = "1";
            }

        }

        function chkHeight() {
            var windowHeight = 0;
            windowHeight = $(document).height(); //window.innerHeight;
            resetFilterSearch();
            document.getElementById('prepage').style.height = windowHeight + 'px';
            document.getElementById('prepage').style.display = '';
        }
 function resetSearchAdvance() {

          
            document.getElementById('ContentPlaceHolder1_grvOrderlist_txtOrderNo_0').value = '';
            document.getElementById('ContentPlaceHolder1_grvOrderlist_ddlStore_0').selectedIndex = 0;
            document.getElementById('ContentPlaceHolder1_grvOrderlist_ddlStatus_0').selectedIndex = 1;
            document.getElementById('ContentPlaceHolder1_grvOrderlist_txtCustomername_0').value = '';
            document.getElementById('ContentPlaceHolder1_grvOrderlist_txtOrderTotalFrom_0').value = '';
            document.getElementById('ContentPlaceHolder1_grvOrderlist_txtOrderTotalTo_0').value = '';
            if (document.getElementById('ContentPlaceHolder1_grvOrderlist_ddlstockstat_0') != null) {
                // document.getElementById('ContentPlaceHolder1_grvOrderlist_ddlstockstat_0').selectedIndex = 0;
            }
            resetFilterSearch();

        }
        function resetSearch() {

            document.getElementById('ContentPlaceHolder1_grvOrderlist_txtOrderFrom_0').value = '';
            document.getElementById('ContentPlaceHolder1_grvOrderlist_txtOrderTo_0').value = '';
            document.getElementById('ContentPlaceHolder1_grvOrderlist_txtOrderNo_0').value = '';
            document.getElementById('ContentPlaceHolder1_grvOrderlist_ddlStore_0').selectedIndex = 0;
            document.getElementById('ContentPlaceHolder1_grvOrderlist_ddlStatus_0').selectedIndex = 1;
            document.getElementById('ContentPlaceHolder1_grvOrderlist_txtCustomername_0').value = '';
            document.getElementById('ContentPlaceHolder1_grvOrderlist_txtOrderTotalFrom_0').value = '';
            document.getElementById('ContentPlaceHolder1_grvOrderlist_txtOrderTotalTo_0').value = '';
            if (document.getElementById('ContentPlaceHolder1_grvOrderlist_ddlstockstat_0') != null) {
               // document.getElementById('ContentPlaceHolder1_grvOrderlist_ddlstockstat_0').selectedIndex = 0;
            }
            resetFilterSearch();

        }
        function resetFilterSearch() {



            if (document.getElementById('<%=txtEmail.ClientID %>')) { document.getElementById('<%=txtEmail.ClientID %>').value = ''; }
            if (document.getElementById('<%=txtpo.ClientID %>')) { document.getElementById('<%=txtpo.ClientID %>').value = ''; }
            if (document.getElementById('<%=txtzipcode.ClientID %>')) { document.getElementById('<%=txtzipcode.ClientID %>').value = ''; }
            if (document.getElementById('<%=ddlpayment.ClientID %>')) { document.getElementById('<%=ddlpayment.ClientID %>').selectedIndex = 0; }
            if (document.getElementById('<%=txtcompanyname.ClientID %>')) { document.getElementById('<%=txtcompanyname.ClientID %>').value = ''; }
            if (document.getElementById('<%=ddlTransactionStatus.ClientID %>')) { document.getElementById('<%=ddlTransactionStatus.ClientID %>').selectedIndex = 0; }
            if (document.getElementById('<%=dlState.ClientID %>')) { document.getElementById('<%=dlState.ClientID %>').selectedIndex = 0; }
            if (document.getElementById('<%=txtcoupncode.ClientID %>')) { document.getElementById('<%=txtcoupncode.ClientID %>').value = ''; }

            return false;

        }
        function CheckSearch() {

            var fl = false;
            if (document.getElementById('ContentPlaceHolder1_grvOrderlist_txtOrderFrom_0') != null && document.getElementById('ContentPlaceHolder1_grvOrderlist_txtOrderFrom_0').value != '') {
                fl = true;
            }
            else if (document.getElementById('ContentPlaceHolder1_grvOrderlist_txtOrderTo_0') != null && document.getElementById('ContentPlaceHolder1_grvOrderlist_txtOrderTo_0').value != '') {
                fl = true;
            }
            else if (document.getElementById('ContentPlaceHolder1_grvOrderlist_txtOrderNo_0') != null && document.getElementById('ContentPlaceHolder1_grvOrderlist_txtOrderNo_0').value != '') {
                fl = true;
            }
            else if (document.getElementById('ContentPlaceHolder1_grvOrderlist_ddlStore_0') != null && document.getElementById('ContentPlaceHolder1_grvOrderlist_ddlStore_0').selectedIndex >= 0) {
                fl = true;
            }
            else if (document.getElementById('ContentPlaceHolder1_grvOrderlist_ddlStatus_0') != null && document.getElementById('ContentPlaceHolder1_grvOrderlist_ddlStatus_0').selectedIndex >= 0) {
                fl = true;
            }
            else if (document.getElementById('ContentPlaceHolder1_grvOrderlist_txtCustomername_0') != null && document.getElementById('ContentPlaceHolder1_grvOrderlist_txtCustomername_0').value != '') {
                fl = true;
            }
            else if (document.getElementById('ContentPlaceHolder1_grvOrderlist_txtOrderTotalFrom_0') != null && document.getElementById('ContentPlaceHolder1_grvOrderlist_txtOrderTotalFrom_0').value != '') {
                fl = true;
            }
            else if (document.getElementById('ContentPlaceHolder1_grvOrderlist_txtOrderTotalTo_0') != null && document.getElementById('ContentPlaceHolder1_grvOrderlist_txtOrderTotalTo_0').value != '') {
                fl = true;
            }


            if (fl == false) {
                jAlert('Please Fill Up Search criteria.', 'Message');
                return false;
            }
            else {
                chkHeight();
            }
            return true;


        }

        function showhide() {

            if (document.getElementById('imgminmize') != null) {
                var src = document.getElementById('imgminmize').src;
                if (src.indexOf('expand.gif') > -1) {
                    document.getElementById('imgminmize').src = src.replace('expand.gif', 'minimize.png');
                    document.getElementById('imgminmize').title = 'Minimize';
                    document.getElementById('imgminmize').alt = 'Minimize';
                    document.getElementById('imgminmize').style.marginTop = "4px";
                    document.getElementById('imgminmize').className = 'minimize';
                    document.getElementById('ContentPlaceHolder1_grvOrderlist').style.display = '';
                }
                else if (src.indexOf('minimize.png') > -1) {
                    document.getElementById('imgminmize').src = src.replace('minimize.png', 'expand.gif');
                    document.getElementById('imgminmize').title = 'Show';
                    document.getElementById('imgminmize').style.marginTop = "0px";
                    document.getElementById('imgminmize').alt = 'Show';
                    document.getElementById('imgminmize').className = 'close';
                    document.getElementById('ContentPlaceHolder1_grvOrderlist').style.display = 'none';
                }
            }
        }
        function ShowSearchpopup() {

            window.scrollTo(0, 0);
            resetSearchAdvance();
            resetFilterSearch();
            document.getElementById('btnreadmore').click();

            return false;
        }


        function CaptureClick(id) {


            jConfirm('Are you sure want to capture this order?', 'Confirmation', function (r) {
                if (r == true) {
                    chkHeight();
                    document.getElementById('ContentPlaceHolder1_hdnordernumber').value = id;
                    document.getElementById('ContentPlaceHolder1_btnCapture').click();

                    return true;
                }
                else {

                    return false;
                }
            });
            return false;
        }
        function RefundClick(id, Total, ramount) {
            document.getElementById('ContentPlaceHolder1_hdnAmount').value = Total;
            document.getElementById('ContentPlaceHolder1_hdnrefundAmount').value = ramount;
            OpenCenterWindow('refundorder.aspx?ONo=' + id + '&OrderList=1', 600, 300);
        }
        function OpenCenterWindow(pid, wi, he) {
            var w = wi;
            var h = he;
            var left = (screen.width / 2) - (w / 2);
            var top = (screen.height / 2) - (h / 2);
            window.open(pid, '', 'toolbar=no, location=no, directories=no, status=no, menubar=no, scrollbars=yes, resizable=no, copyhistory=no, width=' + w + ', height=' + h + ', top=' + top + ', left=' + left);
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="content-row1">
        <div class="create-new-order" style="width: 100%;">
            <table style="width: 100%;">
                <tr>
                    <td align="left" style="width: 2%;">
                        Store:
                    </td>
                    <td align="left" style="padding-left: 5px; width: 95%;">
                        <asp:DropDownList ID="ddlStoregeneral" runat="server" AutoPostBack="true" CssClass="order-list"
                            Width="190px" Height="20px" onchange="chkHeight();" OnSelectedIndexChanged="ddlStoregeneral_SelectedIndexChanged">
                        </asp:DropDownList>
                        &nbsp; Ware House:&nbsp;<asp:DropDownList ID="ddlwarehouse" runat="server" AutoPostBack="true"
                            Width="191px" CssClass="order-list" OnSelectedIndexChanged="ddlwarehouse_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td align="right" style="text-align: right;">
                        <a id="btnCustomerPhoneOrder" runat="server" title="Create New Order" href="CustomerPhoneOrder.aspx">
                            <img height="23" class="img-right" style="margin: 5px 0 0 0;" title="Create New Order"
                                alt="Create New Order" src="/App_Themes/<%=Page.Theme %>/images/create-new-order.gif" /></a>
                    </td>
                </tr>
            </table>
        </div>
        <div style="display: none;">
            <asp:Button ID="btnTemp" runat="server" OnClientClick="javascript:return false;" />
        </div>
    </div>
    <div class="content-row2">
        <table cellspacing="0" cellpadding="0" border="0" width="100%">
            <tbody>
                <tr>
                    <td height="5" align="left" valign="top">
                        <img height="5" width="1" src="/App_Themes/<%=Page.Theme %>/images/spacer.gif" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <table cellspacing="0" cellpadding="0" border="0" width="100%">
                            <tbody>
                                <tr>
                                    <td>
                                        <table cellspacing="0" cellpadding="0" border="0" width="500" class="pagination">
                                            <tbody>
                                                <tr>
                                                    <td width="29">
                                                        Page
                                                    </td>
                                                    <td width="22">
                                                        <asp:Literal ID="ltrprevious" runat="server"></asp:Literal>
                                                    </td>
                                                    <td width="33">
                                                        <asp:TextBox ID="txtPagenumber" Style="text-align: center; vertical-align: middle;
                                                            width: 28px;" runat="server" CssClass="order-textfield" Text="1" oncontextmenu="return false;"
                                                            onkeypress="return isNumberKeyonlynumber(event);" onkeyup="pagenumber();" ReadOnly="true"></asp:TextBox>
                                                    </td>
                                                    <td width="19">
                                                        <asp:Literal ID="ltrnext" runat="server"></asp:Literal>
                                                    </td>
                                                    <td width="71">
                                                        of
                                                        <asp:Literal ID="ltrPages" runat="server"></asp:Literal>
                                                        pages
                                                        <input type="hidden" id="hdnpages" runat="server" value="0" />
                                                    </td>
                                                    <td width="10" valign="middle">
                                                        |
                                                    </td>
                                                    <td width="37">
                                                        Views
                                                    </td>
                                                    <td width="48">
                                                        <asp:DropDownList ID="ddlPageRecord" runat="server" AutoPostBack="true" Style="width: 47px;"
                                                            CssClass="page-views" onchange="javascript:chkHeight();" Height="20px" OnSelectedIndexChanged="ddlPageRecord_SelectedIndexChanged">
                                                            <asp:ListItem Text="25" Value="25" Selected="True"></asp:ListItem>
                                                            <asp:ListItem Text="50" Value="50"></asp:ListItem>
                                                            <asp:ListItem Text="100" Value="100"></asp:ListItem>
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td width="50">
                                                        per page
                                                    </td>
                                                    <td width="10" valign="middle">
                                                        |
                                                    </td>
                                                    <td width="183">
                                                        Total
                                                        <asp:Literal ID="ltrRecord" runat="server"></asp:Literal>
                                                        record(s) found
                                                    </td>
                                                </tr>
                                            </tbody>
                                        </table>
                                    </td>
                                    <td style="text-align: right;" align="right">
                                        <table cellspacing="0" cellpadding="0" border="0" class="img-right">
                                            <tbody>
                                                <tr>
                                                    <td align="left" style="padding-left: 5px;">
                                                        <img title="Export icon" alt="" src="/App_Themes/<%=Page.Theme %>/icon/export-icon.png" />
                                                    </td>
                                                    <td align="left">
                                                        <strong>Export to:</strong>
                                                    </td>
                                                    <td align="left" style="padding-left: 5px;">
                                                        <asp:DropDownList ID="ddlexport" runat="server" AutoPostBack="false" CssClass="page-views"
                                                            Width="90px" Height="20px">
                                                            <asp:ListItem Text="Csv File" Value="CSV" Selected="True"></asp:ListItem>
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td align="left" style="padding-left: 5px;">
                                                        <asp:Button ID="btnExport" runat="server" ToolTip="Export" Style="background: url(/App_Themes/<%= Page.Theme.ToString()%>/images/export.gif) no-repeat transparent;
                                                            width: 67px; height: 23px; border: none; cursor: pointer;" OnClick="btnExport_Click" />
                                                        <asp:Button ID="btnadvanceSearch" runat="server" ToolTip="Advance Search" OnClientClick="return ShowSearchpopup();" />
                                                        <asp:Button ID="btnUploadOrder" runat="server" ToolTip="Upload Order"
                                                            OnClientClick="return checkCount();" OnClick="btnUploadOrder_Click" />
                                                         <a href="javascript:void(0);" onclick="checkCountCapture();">
                                                            <asp:Image ID="imgbtnMultipleCapture" runat="server" ToolTip="Multi Capture" AlternateText="Multi Capture" style="margin-bottom:-6px;"  />
                                                        </a>
                                                          <asp:Button ID="btnMultiplePrint" runat="server" ToolTip="Print All" OnClientClick="return checkCountPrint();"
                                                           OnClick="btnMultiplePrint_Click"  />
                                                          <asp:Button ID="btnnavuplod" runat="server" Visible="false" ToolTip="UPLOAD ORDERS TO NAV"
OnClientClick="return checkCount();" OnClick="btnnavuplod_Click" /> 
                                                  
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
                <tr>
                    <td height="5" align="left" valign="top">
                        <img height="5" width="1" alt="" src="/App_Themes/<%=Page.Theme %>/images/spacer.gif" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <table cellspacing="0" cellpadding="0" border="0" width="100%">
                            <tbody>
                                <tr>
                                    <td align="left" width="100%" valign="top" class="border-td">
                                        <table cellspacing="0" cellpadding="0" border="0" width="100%">
                                            <tbody>
                                                <tr>
                                                    <td class="border-td-sub">
                                                        <table cellspacing="0" cellpadding="0" border="0" width="100%" class="content-table">
                                                            <tbody>
                                                                <tr>
                                                                    <th>
                                                                        <div class="main-title-left">
                                                                            <img src="/App_Themes/<%=Page.Theme %>/Images/order-list-icon.png" alt="Order List"
                                                                                title="Order List" class="img-left" />
                                                                            <h2>
                                                                                Order List</h2>
                                                                        </div>
                                                                        <div class="main-title-right">
                                                                            <a href="javascript:void(0);" title="close" id="panelshow" onclick="showhide();">
                                                                                <img src="/App_Themes/<%=Page.Theme %>/images/minimize.png" alt="close" id="imgminmize"
                                                                                    title="minimize" class="minimize" /></a></div>
                                                                    </th>
                                                                </tr>
                                                                <tr>
                                                                    <td align="left" valign="top">
                                                                        <asp:GridView ID="grvOrderlist" runat="server" CssClass="order-table" CellPadding="0"
                                                                            AutoGenerateColumns="false" BorderWidth="0" CellSpacing="0" BorderStyle="None"
                                                                            OnRowDataBound="grvOrderlist_RowDataBound" GridLines="None" OnRowCreated="grvOrderlist_RowCreated"
                                                                            ShowFooter="true" AllowSorting="true" OnSorting="grvOrderlist_Sorting" OnRowEditing="grvOrderlist_RowEditing"
                                                                            ShowHeaderWhenEmpty="True">
                                                                            <Columns>
                                                                                <asp:TemplateField SortExpression="Upload" HeaderText=" Upload into NAV">
                                                                                    <HeaderTemplate>
                                                                                         Upload into NAV
                                                                                    </HeaderTemplate>
                                                                                    <ItemTemplate>
                                                                                        <table cellspacing="0" cellpadding="0" border="0" width="100%" class="order-date"
                                                                                            id="chkUploadvalue" runat="server" visible="false" align="center">
                                                                                            <tbody>
                                                                                                <tr>
                                                                                                    <td colspan="2" class="border-none" align="center">
                                                                                                        <span><a style="color: #B92127; font-size: 12px; font-weight: normal;" id="lkbAllowAll"
                                                                                                            href="javascript:selectAll(true);">Check All</a></span>
                                                                                                    </td>
                                                                                                </tr>
                                                                                                <tr>
                                                                                                    <td colspan="2" class="border-none" align="center">
                                                                                                        <span><a style="color: #B92127; font-size: 12px; font-weight: normal;" id="lkbClearAll"
                                                                                                            href="javascript:selectAll(false);">Clear All</a> </span>
                                                                                                    </td>
                                                                                                </tr>
                                                                                            </tbody>
                                                                                        </table>
                                                                                        <asp:CheckBox ID="chkUploadOrderStock" runat="server" Visible="false" />
                                                                                        <asp:Label ID="lblIsBackEnd" runat="server" Visible="false" Text='<%# Eval("IsBackEnd") %>'></asp:Label>
                                                                                         
                                                                                        <asp:Label ID="lblBackEndGUID" runat="server" Visible="false" Text='<%# Eval("BackEndGUID") %>'></asp:Label>
                                                                                        <asp:Label ID="lblUploadMsg" runat="server"></asp:Label>
                                                                                        <asp:Label ID="lblnavcompleted" runat="server" Visible="false" Text='<%# Eval("isnavcompleted") %>'></asp:Label>
                                                                                         <label></label>
                                                                                         <asp:Label ID="lblisnaverror" runat="server" Visible="false" Text='<%# Eval("IsNavError") %>'></asp:Label>
                                                                                        <asp:Label ID="lblnavstatus" runat="server" Visible="false" Text='<%# Eval("isNAVInserted") %>'></asp:Label>
                                                                                        <asp:Label ID="lblpaymentmethod" runat="server" Visible="false" Text='<%# Eval("PaymentMethod") %>'></asp:Label>
                                                                                          <br />
                                                                                        <asp:Image ID="imgnaverror" runat="server" ToolTip='<%# Eval("NAVError") %>' AlternateText='<%# Eval("NAVError") %>' ImageUrl="~/ADMIN/Images/naverrorimg.gif" Visible="false" />
                                                                                    
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                                </asp:TemplateField>
                                                                                 <asp:TemplateField SortExpression="Upload" HeaderText="Upload">
                                                                                    <HeaderTemplate>
                                                                                        Print
                                                                                    </HeaderTemplate>
                                                                                    <ItemTemplate>
                                                                                        <table cellspacing="0" cellpadding="0" border="0" width="100%" class="order-date"
                                                                                           id="chkprintall" runat="server"   align="center">
                                                                                            <tbody>
                                                                                                <tr>
                                                                                                    <td colspan="2" class="border-none" align="center">
                                                                                                        <span><a style="color: #B92127; font-size: 12px; font-weight: normal;" id="lkbAllowAllPrint"
                                                                                                            href="javascript:selectAllPrint(true);">Check All</a></span>
                                                                                                    </td>
                                                                                                </tr>
                                                                                                <tr>
                                                                                                    <td colspan="2" class="border-none" align="center">
                                                                                                        <span><a style="color: #B92127; font-size: 12px; font-weight: normal;" id="lkbClearAllPrint"
                                                                                                            href="javascript:selectAllPrint(false);">Clear All</a> </span>
                                                                                                    </td>
                                                                                                </tr>
                                                                                            </tbody>
                                                                                        </table>
                                                                                        <asp:CheckBox ID="chkprintsalesorder" runat="server" Text="&nbsp;Print" TextAlign="Right" />
                                                                                        
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField SortExpression="OrderDate" HeaderText="Order Date">
                                                                                    <HeaderTemplate>
                                                                                        Order Date
                                                                                        <asp:ImageButton ID="blImage" runat="server" OnClientClick="chkHeight();" ImageUrl="/App_Themes/<%=Page.Theme %>/icon/order-date.png"
                                                                                            CommandArgument="ASC" CommandName="OrderDate" OnClick="Sorting" />
                                                                                    </HeaderTemplate>
                                                                                    <ItemTemplate>
                                                                                        <asp:Literal ID="ltr1" runat="server" Text='<%#String.Format("{0:dd&nbsp;MMM,&nbsp;yyyy&nbsp;hh:mm:ss&nbsp;ttt}",Convert.ToDateTime(Eval("OrderDate")))%>'></asp:Literal>
                                                                                        <asp:Label ID="lblOrderNumber" runat="server" Text='<%# Eval("OrderNumber") %>'></asp:Label>
                                                                                         <br />
                                                                                                        <asp:Literal ID="ltrprime" runat="server" Text='<%# Eval("AmazonPrime") %>'></asp:Literal>
                                                                                        <table cellspacing="0" cellpadding="0" border="0" width="100%" class="order-date"
                                                                                            id="tblDate" runat="server" visible="false">
                                                                                            <tbody>
                                                                                                <tr>
                                                                                                    <td width="40" valign="top" class="border-none">
                                                                                                        From:
                                                                                                    </td>
                                                                                                    <td width="66" valign="top" class="border-none">
                                                                                                        <asp:TextBox ID="txtOrderFrom" runat="server" CssClass="from-textfield" Width="85px"></asp:TextBox>
                                                                                                    </td>
                                                                                                </tr>
                                                                                                <tr>
                                                                                                    <td width="40" valign="top" class="border-none">
                                                                                                        To:
                                                                                                    </td>
                                                                                                    <td width="66" valign="top" class="border-none">
                                                                                                        <asp:TextBox ID="txtOrderTo" runat="server" CssClass="from-textfield" Width="85px"></asp:TextBox>
                                                                                                       
                                                                                                    </td>
                                                                                                </tr>
                                                                                                <tr>
                                                                                                    <td width="66" colspan="2" align="right" valign="top" class="border-none"><asp:DropDownList ID="ddlstockstat" runat="server" Width="71px" CssClass="order-list">
                                                                                                    <asp:ListItem Value="0">All</asp:ListItem>
                                                                                                    <asp:ListItem Value="1">Pending</asp:ListItem>
                                                                                                    <asp:ListItem Value="2">Ready to Import</asp:ListItem>
                                                                                                    <asp:ListItem Value="3">Imported</asp:ListItem>
                                                                                                    </asp:DropDownList>&nbsp;
                                                                                                        <asp:Button ID="btnSearchDate" runat="server" ToolTip="Search" OnClientClick="javascript:return CheckSearch();"
                                                                                                            Style="background: url(/App_Themes/<%= Page.Theme.ToString()%>/images/search.gif) no-repeat transparent;
                                                                                                            width: 67px; height: 23px; border: none; cursor: pointer;" CommandName="Edit"
                                                                                                            Visible="false" />
                                                                                                    </td>
                                                                                                </tr>
                                                                                            </tbody>
                                                                                        </table>
                                                                                    </ItemTemplate>
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="Order No. / Product Details">
                                                                                    <ItemTemplate>
                                                                                        <table cellpadding="0" cellspacing="0" width="100%" border="0" id="tblordernodetails"
                                                                                            runat="server">
                                                                                            <tr>
                                                                                                <td align="left" valign="top" style="border: none;">
                                                                                                     <div style="float:left;padding-right:2px;"><img src="/Admin/images/alert-order.png" border="0" id="imgorderalert" runat="server" visible="false" /></div><div> <asp:Label ID="lblMultiCapOrderNnumber" runat="server" Visible="false" Text='<%# Eval("OrderNumber")%>'></asp:Label><asp:Literal ID="ltr2" runat="server" Text='<%# Eval("OrderNumber")%>'></asp:Literal>
                                                                                                </div>
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr>
                                                                                                <td align="left" valign="top" style="border: none;">
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>
                                                                                        <asp:TextBox ID="txtOrderNo" runat="server" CssClass="order-textfield" Visible="false"></asp:TextBox><br /><br /><asp:CheckBox ID="chkgrndshipp" runat="server" Text=" Order is anything other than ground" Visible="false" />&nbsp;<asp:Button
                                                                                            ID="btnSearchOrdNumber" runat="server" ToolTip="Search" OnClientClick="javascript:return CheckSearch();"
                                                                                            Style="background: url(/App_Themes/<%= Page.Theme.ToString()%>/images/search.gif) no-repeat transparent;
                                                                                            width: 67px; height: 23px; border: none; cursor: pointer;" CommandName="Edit"
                                                                                            Visible="false" />
                                                                                    </ItemTemplate>
                                                                                    <HeaderStyle Width="22%" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="Ordered from (Store)">
                                                                                    <ItemTemplate>
                                                                                        <asp:Literal ID="ltr3" runat="server" Text='<%# Eval("StoreName")%>'></asp:Literal>
                                                                                        <table cellpadding="0" cellspacing="0" width="100%" border="0" id="tblorderStatus"
                                                                                            runat="server">
                                                                                            <tr>
                                                                                                <td align="left" valign="top" style="border: none;">
                                                                                                    <asp:DropDownList ID="ddlStore" runat="server" AutoPostBack="false" Visible="false"
                                                                                                        Width="191px" CssClass="order-list">
                                                                                                    </asp:DropDownList>
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr>
                                                                                                <td align="right" valign="top" style="border: none;">
                                                                                                    <asp:Button ID="btnSearchStore" runat="server" ToolTip="Search" OnClientClick="javascript:return CheckSearch();"
                                                                                                        Style="background: url(/App_Themes/<%= Page.Theme.ToString()%>/images/search.gif) no-repeat transparent;
                                                                                                        width: 67px; height: 23px; border: medium none; cursor: pointer; margin-top: 5px;
                                                                                                        float: right;" CommandName="Edit" Visible="false" />
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>
                                                                                    </ItemTemplate>
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="Customer Name / ZipCode">
                                                                                    <ItemTemplate>
                                                                                        <asp:Literal ID="ltr4" runat="server" Text='<%# "<a href=\"/Admin/Customers/Customer.aspx?mode=edit&CustID="+ Eval("CustomerID") +"\" title=\"\" class=\"order-no\">"+ Eval("CustName") +"</a>" %>'></asp:Literal>
                                                                                        <input type="hidden" id="hdnshoppingcartid" runat="server" value='<%# Eval("ShoppingCardID")%>' />
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
                                                                                        <input type="hidden" id="hdnreforder" runat="server" value='<%# Eval("RefOrderId")%>' />
                                                                                        <input type="hidden" id="hdnIsPhoneOrder" runat="server" value='<%# Eval("IsPhoneOrder")%>' />
                                                                                        <input type="hidden" id="hdncustomername" runat="server" value='<%# Eval("CustName")%>' />
                                                                                        <asp:TextBox ID="txtCustomername" runat="server" CssClass="order-textfield" Width="234px"
                                                                                            Visible="false"></asp:TextBox>
                                                                                        <asp:Button ID="btnSearchCust" runat="server" ToolTip="Search" OnClientClick="javascript:return CheckSearch();"
                                                                                            Style="background: url(/App_Themes/<%= Page.Theme.ToString()%>/images/search.gif) no-repeat transparent;
                                                                                            width: 67px; height: 23px; border: none; cursor: pointer; float: right; margin-top: 5px;"
                                                                                            CommandName="Edit" Visible="false" />
                                                                                    </ItemTemplate>
                                                                                    <FooterTemplate>
                                                                                        <asp:Literal ID="ltrtotalName" runat="server"></asp:Literal>
                                                                                    </FooterTemplate>
                                                                                    <FooterStyle HorizontalAlign="Right" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="Order Total">
                                                                                    <ItemTemplate>
                                                                                        <asp:Literal ID="ltr5" runat="server" Text='<%#String.Format("{0:C}",Convert.ToDecimal(Eval("OrderListTotal")))%>'></asp:Literal>
                                                                                        <table cellspacing="0" cellpadding="0" border="0" width="100%" class="order-date"
                                                                                            id="tblordertotal" runat="server" visible="false">
                                                                                            <tbody>
                                                                                                <tr>
                                                                                                    <td width="40" class="border-none">
                                                                                                        From:
                                                                                                    </td>
                                                                                                    <td width="66" class="border-none">
                                                                                                        <asp:TextBox ID="txtOrderTotalFrom" runat="server" CssClass="from-textfield" onkeypress="return isNumberKey(event);"
                                                                                                            Width="70px"></asp:TextBox>
                                                                                                    </td>
                                                                                                </tr>
                                                                                                <tr>
                                                                                                    <td width="40" class="border-none">
                                                                                                        To:
                                                                                                    </td>
                                                                                                    <td width="66" class="border-none">
                                                                                                        <asp:TextBox ID="txtOrderTotalTo" runat="server" CssClass="from-textfield" onkeypress="return isNumberKey(event);"
                                                                                                            Width="70px"></asp:TextBox>
                                                                                                    </td>
                                                                                                </tr>
                                                                                                <tr>
                                                                                                    <td width="66" colspan="2" align="right" class="border-none">
                                                                                                        <asp:Button ID="btnSearchOrdtotal" runat="server" ToolTip="Search" OnClientClick="javascript:return CheckSearch();"
                                                                                                            Style="background: url(/App_Themes/<%= Page.Theme.ToString()%>/images/search.gif) no-repeat transparent;
                                                                                                            width: 67px; height: 23px; border: none; cursor: pointer;" CommandName="Edit"
                                                                                                            Visible="false" />
                                                                                                    </td>
                                                                                                </tr>
                                                                                            </tbody>
                                                                                        </table>
                                                                                    </ItemTemplate>
                                                                                    <FooterTemplate>
                                                                                        <asp:Literal ID="ltrSummary" runat="server"></asp:Literal>
                                                                                    </FooterTemplate>
                                                                                    <FooterStyle HorizontalAlign="Right" />
                                                                                    <ItemStyle HorizontalAlign="Right" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="Status">
                                                                                    <ItemTemplate>
                                                                                        <asp:Literal ID="ltr6" runat="server" Text='<%# Eval("TransactionStatus")%>'></asp:Literal>
<asp:Label ID="lblIsMailSent" runat="server" Font-Size="10px" ForeColor="Black"></asp:Label>
  <input type="hidden" id="hdnIsMailSent" runat="server" value='<%# Eval("IsMailSent")%>' />
                                                                                        <input type="hidden" id="hdnCaptureTXResult" runat="server" value='<%# Eval("CaptureTXResult")%>' />
                                                                                        <input type="hidden" id="hdnorderStatus" runat="server" value='<%# Eval("orderStatus")%>' />
                                                                                        <input type="hidden" id="hdnSubtotal" runat="server" value='<%# Eval("OrderSubtotal")%>' />
                                                                                        <input type="hidden" id="hdnTotal" runat="server" value='<%# Eval("FullOrderTotal")%>' />
                                                                                        <input type="hidden" id="HdnShippingCost" runat="server" value='<%# Eval("OrderShippingCosts")%>' />
                                                                                        <input type="hidden" id="hdnordertax" runat="server" value='<%# Eval("OrderTax")%>' />
                                                                                        <input type="hidden" id="hdnDiscount" runat="server" value='<%# Eval("CustomDiscount")%>' />
                                                                                        <input type="hidden" id="hdnRefund" runat="server" value='<%# Eval("RefundAmount")%>' />
                                                                                        <input type="hidden" id="hdnAdjAmt" runat="server" value='<%# Eval("AdjustmentAmount")%>' />
                                                                                        <input type="hidden" id="hdnSubtotalF" runat="server" value='<%# Eval("OrderSubtotalF")%>' />
                                                                                        <input type="hidden" id="HdnShippingCostF" runat="server" value='<%# Eval("OrderShippingCostsF")%>' />
                                                                                        <input type="hidden" id="hdnordertaxF" runat="server" value='<%# Eval("OrderTaxF")%>' />
                                                                                        <input type="hidden" id="hdnDiscountF" runat="server" value='<%# Eval("CustomDiscountF")%>' />
                                                                                        <input type="hidden" id="hdnRefundF" runat="server" value='<%# Eval("RefundAmountF")%>' />
                                                                                        <input type="hidden" id="hdnAdjAmtF" runat="server" value='<%# Eval("AdjustmentAmountF")%>' />
                                                                                        <input type="hidden" id="hdnlvelDiscountF" runat="server" value='<%# Eval("LevelDiscountAmountF")%>' />
                                                                                        <input type="hidden" id="hdncoponDiscountF" runat="server" value='<%# Eval("CouponDiscountAmountF")%>' />
                                                                                        <input type="hidden" id="hdnGiftCardDiscount" runat="server" value='<%# Eval("GiftCertificateDiscountAmount")%>' />
                                                                                        <input type="hidden" id="hdnQtyDiscountAmountF" runat="server" value='<%# Eval("QuantityDiscountAmountF")%>' />
                                                                                          <input type="hidden" id="hdnTrackinguplaoded" runat="server" value='<%# Eval("IsBackendProcessed")%>' />
                                                                                        
                                                                                        <asp:DropDownList ID="ddlStatus" runat="server" AutoPostBack="false" Visible="false"
                                                                                            CssClass="status-list">
                                                                                            <asp:ListItem Text="All Types" Value="All" Selected="True"></asp:ListItem>
                                                                                            <asp:ListItem Text="New" Value="New"></asp:ListItem>
                                                                                            <asp:ListItem Text="Shipped" Value="Shipped"></asp:ListItem>
                                                                                            <%--<asp:ListItem Text="Declined" Value="Declined"></asp:ListItem>--%>
                                                                                            <asp:ListItem Text="Pending" Value="Pending"></asp:ListItem>
                                                                                            <asp:ListItem Text="Hold Order" Value="Hold Order"></asp:ListItem>
                                                                                            <asp:ListItem Text="Canceled" Value="Canceled"></asp:ListItem>
                                                                                            <asp:ListItem Text="Tracking # uploaded to partner portal" Value="TrackingUploaded"></asp:ListItem>
                                                                                        </asp:DropDownList>
                                                                                        <asp:Button ID="btnSearchOrdStatus" runat="server" ToolTip="Search" OnClientClick="javascript:return CheckSearch();"
                                                                                            Style="background: url(/App_Themes/<%= Page.Theme.ToString()%>/images/search.gif) no-repeat transparent;
                                                                                            width: 67px; height: 23px; border: none; cursor: pointer;" CommandName="Edit"
                                                                                            Visible="false" />
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="Action">
                                                                                    <ItemTemplate>
                                                                                        <asp:Literal ID="ltr7" runat="server" Text=''></asp:Literal>
                                                                                        <asp:Button ID="btnSearch" runat="server" ToolTip="Search" OnClientClick="javascript:return CheckSearch();"
                                                                                            Style="background: url(/App_Themes/<%= Page.Theme.ToString()%>/images/search.gif) no-repeat transparent;
                                                                                            width: 67px; height: 23px; border: none; cursor: pointer; display: none;" CommandName="Edit"
                                                                                            Visible="false" />
                                                                                        <br />
                                                                                        <br />
                                                                                        <asp:Button ID="btnResetfilter" runat="server" ToolTip="Reset Filter" OnClientClick="javascript:chkHeight();"
                                                                                            Style="background: url(/App_Themes/<%= Page.Theme.ToString()%>/images/reser-filter.gif) no-repeat transparent;
                                                                                            width: 87px; height: 23px; border: none; cursor: pointer;" OnClick="btnResetfilter_Click"
                                                                                            Visible="false" />
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle HorizontalAlign="left" />
                                                                                </asp:TemplateField>
                                                                            </Columns>
                                                                            <AlternatingRowStyle CssClass="even-row" VerticalAlign="top" />
                                                                            <RowStyle CssClass="odd-row" VerticalAlign="top" />
                                                                            <HeaderStyle HorizontalAlign="Center" />
                                                                            <FooterStyle BackColor="#E1E1E1" ForeColor="#000" Font-Bold="true" HorizontalAlign="Right"
                                                                                BorderWidth="0" />
                                                                        </asp:GridView>
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
                <tr>
                    <td height="10" align="left" valign="top">
                        <img height="10" width="1" alt="" src="/App_Themes/<%=Page.Theme %>/images/spacer.gif" />
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                </tr>
            </tbody>
        </table>
    </div>
    <div style="display: none">
        <asp:Button ID="objpageclicknew" runat="server" OnClick="objLb_Click" />
        <asp:Button ID="objpageclickpre" runat="server" OnClick="objLbPre_Click" />
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
    <div style="display: none">
        <input type="button" id="btnreadmore" />
    </div>
    <div id="popupContact" style="z-index: 1000001; width: 750px; height: 260px;">
        <table width="100%" border="0" cellspacing="0" cellpadding="3" style="border: 1px solid #444;">
            <tr style="background-color: #444; height: 25px;">
                <td valign="top" align="left" style="color: #fff; font-family: Arial,Helvetica,sans-serif;
                    padding-top: 10px; font-weight: bold;">
                    &nbsp;Advance Search
                </td>
                <td align="right" valign="top">
                    <asp:ImageButton ID="popupContactClose" Style="position: relative;" ImageUrl="~/images/cancel.png"
                        runat="server" ToolTip="Close" OnClientClick="return false;"></asp:ImageButton>
                </td>
            </tr>
            <tr>
                <td colspan="2" valign="top">
                    <table width="100%" border="0" style="padding-top: 20px" cellpadding="5" cellspacing="0">
                        <tr class="oddrow">
                            <td>
                                Ref. Order No :
                            </td>
                            <td>
                                <asp:TextBox runat="server" EnableViewState="false" ID="txtRefOrderNo" onkeypress="return isEnter(event);"
                                    CssClass="order-textfield" MaxLength="50"></asp:TextBox>
                            </td>
                            <td>
                                P.O. Number :
                            </td>
                            <td>
                                <asp:TextBox runat="server" EnableViewState="false" ID="txtpo" onkeypress="return isEnter(event);"
                                    CssClass="order-textfield" MaxLength="100"></asp:TextBox>
                            </td>
                        </tr>
                        <tr class="altrow">
                            <td>
                                Customer E-Mail :
                            </td>
                            <td>
                                <asp:TextBox runat="server" EnableViewState="false" ID="txtEmail" onkeypress="return isEnter(event);"
                                    CssClass="order-textfield" MaxLength="100"></asp:TextBox>
                            </td>
                            <td>
                                Payment Method :
                            </td>
                            <td align="left">
                                <asp:DropDownList ID="ddlpayment" runat="server" CssClass="order-list" Width="224">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr class="oddrow">
                            <td>
                                Zip Code :
                            </td>
                            <td>
                                <asp:TextBox runat="server" EnableViewState="false" onkeypress="return isEnter(event);"
                                    ID="txtzipcode" CssClass="order-textfield" MaxLength="15"></asp:TextBox>
                            </td>
                            <td>
                                Transaction Status :
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlTransactionStatus" runat="server" CssClass="order-list"
                                    Width="224">
                                    <asp:ListItem Text="All" Value="0"></asp:ListItem>
                                    <asp:ListItem Text="AUTHORIZED" Value="AUTHORIZED"></asp:ListItem>
                                    <asp:ListItem Text="CAPTURED" Value="CAPTURED"></asp:ListItem>
                                    <%-- <asp:ListItem Text="VOIDED" Value="VOIDED"></asp:ListItem>--%>
                                    <asp:ListItem Text="REFUNDED" Value="REFUNDED"></asp:ListItem>
                                    <asp:ListItem Text="PARTIALLY REFUNDED" Value="PARTIALLY REFUNDED"></asp:ListItem>
                                    <asp:ListItem Text="CANCELED" Value="CANCELED"></asp:ListItem>
                                    <asp:ListItem Text="PENDING" Value="PENDING"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr class="altrow">
                            <td>
                                Company Name :
                            </td>
                            <td>
                                <asp:TextBox runat="server" EnableViewState="false" onkeypress="return isEnter(event);"
                                    ID="txtcompanyname" CssClass="order-textfield" MaxLength="100"></asp:TextBox>
                            </td>
                            <td>
                                Ship to State :
                            </td>
                            <td>
                                <asp:DropDownList ID="dlState" runat="server" CssClass="order-list" Width="224">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr class="oddrow">
                            <td>
                                Coupon Code :
                            </td>
                            <td>
                                <asp:TextBox runat="server" EnableViewState="false" onkeypress="return isEnter(event);"
                                    ID="txtcoupncode" CssClass="order-textfield" MaxLength="15"></asp:TextBox>
                            </td>
                            <td>
                                Sales Agent :
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlsalesagents" runat="server" CssClass="order-list" Width="224">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr class="altrow">
                            <td colspan="4" align="center">
                                <asp:ImageButton ID="btnGo" OnClick="btnGo_Click" runat="server" ImageUrl="/App_Themes/<%=Page.Theme %>/images/search.gif" />&nbsp;<asp:ImageButton
                                    ID="btnReset" OnClientClick="return resetFilterSearch();" runat="server" ImageUrl="/App_Themes/<%=Page.Theme %>/images/reser-filter.gif" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>
    <div id="backgroundPopup" style="z-index: 1000000;">
    </div>
    <div style="display: none;">
        <asp:Button ID="btnCapture" runat="server" OnClick="btnCapture_Click" />
        <asp:Button ID="btnRefund" runat="server" OnClick="btnRefund_Click" />
        <input type="hidden" id="hdnordernumber" runat="server" value="0" />
        <input type="hidden" id="hdnAmount" runat="server" value="0" />
        <input type="hidden" id="hdnrefundAmount" runat="server" value="0" />
        <asp:Button ID="btnMultipleCapture" runat="server" OnClick="btnMultipleCapture_Click" />
    </div>
      <div style="visibility: hidden;">
        <iframe id="ifmcontentstoprint" style="height: 0px; width: 0px; position: absolute">
   </iframe>
    </div>
</asp:Content>
