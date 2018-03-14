<%@ Page Title="" Language="C#" MasterPageFile="~/ADMIN/Admin.Master" AutoEventWireup="true" CodeBehind="Reportyearcamparision.aspx.cs" Inherits="Solution.UI.Web.ADMIN.Reports.Reportyearcamparision" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
      <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts-main.js"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-calender-custom.js"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-calender.js"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts.js"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery.ui.accordion.js"></script>
    <%--<link href="http://ajax.googleapis.com/ajax/libs/jqueryui/1.8/themes/base/jquery-ui.css" rel="stylesheet" type="text/css"/>--%>
    <script type="text/javascript">
        $(document).ready(function () {
            $("#accordion").accordion();
        });

    </script>
    <script type="text/javascript">


        function chkHeight() {
            var windowHeight = 0;
            windowHeight = $(document).height(); //window.innerHeight;

            document.getElementById('prepage').style.height = windowHeight + 'px';
            document.getElementById('prepage').style.display = '';
        }
        function Tabdisplay(id) {
            document.getElementById('ContentPlaceHolder1_hdnTabid').value = id;
            for (var i = 1; i < 5; i++) {

                var divid = "divtab" + i.toString()
                var liid = "li" + i.toString()
                if (document.getElementById(divid) != null && ('divtab' + id == divid)) {
                    document.getElementById(divid).style.display = '';
                }
                else {
                    if (document.getElementById(divid) != null) {
                        document.getElementById(divid).style.display = 'none';
                    }
                }
                if (document.getElementById(liid) && ('li' + id == liid)) {
                    document.getElementById(liid).className = 'active';
                }
                else {
                    if (document.getElementById(liid) != null) {
                        document.getElementById(liid).className = '';
                    }
                }
            }

        }

        $(function () {

            $('#ContentPlaceHolder1_txtOrderFrom').datetimepicker({
                showButtonPanel: true, ampm: false,
                showHour: false, showMinute: false, showSecond: false, showTime: false, showOn: "button",
                buttonImage: "/App_Themes/<%=Page.Theme %>/images/date-icon.png", buttonImageOnly: true
            });
            $('#ContentPlaceHolder1_txtOrderTo').datetimepicker({
                showButtonPanel: true, ampm: false,
                showHour: false, showMinute: false, showSecond: false, showTime: false, showOn: "button",
                buttonImage: "/App_Themes/<%=Page.Theme %>/images/date-icon.png", buttonImageOnly: true
            });
        });
        function validation() {
            if (document.getElementById('ContentPlaceHolder1_RadOrderByDays') != null) {

                //                var radio = document.getElementById('rdoList').getElementsByTagName('input');

                //                for (var j = 0; j < radio.length; j++) {

                if (document.getElementById('ContentPlaceHolder1_RadOrderByDays').selectedIndex == 7) {
                    if (document.getElementById('ContentPlaceHolder1_txtOrderFrom').value == '') {
                        jAlert('Please Enter Start Date.', 'Required Information');
                        document.getElementById('ContentPlaceHolder1_txtOrderFrom').focus();
                        return false;
                    }
                    else if (document.getElementById('ContentPlaceHolder1_txtOrderTo').value == '') {
                        jAlert('Please Enter End Date.', 'Required Information');
                        document.getElementById('ContentPlaceHolder1_txtOrderTo').focus();
                        return false;
                    }

                    var startDate = new Date(document.getElementById('ContentPlaceHolder1_txtOrderFrom').value);
                    var endDate = new Date(document.getElementById('ContentPlaceHolder1_txtOrderTo').value);
                    if (startDate > endDate) {
                        jAlert('Please Select Valid Date.', 'Required Information');
                        return false;
                    }

                }

                //}
            }
            if (document.getElementById('prepage') != null) {
                document.getElementById('prepage').style.display = '';
            }
            return true;
        }


    </script>
    <script type="text/javascript">
        function OrderSubTabdisplay(id) {
            document.getElementById('ContentPlaceHolder1_hdnTabidOrders').value = id;
            for (var i = 1; i < 5; i++) {

                var divid = "divordertab" + i.toString()
                var liid = "liorder" + i.toString()
                if (document.getElementById(divid) != null && ('divordertab' + id == divid)) {
                    document.getElementById(divid).style.display = '';
                }
                else {
                    if (document.getElementById(divid) != null) {
                        document.getElementById(divid).style.display = 'none';
                    }
                }
                if (document.getElementById(liid) && ('liorder' + id == liid)) {
                    document.getElementById(liid).className = 'active';
                }
                else {
                    if (document.getElementById(liid) != null) {
                        document.getElementById(liid).className = '';
                    }
                }
            }

        }
        function OrderSubTabdisplayTax(id) {
            document.getElementById('ContentPlaceHolder1_hdnTabidOrders').value = id;
            for (var i = 1; i < 5; i++) {

                var divid = "divtaxtab" + i.toString()
                var liid = "litax" + i.toString()
                if (document.getElementById(divid) != null && ('divtaxtab' + id == divid)) {
                    document.getElementById(divid).style.display = '';
                }
                else {
                    if (document.getElementById(divid) != null) {
                        document.getElementById(divid).style.display = 'none';
                    }
                }
                if (document.getElementById(liid) && ('litax' + id == liid)) {
                    document.getElementById(liid).className = 'active';
                }
                else {
                    if (document.getElementById(liid) != null) {
                        document.getElementById(liid).className = '';
                    }
                }
            }

        }
        function OrderSubTabdisplayRefund(id) {
            document.getElementById('ContentPlaceHolder1_hdnTabidOrders').value = id;
            for (var i = 1; i < 5; i++) {

                var divid = "divrefundtab" + i.toString()
                var liid = "lirefund" + i.toString()
                if (document.getElementById(divid) != null && ('divrefundtab' + id == divid)) {
                    document.getElementById(divid).style.display = '';
                }
                else {
                    if (document.getElementById(divid) != null) {
                        document.getElementById(divid).style.display = 'none';
                    }
                }
                if (document.getElementById(liid) && ('lirefund' + id == liid)) {
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
     <div id="content-width">
        <div class="content-row1">
            <div class="create-new-order" style="width:100%; margin-top:2px;">
                <table cellpadding="3" cellspacing="3">
                    <tr>
                        <td align="left" style="padding:0px;">
                            Store :
                        </td>
                        <td align="left">
                            <asp:DropDownList ID="ddlStore" runat="server" CssClass="order-list" Width="175px"
                                AutoPostBack="false">
                            </asp:DropDownList>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
        <div class="content-row2">
            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                <tr>
                    <td height="5" align="left" valign="top">
                        <img src="/App_Themes/<%=Page.Theme %>/images/spacer.gif" alt="" width="1" height="5" />
                    </td>
                </tr>
                <tr>
                    <td height="5" align="left" valign="top">
                        <img src="/App_Themes/<%=Page.Theme %>/images/spacer.gif" alt="" width="1" height="5" />
                    </td>
                </tr>
                <tr>
                    <td class="border-td">
                        <table width="100%" border="0" cellpadding="0" cellspacing="0" bgcolor="#FFFFFF"
                            class="content-table">
                            <tr>
                                <td class="border-td-sub">
                                    <table width="100%" border="0" cellpadding="0" cellspacing="0" class="add-product">
                                        <tr>
                                            <th>
                                                <div class="main-title-left">
                                                    <img class="img-left" title="Order Statistics" alt="Order Statistics" src="/App_Themes/<%=Page.Theme %>/Images/sales-report-icon.png" />
                                                    <h2>
                                                        Order Comparison</h2>
                                                </div>
                                            </th>
                                        </tr>
                                       <%-- <tr>
                                            <td style="background-color: #E3E3E3; font-weight: bold;">
                                                Search By
                                            </td>
                                        </tr>--%>
                                        <tr class="even-row">
                                            <td>
                                                <table border="0">
                                                    <tr>
                                                        <td align="right">
                                                            Period :
                                                        </td>
                                                        <td align="left">
                                                            <div id="rdoList">
                                                                <asp:DropDownList ID="ddlPeriod" runat="server" AutoPostBack="false" CssClass="order-list"
                                                                    Width="60px">
                                                                    
                                                                    <asp:ListItem Text="Month" Value="Month" Selected="True"></asp:ListItem>
                                                                    <asp:ListItem Text="Year" Value="Year"></asp:ListItem>
                                                                </asp:DropDownList>
                                                            </div>
                                                        </td>
                                                         
                                                        <td valign="top" align="right">
                                                            Start Date:
                                                        </td>
                                                        <td valign="top" align="left">
                                                            <asp:TextBox ID="txtOrderFrom" runat="server" CssClass="from-textfield" Width="70px"
                                                                Style="margin-right: 3px;"></asp:TextBox>
                                                        </td>
                                                        <td valign="top" align="right">
                                                            End Date:
                                                        </td>
                                                        <td valign="top" align="left">
                                                            <asp:TextBox ID="txtOrderTo" runat="server" CssClass="from-textfield" Width="70px"
                                                                Style="margin-right: 3px;"></asp:TextBox>
                                                        </td>
                                                        <td>
                                                        </td>
                                                        <td align="left" colspan="3">
                                                            <asp:ImageButton ID="imgSearch" runat="server" AlternateText="GO" ToolTip="GO" OnClick="imgSearch_Click"
                                                                OnClientClick="return validation();" />
                                                        
                                                        </td>
                                                        <td>
                                                               
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td style="background-color: #E3E3E3; font-weight: bold; width: 99%">
                                    <div id="tab-container-1">
                                        <ul class="menu">
                                            <li class="active" id="li1" onclick="Tabdisplay(1);">ORDER</li>
                                          
                                        </ul>
                                        <div class="tab-content general-tab" id="divtab1">
                                            <table cellspacing="0" cellpadding="0" border="0" width="100%">
                                                <tr>
                                                    <td>
                                                        <ul class="menu">
                                                            <li id="liorder1" class="active" onclick="OrderSubTabdisplay(1);">Amount</li>
                                                            <li id="liorder2" onclick="OrderSubTabdisplay(2);">Orders</li>
                                                        </ul>
                                                        <div class="tab-content general-tab" id="divordertab1" style="display: block;">
                                                            <div class="tab-content-3" style="padding-left: 10%; width: 90%; border: none;">
                                                                <asp:Literal ID="ltrChartAmount" runat="server"></asp:Literal>
                                                            </div>
                                                        </div>
                                                        <div class="tab-content general-tab" id="divordertab2" style="display: none;">
                                                            <div class="tab-content-3" style="padding-left: 10%; width: 90%; border: none;">
                                                                <asp:Literal ID="ltrChartOrder" runat="server"></asp:Literal>
                                                            </div>
                                                        </div>
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
            </table>
        </div>
    </div>
    <div id="prepage" style="position: absolute; font-family: arial; font-size: 16px; left: 0px;
        top: 0px; background-color: #000; opacity: 0.7; filter: alpha(opacity=70); layer-background-color: white;
        height: 100%; width: 100%; z-index: 1000; display: none;">
        <table width="100%" style="padding-top: 200px;" align="center">
            <tr>
                <td align="center" style="color: #fff;" valign="middle">
                    <img alt="" src="/App_Themes/<%=Page.Theme %>/images/loding.png" /><br />
                    <b>Loading ... ... Please wait!</b>
                </td>
            </tr>
        </table>
    </div>
    <div style="display: none;">
        <input type="hidden" id="hdnTabid" runat="server" value="1" />
        <input type="hidden" id="hdnTabidOrders" runat="server" value="1" />
    </div>
</asp:Content>
