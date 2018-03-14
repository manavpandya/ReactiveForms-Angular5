<%@ Page Title="" Language="C#" MasterPageFile="~/ADMIN/Admin.Master" AutoEventWireup="true" CodeBehind="ItemChannelWiseReport.aspx.cs" Inherits="Solution.UI.Web.ADMIN.Reports.ItemChannelWiseReport" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts-main.js"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts.js"></script>
    <script type="text/javascript">
        function SearchValidation() {
            if (document.getElementById('<%=txtSearch.ClientID%>') != null && document.getElementById('<%= txtSearch.ClientID%>').value == '') {
                jAlert('Please enter search value.', 'Message', '<%=txtSearch.ClientID%>');
                return false;
            }
            return true;
        }
        function checkdata() {
            if (confirm('It will take 5 minuets to refresh data. Are you sure want to continue?')) {
                return true;;
            }
            return false;
        }

    </script>
    <style type="text/css">
        .td1 {
            valign: top;
            align: left;
        }

        .Freezing {
            width: 100% !important;
        }
    </style>
    <script type="text/javascript" language="javascript">
        var orgTop = 0;
        $(document).scroll(function () {


            var id = $("tr:.Freezing").get(0);

            var offset = $("#GHead").offset();
            var elPosition = $(id).position();
            var elWidth = $("#GHead").width() - 50;
            var elHeight = $("#GHead").height();
            if (orgTop == 0) {
                orgTop = elPosition.top;
            }

            $('#GHead').css('display', 'none');
            //id.style.display = 'none';
            var tt = $('#header').height();
            var tt1 = $('.altrow').height();


            if ($(window).scrollTop() <= orgTop + parseInt(tt) + parseInt(85) + parseInt(tt1)) {
                $('#GHead').css('position', 'relative');
                $('#GHead').css('top', 'auto');

                /*id.style.width = 'auto';
                id.style.height = 'auto';*/
            }
            else {
                //var allth = document.getElementById('ContentPlaceHolder1_grditemchannel').getElementsByTagName('th');
                //for (i = 0; i < allElts.length; i++) {
                //    var elt = allElts[i];
                //}

                //$('#GHead').style.position = 'absolute';
                $('#GHead').css('position', 'absolute');
                $('#GHead').css('top', $(window).scrollTop() + 'px');
                $('#GHead').css('height', elHeight + 'px');
                $('#GHead').css('display', 'block');

                //$('#GHead').style.top = $(window).scrollTop() + 'px';
                /*id.style.width = elWidth +'px';*/
                // $('#GHead').style.height = elHeight + 'px';



            }
        });
    </script>
    <script type="text/javascript" language="javascript">

        $(document).ready(function () {

            $("#GHead").html('');
            var gridHeader = $('#ContentPlaceHolder1_grditemchannel').clone(true); // Here Clone Copy of Gridview with style

            $(gridHeader).find("tr:gt(0)").remove(); // Here remove all rows except first row (header row)
            $('#ContentPlaceHolder1_grditemchannel tr th').each(function (i) {
                // Here Set Width of each th from gridview to new table(clone table) th
                $("th:nth-child(" + (i + 1) + ")", gridHeader).css('width', ($(this).width()).toString() + "px");
            });
            $("#GHead").append(gridHeader);

            //$('#GHead').css('position', 'absolute');
            //$('#GHead').css('top', $('#ContentPlaceHolder1_grditemchannel').offset().top);
            //var id = $("tr:.Freezing").get(0);
            //var elPosition = $(id).position();
            //if (orgTop == 0) {
            //    orgTop = elPosition.top;
            //}
            //var offset = $(id).offset();

            //var elWidth = $(id).width() - 50;
            //var elHeight = $(id).height();
            //    //if (orgTop == 0) {
            //    //    orgTop = elPosition.top;
            //    //}

            //if ($(window).scrollTop() <= orgTop) {
            //    id.style.position = 'relative';
            //    id.style.top = 'auto';

            //    /*id.style.width = 'auto';
            //    id.style.height = 'auto';*/
            //}
            //else {
            //    //var allth = document.getElementById('ContentPlaceHolder1_grditemchannel').getElementsByTagName('th');
            //    //for (i = 0; i < allElts.length; i++) {
            //    //    var elt = allElts[i];
            //    //}

            //    id.style.position = 'absolute';
            //    id.style.top = $(window).scrollTop() + 'px';
            //    /*id.style.width = elWidth +'px';*/
            //    id.style.height = elHeight + 'px';



            //}
            //if ($(window).scrollTop() <= orgTop) {
            //    $("#GHead").style.position = 'relative';
            //    $("#GHead").style.top = 'auto';

            //}
            //else {
            //    $("#GHead").style.position = 'absolute';
            //    $("#GHead").style.top = $(window).scrollTop() + 'px';

            //}




        });
    </script>
    <script type="text/javascript">

        function sortingfunction(id, sortex) {
            document.getElementById('ContentPlaceHolder1_hdncelid').value = id;
            
            if (sortex.toLowerCase() == "asc")
            {
                document.getElementById('ContentPlaceHolder1_hdnsortexpression').value = "DESC";
            }
            else {
                document.getElementById('ContentPlaceHolder1_hdnsortexpression').value = "ASC";
            }
            document.getElementById('ContentPlaceHolder1_btnSorting').click();

        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">




    <div id="content-width">
        <div class="content-row1">
            <div class="create-new-order" style="width: 100%; display: none;">
                <span style="vertical-align: middle; margin-right: 3px; float: left;">
                    <table style="margin-top: 2px;" cellpadding="3" cellspacing="3">
                        <tr>
                            <td style="padding-left: 0px;" align="left">Store : &nbsp;
                                
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
                                                    <img class="img-left" title="Item List Channelwise Report" alt="Item List Channelwise Report"
                                                        src="/App_Themes/<%=Page.Theme %>/Images/customer-list-icon.png" />
                                                    <h2>Item List Channelwise Report</h2>
                                                </div>
                                            </th>
                                        </tr>
                                        <tr class="altrow">
                                            <td style="padding-right: 0px;">

                                                <table style="width: 100%" cellpadding="3" cellspacing="3">
                                                    <tr>
                                                        <td style="padding-right: 0px; width: 80%" align="right">
                                                            <table>
                                                                <tr>
                                                                    <td valign="top" style="text-align: left">Search :&nbsp;<asp:TextBox ID="txtSearch" runat="server" CssClass="order-textfield"
                                                                        Width="160px"></asp:TextBox><br />
                                                                        <span style="padding-left: 50px;">(eg. SKU) </span>
                                                                    </td>
                                                                    <td valign="top" align="right" style="padding-right: 0px;">
                                                                        <asp:Button ID="btnSearch" runat="server" OnClientClick="return SearchValidation();" OnClick="btnSearch_Click" />
                                                                        <asp:Button ID="btnShowall" runat="server" OnClick="btnShowall_Click" />
                                                                        <asp:Button ID="btnExport" runat="server" ToolTip="Export" Style="background: url(/App_Themes/<%= Page.Theme.ToString()%>/images/export.gif) no-repeat transparent; width: 67px; height: 23px; border: none; cursor: pointer;"
                                                                            OnClick="btnExport_Click" />&nbsp;<asp:Button ID="btnnavuplod" runat="server" ToolTip="" BackColor="#B92127" ForeColor="White" BorderWidth="0" Height="25px" Text="REFRESH DATA"
                                                                                OnClientClick="return checkdata();" OnClick="btnnavuplod_Click" />
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr class="even-row">
                                            <td>
                                                <asp:Literal ID="ltrCart" runat="server"></asp:Literal>
                                                <div id="GHead" style="display: none;"></div>
                                                <asp:GridView ID="grditemchannel" runat="server"
                                                    BorderStyle="Solid" BorderWidth="1" CellSpacing="1" BorderColor="#E7E7E7" EmptyDataText="No Record(s) Found."
                                                    EmptyDataRowStyle-ForeColor="Red" EmptyDataRowStyle-HorizontalAlign="Center"
                                                    ViewStateMode="Enabled" Width="100%" GridLines="None" OnRowDataBound="grditemchannel_RowDataBound"
                                                    CellPadding="2">





                                                    
                                                    <RowStyle HorizontalAlign="Left" CssClass="oddrow" />
                                                    <AlternatingRowStyle CssClass="altrow" HorizontalAlign="Left"  />
                                                    <EmptyDataRowStyle HorizontalAlign="Center" ForeColor="Red"></EmptyDataRowStyle>
                                                    <HeaderStyle ForeColor="White" Font-Bold="false" HorizontalAlign="Left" CssClass="Freezing" />
                                                </asp:GridView>




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
    <div style="display:none;">
        <input type="hidden" id="hdncelid" runat="server" value="" />
        <input type="hidden" id="hdnsortexpression" runat="server" value="ASC" />
        <asp:Button ID="btnSorting" runat="server" Text="savee" OnClick="btnSorting_Click" />
    </div>
</asp:Content>
