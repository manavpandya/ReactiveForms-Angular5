<%@ Page Title="" Language="C#" MasterPageFile="~/ADMIN/Admin.Master" AutoEventWireup="true"
    CodeBehind="ViewOrderReport.aspx.cs" Inherits="Solution.UI.Web.ADMIN.Orders.ViewOrderReport" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts-main.js"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-calender-custom.js"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-calender.js"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts.js"></script>
    <style>
        .pageccc td {
            border-style: none !important;
        }
    </style>
    <script type="text/javascript">

        $(function () {

            $('#ContentPlaceHolder1_txtFromdate').datetimepicker({
                showButtonPanel: true, ampm: false,
                showHour: false, showMinute: false, showSecond: false, showTime: false, showOn: "button",
                buttonImage: "/App_Themes/<%=Page.Theme %>/images/date-icon.png", buttonImageOnly: true
            });
            $('#ContentPlaceHolder1_txtTodate').datetimepicker({
                showButtonPanel: true, ampm: false,
                showHour: false, showMinute: false, showSecond: false, showTime: false, showOn: "button",
                buttonImage: "/App_Themes/<%=Page.Theme %>/images/date-icon.png", buttonImageOnly: true
            });
        });

        function chkSelect() {
            var allElts = document.getElementById('divGrid').getElementsByTagName('INPUT');
            var i;
            var Chktrue;
            Chktrue = 0;
            for (i = 0; i < allElts.length; i++) {
                var elt = allElts[i];
                if (elt.type == "checkbox") {
                    if (elt.checked == true) {
                        Chktrue = Chktrue + 1;

                    }
                }
            }
            if (Chktrue < 1) {
                jAlert("Please select at least one Order.", "Message");
                return false;
            }
        }

        function SearchValidation() {
            if (document.getElementById('ContentPlaceHolder1_txtFromdate').value == '') {
                jAlert('Please Enter From Date.', 'Required Information', 'ContentPlaceHolder1_txtFromdate');
                //document.getElementById('ContentPlaceHolder1_txtMailFrom').focus();
                return false;
            }
            else if (document.getElementById('ContentPlaceHolder1_txtTodate').value == '') {
                jAlert('Please Enter To Date.', 'Required Information', 'ContentPlaceHolder1_txtTodate');
                return false;
            }

            var startDate = new Date(document.getElementById('ContentPlaceHolder1_txtMFromdate').value);
            var endDate = new Date(document.getElementById('ContentPlaceHolder1_txtTodate').value);
            if (startDate > endDate) {
                jAlert('Please Select Valid Date.', 'Required Information', 'ContentPlaceHolder1_txtTodate');
                return false;
            }
            return true;
        }


        function OpenCenterWindow(pid, wi, he) {
            var w = wi;
            var h = he;
            var left = (screen.width / 2) - (w / 2);
            var top = (screen.height / 2) - (h / 2);
            window.open(pid, '', 'toolbar=no, location=no, directories=no, status=no, menubar=no, scrollbars=yes, resizable=no, copyhistory=no, width=' + w + ', height=' + h + ', top=' + top + ', left=' + left);
        }

        function SelectAll(on) {
            var allElts = document.getElementById('divGrid').getElementsByTagName('INPUT');
            var i;
            for (i = 0; i < allElts.length; i++) {
                var elt = allElts[i];
                if (elt.type == "checkbox") {
                    elt.checked = on;
                }
            }
        }
    </script>
    <div id="content-width">
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
                                            <th colspan="3">
                                                <div class="main-title-left">
                                                    <img class="img-left" title="Bulk Order Print" alt="Bulk Order Print" src="/App_Themes/<%=Page.Theme %>/Images/mail-log-icon.png" />
                                                    <h2>Order Export to Back End</h2>
                                                </div>
                                            </th>
                                        </tr>
                                        <tr class="altrow">
                                            <td align="center" colspan="3">
                                                <table cellpadding="3" cellspacing="3">
                                                    <tr>
                                                        <td valign="middle" align="right">From Date:
                                                        </td>
                                                        <td valign="middle" align="right">
                                                            <asp:TextBox ID="txtFromdate" runat="server" CssClass="from-textfield" Width="70px"
                                                                Style="margin-right: 3px;"></asp:TextBox>
                                                        </td>
                                                        <td valign="middle" align="right">To Date:
                                                        </td>
                                                        <td valign="middle" align="right">
                                                            <asp:TextBox ID="txtTodate" runat="server" CssClass="from-textfield" Width="70px"
                                                                Style="margin-right: 3px;"></asp:TextBox>
                                                        </td>
                                                        <td align="right">Select Year/Month:
                                                            <asp:DropDownList ID="ddlsearchBy" runat="server" CssClass="order-list" Width="100px"
                                                                AutoPostBack="false" Style="margin-top: 5px;">
                                                                <asp:ListItem Value="">All</asp:ListItem>
                                                                <asp:ListItem Value="Month">Month</asp:ListItem>
                                                                <asp:ListItem Value="Year">Year</asp:ListItem>
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td align="right">Search Like:
                                                            <asp:DropDownList ID="ddlSearchLike" runat="server" CssClass="order-list" Width="100px"
                                                                AutoPostBack="false" Style="margin-top: 5px;">
                                                                <asp:ListItem Value="Starts with">Starts with</asp:ListItem>
                                                                <asp:ListItem Value="Contains">Contains</asp:ListItem>
                                                                <asp:ListItem Value="Exact">Exact</asp:ListItem>
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td align="right">Order/Quantity:
                                                            <asp:DropDownList ID="ddlOption" runat="server" CssClass="order-list" Width="100px"
                                                                AutoPostBack="false" Style="margin-top: 5px;">
                                                                <asp:ListItem Value="1">Quantity</asp:ListItem>
                                                                <asp:ListItem Value="2">Orders</asp:ListItem>
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td align="right">Child/Parent:
                                                            <asp:DropDownList ID="ddlParent" runat="server" CssClass="order-list" Width="100px"
                                                                AutoPostBack="false" Style="margin-top: 5px;">
                                                                <asp:ListItem Value="0">Parent</asp:ListItem>
                                                                <asp:ListItem Value="1">Child</asp:ListItem>
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td align="right">
                                                            <asp:TextBox ID="txtSearchText" runat="server" CssClass="order-list" Width="200px"></asp:TextBox>
                                                        </td>
                                                        <td align="right">
                                                            <asp:Button ID="btnSearch" runat="server" OnClientClick="return SearchValidation();"
                                                                OnClick="btnSearch_Click" />
                                                        </td>
                                                        <td align="right"></td>
                                                        <td align="right" style="display: none;"></td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" colspan="3">
                                                <asp:Literal ID="ltReport" runat="server"></asp:Literal>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </div>
    </div>
    <div id="prepage" style="position: absolute; font-family: arial; font-size: 16px; left: 0px; top: 0px; background-color: #000; opacity: 0.7; filter: alpha(opacity=70); layer-background-color: white; height: 100%; width: 100%; z-index: 1000; display: none;">
        <table width="100%" style="padding-top: 200px;" align="center">
            <tr>
                <td align="center" style="color: #fff;" valign="middle">
                    <img alt="" src="/App_Themes/<%=Page.Theme %>/images/loding.png" /><br />
                    <b>Loading ... ... Please wait!</b>
                </td>
            </tr>
        </table>
    </div>
    <script type="text/javascript">
        function funHideShow(yval) {
            $('table[id^="tb_"]').hide();
            $('table[id^="tb_' + yval + '"]').show();
            $('table[id^="tb_' + yval + '"]').removeAttr('style');
            $('table[id^="tb_' + yval + '"]').attr('style','width:100%;border-color:#DFDFDF;border-width:1px;border-style:Solid;width:100%;margin-bottom:0 !important;');
            
        }
    </script>
</asp:Content>
