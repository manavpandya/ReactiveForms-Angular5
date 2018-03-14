<%@ Page Title="" Language="C#" MasterPageFile="~/ADMIN/Admin.Master" AutoEventWireup="true"
    CodeBehind="YahooReports.aspx.cs" Inherits="Solution.UI.Web.ADMIN.Reports.YahooReports" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts-main.js"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-calender-custom.js"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-calender.js"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts.js"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery.ui.accordion.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $("#accordion").accordion();
        });

    </script>
    <style type="text/css">
        .table-none
        {
            border-collapse: collapse;
            padding: 10px 0 0 0;
            font-size: 12px;
            color: #141414;
            border: 1px solid #DFDFDF;
        }
        .table-none td
        {
            padding: 5px 8px;
            background: #fff;
            font-size: 12px;
            color: #141414;
            line-height: 18px;
            max-height: 30px;
            border: none;
        }
        .table-none th
        {
            background: #ececec;
            font-size: 12px;
            padding: 5px 8px;
            border-bottom: none;
            border: 1px solid #DFDFDF;
            color: #141414;
            font-weight: bold;
            text-align: left;
        }
        .table-none-border
        {
            border: 1px solid #DFDFDF;
        }
        .table-none a
        {
            color: #ea702f;
            text-decoration: none;
        }
        .table-none a:hover
        {
            color: #141414;
            text-decoration: underline;
        }
    </style>
    <script type="text/javascript">
        $(function () {

            $('#ContentPlaceHolder1_txtFromDate').datetimepicker({
                showButtonPanel: true, ampm: false,
                showHour: false, showMinute: false, showSecond: false, showTime: false, showOn: "button",
                buttonImage: "/App_Themes/<%=Page.Theme %>/images/date-icon.png", buttonImageOnly: true
            });
            $('#ContentPlaceHolder1_txtToDate').datetimepicker({
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
                    if (document.getElementById('ContentPlaceHolder1_txtFromDate').value == '') {
                        jAlert('Please Enter Start Date.', 'Required Information');
                        document.getElementById('ContentPlaceHolder1_txtFromDate').focus();
                        return false;
                    }
                    else if (document.getElementById('ContentPlaceHolder1_txtToDate').value == '') {
                        jAlert('Please Enter End Date.', 'Required Information');
                        document.getElementById('ContentPlaceHolder1_txtToDate').focus();
                        return false;
                    }

                    var startDate = new Date(document.getElementById('ContentPlaceHolder1_txtFromDate').value);
                    var endDate = new Date(document.getElementById('ContentPlaceHolder1_txtToDate').value);
                    if (startDate > endDate) {
                        jAlert('Please Select Valid Date.', 'Required Information');
                        return false;
                    }

                }

                //}
            }
            return true;
        }
    </script>
    <div id="content-width">
        <div class="content-row1">
            <div class="create-new-order" style="width: 100%; margin-top: 2px;">
                <table cellpadding="3" cellspacing="3">
                    <tr>
                        <td align="left" style="padding: 0px;">
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
                                                    <img class="img-left" title="Yahoo Reports - Time Graphs" alt="Yahoo Reports - Time Graphs" src="/App_Themes/<%=Page.Theme %>/Images/sales-report-icon.png" />
                                                    <h2>
                                                        Yahoo Reports - Time Graphs</h2>
                                                </div>
                                            </th>
                                        </tr>
                                        <tr class="even-row">
                                            <td align="center">
                                                <b>
                                                    <asp:Literal ID="ltCriteria" runat="server"></asp:Literal>
                                                    :
                                                    <asp:Literal ID="ltPeriod" runat="server"></asp:Literal></b>
                                            </td>
                                        </tr>
                                        <tr style="background-color: #E3E3E3;">
                                            <td align="center">
                                                Total :
                                                <asp:Literal ID="ltTotalNoOfOrders" runat="server"></asp:Literal>&nbsp;&nbsp;<asp:LinkButton
                                                    ID="lkSpreadsheet" runat="server" Text="[Spreadsheet]" OnClick="lkSpreadsheet_Click"></asp:LinkButton>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="font-weight: bold; width: 99%" align="center">
                                                <asp:Literal ID="ltrChartOrder" runat="server"></asp:Literal>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center">
                                                <asp:ImageButton ID="imgGraph" runat="server" AlternateText="Graph" ToolTip="Graph"
                                                    OnClientClick="return ValidatePage();" OnClick="imgGraph_Click" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center">
                                                <table width="50%" class="table-none">
                                                    <tr>
                                                        <td align="left" style="border-right: 1px solid #DFDFDF; line-height: 25px; vertical-align: top;
                                                            width: 25%">
                                                            <b>Show</b>
                                                            <br />
                                                            <asp:RadioButtonList runat="server" ID="rblCriteria">
                                                                <asp:ListItem Text=" Customers" Value="1" Selected="True"></asp:ListItem>
                                                                <asp:ListItem Text=" Number of Orders" Value="2"></asp:ListItem>
                                                                <asp:ListItem Text=" Number of Gift Certificate Orders" Value="3"></asp:ListItem>
                                                                <asp:ListItem Text=" Items Sold" Value="4"></asp:ListItem>
                                                                <asp:ListItem Text=" Revenue" Value="5"></asp:ListItem>
                                                                <asp:ListItem Text=" Revenue from Gift Certificates" Value="6"></asp:ListItem>
                                                                <asp:ListItem Text=" Orders / Customer" Value="7"></asp:ListItem>
                                                                <asp:ListItem Text=" Revenue / Customer" Value="8"></asp:ListItem>
                                                                <asp:ListItem Text=" Revenue / Order" Value="9"></asp:ListItem>
                                                                <asp:ListItem Text=" Items / Order" Value="10"></asp:ListItem>
                                                            </asp:RadioButtonList>
                                                            <hr style="border: 1px solid #DFDFDF;" />
                                                            <asp:RadioButtonList runat="server" ID="rblGraphType">
                                                                <asp:ListItem Text=" Graph over time" Value="Overtime" Selected="True"></asp:ListItem>
                                                                <asp:ListItem Text=" Graph daily cycle" Value="Dailycycle"></asp:ListItem>
                                                            </asp:RadioButtonList>
                                                        </td>
                                                        <td align="left" style="line-height: 25px; vertical-align: top; width: 25%">
                                                            <b>Period</b>
                                                            <br />
                                                            <asp:RadioButtonList ID="rblPeriod" runat="server">
                                                            </asp:RadioButtonList>
                                                            <span style="padding-left: 25px;">
                                                                <asp:TextBox ID="txtFromDate" runat="server" CssClass="from-textfield" Width="70px"
                                                                    Style="margin-right: 3px;"></asp:TextBox>&nbsp; to &nbsp;
                                                                <asp:TextBox ID="txtToDate" runat="server" CssClass="from-textfield" Width="70px"
                                                                    Style="margin-right: 3px;"></asp:TextBox>
                                                            </span>
                                                            <br />
                                                        </td>
                                                    </tr>
                                                </table>
                                                <br />
                                                <br />
                                            </td>
                                        </tr>
                                    </table>
                                    <br />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </div>
    </div>
</asp:Content>
