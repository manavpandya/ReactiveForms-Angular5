<%@ Page Title="" Language="C#" MasterPageFile="~/ADMIN/Admin.Master" AutoEventWireup="true" CodeBehind="SetProductDisplayOrder.aspx.cs" Inherits="Solution.UI.Web.ADMIN.Products.SetProductDisplayOrder" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts-main.js"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts.js"></script>
     <style type="text/css">
        @media(max-width:768px) {
            .row-fluid {
                width: 92% !important;
            }
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <style>
 .newarrivalItem { left: 0px; top: 0; position: absolute; width: 60px !important; height: 33px !important; }
.bestsellerItem { left: 0px; top: 0; position: absolute; width: 60px !important; height: 33px !important; }
.newarrival { left: 0; top: 0; position: absolute; width: 60px !important; height: 33px !important; }

.bestseller { left: 0; top: 0px; position: absolute; width: 60px !important; height: 33px !important; }

.onsaleth
{
width:40% !important;max-width:78px !important;float:right;
}
.sales-outlet { float: right; background: #b92127; color: #fff; font-size: 14px; font-family: 'Lato', sans-serif; padding: 10px; }
.sales-offer-img { left: 0; top: 0; position: absolute; max-width: 78px !important; width: 40% !important; }
        h2 {
            font-size: 15px;
            text-align: center;
        }
.fp-box-p input{display:none;}
        .fp-display {
            float: left;
            width: 100%;
            padding-bottom: 10%;
        }

        ul li .fp-display {
            float: left;
            width: 100%;
            padding-bottom: 10%;
position:relative;
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
.rep-drag li{width:17.5% !important;margin-left:15px !important;}
  
.fp-display .fp-box-div center img{max-width:100% !important;min-height:385px;} 
.fp-display .fp-display-title{height:40px !important;} 
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
   <%-- <link href="../../REPLENISHMENTMANAGEMENT/css/bootstrap.css" rel="stylesheet" />--%>
    <%--<link href="../../REPLENISHMENTMANAGEMENT/css/bootstrap.min.css" rel="stylesheet" />--%>
    <%--<script type="text/javascript" src="/admin/js/bootstrap.min.js"></script>--%>
     <script type="text/javascript" src="/admin/js/jquery-1.11.1.min.js"></script>
    <script type="text/javascript" src="/admin/js/jquery.dragsort-0.5.2.min.js"></script>
    <script type="text/javascript" src="/admin/js/jQuery_bootstrap.js"></script>
    <%--<script type="text/javascript" src="/admin/js/bootbox.min.js?454544545"></script>--%>
    <script type="text/javascript">

        //$(function () {
        //    //var alldivdat = 'divchild' + document.getElementById('ContentPlaceHolder1_hdncatidforjs').value;
        //    //$("#divchild" + document.getElementById('ContentPlaceHolder1_hdncatidforjs').value + " tbody").addClass('rep-drag');
        //    //$("#ContentPlaceHolder1_grdParentCategory tbody.rep-drag").sortable({
        //    //    revert: true
        //    //});
        //    //$("#ContentPlaceHolder1_grdParentCategory tbody.rep-drag").disableSelection();
        //    $("#ContentPlaceHolder1_grdRowParentCategory_grdParentCategory ul.rep-drag").sortable({
        //        revert: false
        //    });
        //    $("#ContentPlaceHolder1_grdRowParentCategory_grdParentCategory ul.rep-drag").disableSelection();
        //});
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
            var alldivdat = 'divchild' + document.getElementById('ContentPlaceHolder1_hdncatidforjs').value;
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
                if (cntrl.id.toString().toLowerCase().indexOf('hdncatid') > -1) {
                    pcatid = cntrl.value.toString();
                }

            }
            $.ajax(
              {
                  type: "POST",
                  url: "/TestMail.aspx/updateproduct",
                  data: "{ids: '" + allids + "', catid: '" + pcatid + "',prtid:'" + document.getElementById('ContentPlaceHolder1_ddlCategory').options[document.getElementById('ContentPlaceHolder1_ddlCategory').selectedIndex].value + "'}",
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


        function UpdateByTextBox() {
            var count = false;
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
                // $('.bootbox').show();
                chkHeight();
            }
            var count = 0;
            var alldivdat = 'divchild' + document.getElementById('ContentPlaceHolder1_hdncatidforjs').value;
            var allhdn = document.getElementById(alldivdat).getElementsByTagName('input');
            var allids = '';
            var alldisplayorder = '';
            var icount = 1;
            var rowid = 0;
            var pcatid = 0;
            var icount2 = 0;
            var i = 0;
            var category_Array = [];

            for (var i = 0; i < allhdn.length; i++) {
                var cntrl = allhdn[i];
                if (cntrl.id.toString().toLowerCase().indexOf('hdnproductid') > -1) {
                    alldisplayorder = alldisplayorder + icount.toString() + ',';
                    allids = allids + cntrl.value.toString() + ',';
                    icount = icount + 1;
                    rowid = rowid + 1;
                    var txtDisplayOrder = document.getElementById("txt-displayorder-" + cntrl.value.toString()).value;
                    var spnVal = document.getElementById("spn-displayorder-" + cntrl.value.toString()).innerHTML;
                    if (parseInt(document.getElementById("txt-displayorder-" + cntrl.value.toString()).value) != parseInt(document.getElementById("spn-displayorder-" + cntrl.value.toString()).innerHTML)) {
                        category_Array.push([[cntrl.value.toString()], [txtDisplayOrder]]);
                    }
                }
                if (cntrl.id.toString().toLowerCase().indexOf('txtdisplayorder') > -1) {
                    cntrl.value = icount2
                    icount2 = icount2 + 1;
                }
                if (cntrl.id.toString().toLowerCase().indexOf('hdncatid') > -1) {
                    pcatid = cntrl.value.toString();
                }
            }

            category_Array.sort(
            function (a, b) {
                if (a[1] == b[1])
                    return a[0] > b[0] ? -1 : 1;
                return a[1] > b[1] ? 1 : -1;
            }
            );

            for (var i = 0; i < category_Array.length; i++) {
                try {
                    $.ajax(
                    {
                        type: "POST",
                        url: "/TestMail.aspx/updateproductByManual",
                        data: "{ids: '" + category_Array[i][1] + "', catid: '" + pcatid + "' , ProductID: '" + category_Array[i][0] + "',prtid:'" + document.getElementById('ContentPlaceHolder1_ddlCategory').options[document.getElementById('ContentPlaceHolder1_ddlCategory').selectedIndex].value + "'}",
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        async: "true",
                        cache: "false",
                        success: function (msg) {
                            //$('.bootbox').hide();
                            document.getElementById('prepage').style.display = 'none';
                            count = true;
                        }, Error: function (x, e) {
                            //$('.bootbox').hide();
                            document.getElementById('prepage').style.display = 'none';
                            count = false;
                        }
                    });
                }
                catch (err) {
                    // $('.bootbox').hide();
                    document.getElementById('prepage').style.display = 'none';
                }
            }
            // $('.bootbox').hide();
            document.getElementById('prepage').style.display = 'none';
           
            if (count) {
               
                document.getElementById("ContentPlaceHolder1_btnDivClick").click();
                
            }
        }


        function DisplayOrderChange(id) {
            if (document.getElementById("txt-displayorder-" + id).value == "" || document.getElementById("txt-displayorder-" + id).value == null || parseInt(document.getElementById("txt-displayorder-" + id).value) < 0) {
                document.getElementById("txt-displayorder-" + id).value = document.getElementById("spn-displayorder-" + id).innerHTML;
            }
        }
    </script>

    <asp:ScriptManager ID="sc1" runat="server">
    </asp:ScriptManager>
    <div id="content-width">
        <div class="content-row1">
            <div class="create-new-order" style="width: 100%;">
            </div>
        </div>
        <div class="content-row2">
            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                <tr>
                    <td height="5" align="left" valign="top">
                        <img src="/App_Themes/<%=Page.Theme %>/images/spacer.gif" width="1" height="5" />
                    </td>
                </tr>
                <tr>
                    <td height="5" align="left" valign="top">
                        <img src="/App_Themes/<%=Page.Theme %>/images/spacer.gif" width="1" height="5" />
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
                                            <td>
                                                <table>
                                                     <tr>
                                                        <td align="left">Root Category :
                                                        </td>
                                                        <td align="left">
                                                            <asp:DropDownList ID="ddlCategory" runat="server" Width="210px" AutoPostBack="false" CssClass="order-list">
                                                            </asp:DropDownList>
                                                        </td>
                                                         <td align="left">Status :
                                                        </td>
                                                        <td align="left">
                                                            <asp:DropDownList ID="ddlcateggosystatus" runat="server" Width="210px" AutoPostBack="false" CssClass="order-list">
                                                                 <asp:ListItem Value="">All</asp:ListItem>
                                                                  <asp:ListItem Value="1" Selected="True">Active</asp:ListItem>
                                                                <asp:ListItem Value="0">InActive</asp:ListItem>
                                                            </asp:DropDownList>
                                                        </td>
                                                         <td>
                                                               <asp:Button ID="btnSearch" runat="server" OnClick="btnSearch_Click" />
                                                         </td>
                                                         
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <th>
                                                <div class="main-title-left">
                                                    <img class="img-left" title="Add Country" alt="Add Country" src="/App_Themes/<%=Page.Theme %>/images/category-list-icon.png">
                                                    <h2 style="text-align:left;">Product Display Order</h2>
                                                </div>
                                            </th>
                                        </tr>
                                        <tr style="display:none;">
                                            <td style="float:right;">
                                                <asp:TextBox ID="txtcategoryinsert" runat="server" CssClass="order-textfield" placeholder="Enter Category Name"></asp:TextBox>&nbsp;&nbsp;&nbsp;<asp:Button ID="lnkbtnsearch" runat="server" OnClientClick="return chekcategoryinsert();" OnClick="lnkbtnsearch_Click1"  />
                                                
                        
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center">
                                                <asp:Label ID="lblMessage" runat="server" ForeColor="Red" Font-Bold="true"></asp:Label>
                                            </td>

                                        </tr>
                                        <tr >
                                            <td>
                                                  <asp:GridView ID="grdRowParentCategory" runat="server" EmptyDataText="No Record Found" GridLines="None" AutoGenerateColumns="false" style="position: relative; left: 15px; overflow: auto; width: 97%; margin-top: 10px;"
                                 AllowPaging="false" DataKeyNames="CategoryID"
                                                      Width="100%" CellPadding="2" CellSpacing="1" CssClass="border-td" 
                                                                                RowStyle-ForeColor="Black" HeaderStyle-ForeColor="#3c2b1b" PagerSettings-Mode="NumericFirstLast" 
                                                                                AllowSorting="True" EmptyDataRowStyle-ForeColor="Red" EmptyDataRowStyle-HorizontalAlign="Center"
                                ShowFooter="false" OnRowDataBound="grdRowParentCategory_RowDataBound" OnRowCommand="grdRowParentCategory_RowCommand">
                                <Columns>
                                      <asp:TemplateField HeaderText="&nbsp;Category Name">
                                         <ItemTemplate>
                                            <div onclick="javascript:expandcollapseRow('divRowchild<%# Eval("CategoryID") %>', 'one');" style="cursor: pointer;">
                                                <div style="float: left;">
                                                    <%--<a href="javascript:expandcollapse('divchild<%# Eval("CategoryID") %>', 'one');">--%>
                                                    <a href="javascript:void(0);">
                                                        <img id="imgdivRow<%# Eval("CategoryID") %>" title="Collapse" alt="Expand" src="/images/expand.gif" />
                                                    </a>
                                                </div>
                                                &nbsp;&nbsp;<asp:Literal ID="lttitle" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"Name") %>'></asp:Literal>
                                                <span
                                                    style="float: right;"></span>
                                                <input type="hidden" id="hdnCategoryID" runat="server" value='<%# DataBinder.Eval(Container.DataItem,"CategoryID") %>' />
                                            </div>
                                        </ItemTemplate>

                                        <HeaderStyle Width="37.5%" HorizontalAlign="left" VerticalAlign="Middle" CssClass="table-title" />
                                         <HeaderStyle Width="37.5%" HorizontalAlign="left" VerticalAlign="Middle" CssClass="table-title" />
                                        <ItemStyle HorizontalAlign="left" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Display&nbsp;Order" ItemStyle-HorizontalAlign="Center"
                                        ItemStyle-Width="50px" HeaderStyle-HorizontalAlign="left">
                                        <HeaderTemplate>
                                            <table cellpadding="0" cellspacing="1" border="0" align="center">
                                                <tr style="border: 0px;">
                                                    <td style="border: 0px; padding: 0px; background-color: transparent;">
                                                        <strong>Inner Category Count</strong>
                                                    </td>
                                                </tr>
                                            </table>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="txtDisplayOrder" runat="server" Text=' <%#Eval("DisplayOrder").ToString().Equals("999")?null:Eval("DisplayOrder")%>' Style="display: none;"></asp:Label>
                                            <center>
                                                <asp:Label ID="lbinv" runat="server" Text='<%#  DataBinder.Eval(Container.DataItem,"ChildCatCount") %>'></asp:Label></center>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Left" Width="39.5%" />
                                        <ItemStyle HorizontalAlign="Center" Width="50px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Status" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="50px"
                                        HeaderStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:HiddenField ID="hdnActive" runat="server" Value='<%# (DataBinder.Eval(Container.DataItem,"Active")) %>' />
                                            <asp:Literal ID="ltrStatus" runat="server"></asp:Literal>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Center" Width="11%" />
                                        <ItemStyle HorizontalAlign="Center" Width="50px" />
                                    </asp:TemplateField>
                                      <asp:TemplateField>
                                        <ItemTemplate>
                                            <tr>
                                                <td colspan="100%">
                                                <asp:HiddenField ID="hdnCategory" runat="server" />
                                                     <div id="divRowchild<%# Eval("CategoryID") %>" style="display: none; position: relative; left: 15px; overflow: auto; width: 97%; margin-top: 10px;">
                                                <asp:GridView ID="grdParentCategory" runat="server" GridLines="None" AutoGenerateColumns="false" DataKeyNames="CategoryID"
                                                    Width="100%" CellPadding="2" CellSpacing="1" CssClass="border-td" EmptyDataText="No Record Found"
                                                                                RowStyle-ForeColor="Black" HeaderStyle-ForeColor="#3c2b1b" PagerSettings-Mode="NumericFirstLast" 
                                                                                AllowPaging="false" AllowSorting="True" EmptyDataRowStyle-ForeColor="Red" EmptyDataRowStyle-HorizontalAlign="Center"
                                                    ShowFooter="false" OnRowDataBound="grdParentCategory_RowDataBound" OnRowCommand="grdParentCategory_RowCommand">
                                                    <Columns>

                                                         <asp:TemplateField HeaderText="&nbsp;Category Name">
                                                                    <ItemTemplate>

                                                                        <div style="cursor: pointer;" id="divproductlist" runat="server">
                                                                            <div style="float: left;">
                                                                                <%--<a href="javascript:expandcollapse('divchild<%# Eval("CategoryID") %>', 'one');">--%>
                                                                                <a href="javascript:void(0);">
                                                                                   <img class="icon-plus-sign" id="imgdiv<%# Eval("CategoryID") %>" title="Collapse" alt="Expand" src="/images/expand.gif" />
                                                                                </a>
                                                                            </div>
                                                                            &nbsp;&nbsp;<asp:Literal ID="lttitle" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"Name") %>'></asp:Literal>
                                                                            <span
                                                                                style="float: right;"></span>
                                                                            <input type="hidden" id="hdnCategoryID" runat="server" value='<%# DataBinder.Eval(Container.DataItem,"CategoryID") %>' />
                                                                            <asp:HiddenField ID="hdnPcategory" runat="server" Value='<%# (DataBinder.Eval(Container.DataItem,"CategoryID")) %>' />
                                                                        </div>
                                                                    </ItemTemplate>

                                                                    <HeaderStyle Width="37.5%" HorizontalAlign="left" VerticalAlign="Middle" CssClass="table-title" />
                                                                    <ItemStyle HorizontalAlign="left" />
                                                                </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Display&nbsp;Order" ItemStyle-HorizontalAlign="Center"
                                                                    ItemStyle-Width="50px" HeaderStyle-HorizontalAlign="left">
                                                                    <HeaderTemplate>
                                                                        <table cellpadding="0" cellspacing="1" border="0" align="center">
                                                                            <tr style="border: 0px;">
                                                                                <td style="border: 0px; padding: 0px; background-color: transparent;">
                                                                                    <strong>Inner Product Count</strong>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </HeaderTemplate>
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="txtDisplayOrder" runat="server" Text=' <%#Eval("DisplayOrder").ToString().Equals("999")?null:Eval("DisplayOrder")%>' Style="display: none;"></asp:Label>
                                                                        <center>
                                                                            <asp:Label ID="lbinv" runat="server" Text='<%#  DataBinder.Eval(Container.DataItem,"ProductCount") %>'></asp:Label></center>
                                                                    </ItemTemplate>
                                                                    <HeaderStyle HorizontalAlign="Left" Width="39.5%" />
                                                                    <ItemStyle HorizontalAlign="Center" Width="50px" />
                                                                </asp:TemplateField>

                                                         <asp:TemplateField HeaderText="Status" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="50px"
                                                                    HeaderStyle-HorizontalAlign="Center">
                                                                    <ItemTemplate>
                                                                        <asp:HiddenField ID="hdnActive" runat="server" Value='<%# (DataBinder.Eval(Container.DataItem,"Active")) %>' />
                                                                        <asp:Literal ID="ltrStatus" runat="server"></asp:Literal>
                                                                    </ItemTemplate>
                                                                    <HeaderStyle HorizontalAlign="Center" Width="11%" />
                                                                    <ItemStyle HorizontalAlign="Center" Width="50px" />
                                                                </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <tr>
                                                                    <td colspan="100%">

                                                                         <div id="divchild<%# Eval("CategoryID") %>" style="display: none; position: relative; left: 15px; overflow: auto; width: 96%; margin-top: 10px;margin-left:5px;">
                                                                            <asp:GridView ID="gvCategory" AutoGenerateColumns="false" runat="server" GridLines="None" DataKeyNames="CategoryID"
                                                                                Width="100%" CellPadding="2" CellSpacing="1" CssClass="border-td" EmptyDataText="No Record Found"
                                                                                RowStyle-ForeColor="Black" HeaderStyle-ForeColor="#3c2b1b" PagerSettings-Mode="NumericFirstLast" ShowFooter="true"
                                                                                AllowPaging="false" AllowSorting="True" EmptyDataRowStyle-ForeColor="Red" EmptyDataRowStyle-HorizontalAlign="Center"
                                                                                OnRowDataBound="gvCategory_RowDataBound" OnRowCommand="gvCategory_RowCommand" Visible="false"
                                                                                 PageSize="20">
                                                                                <EmptyDataTemplate>
                                                                                    <span style="color: Red; font-size: 12px; text-align: center;">No Record(s) Found !</span>
                                                                                </EmptyDataTemplate>
                                                                                <Columns>
                                                                                    <asp:TemplateField HeaderText="Select" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="0%" Visible="false">
                                                                                                <ItemTemplate>
                                                                                                    <asp:CheckBox ID="chkSelect" runat="server" CssClass="input-checkbox" />
                                                                                                    
                                                                                                </ItemTemplate>
                                                                                                <FooterTemplate>
                                                                                                    <div class="row-fluid" style="width: 185px;">
                                                                                                        <span><a id="lkbAllowAll" runat="server" class="btn btn-mini btn-info">Check All</a> | <a id="lkbClearAll" runat="server"
                                                                                                            class="btn btn-mini btn-info">Clear All</a> </span><span style="float: right;"></span>
                                                                                                    </div>
                                                                                                </FooterTemplate>
                                                                                                <HeaderStyle HorizontalAlign="Center" CssClass="tdpro-checkbox" Width="7%" />
                                                                                                <ItemStyle HorizontalAlign="Center" CssClass="tdpro-checkbox" />
                                                                                            </asp:TemplateField>
                                                                                            <asp:TemplateField HeaderText="Price($)" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                                                                <HeaderTemplate>
                                                                                                    Product Image
                                                                                                </HeaderTemplate>
                                                                                                <ItemTemplate>
                                                                                                    <asp:HiddenField ID="hdnCatid" runat="server" Value='<%#Eval("CategoryID") %>' />
                                                                                                    <asp:HiddenField ID="hdnProductID" runat="server" Value='<%#Eval("ProductID") %>' />
                                                                                                    <center>
                                                                                                        <asp:Image Style="width: 70PX !important; border: 1px solid #ccc; text-align: center;" ImageUrl='<%# GetIconImageProduct(Convert.ToString(Eval("ImageName"))) %>'
                                                                                                            ID="imgName"
                                                                                                            ToolTip='<%# (Convert.ToString(Eval("Name")))%>' runat="server" /></center>
                                                                                                </ItemTemplate>
                                                                                                <HeaderStyle HorizontalAlign="Right" Width="10%"></HeaderStyle>
                                                                                                <ItemStyle HorizontalAlign="Right" Width="10%"></ItemStyle>
                                                                                            </asp:TemplateField>
                                                                                            <asp:TemplateField HeaderText="Product Name" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="20%"
                                                                                                HeaderStyle-HorizontalAlign="Center">
                                                                                                <HeaderTemplate>
                                                                                                    <table cellpadding="0" cellspacing="1" border="0" align="left">
                                                                                                        <tr style="border: 0px;">
                                                                                                            <td style="border: 0px; padding: 0px; background-color: transparent;">
                                                                                                                <strong>ProductName</strong>
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                    </table>
                                                                                                </HeaderTemplate>
                                                                                                <ItemTemplate>
                                                                                                    <div style="text-align: justify; width: 300px;">
                                                                                                        <asp:Literal ID="lblProductName" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"Name") %>'></asp:Literal>
                                                                                                    </div>
                                                                                                </ItemTemplate>
                                                                                                <HeaderStyle HorizontalAlign="Left" />
                                                                                                <ItemStyle HorizontalAlign="Left" Width="20%" />
                                                                                            </asp:TemplateField>
                                                                                            <asp:TemplateField HeaderText="Display&nbsp;Order" ItemStyle-HorizontalAlign="Center"
                                                                                                ItemStyle-Width="50px" HeaderStyle-HorizontalAlign="Center">
                                                                                                <HeaderTemplate>
                                                                                                    <table cellpadding="0" cellspacing="1" border="0" align="left">
                                                                                                        <tr style="border: 0px;">
                                                                                                            <td style="border: 0px; padding: 0px; background-color: transparent;">
                                                                                                                <strong>Display&nbsp;Order</strong>
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                    </table>
                                                                                                </HeaderTemplate>
                                                                                                <ItemTemplate>
                                                                                                    <asp:TextBox ID="txtDisplayOrder" runat="server" Text='' style='display:none;'></asp:TextBox>
                                                                                                    <asp:Label ID="lblOldDisplayOrder" Text='<%#Eval("DisplayOrder").ToString().Equals("999")?null:Eval("DisplayOrder")%>' runat="server"></asp:Label>
                                                                                                </ItemTemplate>
                                                                                                <FooterTemplate>
                                                                                                    <asp:LinkButton CommandName="ChildAllSave" ID="btnSave" runat="server" CssClass="btn btn-mini btn-info" ToolTip="Save">Save</asp:LinkButton>
                                                                                                </FooterTemplate>
                                                                                                <HeaderStyle HorizontalAlign="Left" />
                                                                                                <ItemStyle HorizontalAlign="Center" Width="50px" />
                                                                                            </asp:TemplateField>
                                                                                            <asp:TemplateField HeaderText="Price($)" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                                                                <HeaderTemplate>
                                                                                                    Price($)
                                                                                                </HeaderTemplate>
                                                                                                <ItemTemplate>
                                                                                                    <asp:Label ID="lblprice" runat="server" Style="text-align: right; float: right;" Text='<%# string.Format("{0:0.00}",Convert.ToDecimal(DataBinder.Eval(Container.DataItem,"Price"))) %>'></asp:Label>
                                                                                                </ItemTemplate>
                                                                                                <HeaderStyle HorizontalAlign="Right" Width="10%" CssClass="right-1"></HeaderStyle>
                                                                                                <ItemStyle HorizontalAlign="Right" Width="10%"></ItemStyle>
                                                                                            </asp:TemplateField>
                                                                                            <asp:TemplateField HeaderText="Code" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                                                                <HeaderTemplate>
                                                                                                    Inventory
                                                                                                </HeaderTemplate>
                                                                                                <ItemTemplate>
                                                                                                    <asp:Label ID="lbinv" runat="server" Text='<%#  DataBinder.Eval(Container.DataItem,"Inventory") %>'></asp:Label>
                                                                                                </ItemTemplate>
                                                                                                <HeaderStyle HorizontalAlign="center" Width="10%" CssClass="centerimg"></HeaderStyle>
                                                                                                <ItemStyle HorizontalAlign="center" Width="10%" CssClass="centerimg"></ItemStyle>
                                                                                            </asp:TemplateField>
                                                                                            <asp:TemplateField HeaderText="Status" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="50px"
                                                                                                HeaderStyle-HorizontalAlign="Center">
                                                                                                <ItemTemplate>
                                                                                                    <asp:HiddenField ID="hdnActive" runat="server" Value='<%# (DataBinder.Eval(Container.DataItem,"Active")) %>' />
                                                                                                    <asp:Literal ID="ltrStatus" runat="server"></asp:Literal>
                                                                                                </ItemTemplate>
                                                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                                                <ItemStyle HorizontalAlign="Center" Width="50px" />
                                                                                            </asp:TemplateField>
                                                                                            <asp:TemplateField HeaderText="Edit" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="29px"
                                                                                                HeaderStyle-HorizontalAlign="Center" Visible="false">
                                                                                                <ItemTemplate>
                                                                                                    <asp:LinkButton CommandName="ChildSingleSave" ID="btnSingleSave" runat="server" CssClass="btn btn-mini btn-info" ToolTip="Save">Save</asp:LinkButton>
                                                                                                </ItemTemplate>
                                                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                                                <ItemStyle HorizontalAlign="Center" Width="29px" />
                                                                                            </asp:TemplateField>
                                                                                </Columns>
                                                                                <PagerSettings Mode="NumericFirstLast" Position="TopAndBottom"></PagerSettings>
                                                                                <PagerStyle HorizontalAlign="Right" CssClass="grid_paging" />
                                                                                <RowStyle HorizontalAlign="Center" CssClass="oddrow" VerticalAlign="Top" />
                                                                                <AlternatingRowStyle CssClass="altrow" VerticalAlign="Top" />
                                                                                <EmptyDataRowStyle ForeColor="Red" HorizontalAlign="Center" />
                                                                                <HeaderStyle ForeColor="White" Font-Bold="false" />
                                                                            </asp:GridView>
                                                                            <asp:Literal ID="ltrrepeater" runat="server"></asp:Literal>
                                                                        </div>
                                                                    </td>

                                                                </tr>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                    <PagerSettings Mode="NumericFirstLast" Position="TopAndBottom"></PagerSettings>
                                                    <PagerStyle HorizontalAlign="Right" CssClass="grid_paging" />
                                                    <RowStyle HorizontalAlign="Center" CssClass="oddrow" VerticalAlign="Top" />
                                                    <AlternatingRowStyle CssClass="altrow" VerticalAlign="Top" />
                                                    <EmptyDataRowStyle ForeColor="Red" HorizontalAlign="Center" />
                                                    <HeaderStyle ForeColor="White" Font-Bold="false" />
                                                </asp:GridView>
                                                         </div>

                                            </td>
                                        </tr>
                                            </ItemTemplate>
                                          </asp:TemplateField>
                                     </Columns>
                                                       <PagerSettings Mode="NumericFirstLast" Position="TopAndBottom"></PagerSettings>
                                                                                <PagerStyle HorizontalAlign="Right" CssClass="grid_paging" />
                                                                                <RowStyle HorizontalAlign="Center" CssClass="oddrow" VerticalAlign="Top" />
                                                                                <AlternatingRowStyle CssClass="altrow" VerticalAlign="Top" />
                                                                                <EmptyDataRowStyle ForeColor="Red" HorizontalAlign="Center" />
                                                                                <HeaderStyle ForeColor="White" Font-Bold="false" />
                            </asp:GridView>
                                                </td>
                                            </tr>
                                    </table>
                                </td>
                            </tr>


                             <tr style='display:none;'>
                                                        <td align="center"  >
                                                            <div id="divfloating" class="divfloatingcss" style="width: 300px;">
                                                                <div style="margin-bottom: 1px; margin-top: 3px;">
                                                                   <asp:Button ID="btnsave" runat="server"  OnClientClick="UpdateByTextBox();" />
                                                                   
                                                       <asp:Button  ID="btncancel" runat="server" OnClick="btncancel_Click"  />
                                                                </div>
                                                            </div>
                                                        </td>
                                                    </tr>

                        </table>
                    </td>
                </tr>
                <tr>
                    <td height="10" align="left" valign="top">
                        <img src="/App_Themes/<%=Page.Theme %>/images/spacer.gif" width="1" height="10">
                    </td>
                </tr>
                <tr>
                    <td>&nbsp;
                    </td>
                </tr>
            </table>
        </div>
    </div>
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
        <input type="hidden" id="hdnrowid" runat="server" value="" />
        <input type="hidden" id="hdnProwid" runat="server" value="" />
        <input type="hidden" id="hdngroupid" runat="server" value="" />
        <input type="hidden" id="hdngroupactiontype" runat="server" value="" />
        <input type="hidden" id="hdngroupname" runat="server" value="" />
        <input type="hidden" id="hdncatidforjs" runat="server" value="" />
        <input type="hidden" id="hdnRowcatidforjs" runat="server" value="" />
        <asp:Button ID="btnDivClick" runat="server" OnClick="btnDivClick_Click" />
        <input type="hidden" id="hdnCollspanMain" runat="server" value="" />
        <input type="hidden" id="hdnCollspanChild" runat="server" value="" />
       <div id="divCollsapnMain" runat="server"></div>
        <div id="divCollsapnChild" runat="server"></div>
    </div>
    <script language="javascript" type="text/javascript">

        function expandcollapseRow(obj, row) {

            document.getElementById('ContentPlaceHolder1_hdnCollspanMain').value = 'expandcollapseRow(' + obj + ',' + row + ')';
            var alldiv = document.getElementById('ContentPlaceHolder1_grdRowParentCategory').getElementsByTagName('div');
            // document.getElementById('ContentPlaceHolder1_hdnRowcatidforjs').value = obj.replace('divRowchild', '');
            //$("#divchild" + document.getElementById('ContentPlaceHolder1_hdncatidforjs').value + " ul").addClass('rep-drag');
            //$("#divRowchild" + document.getElementById('ContentPlaceHolder1_hdnRowcatidforjs').value + " tbody").addClass('rep-drag');
            //$("#ContentPlaceHolder1_grdRowParentCategory tbody.rep-drag").sortable({
            //    revert: false
            //});
            //$("#ContentPlaceHolder1_grdRowParentCategory tbody.rep-drag").disableSelection();
            //$("#ContentPlaceHolder1_grdRowParentCategory ul.rep-drag").sortable({
            //    revert: false
            //});
            //$("#ContentPlaceHolder1_grdRowParentCategory ul.rep-drag").disableSelection();
            if (alldiv != null && alldiv.length > 0) {
                for (var i = 0; i < alldiv.length; i++) {
                    var divall = alldiv[i];

                    if (divall.id == obj) {
                        var div = document.getElementById(obj);
                        var img = obj.toString().replace('divRowchild', 'imgdivRow'); //

                        if (div.style.display == "none") {
                            if (div.id.toString().toLowerCase().indexOf('divrowchild') > -1) {
                                div.style.display = "block";
                                document.getElementById('ContentPlaceHolder1_hdnProwid').value = obj;
                                if (row == 'alt') {
                                    //document.getElementById(img).removeAttribute("class");
                                    //document.getElementById(img).className = 'icon-minus-sign';
                                    document.getElementById(img).src = '/images/minimize.png';
                                }
                                else {
                                    //document.getElementById(img).removeAttribute("class");
                                    //document.getElementById(img).className = 'icon-minus-sign';
                                    document.getElementById(img).src = '/images/minimize.png';
                                }
                            }
                        }
                        else {
                            if (div.id.toString().toLowerCase().indexOf('divrowchild') > -1) {
                                div.style.display = "none";
                                document.getElementById('ContentPlaceHolder1_hdnProwid').value = "";
                                if (row == 'alt') {
                                    //document.getElementById(img).removeAttribute("class");
                                    //document.getElementById(img).className = 'icon-plus-sign';
                                    document.getElementById(img).src = '/images/expand.gif';
                                }
                                else {
                                    //document.getElementById(img).removeAttribute("class");
                                    //document.getElementById(img).className = 'icon-plus-sign';
                                    document.getElementById(img).src = '/images/expand.gif';
                                }
                            }

                        }
                    }
                    else if (divall.id != "" && divall.id != obj) {
                        if (divall.id.toString().toLowerCase().indexOf('divrowchild') > -1) {
                            var imgid = divall.id.toString().replace('divRowchild', 'imgdivRow');
                            var img = document.getElementById(imgid);
                            divall.style.display = "none";
                            if (row == 'alt') {
                                //img.removeAttribute("class");
                                //img.className = 'icon-plus-sign';
                                img.src = '/images/expand.gif';
                            }
                            else {
                                //img.removeAttribute("class");
                                //img.className = 'icon-plus-sign';
                                img.src = '/images/expand.gif';
                            }
                        }
                    }
                }
            }
        }



        function expandcollapse(obj, row) {

            document.getElementById('ContentPlaceHolder1_hdnCollspanChild').value = 'expandcollapse(' + obj + ',' + row + ')';
            var alldiv = document.getElementById('ContentPlaceHolder1_grdRowParentCategory_grdParentCategory_' + row).getElementsByTagName('div');
            document.getElementById('ContentPlaceHolder1_hdncatidforjs').value = obj.replace('divchild', '');
            //$("#divchild" + document.getElementById('ContentPlaceHolder1_hdncatidforjs').value + " ul").addClass('rep-drag');
            //$("#divchild" + document.getElementById('ContentPlaceHolder1_hdncatidforjs').value + " tbody").addClass('rep-drag');
            //$("#ContentPlaceHolder1_grdRowParentCategory_grdParentCategory_"+row+" tbody.rep-drag").sortable({
            //    revert: false
            //});
            //$("#ContentPlaceHolder1_grdRowParentCategory_grdParentCategory_" + row + " tbody.rep-drag").disableSelection();
            //$("#ContentPlaceHolder1_grdRowParentCategory_grdParentCategory_" + row + " ul.rep-drag").sortable({
            //    revert: false
            //});
            //$("#ContentPlaceHolder1_grdRowParentCategory_grdParentCategory_" + row + " ul.rep-drag").disableSelection();
            if (alldiv != null && alldiv.length > 0) {
                for (var i = 0; i < alldiv.length; i++) {
                    var divall = alldiv[i];

                    if (divall.id == obj) {
                        if (divall.id.toString().toLowerCase().indexOf('divproductlist') <= -1) {


                            var div = document.getElementById(obj);
                            var img = obj.toString().replace('divchild', 'imgdiv'); //

                            if (div.style.display == "none") {
                                div.style.display = "block";
                                document.getElementById('ContentPlaceHolder1_hdnrowid').value = obj;
                                if (row == 'alt') {
                                    //document.getElementById(img).removeAttribute("class");
                                    //document.getElementById(img).className = 'icon-minus-sign';
                                    document.getElementById(img).src = '/images/minimize.png';
                                }
                                else {
                                    //document.getElementById(img).removeAttribute("class");
                                    //document.getElementById(img).className = 'icon-minus-sign';
                                    document.getElementById(img).src = '/images/minimize.png';
                                }
                            }
                            else {
                                div.style.display = "none";
                                document.getElementById('ContentPlaceHolder1_hdnrowid').value = "";
                                if (row == 'alt') {
                                    //document.getElementById(img).removeAttribute("class");
                                    //document.getElementById(img).className = 'icon-plus-sign';
                                    document.getElementById(img).src = '/images/expand.gif';
                                }
                                else {
                                    //document.getElementById(img).removeAttribute("class");
                                    //document.getElementById(img).className = 'icon-plus-sign';
                                    document.getElementById(img).src = '/images/expand.gif';
                                }
                            }
                        }
                    }
                    else if (divall.id != "" && divall.id != obj) {
                        if (divall.id.toString().toLowerCase().indexOf('divproductlist') <= -1) {
                            var imgid = divall.id.toString().replace('divchild', 'imgdiv');
                            var img = document.getElementById(imgid);
                            divall.style.display = "none";
                            if (row == 'alt') {
                                //img.removeAttribute("class");
                                //img.className = 'icon-plus-sign';
                                img.src = '/images/expand.gif';
                            }
                            else {
                                //img.removeAttribute("class");
                                //img.className = 'icon-plus-sign';
                                img.src = '/images/expand.gif';
                            }
                        }
                    }
                }
            }
            var $jjjj = jQuery.noConflict();
            $jjjj(document).ready(function () {
                var st = $jjjj("#divchild" + document.getElementById('ContentPlaceHolder1_hdncatidforjs').value + " .rep-drag li").attr('style');
                var hh = $jjjj("#divchild" + document.getElementById('ContentPlaceHolder1_hdncatidforjs').value + " .rep-drag li").innerHeight();
                hh = parseInt(hh) - parseInt(10);
                st = st.replace('border:dashed 1px #454545;', '') + ''
                $jjjj("#divchild" + document.getElementById('ContentPlaceHolder1_hdncatidforjs').value + " .rep-drag").dragsort({ dragSelector: "li", dragEnd: updatenn, placeHolderTemplate: "<li class='placeHolder' style='" + st + "'></li>" });
            });
        }

        function selectAll(on) {
            var myform = document.forms[0];
            var len = myform.elements.length;
            for (var i = 0; i < len; i++) {
                if (myform.elements[i].type == 'checkbox') {
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

        function selectAllGrid(on, id) {
            var myform = document.getElementById(id).getElementsByTagName('input');
            var len = myform.length;
            for (var i = 0; i < len; i++) {
                if (myform[i].type == 'checkbox') {
                    if (on.toString() == 'false') {
                        if (myform[i].checked) {
                            myform[i].checked = false;
                        }
                    }
                    else {
                        myform[i].checked = true;
                    }
                }
            }
        }

        function SetBlank(id) {
            var myform = document.getElementById(id).getElementsByTagName('input');
            var len = myform.length;
            for (var i = 0; i < len; i++) {
                if (myform[i].type == 'text') {
                    var chkid = myform[i].id.toString().replace('_txtDisplayOrder_', '_chkSelect_');
                    if (document.getElementById(chkid).checked && myform[i].value == '') {
                        jAlert('PLease enter display order', 'Required', myform[i].id.toString());
                        return false;
                    }
                }
            }
            return true;
        }

        function chekcategoryinsert() {
            var myvalue = document.getElementById('ContentPlaceHolder1_txtcategoryinsert').value;
            if (myvalue == '' || myvalue == null) {
                //alert("af");
                // alert('PLease enter category name.');
                jAlert('Please Enter category name.', 'Message', '<%=txtcategoryinsert.ClientID %>');
                return false;
            }
            chkHeight();
            return true;
        }

        function checkCount(id) {
            var myform = document.getElementById(id).getElementsByTagName('input');
            var len = myform.length;
            var count = 0;
            for (var i = 0; i < len; i++) {
                if (myform[i].type == 'checkbox' && myform[i].checked == true) {
                    count++;
                }
            }
            if (count == 0) {
                jAlert('Check at least One Record!', 'Message');
                return false;
            }
            if (SetBlank(id)) {
                return true;
            }
            else {
                return false;
            }
            return true;
        }

        function checkaa() {
            jConfirm('Are you sure want to delete all selected Category ?', 'Confirmation', function (r) {
                if (r == true) {
                    document.getElementById('ContentPlaceHolder1_btnDeleteTemp').click();
                    return true;
                }
                else {
                    return false;
                }
            });
            return false;
        }

    </script>
</asp:Content>
