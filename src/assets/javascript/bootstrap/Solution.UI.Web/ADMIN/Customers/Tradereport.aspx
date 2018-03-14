<%@ Page Title="" Language="C#" MasterPageFile="~/ADMIN/Admin.Master" AutoEventWireup="true" CodeBehind="Tradereport.aspx.cs" Inherits="Solution.UI.Web.ADMIN.Customers.Tradereport" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts-main.js"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-calender-custom.js"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-calender.js"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts.js"></script>
    <%-- <link href="/Admin/Jqgrid-admin/admin-jqx.base.css" rel="stylesheet" />--%>
    <link rel="stylesheet" href="/admin/Jqgrid-admin/jqx.base.css" type="text/css" />
    <%--<script type="text/javascript" src="/admin/Jqgrid-admin/jquery-1.10.2.min.js"></script>--%>
    <script type="text/javascript" src="/admin/Jqgrid-admin/jqxcore.js"></script>
    <script type="text/javascript" src="/admin/Jqgrid-admin/jqxdata.js"></script>
    <script type="text/javascript" src="/admin/Jqgrid-admin/jqxbuttons.js"></script>
    <script type="text/javascript" src="/admin/Jqgrid-admin/jqxscrollbar.js"></script>
    <script type="text/javascript" src="/admin/Jqgrid-admin/jqxmenu.js"></script>
    <script type="text/javascript" src="/admin/Jqgrid-admin/jqxcheckbox.js"></script>
    <script type="text/javascript" src="/admin/Jqgrid-admin/jqxlistbox.js"></script>
    <script type="text/javascript" src="/admin/Jqgrid-admin/jqxdropdownlist.js"></script>
    <script type="text/javascript" src="/admin/Jqgrid-admin/jqxgrid.js"></script>
    <script type="text/javascript" src="/admin/Jqgrid-admin/jqxgrid.sort.js"></script>
    <script type="text/javascript" src="/admin/Jqgrid-admin/jqxdata.js"></script>
    <script type="text/javascript" src="/admin/Jqgrid-admin/jqxdata.export.js"></script>
    <script type="text/javascript" src="/admin/Jqgrid-admin/jqxgrid.export.js"></script>

    <script type="text/javascript" src="/admin/Jqgrid-admin/jqxscrollbar.js"></script>
    <script type="text/javascript" src="/admin/Jqgrid-admin/jqxgrid.filter.js"></script>
    <script type="text/javascript" src="/admin/Jqgrid-admin/jqxgrid.pager.js"></script>
    <script type="text/javascript" src="/admin/Jqgrid-admin/jqxgrid.selection.js"></script>
    <script type="text/javascript" src="/admin/Jqgrid-admin/jqxgrid.edit.js"></script>
    <%-- <script type="text/javascript" src="/admin/Jqgrid-admin/demos.js"></script>--%>



    <script type="text/javascript" src="/admin/Jqgrid-admin/jqxdatatable.js"></script>
    <script type="text/javascript" src="/admin/Jqgrid-admin/jqxtreegrid.js"></script>

    <script type="text/javascript" src="/admin/Jqgrid-admin/jqxwindow.js"></script>



    <script type="text/javascript">
        $(document).ready(function () {
            Getreportdata();

            //$(function () {

           <%-- $('#ContentPlaceHolder1_txtMailFrom').datetimepicker({
                showButtonPanel: true, ampm: false,
                showHour: false, showMinute: false, showSecond: false, showTime: false, showOn: "button",
                buttonImage: "/App_Themes/<%=Page.Theme %>/images/date-icon.png", buttonImageOnly: true
            });
            $('#ContentPlaceHolder1_txtMailTo').datetimepicker({
                showButtonPanel: true, ampm: false,
                showHour: false, showMinute: false, showSecond: false, showTime: false, showOn: "button",
                buttonImage: "/App_Themes/<%=Page.Theme %>/images/date-icon.png", buttonImageOnly: true
            });--%>

            //});

        });



        function Getreportdataall() {


            $.ajax({
                url: '/TestMail.aspx/gettradereport',

                type: 'POST',
                dataType: 'json',

                data: "{fromdate: '" + document.getElementById("ContentPlaceHolder1_txtMailFrom").value + "',todate: '" + document.getElementById("ContentPlaceHolder1_txtMailTo").value + "',search: '" + document.getElementById("ContentPlaceHolder1_txtSearch").value + "',flag:'1'}",
                contentType: 'application/json; charset=utf-8',
                success: function (data) {
                    var obj = JSON.parse(data.d);
                    // prepare the data
                    var source =
                     {
                         datatype: "json",
                         datafields: [
                             { name: 'Email', type: 'string' },
                             { name: 'Name', type: 'string' },
                             { name: 'Registered Date', type: 'date' },
                             { name: 'CouponCode', type: 'string' },
                             { name: 'Total Order', type: 'int' },
                             { name: 'Total Amount', type: 'string' },
                             { name: 'istaxable', type: 'string' },
                             { name: 'OrderDate', type: 'date' },
                              { name: 'CustomerID', type: 'int' }

                         ],
                         id: 'CustomerID',
                         localdata: obj
                     };



                    var actionlink = function (row, columnfield, value, defaulthtml, columnproperties, rowdata) {

                        var actionLink = '';
                        var CustomerID = $("#Equipmentlist").jqxGrid('getrowdata', row).CustomerID;

                        var Email = $("#Equipmentlist").jqxGrid('getrowdata', row).Email;


                        actionLink = '<a id="btnedit_' + row + '" style="cursor:pointer;" class="edit-row" href="/Admin/Customers/Customer.aspx?mode=edit&CustID=' + CustomerID + '">' + Email + '</a>'

                        return actionLink;
                    }
                    var aviSrNo = function (row, columnfield, value, defaulthtml, columnproperties, rowdata) {

                        var count = parseInt(row) + parseInt(1);

                        return "<div style=\"padding-left:10px\"><span class=\"font-12\">" + count + "</span></div>";
                    }




                    var actionHeader = function (value) {
                        return '<div style="text-align: center;">' + value + '</div>';
                    }
                    var dataAdapter = new $.jqx.dataAdapter(source, {
                        downloadComplete: function (data, status, xhr) { },
                        loadComplete: function (data) { },
                        loadError: function (xhr, status, error) { }
                    });
                    var headerCenter = function (value) {
                        return '<div style="text-align: center; padding: 17px 15px 0 0;">' + value + '</div>';
                    }

                    //, renderer: headerCenter
                    $("#Equipmentlist").jqxGrid(
                         {
                             source: dataAdapter,
                             pagesizeoptions: ["5", "10", "20", "50", "100"],
                             pagesize: 10,
                             pageable: true,
                             //autowidth: true,
                             width: "100%",
                             // height: "100%",
                             autoheight: true,
                             sortable: true,
                             rowsheight: 35,
                             altrows: false,
                             columnsresize: false,
                             enabletooltips: true,
                             editable: false,
                             //showaggregates: true,
                             selectionmode: 'singlerow',
                             //selectionmode: 'checkbox',
                             pagermode: 'simple',
                             filterable: true,

                             // showstatusbar: true,
                             renderstatusbar: function (statusbar) {
                                 // appends buttons to the status bar.
                                 var container = $("<div style='overflow: hidden; position: relative; margin: 5px;'></div>");
                                 //var addButton = $("<div style='float: left; margin-left: 5px;'><img style='position: relative; margin-top: 2px;' src='../../images/add.png'/><span style='margin-left: 4px; position: relative; top: -3px;'>Add</span></div>");
                                 //var deleteButton = $("<div style='float: left; margin-left: 5px;'><img style='position: relative; margin-top: 2px;' src='../../images/close.png'/><span style='margin-left: 4px; position: relative; top: -3px;'>Delete</span></div>");
                                 //var reloadButton = $("<div style='float: left; margin-left: 5px;'><img style='position: relative; margin-top: 2px;' src='../../images/refresh.png'/><span style='margin-left: 4px; position: relative; top: -3px;'>Reload</span></div>");
                                 var searchButton = $("<div style='float: left; margin-left: 5px;'><img style='position: relative; margin-top: 2px;' src='../../images/search-button.png'/><span style='margin-left: 4px; position: relative; top: 1px;font-weight: normal;color: #333333;'>Find</span></div>");
                                 $('#mediaadd').html(searchButton);
                                 //  container.append(searchButton);
                                 //statusbar.append(container);

                                 searchButton.jqxButton({ width: 50, height: 20 });

                                 // search for a record.
                                 searchButton.click(function (event) {
                                     var offset = $("#Equipmentlist").offset();
                                     $("#jqxwindow").jqxWindow('open');
                                     $("#jqxwindow").jqxWindow('move', offset.left + 30, offset.top + 30);
                                 });
                             },

                             columns: [
                               //{ text: 'SrNo.', columngroup: 'userDetails', cellsrenderer: aviSrNo, width: '8%', cellsalign: 'center', filterable: false, sortable: false, menu: false },
                                 { text: 'Email', columngroup: 'userDetails', datafield: 'Email', width: '20%', cellsrenderer: actionlink },
                                  { text: 'Name', columngroup: 'userDetails', datafield: 'Name', width: '20%' },
                                   { text: 'Registered Date', columngroup: 'userDetails', datafield: 'Registered Date', width: '10%', cellsformat: 'dd-MM-yyyy' },
                                    { text: 'CouponCode', columngroup: 'userDetails', datafield: 'CouponCode', width: '10%' },
                                     { text: 'Total Order', columngroup: 'userDetails', datafield: 'Total Order', width: '10%' },
                                      { text: 'Is Non Taxable', columngroup: 'userDetails', datafield: 'Is Non Taxable', width: '10%' },
                                { text: 'Total Amount', columngroup: 'userDetails', width: '10%', datafield: 'Total Amount', cellsalign: 'center' },
                                  { text: 'OrderDate', columngroup: 'userDetails', width: '10%', datafield: 'OrderDate', cellsalign: 'center', cellsformat: 'dd-MM-yyyy' }

                             ]
                         });

                    //$("#excelExport").jqxButton({ theme: theme });
                    $("#excelExport").click(function () {
                        $("#Equipmentlist").jqxGrid('exportdata', 'csv', 'jqxGrid');
                    });

                    // create jqxWindow.
                    $("#jqxwindow").jqxWindow({ resizable: false, autoOpen: false, width: 210, height: 180, draggable: true, isModal: false, showCollapseButton: true });
                    // create find and clear buttons.
                    $("#findButton").jqxButton({ width: 70 });
                    $("#clearButton").jqxButton({ width: 70 });

                    $("#dropdownlist").jqxDropDownList({
                        autoDropDownHeight: true, selectedIndex: 0, width: 200, height: 23,
                        source: [
                            'Name'
                        ]
                    });


                    // clear filters.
                    $("#clearButton").click(function () {
                        $("#Equipmentlist").jqxGrid('clearfilters');
                    });

                    $('#Equipmentlist').jqxGrid('showcolumn', 'Image');

                    //  find records that match a criteria.
                    $("#findButton").click(function () {
                        $("#Equipmentlist").jqxGrid('clearfilters');
                        var searchColumnIndex = $("#dropdownlist").jqxDropDownList('selectedIndex');
                        var datafield = "";
                        switch (searchColumnIndex) {
                            case 0:
                                datafield = "Name";
                                break;
                                //case 1:
                                //    datafield = "VendorName";
                                //    break;


                        }

                        var searchText = $("#inputField").val();
                        var filtergroup = new $.jqx.filter();
                        var filter_or_operator = 1;
                        var filtervalue = searchText;
                        var filtercondition = 'contains';
                        var filter = filtergroup.createfilter('stringfilter', filtervalue, filtercondition);
                        filtergroup.addfilter(filter_or_operator, filter);
                        $("#Equipmentlist").jqxGrid('addfilter', datafield, filtergroup);
                        // apply the filters.
                        $("#Equipmentlist").jqxGrid('applyfilters');
                    });

                    $("#Equipmentlist").bind('cellclick', function (event) {
                        var columnheader = $("#Equipmentlist").jqxGrid('getcolumn', event.args.datafield).text;
                        if (columnheader == 'Action') {
                            $("#Equipmentlist").jqxGrid('selectrow', event.args.rowindex);
                            var scrollTop = $(window).scrollTop();
                            var scrollLeft = $(window).scrollLeft();
                            contextMenu.jqxMenu('open', parseInt(event.args.originalEvent.clientX) + scrollLeft - 70, parseInt(event.args.originalEvent.clientY) + 8 + scrollTop);
                            return false;
                        }
                    });
                    $('#ContentPlaceHolder1_txtMailFrom').datetimepicker({
                        showButtonPanel: true, ampm: false,
                        showHour: false, showMinute: false, showSecond: false, showTime: false, showOn: "button",
                        buttonImage: "/App_Themes/<%=Page.Theme %>/images/date-icon.png", buttonImageOnly: true
                    });
                    $('#ContentPlaceHolder1_txtMailTo').datetimepicker({
                        showButtonPanel: true, ampm: false,
                        showHour: false, showMinute: false, showSecond: false, showTime: false, showOn: "button",
                        buttonImage: "/App_Themes/<%=Page.Theme %>/images/date-icon.png", buttonImageOnly: true
                    });

                }
            });

        }



        function Getreportdata() {


            $.ajax({
                url: '/TestMail.aspx/gettradereport',

                type: 'POST',
                dataType: 'json',

                data: "{fromdate: '" + document.getElementById("ContentPlaceHolder1_txtMailFrom").value + "',todate: '" + document.getElementById("ContentPlaceHolder1_txtMailTo").value + "',search: '" + document.getElementById("ContentPlaceHolder1_txtSearch").value + "',flag:'" + document.getElementById("ContentPlaceHolder1_hdnflag").value + "'}",
                contentType: 'application/json; charset=utf-8',
                success: function (data) {
                    var obj = JSON.parse(data.d);
                    // prepare the data
                    var source =
                     {
                         datatype: "json",
                         datafields: [
                             { name: 'Email', type: 'string' },
                             { name: 'Name', type: 'string' },
                             { name: 'Registered Date', type: 'date' },
                             { name: 'CouponCode', type: 'string' },
                             { name: 'Total Order', type: 'int' },
                             { name: 'Total Amount', type: 'string' },
                             { name: 'OrderDate', type: 'date' },
                              { name: 'CustomerID', type: 'int' }

                         ],
                         id: 'CustomerID',
                         localdata: obj
                     };



                    var actionlink = function (row, columnfield, value, defaulthtml, columnproperties, rowdata) {

                        var actionLink = '';
                        var CustomerID = $("#Equipmentlist").jqxGrid('getrowdata', row).CustomerID;

                        var Email = $("#Equipmentlist").jqxGrid('getrowdata', row).Email;


                        actionLink = '<a id="btnedit_' + row + '" style="cursor:pointer;" class="edit-row" href="/Admin/Customers/Customer.aspx?mode=edit&CustID=' + CustomerID + '">' + Email + '</a>'

                        return actionLink;
                    }
                    var aviSrNo = function (row, columnfield, value, defaulthtml, columnproperties, rowdata) {

                        var count = parseInt(row) + parseInt(1);

                        return "<div style=\"padding-left:10px\"><span class=\"font-12\">" + count + "</span></div>";
                    }




                    var actionHeader = function (value) {
                        return '<div style="text-align: center;">' + value + '</div>';
                    }
                    var dataAdapter = new $.jqx.dataAdapter(source, {
                        downloadComplete: function (data, status, xhr) { },
                        loadComplete: function (data) { },
                        loadError: function (xhr, status, error) { }
                    });
                    var headerCenter = function (value) {
                        return '<div style="text-align: center; padding: 17px 15px 0 0;">' + value + '</div>';
                    }

                    //, renderer: headerCenter
                    $("#Equipmentlist").jqxGrid(
                         {
                             source: dataAdapter,
                             pagesizeoptions: ["5", "10", "20", "50", "100"],
                             pagesize: 10,
                             pageable: true,
                             //autowidth: true,
                             width: "100%",
                             // height: "100%",
                             autoheight: true,
                             sortable: true,
                             rowsheight: 35,
                             altrows: false,
                             columnsresize: false,
                             enabletooltips: true,
                             editable: false,
                             //showaggregates: true,
                             selectionmode: 'singlerow',
                             //selectionmode: 'checkbox',
                             pagermode: 'simple',
                             filterable: true,

                             // showstatusbar: true,
                             renderstatusbar: function (statusbar) {
                                 // appends buttons to the status bar.
                                 var container = $("<div style='overflow: hidden; position: relative; margin: 5px;'></div>");
                                 //var addButton = $("<div style='float: left; margin-left: 5px;'><img style='position: relative; margin-top: 2px;' src='../../images/add.png'/><span style='margin-left: 4px; position: relative; top: -3px;'>Add</span></div>");
                                 //var deleteButton = $("<div style='float: left; margin-left: 5px;'><img style='position: relative; margin-top: 2px;' src='../../images/close.png'/><span style='margin-left: 4px; position: relative; top: -3px;'>Delete</span></div>");
                                 //var reloadButton = $("<div style='float: left; margin-left: 5px;'><img style='position: relative; margin-top: 2px;' src='../../images/refresh.png'/><span style='margin-left: 4px; position: relative; top: -3px;'>Reload</span></div>");
                                 var searchButton = $("<div style='float: left; margin-left: 5px;'><img style='position: relative; margin-top: 2px;' src='../../images/search-button.png'/><span style='margin-left: 4px; position: relative; top: 1px;font-weight: normal;color: #333333;'>Find</span></div>");
                                 $('#mediaadd').html(searchButton);
                                 //  container.append(searchButton);
                                 //statusbar.append(container);

                                 searchButton.jqxButton({ width: 50, height: 20 });

                                 // search for a record.
                                 searchButton.click(function (event) {
                                     var offset = $("#Equipmentlist").offset();
                                     $("#jqxwindow").jqxWindow('open');
                                     $("#jqxwindow").jqxWindow('move', offset.left + 30, offset.top + 30);
                                 });
                             },

                             columns: [
                               //{ text: 'SrNo.', columngroup: 'userDetails', cellsrenderer: aviSrNo, width: '8%', cellsalign: 'center', filterable: false, sortable: false, menu: false },
                                 { text: 'Email', columngroup: 'userDetails', datafield: 'Email', width: '30%', cellsrenderer: actionlink },
                                  { text: 'Name', columngroup: 'userDetails', datafield: 'Name', width: '20%' },
                                   { text: 'Registered Date', columngroup: 'userDetails', datafield: 'Registered Date', width: '10%', cellsformat: 'dd-MM-yyyy' },
                                    { text: 'CouponCode', columngroup: 'userDetails', datafield: 'CouponCode', width: '10%' },
                                     { text: 'Total Order', columngroup: 'userDetails', datafield: 'Total Order', width: '10%' },

                                { text: 'Total Amount', columngroup: 'userDetails', width: '10%', datafield: 'Total Amount', cellsalign: 'center' },
                                  { text: 'OrderDate', columngroup: 'userDetails', width: '10%', datafield: 'OrderDate', cellsalign: 'center', cellsformat: 'dd-MM-yyyy' }

                             ]
                         });

                    //$("#excelExport").jqxButton({ theme: theme });
                    $("#excelExport").click(function () {
                        $("#Equipmentlist").jqxGrid('exportdata', 'xls', 'jqxGrid');
                    });

                    // create jqxWindow.
                    $("#jqxwindow").jqxWindow({ resizable: false, autoOpen: false, width: 210, height: 180, draggable: true, isModal: false, showCollapseButton: true });
                    // create find and clear buttons.
                    $("#findButton").jqxButton({ width: 70 });
                    $("#clearButton").jqxButton({ width: 70 });

                    $("#dropdownlist").jqxDropDownList({
                        autoDropDownHeight: true, selectedIndex: 0, width: 200, height: 23,
                        source: [
                            'Name'
                        ]
                    });


                    // clear filters.
                    $("#clearButton").click(function () {
                        $("#Equipmentlist").jqxGrid('clearfilters');
                    });

                    $('#Equipmentlist').jqxGrid('showcolumn', 'Image');

                    //  find records that match a criteria.
                    $("#findButton").click(function () {
                        $("#Equipmentlist").jqxGrid('clearfilters');
                        var searchColumnIndex = $("#dropdownlist").jqxDropDownList('selectedIndex');
                        var datafield = "";
                        switch (searchColumnIndex) {
                            case 0:
                                datafield = "Name";
                                break;
                                //case 1:
                                //    datafield = "VendorName";
                                //    break;


                        }

                        var searchText = $("#inputField").val();
                        var filtergroup = new $.jqx.filter();
                        var filter_or_operator = 1;
                        var filtervalue = searchText;
                        var filtercondition = 'contains';
                        var filter = filtergroup.createfilter('stringfilter', filtervalue, filtercondition);
                        filtergroup.addfilter(filter_or_operator, filter);
                        $("#Equipmentlist").jqxGrid('addfilter', datafield, filtergroup);
                        // apply the filters.
                        $("#Equipmentlist").jqxGrid('applyfilters');
                    });

                    $("#Equipmentlist").bind('cellclick', function (event) {
                        var columnheader = $("#Equipmentlist").jqxGrid('getcolumn', event.args.datafield).text;
                        if (columnheader == 'Action') {
                            $("#Equipmentlist").jqxGrid('selectrow', event.args.rowindex);
                            var scrollTop = $(window).scrollTop();
                            var scrollLeft = $(window).scrollLeft();
                            contextMenu.jqxMenu('open', parseInt(event.args.originalEvent.clientX) + scrollLeft - 70, parseInt(event.args.originalEvent.clientY) + 8 + scrollTop);
                            return false;
                        }
                    });
                    $('#ContentPlaceHolder1_txtMailFrom').datetimepicker({
                        showButtonPanel: true, ampm: false,
                        showHour: false, showMinute: false, showSecond: false, showTime: false, showOn: "button",
                        buttonImage: "/App_Themes/<%=Page.Theme %>/images/date-icon.png", buttonImageOnly: true
                    });
                    $('#ContentPlaceHolder1_txtMailTo').datetimepicker({
                        showButtonPanel: true, ampm: false,
                        showHour: false, showMinute: false, showSecond: false, showTime: false, showOn: "button",
                        buttonImage: "/App_Themes/<%=Page.Theme %>/images/date-icon.png", buttonImageOnly: true
                    });

                }
            });

        }

    </script>


    <script type="text/javascript">



        function SearchValidation(id) {

            if (id == 0) {
                //                if (document.getElementById('ContentPlaceHolder1_ddlSearch').selectedIndex == 0) {
                //                    jAlert('Please Select Search By.', 'Required Information', 'ContentPlaceHolder1_ddlSearch');
                //                    //document.getElementById('ContentPlaceHolder1_ddlSearch').focus();
                //                    return false;
                //                }
                //                if (document.getElementById('ContentPlaceHolder1_txtSearch').value == '') {
                //                    jAlert('Please Enter Search Keyword.', 'Required Information', 'ContentPlaceHolder1_txtSearch');
                //                    //document.getElementById('ContentPlaceHolder1_txtSearch').focus();
                //                    return false;
                //                }
                //                else if (document.getElementById('ContentPlaceHolder1_txtSearch').value != '' && (document.getElementById('ContentPlaceHolder1_txtSearch').value.toString().toLowerCase() == 'search keyword' || document.getElementById('ContentPlaceHolder1_txtSearch').value.toString().toLowerCase() == 'searchkeyword')) {
                //                    jAlert('Please Enter Search Keyword.', 'Required Information', 'ContentPlaceHolder1_txtSearch');
                //                    //document.getElementById('ContentPlaceHolder1_txtSearch').focus();
                //                    return false;
                //                }

            }
            if (document.getElementById('ContentPlaceHolder1_txtMailFrom').value == '') {
                jAlert('Please Enter From Date.', 'Required Information', 'ContentPlaceHolder1_txtMailFrom');
                //document.getElementById('ContentPlaceHolder1_txtMailFrom').focus();
                return false;
            }
            else if (document.getElementById('ContentPlaceHolder1_txtMailTo').value == '') {
                jAlert('Please Enter To Date.', 'Required Information', 'ContentPlaceHolder1_txtMailTo');
                // document.getElementById('ContentPlaceHolder1_txtMailTo').focus();
                return false;
            }

            var startDate = new Date(document.getElementById('ContentPlaceHolder1_txtMailFrom').value);
            var endDate = new Date(document.getElementById('ContentPlaceHolder1_txtMailTo').value);
            if (startDate > endDate) {
                jAlert('Please Select Valid Date.', 'Required Information', 'ContentPlaceHolder1_txtMailTo');
                return false;
            }
            document.getElementById('ContentPlaceHolder1_hdnflag').Value = "0";
            Getreportdata();
            return false;
        }


        function setflag() {
            document.getElementById('ContentPlaceHolder1_hdnflag').Value = "1";

            Getreportdataall();
            return false;
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
        function chkSelectresend() {
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
                jAlert("Please select at least one row.", "Message");
                return false;

            }

            return true;

        }
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
                jAlert("Please select at least one Mail.", "Message");
                return false;

            }
            else {
                jConfirm('Are you sure want to delete all selected Mail ?', 'Confirmation', function (r) {
                    if (r == true) {

                        document.getElementById('ContentPlaceHolder1_btnDeleteTemp').click();
                        return true;
                    }
                    else {

                        return false;
                    }
                });
            }
            return false;

        }
        function OpenCenterWindow(pid, wi, he) {
            var w = wi;
            var h = he;
            var left = (screen.width / 2) - (w / 2);
            var top = (screen.height / 2) - (h / 2);
            window.open(pid, '', 'toolbar=no, location=no, directories=no, status=no, menubar=no, scrollbars=yes, resizable=no, copyhistory=no, width=' + w + ', height=' + h + ', top=' + top + ', left=' + left);
        }


    </script>
    <div id="content-width">
        <div class="content-row1">
            <div class="create-new-order" style="width: 100%;">
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
                                            <th colspan="3">
                                                <div class="main-title-left">
                                                    <img class="img-left" title="Trade Report" alt="Trade Report" src="/App_Themes/<%=Page.Theme %>/Images/mail-log-icon.png" />
                                                    <h2>Trade Report</h2>
                                                </div>
                                            </th>
                                        </tr>
                                        <tr class="altrow">

                                            <td align="left"></td>
                                            <td align="right" colspan="2">
                                                <table cellpadding="3" cellspacing="3">
                                                    <tr>
                                                        <td valign="middle" align="right">From Date:
                                                        </td>
                                                        <td valign="middle" align="right">
                                                            <asp:TextBox ID="txtMailFrom" runat="server" CssClass="from-textfield" Width="70px"
                                                                Style="margin-right: 3px;"></asp:TextBox>
                                                        </td>
                                                        <td valign="middle" align="right">To Date:
                                                        </td>
                                                        <td valign="middle" align="right">
                                                            <asp:TextBox ID="txtMailTo" runat="server" CssClass="from-textfield" Width="70px"
                                                                Style="margin-right: 3px;"></asp:TextBox>
                                                            <asp:HiddenField ID="hdnflag" runat="server" Value="0" />
                                                        </td>
                                                        <td align="left" style="width: 65px;">Search By :
                                                        </td>

                                                        <td align="left">
                                                            <asp:TextBox ID="txtSearch"
                                                                CssClass="order-textfield" Width="124px" runat="server"></asp:TextBox>
                                                        </td>
                                                        <td align="right">
                                                            <asp:Button ID="btnSearch" runat="server" OnClick="btnSearch_Click" OnClientClick="return SearchValidation(0);" />
                                                        </td>
                                                        <td align="right">
                                                            <asp:Button ID="btnshowall" runat="server" OnClientClick="return setflag();" />
                                                        </td>

                                                        <td align="right">
                                                            <input type="button" value="" id='excelExport' style="background-image: url(/App_Themes/gray/images/export.gif); width: 61px; height: 23px; border: none;" />
                                                            <asp:ImageButton ID="btnexport" Style="display: none;" runat="server" ImageUrl="/App_Themes/gray/images/export.gif" OnClick="btnexport_Click" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr class="even-row">
                                            <td colspan="3">
                                                <table border="0">
                                                    <tr>
                                                        <td align="left" valign="bottom" id="datetd" runat="server" style="display: none;">
                                                            <table cellpadding="0" cellspacing="0">
                                                                <tr>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center" colspan="3">
                                                <div class="heading text-right" id="mediaadd"></div>
                                                <div class="form-group">
                                                    <div class="table-responsive">
                                                        <div id="Equipmentlist" class="table table-bordered table-striped">
                                                        </div>
                                                        <div id="EquipmentOption" style="display: none;">
                                                            <button id="btntemp" value=""></button>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div id="divGrid" style="display: none;">
                                                    <asp:GridView ID="grvMailReport" runat="server" AutoGenerateColumns="False" BorderStyle="Solid"
                                                        CellPadding="2" CellSpacing="1" GridLines="None" Width="99%" PageSize="50" BorderColor="#E7E7E7"
                                                        BorderWidth="1px" AllowPaging="true" EmptyDataText="No Record(s) Found." EmptyDataRowStyle-ForeColor="Red" OnSorting="grvMailReport_Sorting"
                                                        EmptyDataRowStyle-HorizontalAlign="Center" PagerSettings-Mode="NumericFirstLast" OnRowDataBound="grvMailReport_RowDataBound"
                                                        OnPageIndexChanging="grvMailReport_PageIndexChanging" AllowSorting="true">
                                                        <Columns>

                                                            <asp:TemplateField>
                                                                <HeaderTemplate>
                                                                    Email
                                                                      <asp:ImageButton ID="btnemail" runat="server" CommandArgument="DESC" CommandName="Email"
                                                                          AlternateText="Ascending Order" CausesValidation="false" OnClick="Sorting" />
                                                                </HeaderTemplate>
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                                <ItemTemplate>
                                                                    <a href="/Admin/Customers/Customer.aspx?mode=edit&CustID=<%# Eval("CustomerID") %>"
                                                                        style="font-weight: normal; color: #6A6A6A; font-size: 11px" target="_blank" title="Click to Edit Customer">
                                                                        <asp:Label ID="lblfromEamil" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"Email") %>'></asp:Label></a>
                                                                </ItemTemplate>
                                                                <ItemStyle VerticalAlign="Top" HorizontalAlign="Left" CssClass="border" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField>
                                                                <HeaderTemplate>
                                                                    Name
                                                                       <asp:ImageButton ID="btnname" runat="server" CommandArgument="DESC" CommandName="Name"
                                                                           AlternateText="Ascending Order" CausesValidation="false" OnClick="Sorting" />
                                                                </HeaderTemplate>
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lbltoEamil" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"Name") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle VerticalAlign="Top" HorizontalAlign="Left" CssClass="border" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField>
                                                                <HeaderTemplate>
                                                                    Registered Date
                                                                     <asp:ImageButton ID="btnregdate" runat="server" CommandArgument="DESC" CommandName="Registered Date"
                                                                         AlternateText="Ascending Order" CausesValidation="false" OnClick="Sorting" />
                                                                </HeaderTemplate>
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                                <ItemTemplate>

                                                                    <asp:Label ID="lblregdate" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"Registered Date") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle VerticalAlign="Top" HorizontalAlign="Left" CssClass="border" />
                                                            </asp:TemplateField>

                                                            <asp:TemplateField>
                                                                <HeaderTemplate>
                                                                    CouponCode
                                                                     <asp:ImageButton ID="btncouponcode" runat="server" CommandArgument="DESC" CommandName="CouponCode"
                                                                         AlternateText="Ascending Order" CausesValidation="false" OnClick="Sorting" />
                                                                </HeaderTemplate>
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblIpaddress" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"CouponCode") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle VerticalAlign="Top" HorizontalAlign="Left" CssClass="border" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField>
                                                                <HeaderTemplate>
                                                                    Total Order
                                                                     <asp:ImageButton ID="btntotalorder" runat="server" CommandArgument="DESC" CommandName="Total Order"
                                                                         AlternateText="Ascending Order" CausesValidation="false" OnClick="Sorting" />
                                                                </HeaderTemplate>
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblmaildate" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"Total Order") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle VerticalAlign="Top" HorizontalAlign="Left" CssClass="border" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField>
                                                                <HeaderTemplate>
                                                                    Total Amount
                                                                     <asp:ImageButton ID="btntotalamount" runat="server" CommandArgument="DESC" CommandName="Total Amount"
                                                                         AlternateText="Ascending Order" CausesValidation="false" OnClick="Sorting" />
                                                                </HeaderTemplate>
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lbltotalamount" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"Total Amount") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle VerticalAlign="Top" HorizontalAlign="Left" CssClass="border" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField>
                                                                <HeaderTemplate>
                                                                    Last Order Date
                                                                     <asp:ImageButton ID="btnlastorderdate" runat="server" CommandArgument="DESC" CommandName="OrderDate"
                                                                         AlternateText="Ascending Order" CausesValidation="false" OnClick="Sorting" />
                                                                </HeaderTemplate>
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblorderdate" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"OrderDate") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle VerticalAlign="Top" HorizontalAlign="Left" CssClass="border" />
                                                            </asp:TemplateField>

                                                        </Columns>
                                                        <PagerSettings Mode="NumericFirstLast" Position="TopAndBottom"></PagerSettings>
                                                        <PagerStyle HorizontalAlign="Right" CssClass="grid_paging" />
                                                        <RowStyle HorizontalAlign="Center" CssClass="oddrow" />
                                                        <AlternatingRowStyle CssClass="altrow" />
                                                        <HeaderStyle ForeColor="White" Font-Bold="false" />
                                                        <FooterStyle ForeColor="White" Font-Bold="true" BackColor="#545454" />
                                                    </asp:GridView>
                                                </div>
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
    <div id="jqxwindow" style="display: none;">
        <div style="background: #f8f8f8;">
            Find Record
            <div class="jqx-window-close-button-background jqx-window-close-button-background-arctic" style="visibility: visible; width: 16px; height: 16px; margin-right: 5px; margin-left: 0px; position: absolute; right: 0px;"></div>
        </div>

        <div style="overflow: hidden;">
            <div>
                Find what:
            </div>
            <div style='margin-top: 5px;'>
                <input id='inputField' type="text" class="jqx-input" style="width: 200px; height: 23px;" />
            </div>
            <div style="margin-top: 7px; clear: both;">
                Look in:
            </div>
            <div style='margin-top: 5px;'>
                <div id='dropdownlist'>
                </div>
            </div>
            <div>
                <input type="button" id="findButton" value="Find" style="margin-top: 15px; margin-left: 50px; float: left; width: 70px;" role="button" class="jqx-rc-all jqx-rc-all-arctic jqx-button jqx-button-arctic jqx-widget jqx-widget-arctic jqx-fill-state-normal jqx-fill-state-normal-arctic" aria-disabled="false">
                <input type="button" id="clearButton" value="Clear" style="margin-left: 5px; margin-top: 15px; float: left; width: 70px;" role="button" class="jqx-rc-all jqx-rc-all-arctic jqx-button jqx-button-arctic jqx-widget jqx-widget-arctic jqx-fill-state-normal jqx-fill-state-normal-arctic" aria-disabled="false">
            </div>
        </div>
    </div>
    <div id="prepage" style="position: absolute; font-family: arial; font-size: 16; left: 0px; top: 0px; background-color: #000; opacity: 0.7; filter: alpha(opacity=70); layer-background-color: white; height: 100%; width: 100%; z-index: 1000; display: none;">
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
