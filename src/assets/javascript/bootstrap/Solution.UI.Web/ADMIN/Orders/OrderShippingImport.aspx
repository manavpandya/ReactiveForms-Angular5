<%@ Page Title="" Language="C#" MasterPageFile="~/ADMIN/Admin.Master" AutoEventWireup="true" CodeBehind="OrderShippingImport.aspx.cs" Inherits="Solution.UI.Web.ADMIN.Orders.OrderShippingImport" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts-main.js"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <style type="text/css">
        .divfloatingcss {
            bottom: 0;
            right: 0;
            position: fixed;
            width: 10%;
            margin-right: 46%;
            background-image: url("/Admin/images/title-bg-trans.png");
            -webkit-border-top-left-radius: 15px;
            -webkit-border-top-right-radius: 15px;
            -moz-border-radius-topleft: 15px;
            -moz-border-radius-topright: 15px;
            border-top-left-radius: 15px;
            border-top-right-radius: 15px; /*-webkit-border-radius: 20px;
-moz-border-radius: 20px;
border-radius: 20px;*/
        }
    </style>
    <script language="javascript" type="text/javascript">
            function chkHeight() {
                var windowHeight = 0;
                windowHeight = $(document).height(); //window.innerHeight;
                document.getElementById('prepage').style.height = windowHeight + 'px';
                document.getElementById('prepage').style.display = '';
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
            function chkselectcheckboxInv() {
                var allElts = document.getElementById('ContentPlaceHolder1_gvOrders').getElementsByTagName('input')
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

                    jAlert('Select at least one order!', 'Message');
                    return false;

                }
                else {
                    if (confirm('Are you sure want to import shipping info. for selected order?')) {
                        chkHeight();
                        return true;
                    }
                    else {
                        return false;
                    }
                }

                return true;

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
                                            <th>
                                                <div class="main-title-left">
                                                    <img class="img-left" title="Add Country" alt="Add Country" src="/App_Themes/<%=Page.Theme %>/images/category-list-icon.png">
                                                    <h2>Import Shipping Information</h2>
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
                                                <asp:Label ID="lblImportCSv" runat="server" Text="Import CSV :"></asp:Label>

                                                <asp:FileUpload ID="uploadCSV" runat="server" Style="border: 1px solid #1a1a1a; background: #f5f5f5; color: #000000;" />

                                                <asp:LinkButton ID="btnUpload" AlternateText="Upload" ImageAlign="Top"
                                                    runat="server" Visible="false">Upload</asp:LinkButton>

                                                <asp:Button ID="btnImport" runat="server" OnClick="btnImport_Click" ValidationGroup="importfile" CausesValidation="true" />


                                                <asp:RegularExpressionValidator ID="revImage" ControlToValidate="uploadCSV" ValidationExpression="(.*?)\.(csv)$"
                                                    Text="Select csv File Only (Ex.: .csv)" runat="server"
                                                    ValidationGroup="importfile" Display="Dynamic" ForeColor="Red" />
                                                <asp:RequiredFieldValidator ID="reqfile" runat="server" Text="Please select.csv file" ValidationGroup="importfile" ControlToValidate="uploadCSV" Display="Dynamic" ForeColor="Red"></asp:RequiredFieldValidator>
                                                <asp:Label ID="Label1" runat="server" Text=""></asp:Label>
                                                <div style="float: right;">
                                                    <table cellpadding="0" cellspacing="0">
                                                        <tr>
                                                            <td valign="top" align="left" style="color: red; height: 5px; width: 5px; font-size: 30px">•</td>
                                                            <td>: Multiple order for same destination detail.</td>

                                                        </tr>
                                                    </table>

                                                </div>

                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:GridView ID="gvOrders" AutoGenerateColumns="false" runat="server" GridLines="None"
                                                    Width="100%" CellPadding="2" CellSpacing="1" CssClass="border-td"
                                                    RowStyle-ForeColor="Black" HeaderStyle-ForeColor="#3c2b1b" PagerSettings-Mode="NumericFirstLast" ShowFooter="true" EmptyDataText="No data to display."
                                                    AllowPaging="false" AllowSorting="false" EmptyDataRowStyle-ForeColor="Red" EmptyDataRowStyle-HorizontalAlign="Center" PageSize="20" OnRowDataBound="gvOrders_RowDataBound">
                                                    <EmptyDataTemplate>
                                                        <span style="color: Red; font-size: 12px; text-align: center;">No Record(s) Found !</span>
                                                    </EmptyDataTemplate>
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="Select" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="7%">
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="chkSelect" runat="server" CssClass="input-checkbox" />

                                                                <asp:HiddenField ID="hdnProductID" runat="server" Value='<%#Eval("MatchOrders") %>' />
                                                                <asp:HiddenField ID="hdndate" runat="server" Value='<%#Eval("USPSTrackingDate") %>' />
                                                                <asp:HiddenField ID="hdnShoppingCardID" runat="server" Value='<%#Eval("ShoppingCardID") %>' />
                                                                <asp:HiddenField ID="hdnmulti" runat="server" Value='<%#Eval("isMulti") %>' />
                                                            </ItemTemplate>

                                                            <HeaderStyle HorizontalAlign="Center" CssClass="tdpro-checkbox" Width="7%" />
                                                            <ItemStyle HorizontalAlign="Center" CssClass="tdpro-checkbox" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText=" Match Order #" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="20%"
                                                            HeaderStyle-HorizontalAlign="Left">
                                                            <HeaderTemplate>
                                                                <table cellpadding="0" cellspacing="1" border="0" align="left">
                                                                    <tr style="border: 0px;">
                                                                        <td style="border: 0px; padding: 0px; background-color: transparent;">Match Order #

                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <div style="text-align: justify; width: 300px;">

                                                                    <asp:Literal ID="lblordernumber" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"MatchOrders") %>'></asp:Literal>

                                                                </div>
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                            <ItemStyle HorizontalAlign="Left" Width="20%" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText=" Match Order #" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="20%"
                                                            HeaderStyle-HorizontalAlign="Left">
                                                            <HeaderTemplate>
                                                                <table cellpadding="0" cellspacing="1" border="0" align="left">
                                                                    <tr style="border: 0px;">
                                                                        <td style="border: 0px; padding: 0px; background-color: transparent;">Transaction Status

                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <div style="text-align: justify; width: 300px;">

                                                                    <asp:Literal ID="lblTransactionstatus" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"Transactionstatus") %>'></asp:Literal>

                                                                </div>
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                            <ItemStyle HorizontalAlign="Left" Width="20%" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText=" USPS Destination Details" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="20%"
                                                            HeaderStyle-HorizontalAlign="Left">
                                                            <HeaderTemplate>
                                                                <table cellpadding="0" cellspacing="1" border="0" align="left">
                                                                    <tr style="border: 0px;">
                                                                        <td style="border: 0px; padding: 0px; background-color: transparent;">USPS Destination Details

                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <div style="text-align: justify; width: 300px;">

                                                                    <asp:Literal ID="lblDestination" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"USPSAddressdetails") %>'></asp:Literal>

                                                                </div>
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                            <ItemStyle HorizontalAlign="Left" Width="20%" />
                                                        </asp:TemplateField>

                                                        <asp:TemplateField HeaderText=" Match Shipping Details" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="20%"
                                                            HeaderStyle-HorizontalAlign="Left">
                                                            <HeaderTemplate>
                                                                <table cellpadding="0" cellspacing="1" border="0" align="left">
                                                                    <tr style="border: 0px;">
                                                                        <td style="border: 0px; padding: 0px; background-color: transparent;">Match Shipping Details

                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <div style="text-align: justify; width: 300px;">

                                                                    <asp:Literal ID="lblMatchShipping" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"MatchShippingdetails") %>'></asp:Literal>

                                                                </div>
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                            <ItemStyle HorizontalAlign="Left" Width="20%" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText=" USPS Tracking Details" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="20%"
                                                            HeaderStyle-HorizontalAlign="Left">
                                                            <HeaderTemplate>
                                                                <table cellpadding="0" cellspacing="1" border="0" align="left">
                                                                    <tr style="border: 0px;">
                                                                        <td style="border: 0px; padding: 0px; background-color: transparent;">USPS Tracking Details

                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <div style="text-align: justify; width: 300px;">

                                                                    <asp:Literal ID="lbltrackingInfo" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"USPSTrackingNumber") %>'></asp:Literal>

                                                                </div>
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                            <ItemStyle HorizontalAlign="Left" Width="20%" />
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
                                        <tr>
                                            <td style="text-align: center;">
                                                <div style="float: right; width: 100%; height: 40px; padding-top: 5px;">
                                                    <div id="divfloating" class="divfloatingcss" style="text-align: center; height: 25px; vertical-align: middle;">
                                                        <asp:ImageButton ID="btnSave" runat="server" ImageUrl="/App_Themes/Gray/images/save.gif" OnClientClick="return chkselectcheckboxInv();"
                                                            CommandName="Save" AlternateText="Save" OnClick="btnSave_Click" Visible="false" />
                                                    </div>
                                                </div>
                                            </td>

                                        </tr>
                                        <tr class="altrow" id="trBottom" runat="server" visible="false">
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
</asp:Content>
