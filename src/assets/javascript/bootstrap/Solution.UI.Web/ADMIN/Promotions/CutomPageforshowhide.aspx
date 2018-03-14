<%@ Page Title="" Language="C#" MasterPageFile="~/ADMIN/Admin.Master" AutoEventWireup="true" CodeBehind="CutomPageforshowhide.aspx.cs" Inherits="Solution.UI.Web.ADMIN.Promotions.CutomPageforshowhide" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
        <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts-main.js"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts.js"></script>
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
            margin-right: 46%;
            background-image: url("/Admin/images/title-bg-trans.png");
            -webkit-border-top-left-radius: 15px;
            -webkit-border-top-right-radius: 15px;
            -moz-border-radius-topleft: 15px;
            -moz-border-radius-topright: 15px;
            border-top-left-radius: 15px;
            border-top-right-radius: 15px;
        }
    </style>
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
                                                <img class="img-left" title="Custom Product List" alt="OnSale Products Display Order" src="/App_Themes/<%=Page.Theme %>/Images/coupon-discount-icon.png" />
                                                <h2>&nbsp;Custom Product List</h2>
                                            </div>
                                        </th>
                                    </tr>
                                      <tr class="altrow" id="tr2" runat="server">
                                            <td>
                                                <span><a id="lkbAllowAll" href="javascript:selectAll(true);">Check All</a> | <a id="lkbClearAll"
                                                    href="javascript:selectAll(false);">Clear All</a> </span> 
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
                                                       <%-- <asp:TemplateField HeaderText="SKU" HeaderStyle-Width="15%">
                                                            <HeaderTemplate>
                                                                SKU
                                                            
                                                            </HeaderTemplate>
                                                            <ItemStyle HorizontalAlign="left" Width="15%" />
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                            <ItemTemplate>
                                                                


                                                                <asp:Label ID="lblsku" runat="server" Text='<%# bind("SKU") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>--%>
                                                        <asp:TemplateField HeaderText="Display Order" HeaderStyle-Width="15%">
                                                            <HeaderTemplate>
                                                                Display Order
                                                            
                                                            </HeaderTemplate>
                                                            <ItemStyle HorizontalAlign="left" />
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                            <ItemTemplate>
                                                                <asp:HiddenField ID="hdnProductid" runat="server" Value='<%#Eval("ProductID") %>' />
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
                                    <tr class="altrow" id="trBottom" runat="server">
                                        <td>
                                            <table width="100%">
                                                <tr>

                                                    <td align="center" colspan="2">
                                                           <div id="divfloating" class="divfloatingcss" style="width: 150px;">
                                                                <div style="margin-bottom: 1px; margin-top: 3px;">
                                                        <asp:Button ID="btnsave" runat="server" ToolTip="Save"
                                                            OnClick="btnsave_Click" />
                                                        <asp:Button ID="btncancel" runat="server" ToolTip="Cancel" Visible="false"  />
                                                                    </div>
                                                               </div>

                                                    </td>
                                                </tr>

                                            </table>
                                        </td>
                                    </tr>
                                    <tr class="altrow" id="tr1" runat="server">
                                            <td>
                                                <span><a id="lkbAllowAll" href="javascript:selectAll(true);">Check All</a> | <a id="lkbClearAll"
                                                    href="javascript:selectAll(false);">Clear All</a> </span> 
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

       
    </div>
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


         function selectAll(bl) {
             var allElts = document.getElementById('divchild').getElementsByTagName('input')
             var i;
             var Chktrue;
             Chktrue = 0;
             for (i = 0; i < allElts.length; i++) {
                 allElts[i].checked = bl;
             }
         }
         
    </script>
</asp:Content>
