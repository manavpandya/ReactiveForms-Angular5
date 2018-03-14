<%@ Page Title="" Language="C#" MasterPageFile="~/ADMIN/Admin.Master" AutoEventWireup="true" CodeBehind="SetCategoryDisplayOrder.aspx.cs" Inherits="Solution.UI.Web.ADMIN.Products.SetCategoryDisplayOrder" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
     <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts-main.js"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts.js"></script>
    <script type="text/javascript" src="/admin/js/jquery-1.11.1.min.js"></script>
    <script type="text/javascript" src="/admin/js/jquery.dragsort-0.5.2.min.js"></script>
    <script type="text/javascript" src="/admin/js/jQuery_bootstrap.js"></script>
    <style type="text/css">
        @media(max-width:768px) {
            .row-fluid {
                width: 92% !important;
            }
        }
    </style>

    <style type="text/css">
        tr {
            cursor: pointer;
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
            left: 38%;
            top: 15.5%;
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
        function chkHeight() {
            var windowHeight = 0;
            var $q = jQuery.noConflict();
            windowHeight = $q(document).height(); //window.innerHeight;
            document.getElementById('prepage').style.height = windowHeight + 'px';
            document.getElementById('prepage').style.display = '';
            //  $(".removetr").hide(); //window.scrollTo(0, 0);;
            //window.scrollTo(0, 0);
        }
    </script>
    <script type="text/javascript">
        var ii = 0;
       
        function updatecategorydisplaymenu(ids)
        {
            chkHeight();
            try
            {
                //var allids = '';
                //var allinput = $('#ContentPlaceHolder1_grdParentCategory').find('input[id^="ContentPlaceHolder1_grdParentCategory_txtDisplayOrder_"]');
                //for (var i = 0; i < allinput.length; i++) {
                //    allids = allids + $(allinput[i]).val().toString() + ',';
                //}
                $.ajax(
               {
                   type: "POST",
                   url: "/TestMail.aspx/updatecategorydisplaymenu",
                   data: "{ids: '" + $('#'+$(ids).attr('id').replace('txtDisplayOrder', 'hdnCategoryID')).val() + "',disorer:'" + $(ids).val() + "'}",
                   contentType: "application/json; charset=utf-8",
                   dataType: "json",
                   async: "true",
                   cache: "false",
                   success: function (msg) { document.getElementById('prepage').style.display = 'none'; }
               });
            }
            catch (err) {

                document.getElementById('prepage').style.display = 'none';
            }
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
                //$('.bootbox').fadeIn();
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
                if (cntrl.id.toString().toLowerCase().indexOf('hdncatid') > -1) {
                    alldisplayorder = alldisplayorder + icount.toString() + ',';
                    allids = allids + cntrl.value.toString() + ',';
                    icount = icount + 1;
                    rowid = rowid + 1;
                }
                if (cntrl.id.toString().toLowerCase().indexOf('txtdisplayorder') > -1) {
                    cntrl.value = icount2
                    icount2 = icount2 + 1;
                }
                if (cntrl.id.toString().toLowerCase().indexOf('hdnparentcatid') > -1) {
                    pcatid = cntrl.value.toString();
                }
            }
       
            try {
                $.ajax(
               {
                   type: "POST",
                   url: "/TestMail.aspx/updatecategorydisplay",
                   data: "{ids: '" + allids + "', Parentcatid: '" + pcatid + "',ppid:'" + document.getElementById('ContentPlaceHolder1_ddlCategory').options[document.getElementById('ContentPlaceHolder1_ddlCategory').selectedIndex].value + "'}",
                   contentType: "application/json; charset=utf-8",
                   dataType: "json",
                   async: "true",
                   cache: "false",
                   success: function (msg) { document.getElementById('prepage').style.display = 'none'; }
               });
            }
            catch (err) {
                
                document.getElementById('prepage').style.display = 'none';
            }

        }


        
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <style type="text/css">
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
    </style>
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
                                                    <h2  style="text-align:left;">Category Display Order List</h2>
                                                </div>
                                            </th>
                                        </tr>
                                        <tr>
                                            <td align="center">
                                                <asp:Label ID="lblMessage" runat="server" ForeColor="Red" Font-Bold="true"></asp:Label>
                                            </td>

                                        </tr>
                                        <tr class="even-row">
                                            <td>

                                                <asp:HiddenField ID="hdnCategory" runat="server" />
                                                <asp:GridView ID="grdParentCategory" runat="server" GridLines="None" AutoGenerateColumns="false" Width="100%"
                                                    AllowPaging="false" DataKeyNames="CategoryID" CssClass="border-td"
                                                    ShowFooter="false" OnRowDataBound="grdParentCategory_RowDataBound" OnRowCommand="grdParentCategory_RowCommand">
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="&nbsp;Parent Category Name">
                                                            <ItemTemplate>
                                                                <div style="float: left;">
                                                                    <a href="javascript:expandcollapse('divchild<%# Eval("CategoryID") %>', 'one');">

                                                                        <img id="imgdiv<%# Eval("CategoryID") %>" title="Collapse" alt="Expand" src="/images/expand.gif">
                                                                    </a>
                                                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</div>
                                                                &nbsp;&nbsp;<asp:Literal ID="lttitle" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"Name") %>'></asp:Literal><span
                                                                    style="float: right;">
                                                                    <%--<asp:ImageButton ID="ImgDelete" OnClientClick="javascript:if(confirm('Are you sure want to delete this record?')){chkHeight();  return true;}else{return false;}"
                                                                            runat="server" ImageUrl="/images/delete-icon.gif" CommandName="Remove" CommandArgument='<%# DataBinder.Eval(Container.DataItem,"ParentCategoryID") %>' />--%>
                                                                    <%--<input type="hidden" id="hdnisPublish" runat="server" value='<%# DataBinder.Eval(Container.DataItem,"IsPublish") %>' />--%>
                                                                </span>
                                                                <input type="hidden" id="hdnParentCategoryID" runat="server" value='<%# DataBinder.Eval(Container.DataItem,"ParentCategoryID") %>' />
                                                                <input type="hidden" id="hdnCategoryID" runat="server" value='<%# DataBinder.Eval(Container.DataItem,"CategoryID") %>' />
                                                                 <asp:HiddenField ID="hdncategoryName" runat="server" Value='<%#Eval("Name") %>' />
                                                            </ItemTemplate>

                                                            <HeaderStyle Width="37.5%" HorizontalAlign="left" VerticalAlign="Middle" CssClass="table-title" />
                                                            <ItemStyle HorizontalAlign="left" />
                                                        </asp:TemplateField>
                                                         <asp:TemplateField HeaderText="Display&nbsp;Order" ItemStyle-HorizontalAlign="Center"
                                                            ItemStyle-Width="50px" HeaderStyle-HorizontalAlign="left">
                                                            <HeaderTemplate>
                                                                <table cellpadding="0" cellspacing="1" border="0" align="center">
                                                                    <tr style="border: 0px;">
                                                                        <td style="border: 0px; padding: 0px; background-color: transparent;">Display&nbsp;Order
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                               
                                                                <center>
                                                                     <asp:TextBox ID="txtDisplayOrder" onchange="updatecategorydisplaymenu(this);" style="height:30px;width:50px;text-align:center;"  runat="server" Text=' <%#Eval("DisplayOrder").ToString().Equals("999")?null:Eval("DisplayOrder")%>'></asp:TextBox></center>
                                                            </ItemTemplate>

                                                            <HeaderStyle HorizontalAlign="Left" Width="39.5%" />
                                                            <ItemStyle HorizontalAlign="Center" Width="50px" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Display&nbsp;Order" ItemStyle-HorizontalAlign="Center"
                                                            ItemStyle-Width="50px" HeaderStyle-HorizontalAlign="left">
                                                            <HeaderTemplate>
                                                                <table cellpadding="0" cellspacing="1" border="0" align="center">
                                                                    <tr style="border: 0px;">
                                                                        <td style="border: 0px; padding: 0px; background-color: transparent;">Inner Category Count
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                
                                                                <center>
                                                                    <asp:Label ID="lbinv" runat="server" Text='<%#  DataBinder.Eval(Container.DataItem,"ChildCatCount") %>'></asp:Label></center>
                                                            </ItemTemplate>

                                                            <HeaderStyle HorizontalAlign="Left" Width="39.5%" />
                                                            <ItemStyle HorizontalAlign="Center" Width="50px" />
                                                        </asp:TemplateField>

                                                        <asp:TemplateField HeaderText="Status" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="50px"
                                                            HeaderStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <%--<img src='<%# SetImage(Convert.ToBoolean(DataBinder.Eval(Container.DataItem,"Active"))) %>' />--%>
                                                                <asp:HiddenField ID="hdnActive" runat="server" Value='<%# (DataBinder.Eval(Container.DataItem,"Active")) %>' />
                                                                <asp:Literal ID="ltrStatus" runat="server"></asp:Literal>
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Center" Width="11%" />
                                                            <ItemStyle HorizontalAlign="Center" Width="50px" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Edit" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="29px"
                                                            HeaderStyle-HorizontalAlign="Center" Visible="false">
                                                            <ItemTemplate>

                                                                <asp:ImageButton CommandName="ParentSave" ID="btnSave" runat="server" />

                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Center" Width="11%" />
                                                            <ItemStyle HorizontalAlign="Center" Width="29px" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <tr>
                                                                    <td colspan="100%">

                                                                        <div id="divchild<%# Eval("CategoryID") %>" style="display: none; position: relative; left: 15px; overflow: auto; width: 89%; margin-top: 10px;margin-left:100px;">
                                                                            <asp:GridView ID="gvCategory" AutoGenerateColumns="false" runat="server" GridLines="None" DataKeyNames="CategoryID"
                                                                                Width="100%" CellPadding="2" CellSpacing="1" CssClass="border-td"
                                                                                RowStyle-ForeColor="Black" HeaderStyle-ForeColor="#3c2b1b" PagerSettings-Mode="NumericFirstLast" ShowFooter="true"
                                                                                AllowPaging="True" AllowSorting="True" EmptyDataRowStyle-ForeColor="Red" EmptyDataRowStyle-HorizontalAlign="Center"
                                                                                OnRowDataBound="gvCategory_RowDataBound" OnRowCommand="gvCategory_RowCommand"
                                                                                OnPageIndexChanging="gvCategory_PageIndexChanging" PageSize="20" Visible="false">
                                                                                <EmptyDataTemplate>
                                                                                    <span style="color: Red; font-size: 12px; text-align: center;">No Record(s) Found !</span>
                                                                                </EmptyDataTemplate>
                                                                                <Columns>
                                                                                    <asp:TemplateField HeaderText="Select" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="7%" Visible="false">
                                                                                        <ItemTemplate>
                                                                                            <asp:CheckBox ID="chkSelect" runat="server" CssClass="input-checkbox" />

                                                                                        </ItemTemplate>
                                                                                        <FooterTemplate>
                                                                                            <div class="row-fluid" style="width: 150px;">

                                                                                                <span><a id="lkbAllowAll" runat="server">Check All</a> | <a id="lkbClearAll" runat="server">Clear All</a> </span><span style="float: right;"></span>

                                                                                            </div>
                                                                                        </FooterTemplate>
                                                                                        <HeaderStyle HorizontalAlign="Center" CssClass="tdpro-checkbox" Width="7%" />
                                                                                        <ItemStyle HorizontalAlign="Center" CssClass="tdpro-checkbox" />
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField HeaderText="Price($)" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right">
                                                                                        <HeaderTemplate>
                                                                                            Product Image
                                                                                        </HeaderTemplate>
                                                                                        <ItemTemplate>
                                                                                            <asp:HiddenField ID="hdnCatid" runat="server" Value='<%#Eval("CategoryID") %>' />
                                                                                            <asp:HiddenField ID="hdnParentCatID" runat="server" Value='<%#Eval("ParentCategoryID") %>' />
                                                                                             <asp:HiddenField ID="hdncategoryName" runat="server" Value='<%#Eval("Name") %>' />
                                                                                            <center>
                                                                                                <asp:Image Style="width: 70PX !important; border: 1px solid #ccc; text-align: center;" ImageUrl='<%# GetIconImageProduct(Convert.ToString(Eval("ImageName"))) %>'
                                                                                                    ID="imgName"
                                                                                                    ToolTip='<%# (Convert.ToString(Eval("Name")))%>' runat="server" /></center>
                                                                                        </ItemTemplate>
                                                                                        <HeaderStyle HorizontalAlign="Right" Width="10%"></HeaderStyle>
                                                                                        <ItemStyle HorizontalAlign="Right" Width="10%"></ItemStyle>
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField HeaderText="Category Name" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="20%"
                                                                                        HeaderStyle-HorizontalAlign="Left">
                                                                                        <HeaderTemplate>
                                                                                            <table cellpadding="0" cellspacing="1" border="0" align="left">
                                                                                                <tr style="border: 0px;">
                                                                                                    <td style="border: 0px; padding: 0px; background-color: transparent;">Category Name

                                                                                                    </td>
                                                                                                </tr>
                                                                                            </table>
                                                                                        </HeaderTemplate>
                                                                                        <ItemTemplate>
                                                                                            <div style="text-align: justify; width: 300px;">
                                                                                                <%--<a href='<%# "Category.aspx?Mode=edit&ID="+ DataBinder.Eval(Container.DataItem, "CategoryID") +"&storeid="+DataBinder.Eval(Container.DataItem, "StoreID")    %>'>--%>
                                                                                                <%#Eval("Name").ToString().Length>50?Eval("Name").ToString().Substring(0,50):Eval("Name")%>
                                                                                                <%-- </a>--%>
                                                                                            </div>
                                                                                        </ItemTemplate>
                                                                                        <HeaderStyle HorizontalAlign="Left" />
                                                                                        <ItemStyle HorizontalAlign="Left" Width="20%" />
                                                                                    </asp:TemplateField>

                                                                                    <asp:TemplateField HeaderText="Display&nbsp;Order" ItemStyle-HorizontalAlign="Center"
                                                                                        ItemStyle-Width="50px" HeaderStyle-HorizontalAlign="left">
                                                                                        <HeaderTemplate>
                                                                                            <table cellpadding="0" cellspacing="1" border="0" align="center">
                                                                                                <tr style="border: 0px;">
                                                                                                    <td style="border: 0px; padding: 0px; background-color: transparent;">Display&nbsp;Order
                                                                                                    </td>
                                                                                                </tr>
                                                                                            </table>
                                                                                        </HeaderTemplate>
                                                                                        <ItemTemplate>
                                                                                            <asp:TextBox ID="txtDisplayOrder" runat="server" Text=' <%#Eval("DisplayOrder").ToString().Equals("999")?null:Eval("DisplayOrder")%>'></asp:TextBox>

                                                                                        </ItemTemplate>
                                                                                        <FooterTemplate>

                                                                                            <asp:ImageButton CommandName="ChildAllSave" ID="btnSave" runat="server" />
                                                                                        </FooterTemplate>
                                                                                        <HeaderStyle HorizontalAlign="Left" />
                                                                                        <ItemStyle HorizontalAlign="Center" Width="50px" />
                                                                                        <FooterStyle HorizontalAlign="Center" />
                                                                                    </asp:TemplateField>

                                                                                    <asp:TemplateField HeaderText="Status" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="50px"
                                                                                        HeaderStyle-HorizontalAlign="Center">
                                                                                        <ItemTemplate>
                                                                                            <%--<img src='<%# SetImage(Convert.ToBoolean(DataBinder.Eval(Container.DataItem,"Active"))) %>' />--%>
                                                                                            <asp:HiddenField ID="hdnActive" runat="server" Value='<%# (DataBinder.Eval(Container.DataItem,"Active")) %>' />
                                                                                            <asp:Literal ID="ltrStatus" runat="server"></asp:Literal>
                                                                                        </ItemTemplate>
                                                                                        <HeaderStyle HorizontalAlign="Center" />
                                                                                        <ItemStyle HorizontalAlign="Center" Width="50px" />
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField HeaderText="Edit" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="29px"
                                                                                        HeaderStyle-HorizontalAlign="Center" Visible="false">
                                                                                        <ItemTemplate>
                                                                                            <asp:ImageButton CommandName="ChildSingleSave" ID="btnSingleSave" runat="server" />

                                                                                            <%-- <a title='' id="hplEdit" runat="server" href='<%# "Category.aspx?Mode=edit&ID="+ DataBinder.Eval(Container.DataItem, "CategoryID") +"&storeid="+DataBinder.Eval(Container.DataItem, "StoreID")    %>'><span class="green"><i class="icon-edit bigger-160"></i></span></a>--%>
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

    <div style="display: none;">
        <input type="hidden" id="hdnrowid" runat="server" value="" />
        <input type="hidden" id="hdngroupid" runat="server" value="" />
        <input type="hidden" id="hdngroupactiontype" runat="server" value="" />
        <input type="hidden" id="hdngroupname" runat="server" value="" />
        <input type="hidden" id="hdncatidforjs" runat="server" value="" />
        <table style="width: 100%; margin-top: 10px;">
            <tr id="tr1" runat="server">
                <td style="width: 20%; float: left;">
                    <div class="span6">
                        <div class="row-fluid" style="width: 900px; display: none;">

                            <span><a id="lkbAllowAll" href="javascript:selectAll(true);" class="btn btn-mini btn-info">Check All</a> | <a id="lkbClearAll"
                                href="javascript:selectAll(false);" class="btn btn-mini btn-info">Clear All</a> </span><span style="float: right;"></span>

                        </div>
                    </div>
                </td>
                <td style="float: right; display: none;">
                    <asp:LinkButton ID="LinkButton1" runat="server" CssClass="btn btn-mini btn-info"
                        OnClientClick="return checkCount();" ToolTip="Delete">Delete</asp:LinkButton>
                    <div style="display: none;">
                        <asp:Button ID="Button1" runat="server" />
                    </div>
                </td>
            </tr>
        </table>
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
    <script language="javascript" type="text/javascript">
        //function expandcollapse(obj, row) {
        //    var alldiv = document.getElementById('ContentPlaceHolder1_grdParentCategory').getElementsByTagName('div');
        //   document.getElementById('ContentPlaceHolder1_hdncatidforjs').value = obj.replace('divchild', '');


        //    if (alldiv != null && alldiv.length > 0) {
        //        for (var i = 0; i < alldiv.length; i++) {
        //            var divall = alldiv[i];

        //            if (divall.id == obj) {
        //                var div = document.getElementById(obj);
        //                var img = obj.toString().replace('divchild', 'imgdiv'); //

        //                if (div.style.display == "none") {
        //                    div.style.display = "block";
        //                    document.getElementById('ContentPlaceHolder1_hdnrowid').value = obj;
                           
        //                    if (row == 'alt') {
        //                        document.getElementById(img).src = '/images/minimize.png';
        //                    }
        //                    else {
        //                        document.getElementById(img).src = '/images/minimize.png';
        //                    }
        //                }
        //                else {
        //                    div.style.display = "none";
        //                    document.getElementById('ContentPlaceHolder1_hdnrowid').value = "";
        //                    if (row == 'alt') {
        //                        document.getElementById(img).src = '/images/expand.gif';
        //                    }
        //                    else {
        //                        document.getElementById(img).src = '/images/expand.gif';
        //                    }
        //                }
        //            }
        //            else if (divall.id != "" && divall.id != obj) {

        //                var imgid = divall.id.toString().replace('divchild', 'imgdiv');
        //                var img = document.getElementById(imgid);
        //                alert(divall.id);
        //                alert(document.getElementById(imgid));
        //                alert(img);
        //                divall.style.display = "none";
        //                if (row == 'alt') {
        //                    document.getElementById(img).src = '/images/expand.gif';
        //                }
        //                else {
        //                    document.getElementById(img).src = '/images/expand.gif';
        //                }
        //            }
        //        }
        //    }

        //    var $jjjj = jQuery.noConflict();
        //    $jjjj(document).ready(function () {
        //        var st = $jjjj("#divchild" + document.getElementById('ContentPlaceHolder1_hdncatidforjs').value + " .rep-drag li").attr('style');
        //        var hh = $jjjj("#divchild" + document.getElementById('ContentPlaceHolder1_hdncatidforjs').value + " .rep-drag li").innerHeight();
        //        hh = parseInt(hh) - parseInt(10);
        //        st = st.replace('border:dashed 1px #454545;', '') + ''
        //        $jjjj("#divchild" + document.getElementById('ContentPlaceHolder1_hdncatidforjs').value + " .rep-drag").dragsort({ dragSelector: "li", dragEnd: updatenn, placeHolderTemplate: "<li class='placeHolder' style='" + st + "'></li>" });
        //    });
        //}

        function expandcollapse(obj, row) {
            var alldiv = document.getElementById('ContentPlaceHolder1_grdParentCategory').getElementsByTagName('div');
            document.getElementById('ContentPlaceHolder1_hdncatidforjs').value = obj.replace('divchild', '');
          
            if (alldiv != null && alldiv.length > 0) {
                for (var i = 0; i < alldiv.length; i++) {
                    var divall = alldiv[i];

                    if (divall.id == obj) {
                        var div = document.getElementById(obj);
                        var img = obj.toString().replace('divchild', 'imgdiv'); //

                        if (div.style.display == "none") {
                            div.style.display = "block";
                            document.getElementById('ContentPlaceHolder1_hdnrowid').value = obj;
                            if (row == 'alt') {
                                //document.getElementById(img).src = "/images/collapse.png";

                                document.getElementById(img).src = '/images/minimize.png';

                            }
                            else {
                                //document.getElementById(img).src = "/images/collapse.png";

                                document.getElementById(img).src = '/images/minimize.png';
                            }
                            //document.getElementById(img).alt = "Close to view other Listing";
                        }
                        else {
                            div.style.display = "none";
                            document.getElementById('ContentPlaceHolder1_hdnrowid').value = "";
                            if (row == 'alt') {
                                //document.getElementById(img).src = "/images/expand.png";

                                document.getElementById(img).src = '/images/expand.gif';
                            }
                            else {
                                //document.getElementById(img).src = "/images/expand.png";

                                document.getElementById(img).src = '/images/expand.gif';


                            }
                            //document.getElementById(img).alt = "Expand to show Listing";
                        }
                    }
                    else if (divall.id != "" && divall.id != obj) {

                        var imgid = divall.id.toString().replace('divchild', 'imgdiv');
                        var img = document.getElementById(imgid);
                        divall.style.display = "none";
                        if (row == 'alt') {
                            //img.src = "/images/expand.png";

                            img.src = '/images/expand.gif';
                        }
                        else {
                            // img.src = "/images/expand.png";

                            img.src = '/images/expand.gif';
                        }
                        //img.alt = "Expand to show Listing";

                    }
                }
            }

            var $jjjj = jQuery.noConflict();
                $jjjj(document).ready(function () {
                    var st = $jjjj("#divchild" + document.getElementById('ContentPlaceHolder1_hdncatidforjs').value + " .rep-drag li").attr('style');
                   
                    var hh = $jjjj("#divchild" + document.getElementById('ContentPlaceHolder1_hdncatidforjs').value + " .rep-drag li").innerHeight();
                    hh = parseInt(hh) - parseInt(10);
                    st = st.replace('border:dashed 1px #454545;', '') + '';
                    $jjjj("#divchild" + document.getElementById('ContentPlaceHolder1_hdncatidforjs').value + " .rep-drag").dragsort({ dragSelector: "li", dragEnd: updatenn, placeHolderTemplate: "<li class='placeHolder' style='" + st + "'></li>" });
                });
        }
    </script>



    <script language="javascript" type="text/javascript">
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
    </script>
    <script language="javascript" type="text/javascript">
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
