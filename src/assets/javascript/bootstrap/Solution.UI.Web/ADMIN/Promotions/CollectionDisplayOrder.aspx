<%@ Page Title="" Language="C#" MasterPageFile="~/ADMIN/Admin.Master" AutoEventWireup="true" CodeBehind="CollectionDisplayOrder.aspx.cs" Inherits="Solution.UI.Web.ADMIN.Promotions.CollectionDisplayOrder" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
      <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts-main.js"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts.js"></script>
    <script type="text/javascript" src="/admin/js/jquery-1.11.1.min.js"></script>
    <script type="text/javascript" src="/admin/js/jquery.dragsort-0.5.2.min.js"></script>
    <script type="text/javascript" src="/admin/js/jQuery_bootstrap.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <style>
        h2 {
            font-size: 15px;
            text-align: center;
        }

        .fp-display {
            float: left;
            width: 100%;
            padding-bottom: 10%;
        }

        ul li .fp-display {
            float: left;
            width: 100%;
            padding-bottom: 10%;
        }

        .fp-box-p {
            float: left;
            color: #393939;
            font-size: 16px;
            padding: 1px 0px;
            width: 100%;
            text-align: center;
            font-weight: bold;
        }

        .placeHolder {
            background-color: white !important;
            border: dashed 1px gray !important;
        }
    </style>

    <style type="text/css">
        .modal1 #loader-wrapper {
            position: none !important;
            top: 0;
            left: 0;
            width: 100%;
            height: 100%;
            z-index: 1000;
        }

        .modal1 #loader {
            display: block;
            position: relative;
            left: 41%;
            top: 16%;
            width: 70px;
            height: 70px;
            margin: -35px 0 0 10px;
            border-radius: 50%;
            border: 3px solid transparent;
            border-top-color: #3498db;
            -webkit-animation: spin 2s linear infinite; /* Chrome, Opera 15+, Safari 5+ */
            animation: spin 2s linear infinite; /* Chrome, Firefox 16+, IE 10+, Opera */
        }

        .modal1 {
            left: 55% !important;
        }

            .modal1 #loader:before {
                content: "";
                position: absolute;
                top: 5px;
                left: 5px;
                right: 5px;
                bottom: 5px;
                border-radius: 50%;
                border: 3px solid transparent;
                border-top-color: #e74c3c;
                -webkit-animation: spin 3s linear infinite; /* Chrome, Opera 15+, Safari 5+ */
                animation: spin 3s linear infinite; /* Chrome, Firefox 16+, IE 10+, Opera */
            }

            .modal1 #loader:after {
                content: "";
                position: absolute;
                top: 15px;
                left: 15px;
                right: 15px;
                bottom: 15px;
                border-radius: 50%;
                border: 3px solid transparent;
                border-top-color: #f9c922;
                -webkit-animation: spin 1.5s linear infinite; /* Chrome, Opera 15+, Safari 5+ */
                animation: spin 1.5s linear infinite; /* Chrome, Firefox 16+, IE 10+, Opera */
            }

        .modal-content {
            background-clip: padding-box;
            background-color: #fff;
            border: 1px solid rgba(0, 0, 0, 0.2);
            border-radius: 6px;
            box-shadow: 0 3px 9px rgba(0, 0, 0, 0.5);
            outline: 0 none;
            position: relative;
            width: 100% !important;
            left: 0 !important;
        }

        @-webkit-keyframes spin {
            0% {
                -webkit-transform: rotate(0deg); /* Chrome, Opera 15+, Safari 3.1+ */
                -ms-transform: rotate(0deg); /* IE 9 */
                transform: rotate(0deg); /* Firefox 16+, IE 10+, Opera */
            }

            100% {
                -webkit-transform: rotate(360deg); /* Chrome, Opera 15+, Safari 3.1+ */
                -ms-transform: rotate(360deg); /* IE 9 */
                transform: rotate(360deg); /* Firefox 16+, IE 10+, Opera */
            }
        }

        @keyframes spin {
            0% {
                -webkit-transform: rotate(0deg); /* Chrome, Opera 15+, Safari 3.1+ */
                -ms-transform: rotate(0deg); /* IE 9 */
                transform: rotate(0deg); /* Firefox 16+, IE 10+, Opera */
            }

            100% {
                -webkit-transform: rotate(360deg); /* Chrome, Opera 15+, Safari 3.1+ */
                -ms-transform: rotate(360deg); /* IE 9 */
                transform: rotate(360deg); /* Firefox 16+, IE 10+, Opera */
            }
        }

        .modal1 .modal-backdrop {
            display: none !important;
        }

        .fp-box-p {
            font-weight: normal !important;
        }
    </style>
    <style type="text/css">
        #tab-container-product ul.menu li {
            margin-bottom: 0;
        }

        #tab-container ul.menu li {
            margin-bottom: -1px;
        }

        #tab-container-1 ul.menu li {
            margin-bottom: -1px;
        }

        .slidingDiv {
            height: 300px;
            padding: 20px;
            margin-top: 10px;
        }

        .show_hide {
            display: block;
        }

        .footerBorder {
            border-top: 1px solid #DFDFDF;
            border-right: 1px solid #DFDFDF;
        }

        .footerBorderinventory {
            border-top: 1px solid #DFDFDF;
        }

        .divfloatingcss {
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
        var ii = 0;
        function chkHeight() {
            var windowHeight = 0;
            windowHeight = $(document).height(); //window.innerHeight;

            document.getElementById('prepage').style.height = windowHeight + 'px';
            document.getElementById('prepage').style.display = '';
            $(".removetr").hide(); //window.scrollTo(0, 0);;
            window.scrollTo(0, 0);
        }
        function updatenn() {
            if (ii == 0) {
                //bootbox.dialog({
                //    closeButton: false,

                //    message: "<style>.modal1{width:240px !important;} .modal1 > .modal-dialog{width:240px !important;}.bootbox-body{height:60px;}</style><div style='float:left;width:40%;'><div id='loader-wrapper'><div id='loader'></div><div style='float:right;width:54%;font-size:14px;font-weight:bold;color:#3369b1;padding-top:4.5%; '>Loading...</div></div></div>",
                //    title: "",
                //    className: "modal1"

                //});
                chkHeight();
                ii = 1;
            }
            else {
                //  $('.bootbox').show();
                chkHeight();
            }
            var count = 0;
            var alldivdat = 'divchild';
            var allhdn = document.getElementById(alldivdat).getElementsByTagName('input');
            var allids = '';
            var alldisplayorder = '';
            var icount = 1;
            var rowid = 0;
            var pcatid = 0;
            var icount2 = 0;
            for (var i = 0; i < allhdn.length; i++) {
                var cntrl = allhdn[i];
                if (cntrl.id.toString().toLowerCase().indexOf('hdnproductid') > -1) {
                    alldisplayorder = alldisplayorder + icount.toString() + ',';
                    allids = allids + cntrl.value.toString() + ',';
                    icount = icount + 1;
                    rowid = rowid + 1;
                }
                if (cntrl.id.toString().toLowerCase().indexOf('txtdisplayorder') > -1) {
                    cntrl.value = icount2
                    icount2 = icount2 + 1;
                }
                if (cntrl.id.toString().toLowerCase().indexOf('hdneventid') > -1) {
                    pcatid = cntrl.value.toString();
                }

            }
            $.ajax(
              {
                  type: "POST",
                  url: "/TestMail.aspx/updatecollectiondisplay",
                  data: "{ids: '" + allids + "', eventid: '" + pcatid + "'}",
                  contentType: "application/json; charset=utf-8",
                  dataType: "json",
                  async: "true",
                  cache: "false",
                  success: function (msg) {
                      // $('.bootbox').hide();
                      document.getElementById('prepage').style.display = 'none';
                  }, Error: function (x, e) {// $('.bootbox').hide(); 
                      document.getElementById('prepage').style.display = 'none';
                  }
              });
        }

    </script>
    <script type="text/javascript">


        function verifyoption() {

            jConfirm('Are you sure want to change selected option ?', 'Confirmation', function (r) {
                if (r == true) {

                    SelectOption();
                    if (document.getElementById("ContentPlaceHolder1_rdooption") != null) {
                        var options = '';
                        var RB1 = document.getElementById("rdolist").getElementsByTagName("input");

                        for (var i = 0; i < RB1.length; i++) {
                            if (RB1[i].type == 'radio' && RB1[i].checked) {
                                if (RB1[i].value.toString().toLowerCase() == 'custom') {

                                    options = 'custom';
                                    if (document.getElementById("ContentPlaceHolder1_hdnoptions") != null)
                                        document.getElementById("ContentPlaceHolder1_hdnoptions").value = 'custom';

                                } else if (RB1[i].value.toString().toLowerCase() == 'name') {
                                    options = 'name';
                                    if (document.getElementById("ContentPlaceHolder1_hdnoptions") != null)
                                        document.getElementById("ContentPlaceHolder1_hdnoptions").value = 'name';
                                }
                                else if (RB1[i].value.toString().toLowerCase() == 'price') {
                                    options = 'price';
                                    if (document.getElementById("ContentPlaceHolder1_hdnoptions") != null)
                                        document.getElementById("ContentPlaceHolder1_hdnoptions").value = 'price';
                                }





                            }
                        }

                        $.ajax(
            {
                type: "POST",
                url: "/TestMail.aspx/setdisplayorderoption",
                data: "{option: '" + options + "',pagename:'collection'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: "true",
                cache: "false",
                success: function (msg) {
                    // $('.bootbox').hide();
                    document.getElementById('prepage').style.display = 'none';
                    jAlert('Record Saved successfully.', 'Message', '<%=rdooption.ClientID%>');

                    return true;
                }, Error: function (x, e) {// $('.bootbox').hide(); 
                    document.getElementById('prepage').style.display = 'none';
                }
            });
        }

    }
    else {
        Rollbackoption();
        return false;
    }
            });
    return false;
}


function Rollbackoption() {
    if (document.getElementById("ContentPlaceHolder1_hdnoptions") != null) {
        if (document.getElementById("ContentPlaceHolder1_rdooption") != null) {
            var options = "";
            options = document.getElementById("ContentPlaceHolder1_hdnoptions").value;
            var RB1 = document.getElementById("rdolist").getElementsByTagName("input");

            for (var i = 0; i < RB1.length; i++) {
                if (RB1[i].type == 'radio') {

                    if (RB1[i].value.toString().toLowerCase() == options.toString().toLowerCase()) {
                        RB1[i].checked = true;
                        break;
                    }
                }
            }


            SelectOption();
        }
    }
}

function SelectOption() {

    if (document.getElementById("ContentPlaceHolder1_rdooption") != null) {

        var RB1 = document.getElementById("rdolist").getElementsByTagName("input");

        for (var i = 0; i < RB1.length; i++) {
            if (RB1[i].type == 'radio' && RB1[i].checked) {

                if (RB1[i].value.toString().toLowerCase() == 'custom') {
                    // document.getElementById("ContentPlaceHolder1_trBottom").style.display = 'none';
                    document.getElementById("ContentPlaceHolder1_trproducts").style.display = '';
                }
                else if (RB1[i].value.toString().toLowerCase() == 'name') {
                    // document.getElementById("ContentPlaceHolder1_trBottom").style.display = '';
                    document.getElementById("ContentPlaceHolder1_trproducts").style.display = 'none';
                }
                else if (RB1[i].value.toString().toLowerCase() == 'price') {
                    //document.getElementById("ContentPlaceHolder1_trBottom").style.display = '';
                    document.getElementById("ContentPlaceHolder1_trproducts").style.display = 'none';
                }

            }
        }


    }
    return true;
}
    </script>
    <asp:ScriptManager ID="sc1" runat="server">
    </asp:ScriptManager>
    <div class="content-row1">
        <div class="create-new-order" style="width: 100%;">
            <span style="vertical-align: middle; margin-top: 4px; margin-right: 3px; float: left;"></span><span style="vertical-align: middle; margin-right: 3px; float: right;"></span>
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
                                                <img class="img-left" title="Event Products List" alt="Event Products List" src="/App_Themes/<%=Page.Theme %>/Images/coupon-discount-icon.png" />
                                                <h2>
                                                    <asp:Literal ID="ltrtitle" runat="server"></asp:Literal>
                                                </h2>
                                            </div>
                                            <div class="main-title-right">
                                                <a id="AtagBack" runat="server" style="margin-right: 60px;">
                                                    <img src="/App_Themes/<%=Page.Theme %>/images/back.gif" alt="Go to Event List"
                                                        title="Go to Event List" />
                                                </a>
                                            </div>
                                        </th>

                                    </tr>
                                    <tr id="troptions" runat="server">
                                        <td>
                                            <div id="rdolist">
                                                <span style="float: left; margin-top: 5px;">Choose Option : </span>
                                                <asp:RadioButtonList ID="rdooption" runat="server" RepeatDirection="Horizontal" onchange="return verifyoption();">
                                                    <asp:ListItem Text="Order By DisplayOrder" Value="custom" Selected="True"></asp:ListItem>
                                                    <asp:ListItem Text="Order By Name" Value="name"></asp:ListItem>
                                                    <asp:ListItem Text="Order By Price" Value="price"></asp:ListItem>
                                                </asp:RadioButtonList>
                                                <asp:HiddenField ID="hdnoptions" runat="server" />
                                            </div>
                                        </td>
                                    </tr>
                                    <tr class="even-row" id="trproducts" runat="server">
                                        <td>
                                            <div id="divchild" style="position: relative; left: 15px; overflow: auto; width: 89%; margin-top: 10px; margin-left: 100px;">
                                                <asp:GridView ID="gridproducts" CellSpacing="1" BorderStyle="Solid" BorderWidth="1"
                                                    BorderColor="#E7E7E7" runat="server" AutoGenerateColumns="False"
                                                    EmptyDataText="No Record(s) Found." AllowSorting="false" EmptyDataRowStyle-ForeColor="Red"
                                                    EmptyDataRowStyle-HorizontalAlign="Center" ViewStateMode="Enabled" Width="100%"
                                                    GridLines="None" AllowPaging="false" Visible="false"
                                                    PagerSettings-Mode="NumericFirstLast" CellPadding="2">
                                                    <Columns>

                                                        <asp:TemplateField HeaderText="Image" HeaderStyle-Width="10%">
                                                            <HeaderTemplate>
                                                                Image
                                                           
                                                            </HeaderTemplate>
                                                            <ItemStyle HorizontalAlign="Left" />
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                            <ItemTemplate>

                                                                <img src="/resources/halfpricedraps/product/micro/<%#Eval("Imagename") %>" alt="" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Product Name" HeaderStyle-Width="10%">
                                                            <HeaderTemplate>
                                                                Product Name
                                                           
                                                            </HeaderTemplate>
                                                            <ItemStyle HorizontalAlign="Left" />
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                            <ItemTemplate>

                                                                <asp:Label ID="lblname" runat="server" Text='<%# bind("Name") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="SKU" HeaderStyle-Width="15%">
                                                            <HeaderTemplate>
                                                                SKU
                                                            
                                                            </HeaderTemplate>
                                                            <ItemStyle HorizontalAlign="left" Width="15%" />
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                            <ItemTemplate>
                                                                <asp:HiddenField ID="hdnProductid" runat="server" Value='<%#Eval("ProductID") %>' />


                                                                <asp:Label ID="lblsku" runat="server" Text='<%# bind("SKU") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Display Order" HeaderStyle-Width="15%">
                                                            <HeaderTemplate>
                                                                Display Order
                                                            
                                                            </HeaderTemplate>
                                                            <ItemStyle HorizontalAlign="left" />
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtdisplayorder" runat="server" Text='<%# bind("DisplayOrder") %>'></asp:TextBox>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>


                                                    </Columns>
                                                    <PagerSettings Mode="NumericFirstLast" Position="TopAndBottom"></PagerSettings>
                                                    <PagerStyle HorizontalAlign="Right" CssClass="grid_paging" />
                                                    <RowStyle HorizontalAlign="Center" CssClass="oddrow" />
                                                    <AlternatingRowStyle CssClass="altrow" />
                                                    <EmptyDataRowStyle HorizontalAlign="Center" ForeColor="Red"></EmptyDataRowStyle>
                                                    <HeaderStyle ForeColor="White" Font-Bold="false" />
                                                </asp:GridView>
                                                <asp:Literal ID="ltrrepeater" runat="server"></asp:Literal>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr class="altrow" id="trBottom" runat="server" style="display: none;">
                                        <td>
                                            <table width="100%">
                                                <tr>

                                                    <td align="center" colspan="2">
                                                        <asp:Button ID="btnsave" runat="server" ToolTip="Save"
                                                            OnClick="btnsave_Click" />
                                                        <asp:Button ID="btncancel" runat="server" ToolTip="Cancel" OnClick="btncancel_Click" />

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
                <td>&nbsp;
                </td>
            </tr>
        </table>
        <div id="prepage" style="position: absolute; font-family: arial; font-size: 16; left: 0px; top: 0px; background-color: #000; opacity: 0.7; filter: alpha(opacity=70); layer-background-color: white; height: 100%; width: 100%; z-index: 1000; display: none;">
            <table width="100%" style="padding-top: 400px;" align="center">
                <tr>
                    <td align="center" style="color: #fff;" valign="middle">
                        <img alt="" src="/App_Themes/<%=Page.Theme %>/images/loding.png" /><br />
                        <b>Loading ... ... Please wait!</b>
                    </td>
                </tr>
            </table>
        </div>
        <div style="display: none;">
            <input type="hidden" id="hdncatidforjs" runat="server" value='<%# Request.QueryString["id"].ToString() %>' />
        </div>
        <script type="text/javascript">
            function expandcollapse() {


                var $jjjj = jQuery.noConflict();
                $jjjj(document).ready(function () {
                    var st = $jjjj("#divchild" + " .rep-drag li").attr('style');
                    var hh = $jjjj("#divchild" + " .rep-drag li").innerHeight();
                    hh = parseInt(hh) - parseInt(10);

                    st = st.replace('border:dashed 1px #454545;', '') + '';
                    $jjjj("#divchild" + " .rep-drag").dragsort({ dragSelector: "li", dragEnd: updatenn, placeHolderTemplate: "<li class='placeHolder' style='" + st + "'></li>" });
                });
            }
        </script>
    </div>
</asp:Content>
