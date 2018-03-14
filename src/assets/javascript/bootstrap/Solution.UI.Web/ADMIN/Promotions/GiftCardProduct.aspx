<%@ Page Title="" Language="C#" MasterPageFile="~/ADMIN/Admin.Master" AutoEventWireup="true"
    CodeBehind="GiftCardProduct.aspx.cs" Inherits="Solution.UI.Web.ADMIN.Promotions.GiftCardProduct"
    ValidateRequest="false" %>

<%@ Register Assembly="CKEditor.NET" Namespace="CKEditor.NET" TagPrefix="CKEditor" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" src="../../ckeditor/ckeditor.js"></script>
    <script src="../../ckeditor/_samples/sample.js" type="text/javascript"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts-main.js"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts.js"></script>
    <style type="text/css">
        .slidingDiv
        {
            height: 300px;
            padding: 20px;
            margin-top: 10px;
        }
        .show_hide
        {
            display: block;
        }
    </style>
    <script src="../JS/jquery-1.3.2.js" type="text/javascript"></script>
    <script type="text/javascript">

        $(document).ready(function () {

            $(".slidingDiv").show();
            $(".show_hide").show();

            $('.show_hide').click(function () {
                $(".slidingDiv").slideToggle();
            });

        });

        $(document).ready(function () {

            $(".slidingDivImage").show();
            $(".show_hideImage").show();

            $('.show_hideImage').click(function () {
                $(".slidingDivImage").slideToggle();
            });
        });

        $(document).ready(function () {

            $(".slidingDivSEO").show();
            $(".show_hideSEO").show();

            $('.show_hideSEO').click(function () {
                $(".slidingDivSEO").slideToggle();
            });
        });



        $(document).ready(function () {

            $(".slidingDivDesc").show();
            $(".show_hideDesc").show();

            $('.show_hideDesc').click(function () {
                $(".slidingDivDesc").slideToggle();
            });
        });

        $(document).ready(function () {

            $(".slidingDivMainDiv").show();
            $(".show_hideMainDiv").show();

            $('.show_hideMainDiv').click(function () {
                $(".slidingDivMainDiv").slideToggle();
            });
        });

        $(document).ready(function () {

            $(".slidingDivProductDesc").show();
            $(".show_hideProductDesc").show();

            $('.show_hideProductDesc').click(function () {
                $(".slidingDivProductDesc").slideToggle();
            });
        });
        
    </script>
    <script type="text/javascript" language="javascript">

        function ValidatePage() {

            if ((document.getElementById('ContentPlaceHolder1_txtSKU').value).replace(/^\s*\s*$/g, '') == '') {
                jAlert('Please Enter SKU', 'Message', 'ContentPlaceHolder1_txtSKU');
                $('html, body').animate({ scrollTop: $('#ContentPlaceHolder1_txtSKU').offset().top }, 'slow');
                return false;
            }
            if ((document.getElementById('ContentPlaceHolder1_txtProductName').value).replace(/^\s*\s*$/g, '') == '') {
                jAlert('Please Enter Product Name', 'Message', 'ContentPlaceHolder1_txtProductName');
                $('html, body').animate({ scrollTop: $('#ContentPlaceHolder1_txtProductName').offset().top }, 'slow');
                return false;
            }
            if ((document.getElementById('ContentPlaceHolder1_txtourprice').value).replace(/^\s*\s*$/g, '') == '') {
                jAlert('Please Enter our price', 'Message', 'ContentPlaceHolder1_txtourprice');
                $('html, body').animate({ scrollTop: $('#ContentPlaceHolder1_txtourprice').offset().top }, 'slow');
                return false;
            }

            if ((document.getElementById('ContentPlaceHolder1_txtInventory').value).replace(/^\s*\s*$/g, '') == '') {
                jAlert('Please Enter Inventory', 'Message', 'ContentPlaceHolder1_txtInventory');
                $('html, body').animate({ scrollTop: $('#ContentPlaceHolder1_txtInventory').offset().top }, 'slow');
                return false;
            }
            if ((document.getElementById('ContentPlaceHolder1_txtavailablestartdate').value).replace(/^\s*\s*$/g, '') != '' && (document.getElementById('ContentPlaceHolder1_txtavailableenddate').value).replace(/^\s*\s*$/g, '') != '') {

                if ((document.getElementById('ContentPlaceHolder1_txtavailablestartdate').value).replace(/^\s*\s*$/g, '') > (document.getElementById('ContentPlaceHolder1_txtavailableenddate').value).replace(/^\s*\s*$/g, '')) {
                    jAlert('Please enter end date grater than or equal to start date.', 'Message', 'ContentPlaceHolder1_txtavailableenddate');
                    $('html, body').animate({ scrollTop: $('#ContentPlaceHolder1_txtavailableenddate').offset().top }, 'slow');
                    return false;
                }
            }

            return true;
        }
    </script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-calender-custom.js"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-calender.js"></script>
    <script language="javascript" type="text/javascript">
        $(function () {

            $('#ContentPlaceHolder1_txtavailablestartdate').datetimepicker({
                showButtonPanel: true, ampm: false,
                showHour: false, showMinute: false, showSecond: false, showTime: false
            });
        });
        $(function () {
            $('#ContentPlaceHolder1_txtavailableenddate').datetimepicker({
                showButtonPanel: true, ampm: false,
                showHour: false, showMinute: false, showSecond: false, showTime: false
            });
        });
       
        
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="content-row1">
        <div class="create-new-order">
            &nbsp;</div>
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
            <tr id="trProductData" runat="server">
                <td class="border-td">
                    <table width="100%" border="0" cellpadding="0" cellspacing="0" bgcolor="#FFFFFF"
                        class="content-table">
                        <tr>
                            <td class="border-td-sub">
                                <table width="100%" border="0" cellpadding="0" cellspacing="0" class="add-product">
                                    <tr style="width: 1126px;">
                                        <th>
                                            <div class="main-title-left">
                                                <img class="img-left" title="Add Gift Card Product" alt="Add Product" src="/App_Themes/<%=Page.Theme %>/images/add-product-icon.png" />
                                                <h2 style="padding-top: 3px;">
                                                    Add Gift Card Product</h2>
                                            </div>
                                            <div class="main-title-right">
                                                <a href="javascript:void(0);" class="show_hideMainDiv" onclick="return ShowHideButton('imgMainDiv','tdMainDiv');">
                                                    <img id="imgMainDiv" class="minimize" title="Minimize" alt="Minimize" src="/App_Themes/<%=Page.Theme %>/images/minimize.png" /></a>
                                            </div>
                                        </th>
                                    </tr>
                                    <tr>
                                        <td id="tdMainDiv">
                                            <div id="divMain" class="slidingDivMainDiv">
                                                <table width="100%" border="0" cellpadding="0" cellspacing="0" class="add-product">
                                                    <tr class="altrow">
                                                        <td align="center">
                                                            <asp:Label ID="lblMsg" ForeColor="Red" runat="server"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr class="even-row">
                                                        <td>
                                                            <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                                                <tr class="oddrow">
                                                                    <td width="13%">
                                                                        <span class="star">*</span>Store Name:
                                                                    </td>
                                                                    <td width="88%">
                                                                        <asp:DropDownList ID="ddlStore" runat="server" Width="185px" CssClass="order-list"
                                                                            Style="margin-left: 0px">
                                                                        </asp:DropDownList>
                                                                    </td>
                                                                </tr>
                                                                <tr class="altrow">
                                                                    <td width="13%">
                                                                        <span class="star">*</span>SKU:
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="txtSKU" runat="server" MaxLength="500" class="order-textfield"></asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                                <tr class="oddrow">
                                                                    <td width="13%">
                                                                        <span class="star">*</span>Product&nbsp;Name:
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="txtProductName" runat="server" MaxLength="500" class="order-textfield"
                                                                            Width="700px"></asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                                <tr class="altrow">
                                                                    <td width="13%" valign="top">
                                                                        <span class="star">&nbsp;</span>Description:
                                                                    </td>
                                                                    <td width="88%">
                                                                        <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                                                            <tr>
                                                                                <td width="100%" valign="top">
                                                                                    <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                                                                        <tr>
                                                                                            <td>
                                                                                                <table width="100%" cellspacing="0" cellpadding="0" border="0" class="content-table border-td">
                                                                                                    <tbody>
                                                                                                        <tr>
                                                                                                            <th>
                                                                                                                <div class="main-title-left">
                                                                                                                    <h2>
                                                                                                                        Description</h2>
                                                                                                                </div>
                                                                                                                <div class="main-title-right">
                                                                                                                    <a href="javascript:void(0);" class="show_hideProductDesc" onclick="return ShowHideButton('imgDescription','tdDescription');">
                                                                                                                        <img id="imgDescription" src="/App_Themes/<%=Page.Theme %>/images/minimize.png" class="minimize"
                                                                                                                            title="Minimize" alt="Minimize" /></a>
                                                                                                                </div>
                                                                                                            </th>
                                                                                                        </tr>
                                                                                                        <tr>
                                                                                                            <td id="tdDescription">
                                                                                                                <div id="tab-container-1" class="slidingDivProductDesc">
                                                                                                                    <ul class="menu">
                                                                                                                        <li id="product-Description" class="active">Detail Description</li>
                                                                                                                    </ul>
                                                                                                                    <span class="clear"></span>
                                                                                                                    <div class="tab-content product-Description">
                                                                                                                        <div class="tab-content-3">
                                                                                                                            <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                                                                                                                <tr>
                                                                                                                                    <td>
                                                                                                                                        <div id="divDescription" runat="Server" visible="false">
                                                                                                                                            <table width="100%" border="0" cellpadding="0" cellspacing="0" class="table">
                                                                                                                                                <tr>
                                                                                                                                                    <td width="144px">
                                                                                                                                                        <asp:DropDownList ID="ddlDescription" runat="server" CssClass="order-list" Width="142px">
                                                                                                                                                            <asp:ListItem Text="Description Tab : 01" Value="1"></asp:ListItem>
                                                                                                                                                            <asp:ListItem Text="Description Tab : 02" Value="2"></asp:ListItem>
                                                                                                                                                            <asp:ListItem Text="Description Tab : 03" Value="3"></asp:ListItem>
                                                                                                                                                            <asp:ListItem Text="Description Tab : 04" Value="4"></asp:ListItem>
                                                                                                                                                            <asp:ListItem Text="Description Tab : 05" Value="5"></asp:ListItem>
                                                                                                                                                        </asp:DropDownList>
                                                                                                                                                    </td>
                                                                                                                                                    <td style="text-align: left">
                                                                                                                                                        Tab Title:
                                                                                                                                                        <asp:TextBox ID="txtTitleDesc" runat="server" class="order-textfield" MaxLength="50"></asp:TextBox>&nbsp;&nbsp;
                                                                                                                                                        <asp:LinkButton ID="lnkSaveDesc" runat="server" Font-Bold="true" Font-Size="12px"
                                                                                                                                                            Font-Underline="true" Text="Save Description">Save Description</asp:LinkButton>
                                                                                                                                                    </td>
                                                                                                                                                    <td width="20%" align="left">
                                                                                                                                                    </td>
                                                                                                                                                </tr>
                                                                                                                                                <tr style="display: none;">
                                                                                                                                                    <td>
                                                                                                                                                        <span>Is Tabbing Display</span>
                                                                                                                                                        <asp:CheckBox ID="chkIsTabbingDisplay" Checked="true" runat="server" />
                                                                                                                                                    </td>
                                                                                                                                                    <td>
                                                                                                                                                    </td>
                                                                                                                                                    <td>
                                                                                                                                                        &nbsp;
                                                                                                                                                    </td>
                                                                                                                                                </tr>
                                                                                                                                            </table>
                                                                                                                                        </div>
                                                                                                                                    </td>
                                                                                                                                </tr>
                                                                                                                                <tr>
                                                                                                                                    <td>
                                                                                                                                        <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                                                                                                                            <tr>
                                                                                                                                                <td class="ckeditor-table">
                                                                                                                                                    <asp:TextBox TextMode="multiLine" class="description-textarea" ID="txtDescription"
                                                                                                                                                        Rows="10" Columns="80" runat="server" Style="border: solid 1px #e7e7e7; background: none repeat scroll 0 0 #E7E7E7;"></asp:TextBox>
                                                                                                                                                    <script type="text/javascript">
                                                                                                                                                        CKEDITOR.replace('<%= txtDescription.ClientID %>', { baseHref: '<%= ResolveUrl("~/ckeditor/") %>', height: 400 });
                                                                                                                                                        CKEDITOR.on('dialogDefinition', function (ev) {
                                                                                                                                                            if (ev.data.name == 'image') {
                                                                                                                                                                var btn = ev.data.definition.getContents('info').get('browse');
                                                                                                                                                                btn.hidden = false;
                                                                                                                                                                btn.onClick = function () { window.open(CKEDITOR.basePath + 'ImageBrowser.aspx', 'popuppage', 'scrollbars=no,width=780,height=630,left=' + ((screen.width - 780) / 2) + ',top=' + ((screen.height - 630) / 2) + ',resizable=no,toolbar=no,titlebar=no'); };
                                                                                                                                                            }
                                                                                                                                                            if (ev.data.name == 'link') {
                                                                                                                                                                var btn = ev.data.definition.getContents('info').get('browse');
                                                                                                                                                                btn.hidden = false;
                                                                                                                                                                btn.onClick = function () { window.open(CKEDITOR.basePath + 'LinkBrowser.aspx', 'popuppage', 'scrollbars=no,width=520,height=580,left=' + ((screen.width - 520) / 2) + ',top=' + ((screen.height - 580) / 2) + ',resizable=no,toolbar=no,titlebar=no'); };
                                                                                                                                                            }
                                                                                                                                                        });
                                                                                                                                                    </script>
                                                                                                                                                </td>
                                                                                                                                            </tr>
                                                                                                                                        </table>
                                                                                                                                    </td>
                                                                                                                                </tr>
                                                                                                                                <tr>
                                                                                                                                    <td>
                                                                                                                                        &nbsp;
                                                                                                                                    </td>
                                                                                                                                </tr>
                                                                                                                            </table>
                                                                                                                        </div>
                                                                                                                    </div>
                                                                                                                </div>
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                    </tbody>
                                                                                                </table>
                                                                                            </td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </td>
                                                                                <td valign="top">
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                </tr>
                                                                <tr class="oddrow" style="display: none;">
                                                                    <td width="13%">
                                                                        <span class="star">&nbsp;</span>Available Start Date :
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="txtavailablestartdate" runat="server" MaxLength="15" class="order-textfield"></asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                                <tr class="altrow" style="display: none;">
                                                                    <td width="13%">
                                                                        <span class="star">&nbsp;</span>Available End Date :
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="txtavailableenddate" runat="server" MaxLength="15" class="order-textfield"></asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                                <tr class="oddrow">
                                                                    <td width="13%">
                                                                        <span class="star">*</span>Our Price :
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="txtourprice" runat="server" MaxLength="15" class="order-textfield"></asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                                <tr class="altrow">
                                                                    <td width="13%">
                                                                        <span class="star">*</span>Inventory :
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="txtInventory" runat="server" MaxLength="8" class="order-textfield"></asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                                <tr class="oddrow">
                                                                    <td width="13%">
                                                                        <span class="star">*</span>Is Active :
                                                                    </td>
                                                                    <td>
                                                                        <asp:CheckBox ID="chkIsActive" runat="server" />
                                                                    </td>
                                                                </tr>
                                                                <tr class="oddrow">
                                                                    <td width="13%">
                                                                        <span class="star">&nbsp;</span>SE-Page Title :
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="txtSEPageTitle" runat="server" MaxLength="500" Height="86px" class="order-textfield"
                                                                            TextMode="MultiLine"></asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                                <tr class="altrow">
                                                                    <td width="13%">
                                                                        <span class="star">&nbsp;</span>SE-Keywords :
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="txtSEKeywords" runat="server" MaxLength="8" Height="86px" class="order-textfield"
                                                                            TextMode="MultiLine"></asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                                <tr class="oddrow">
                                                                    <td width="13%">
                                                                        <span class="star">&nbsp;</span>SE-Description :
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="txtSEDescription" runat="server" MaxLength="500" Height="86px" class="order-textfield"
                                                                            TextMode="MultiLine"></asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                                <tr class="altrow">
                                                                    <td width="13%">
                                                                        <span class="star">&nbsp;</span> Icon Image :
                                                                    </td>
                                                                    <td>
                                                                        <table width="60%" cellspacing="0" cellpadding="0" border="0" class="content-table border-td">
                                                                            <tbody>
                                                                                <tr>
                                                                                    <th>
                                                                                        <div class="main-title-left">
                                                                                            <img class="img-left" title="Add Image" alt="Add Iamage" src="/App_Themes/<%=Page.Theme %>/icon/add-product.png">
                                                                                            <h2>
                                                                                                Images</h2>
                                                                                        </div>
                                                                                        <div class="main-title-right">
                                                                                            <a href="javascript:void(0);" class="show_hideImage" title="Close" onclick="return ShowHideButton('ImgImages','tdImages');">
                                                                                                <img id="ImgImages" src="/App_Themes/<%=Page.Theme %>/images/minimize.png" class="minimize"
                                                                                                    title="Minimize" alt="Minimize"></a></div>
                                                                                    </th>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td id="tdImages">
                                                                                        <div id="divImage" class="slidingDivImage">
                                                                                            <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                                                                                <tr>
                                                                                                    <td valign="middle">
                                                                                                        <img alt="Upload" id="ImgLarge" runat="server" width="150" style="margin-bottom: 5px;
                                                                                                            border: 1px solid darkgray" /><br />
                                                                                                    </td>
                                                                                                    <td valign="middle">
                                                                                                    </td>
                                                                                                </tr>
                                                                                                <tr>
                                                                                                    <td>
                                                                                                        <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                                                                                            <tr>
                                                                                                                <td width="10%">
                                                                                                                    <asp:FileUpload ID="fuProductIcon" runat="server" Width="220px" Style="border: 1px solid #1a1a1a;
                                                                                                                        background: #f5f5f5; color: #000000;" />
                                                                                                                </td>
                                                                                                                <td width="9%">
                                                                                                                    <asp:ImageButton ID="btnUpload" runat="server" AlternateText="Upload" OnClick="btnUpload_Click" />
                                                                                                                </td>
                                                                                                                <td width="64%">
                                                                                                                    <asp:ImageButton ID="btnDelete" runat="server" Visible="false" AlternateText="Delete"
                                                                                                                        OnClick="btnDelete_Click" Style="width: 14px" />
                                                                                                                </td>
                                                                                                            </tr>
                                                                                                        </table>
                                                                                                    </td>
                                                                                                    <td>
                                                                                                    </td>
                                                                                                    <td id="tduploadPdf" runat="server" visible="false">
                                                                                                        <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                                                                                            <tr>
                                                                                                                <td width="10%">
                                                                                                                </td>
                                                                                                                <td width="9%">
                                                                                                                </td>
                                                                                                                <td width="64%">
                                                                                                                </td>
                                                                                                            </tr>
                                                                                                        </table>
                                                                                                    </td>
                                                                                                </tr>
                                                                                            </table>
                                                                                        </div>
                                                                                    </td>
                                                                                </tr>
                                                                            </tbody>
                                                                        </table>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center">
                                            <br />
                                            <asp:ImageButton ID="btnSave" runat="server" OnClientClick="return ValidatePage();"
                                                OnClick="btnSave_Click" />
                                            &nbsp;&nbsp;
                                            <asp:ImageButton ID="btnCancel" runat="server" OnClick="btnCancel_Click" />
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
        </table>
    </div>
    <!--start tab--->
    <!--end tab--->
</asp:Content>
