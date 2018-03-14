<%@ Page Title="" Language="C#" MasterPageFile="~/ADMIN/Admin.Master" AutoEventWireup="true"
    CodeBehind="FiledTransactionSummary.aspx.cs" Inherits="Solution.UI.Web.ADMIN.Orders.FiledTransactionSummary" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts-main.js"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-calender-custom.js"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-calender.js"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts.js"></script>
    <style type="text/css">
        .pop_border_new2
        {
            font-family: Arial, Helvetica, sans-serif;
            font-size: 12px;
            color: #2A2A2A;
            text-decoration: none;
            line-height: 25px;
        }
        .pop_border_new2 th
        {
            background: #e7e7e7;
            font-family: Arial, Helvetica, sans-serif;
            font-size: 12px;
            color: #fff;
            text-decoration: none;
            line-height: 25px;
        }
        .pop_border_new2 td
        {
            border: 1px solid #CFCFCF;
            padding: 2px 5px;
            font-family: Arial, Helvetica, sans-serif;
            font-size: 12px;
            color: #2A2A2A;
            text-decoration: none;
            line-height: 25px;
        }
    </style>
    <script type="text/javascript">
        $(function () {
            $('#ContentPlaceHolder1_txtFromDate').datetimepicker({
                showButtonPanel: true, ampm: false,
                showHour: false, showMinute: false, showSecond: false, showTime: false
            });
            $('#ContentPlaceHolder1_txtToDate').datetimepicker({
                showButtonPanel: true, ampm: false,
                showHour: false, showMinute: false, showSecond: false, showTime: false
            });
        });

        function ValidDate() {
            if (document.getElementById('ContentPlaceHolder1_txtFromDate') != null && document.getElementById('ContentPlaceHolder1_txtFromDate').value.replace(/^\s+|\s+$/g, "") == '') {
                jAlert('Please Enter From Date.', 'Message', 'ContentPlaceHolder1_txtFromDate');
                return false;
            }
            //            if (document.getElementById('txtToDate') != null && document.getElementById('txtToDate').value.replace(/^\s+|\s+$/g, "") == '') {
            //                jAlert('Please Enter To Date.', 'Message', 'txtToDate');
            //                return false;
            //            }
            return true;
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
        <div class="create-new-order" style="width: 100%">
            <span style="vertical-align: middle; margin-right: 3px; float: left;">
                <table style="margin-top: 5px; float: left">
                    <tr>
                        <td>
                            Store :
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlstore" runat="server" CssClass="order-list" Width="170px"
                                AutoPostBack="true" OnSelectedIndexChanged="ddlstore_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                    </tr>
                </table>
            </span>
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
                                    <tr>
                                        <th>
                                            <div class="main-title-left">
                                                <img class="img-left" title="Failed Transaction Summary" alt="Failed Transaction Summary"
                                                    src="/App_Themes/<%=Page.Theme %>/Images/FailedTran.png" />
                                                <h2>
                                                    Failed Transaction Summary
                                                </h2>
                                            </div>
                                        </th>
                                    </tr>
                                    <tr id="datetr" runat="server">
                                        <td>
                                            <table cellpadding="0" cellspacing="0" border="0" width="40%">
                                                <tr>
                                                    <td align="left" style="width:12%;">
                                                        From Date :
                                                    </td>
                                                    <td align="left" width="15%">
                                                        <asp:TextBox runat="server" ID="txtFromDate" CssClass="order-textfield" 
                                                            Width="80px"></asp:TextBox>
                                                    </td>
                                                    <td align="center" width="10%">
                                                        To Date :
                                                    </td>
                                                    <td align="left" width="15%">
                                                        <asp:TextBox runat="server" ID="txtToDate" CssClass="order-textfield" 
                                                            Width="80px"></asp:TextBox>
                                                    </td>
                                                    <td align="left">
                                                        <asp:ImageButton ID="btnSubmit" runat="server" OnClientClick="return ValidDate();"
                                                            OnClick="btnSubmit_Click" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="border-td" align="center" style="padding:5px;">
                                            <table cellpadding="0" cellspacing="0" border="0" width="99%" class="content-table border-td">
                                                <tr>
                                                    <th align="center" width="10%" style="color: #fff">
                                                        Total Order(s)
                                                    </th>
                                                    <th align="left" style="color: #fff">
                                                        &nbsp;Order#
                                                    </th>
                                                    <th align="right" width="10%" style="color: #fff">
                                                        Order Total&nbsp;
                                                    </th>
                                                </tr>
                                                <tr>
                                                    <td align="center" style="border-right: 1px solid #e7e7e7;">
                                                        <asp:Label ID="lblOrderCount" runat="server"></asp:Label>
                                                    </td>
                                                    <td align="left" style="border-right: 1px solid #e7e7e7;">
                                                        <asp:Literal ID="ltrOrderNumber" runat="server"></asp:Literal>
                                                    </td>
                                                    <td align="right">
                                                        <asp:Label ID="lblOrderTotal" runat="server"></asp:Label>
                                                    </td>
                                                </tr>
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
                <td height="10" align="left" valign="top">
                    <img alt="" src="/App_Themes/<%=Page.Theme %>/images/spacer.gif" width="1" height="10" />
                </td>
            </tr>
            <tr>
                <td>
                    &nbsp;
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
