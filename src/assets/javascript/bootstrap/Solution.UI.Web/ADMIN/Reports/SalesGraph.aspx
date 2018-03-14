<%@ Page Title="" Language="C#" MasterPageFile="~/ADMIN/Admin.Master" AutoEventWireup="true" CodeBehind="SalesGraph.aspx.cs" Inherits="Solution.UI.Web.ADMIN.Reports.SalesGraph" %>
 
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
 <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts-main.js"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-calender-custom.js"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-calender.js"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts.js"></script>
    <script type="text/javascript">



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

        function Datevisible() {

            if (document.getElementById('ContentPlaceHolder1_RadOrderByDays') != null) {

                if (document.getElementById('ContentPlaceHolder1_RadOrderByDays').selectedIndex == 7) {
                    if (document.getElementById('ContentPlaceHolder1_datetd') != null) {
                        document.getElementById('ContentPlaceHolder1_datetd').style.display = '';
                    }
                }
                else {
                    if (document.getElementById('ContentPlaceHolder1_datetd') != null) {
                        document.getElementById('ContentPlaceHolder1_datetd').style.display = 'none';
                    }
                }
                //                var radio = document.getElementById('rdoList').getElementsByTagName('input');
                //                
                //                for (var j = 0; j < radio.length; j++) {

                //                    if (radio[j].checked && j == 3) {
                //                        if (document.getElementById('ContentPlaceHolder1_datetd') != null) {
                //                            document.getElementById('ContentPlaceHolder1_datetd').style.display = '';
                //                        }
                //                    }
                //                    else {
                //                        if (document.getElementById('ContentPlaceHolder1_datetd') != null) {
                //                            document.getElementById('ContentPlaceHolder1_datetd').style.display = 'none';
                //                        }
                //                    }
                //                }
            }

        }

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
    <div id="content-width">
        <div class="content-row1">
            <div class="create-new-order">
                &nbsp;</div>
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
                                                    <img class="img-left" title="Customer" alt="Customer" src="/App_Themes/<%=Page.Theme %>/icon/report_icon.gif" />
                                                    <h2>
                                                        Sales Graph</h2>
                                                </div>
                                            </th>
                                        </tr>
                                        <tr class="altrow">
                                            <td align="left">
                                                <table cellpadding="3" cellspacing="3">
                                                    <tr>
                                                        <td align="left">
                                                            Store :
                                                        </td>
                                                        <td align="left">
                                                            <asp:DropDownList ID="ddlStore" runat="server" CssClass="order-list" Width="175px" AutoPostBack="false">
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="background-color: #E3E3E3; font-weight: bold;">
                                                Search By
                                            </td>
                                        </tr>
                                        <tr class="even-row">
                                            <td>
                                                <table border="0">
                                                    <tr>
                                                        <td align="left">
                                                        <div id="rdoList">
                                                            <asp:DropDownList ID="RadOrderByDays" runat="server" AutoPostBack="false" CssClass="order-list" Width="160px" 
                                                                onchange="Datevisible();">
                                                                   <asp:ListItem Text=" Last 10 Days Orders" Value="10" Selected="true"></asp:ListItem>
                                                        <asp:ListItem Text="Last 30 Days Orders" Value="30"></asp:ListItem>
                                                        <asp:ListItem Text="Last 60 Days Orders" Value="60"></asp:ListItem>
                                                        <asp:ListItem Text="Last 90 Days Orders" Value="90"></asp:ListItem>
                                                        <asp:ListItem Text="Last 120 Days Orders" Value="120"></asp:ListItem>
                                                        <asp:ListItem Text="Last 180 Days Orders" Value="180"></asp:ListItem>
                                                        <asp:ListItem Text="Last 365 Days Orders" Value="365"></asp:ListItem>
                                                        <asp:ListItem Text="Custom" Value="1"></asp:ListItem>
                                                            </asp:DropDownList>
                                                             

                                                            </div>
                                                        </td>
                                                        <td align="left" valign="bottom" id="datetd" runat="server" style="display: none;">
                                                            <table cellpadding="0" cellspacing="0">
                                                                <tr>
                                                                    <td valign="top">
                                                                        Start Date:
                                                                    </td>
                                                                    <td valign="top">
                                                                        <asp:TextBox ID="txtOrderFrom" runat="server" CssClass="from-textfield" Width="70px"
                                                                            Style="margin-right: 3px;"></asp:TextBox>
                                                                    </td>
                                                                    <td valign="top">
                                                                        End Date:
                                                                    </td>
                                                                    <td valign="top">
                                                                        <asp:TextBox ID="txtOrderTo" runat="server" CssClass="from-textfield" Width="70px"
                                                                            Style="margin-right: 3px;"></asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                        <td align="left">
                                                <asp:ImageButton ID="imgSearch" runat="server" AlternateText="Go" ToolTip="Go"
                                                    OnClick="imgSearch_Click" OnClientClick="return validation();" />
                                            </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        
                                        <tr>
                                            <td style="background-color: #E3E3E3; font-weight: bold;">
                                                Graph
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center">
                                                <asp:Literal ID="FCLiteral" runat="server"></asp:Literal>
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
    <div id="prepage" style="position: absolute; font-family: arial; font-size: 16; left: 0px;
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
</asp:Content>
