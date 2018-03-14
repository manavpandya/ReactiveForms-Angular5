<%@ Page Title="" Language="C#" MasterPageFile="~/ADMIN/Admin.Master" AutoEventWireup="true"
    CodeBehind="GenerateYahooCSV.aspx.cs" Inherits="Solution.UI.Web.ADMIN.Products.GenerateYahooCSV" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
   <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts-main.js"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-calender-custom.js"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-calender.js"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts.js"></script>
    <script type="text/javascript">

        $(function () {

            $('#ContentPlaceHolder1_txtStartDate').datetimepicker({
                showButtonPanel: true, ampm: false,
                showHour: false, showMinute: false, showSecond: false, showTime: false, showOn: "button",
                buttonImage: "/App_Themes/<%=Page.Theme %>/images/date-icon.png", buttonImageOnly: true
            });
            $('#ContentPlaceHolder1_txtEndDate').datetimepicker({
                showButtonPanel: true, ampm: false,
                showHour: false, showMinute: false, showSecond: false, showTime: false, showOn: "button",
                buttonImage: "/App_Themes/<%=Page.Theme %>/images/date-icon.png", buttonImageOnly: true
            });
        });

        function SearchValidation(val) {
            var startDate = new Date(document.getElementById('ContentPlaceHolder1_txtStartDate').value);
            var endDate = new Date(document.getElementById('ContentPlaceHolder1_txtEndDate').value);

            if (document.getElementById('ContentPlaceHolder1_ddlCriteria') != null && document.getElementById('ContentPlaceHolder1_ddlCriteria').selectedIndex == 0) {
                jAlert('Please Select Valid Product Criteria.', 'Required Information', 'ContentPlaceHolder1_ddlCriteria');
                return false;
            }
            else if (startDate > endDate) {
                jAlert('Please Select Valid Date.', 'Required Information', 'ContentPlaceHolder1_txtEndDate');
                return false;
            }
            else if (parseInt(val) == 1) {
                if (document.getElementById("chklist") != null) {

                    var element = document.getElementById("chklist").getElementsByTagName("INPUT");
                    var i;
                    var chkcnt = 0;
                    for (i = 0; i < element.length; i++) {
                        var elt = element[i];
                        if (elt.type == "checkbox" && elt.checked == true) {
                            chkcnt++
                        }
                    }
                    if (chkcnt == 0) {
                        jAlert('Please Select Fields.', 'Required Information');
                        return false;
                    }
                }
            }
            return true;
        }

        function CheckboxSelection(val) {
            var element = document.getElementById("chklist").getElementsByTagName("INPUT");
            var i;
            var Chktrue = true;
            Chktrue = 0;
            for (i = 0; i < element.length; i++) {
                var elt = element[i];
                if (elt.type == "checkbox") {
                    elt.checked = val;
                }
            }
        }
        function chkHeight() {
            var windowHeight = 0;
            windowHeight = $(document).height(); //window.innerHeight;

            document.getElementById('prepage').style.height = windowHeight + 'px';
            document.getElementById('prepage').style.display = '';
        }
        function Loader() {
            chkHeight();
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
                                                    <img class="img-left" title="Generate Yahoo CSV" alt="Generate Yahoo CSV" src="/App_Themes/<%=Page.Theme %>/Images/product_export.png" />
                                                    <h2>
                                                        Generate Yahoo CSV</h2>
                                                </div>
                                            </th>
                                        </tr>
                                        <tr>
                                            <td align="center" colspan="2" valign="middle" style="text-align: center">
                                                <asp:Label ID="lblMsg" runat="server" ForeColor="Red"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr class="altrow">
                                            <td align="right">
                                                <span class="star">*</span>Required Field
                                            </td>
                                        </tr>
                                        <tr class="even-row">
                                            <td>
                                                <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                                    <tr class="oddrow">
                                                        <td style="width: 12%">
                                                            <span class="star">*</span>Store Name:
                                                        </td>
                                                        <td style="width: 80%; vertical-align: top">
                                                            <asp:DropDownList runat="server" ID="ddlStore" CssClass="add-product-list" Width="190px"
                                                                Height="20px">
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                    <tr class="altrow">
                                                        <td>
                                                            <span class="star">*</span>Product Criteria:
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="ddlCriteria" runat="server" CssClass="add-product-list" Width="148px">
                                                                <asp:ListItem Text="Select Criteria" Value="0" Selected="True"></asp:ListItem>
                                                                <asp:ListItem Text="All" Value="1"></asp:ListItem>
                                                                <asp:ListItem Text="New" Value="2"></asp:ListItem>
                                                                <asp:ListItem Text="Updated" Value="3"></asp:ListItem>
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                    <tr class="altrow">
                                                        <td>
                                                            <span class="star">*</span>Start Date:
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtStartDate" runat="server" CssClass="from-textfield" Width="70px"
                                                                Style="margin-right: 3px;"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr class="oddrow">
                                                        <td>
                                                            <span class="star">*</span>End Date:
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtEndDate" runat="server" CssClass="from-textfield" Width="70px"
                                                                Style="margin-right: 3px;"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr class="altrow">
                                                        <td valign="top">
                                                            <span class="star">*</span>Fields:
                                                        </td>
                                                        <td>
                                                            <div class="list" id="chklist">
                                                                <asp:CheckBoxList ID="lbFields" runat="server" RepeatColumns="5" RepeatDirection="Horizontal">
                                                                    <asp:ListItem Text="Path" Value="path"></asp:ListItem>
                                                                    <asp:ListItem Text="ID" Value="YahooID"></asp:ListItem>
                                                                    <asp:ListItem Text="Code" Value="SKU"></asp:ListItem>
                                                                    <asp:ListItem Text="Image" Value="ImageName1"></asp:ListItem>
                                                                    <asp:ListItem Text="Sale-Price" Value="SalePrice"></asp:ListItem>
                                                                    <asp:ListItem Text="Price" Value="price"></asp:ListItem>
                                                                    <asp:ListItem Text="Abstract" Value="Abstract"></asp:ListItem>
                                                                <%--    <asp:ListItem Text="Meta-Description" Value="SEDescription"></asp:ListItem>--%>
                                                                    <asp:ListItem Text="Orderable" Value="orderable"></asp:ListItem>
                                                                    <asp:ListItem Text="Options" Value="options"></asp:ListItem>
                                                                    <asp:ListItem Text="Ship-Weight" Value="Weight"></asp:ListItem>
                                                                    <%--<asp:ListItem Text="Meta-Keywords" Value="SEKeywords"></asp:ListItem>--%>
                                                                    <asp:ListItem Text="Manufacturer" Value="Manufacturer"></asp:ListItem>
                                                                    <asp:ListItem Text="Availability" Value="Avail"></asp:ListItem>
                                                                    <asp:ListItem Text="Taxable" Value="taxable"></asp:ListItem>
                                                                    <asp:ListItem Text="Page-Title" Value="SETitle"></asp:ListItem>
                                                                    <asp:ListItem Text="Product-URL" Value="ProductURL"></asp:ListItem>
                                                                    <asp:ListItem Text="Name" Value="name"></asp:ListItem>
                                                                    <asp:ListItem Text="Caption" Value="Description"></asp:ListItem>
                                                                    <asp:ListItem Text="Free-Shipping" Value="IsFreeShipping"></asp:ListItem>
                                                                    <asp:ListItem Text="Condition" Value="Condition"></asp:ListItem>
                                                                    <%-- <asp:ListItem Text="Item-Condition" Value="ItemCondition"></asp:ListItem>--%>
                                                                   <%-- <asp:ListItem Text="Pdf-Link" Value="pdf-link"></asp:ListItem>--%>
                                                                    <asp:ListItem Text="UPC" Value="UPC"></asp:ListItem>
                                                                    <%--<asp:ListItem Text="Pdf2-Link" Value="pdf2-link"></asp:ListItem>--%>
                                                                    <%--<asp:ListItem Text="Pdf2-Title" Value="pdf2-title"></asp:ListItem>--%>
                                                                </asp:CheckBoxList>
                                                            </div>
                                                        </td>
                                                    </tr>
                                                    <tr class="oddrow">
                                                        <td valign="top">
                                                        </td>
                                                        <td>
                                                            <a href="javascript:void(0)" onclick="CheckboxSelection(true);">Check All</a> |
                                                            <a href="javascript:void(0)" onclick="CheckboxSelection(false);">Clear All</a>
                                                        </td>
                                                    </tr>
                                                    <tr class="altrow">
                                                        <td valign="top">
                                                        </td>
                                                        <td>
                                                            <asp:ImageButton ID="btnGenerate" runat="server" OnClick="btnGenerate_Click" OnClientClick="return SearchValidation(1);" />
                                                            &nbsp;&nbsp;
                                                            <asp:ImageButton ID="btnGenerateImages" runat="server" OnClick="btnGenerateImages_Click"
                                                                OnClientClick="return SearchValidation(2);" />
                                                        </td>
                                                    </tr>
                                                    <tr class="oddrow">
                                                        <td colspan="2">
                                                            <asp:Label ID="lblMsg1" runat="server" CssClass="star" Font-Bold="True"></asp:Label><br />
                                                            <br />
                                                            <asp:Label ID="lblLinks" runat="server" CssClass="star" Font-Bold="True"></asp:Label>
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
