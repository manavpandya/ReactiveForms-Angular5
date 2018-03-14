<%@ Page Title="" Language="C#" MasterPageFile="~/ADMIN/Admin.Master" AutoEventWireup="true"
    CodeBehind="AddProductFabric.aspx.cs" Inherits="Solution.UI.Web.ADMIN.Product.AddProductFabric"
    ValidateRequest="false" %>

<%@ Register Assembly="CKEditor.NET" Namespace="CKEditor.NET" TagPrefix="CKEditor" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" src="../../ckeditor/ckeditor.js"></script>
    <script src="../../ckeditor/_samples/sample.js" type="text/javascript"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts-main.js"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts.js"></script>
    <style type="text/css">
        .slidingDiv {
            height: 300px;
            padding: 20px;
            margin-top: 10px;
        }

        .show_hide {
            display: block;
        }
    </style>
    <script src="../JS/jquery-1.3.2.js" type="text/javascript"></script>
    <script type="text/javascript">
        function keyRestrict(e, validchars) {
            var key = '', keychar = '';
            key = getKeyCode(e);
            if (key == null) return true;
            keychar = String.fromCharCode(key);
            keychar = keychar.toLowerCase();
            validchars = validchars.toLowerCase();
            if (validchars.indexOf(keychar) != -1)
                return true;
            if (key == null || key == 0 || key == 8 || key == 9 || key == 13 || key == 27)
                return true;
            return false;
        }

        function getKeyCode(e) {
            if (window.event)
                return window.event.keyCode;
            else if (e)
                return e.which;
            else
                return null;
        }

      
    </script>
    <script type="text/javascript" language="javascript">

        function ValidatePage() {

            if ((document.getElementById('ContentPlaceHolder1_txtFabric').value).replace(/^\s*\s*$/g, '') == '') {
                jAlert('Please Enter Fabric Name', 'Message', 'ContentPlaceHolder1_txtFabric');
                $('html, body').animate({ scrollTop: $('#ContentPlaceHolder1_txtFabric').offset().top }, 'slow');
                return false;
            }
            if ((document.getElementById('ContentPlaceHolder1_txtDescription').value).replace(/^\s*\s*$/g, '') == '') {
                jAlert('Please Enter Fabric Description', 'Message', 'ContentPlaceHolder1_txtDescription');
                $('html, body').animate({ scrollTop: $('#ContentPlaceHolder1_txtDescription').offset().top }, 'slow');
                return false;
            }
            if ((document.getElementById('ContentPlaceHolder1_txtDisplayOrder').value).replace(/^\s*\s*$/g, '') == '') {
                jAlert('Please Enter Display Order', 'Message', 'ContentPlaceHolder1_txtDisplayOrder');
                $('html, body').animate({ scrollTop: $('#ContentPlaceHolder1_txtDisplayOrder').offset().top }, 'slow');
                return false;
            }

            if (document.getElementById('ContentPlaceHolder1_ImgLarge') != null && (document.getElementById('ContentPlaceHolder1_ImgLarge').src == '' || document.getElementById('ContentPlaceHolder1_ImgLarge').src.indexOf('image_not_available') > -1)) {
                jAlert('Please choose image file for Upload', 'Message', 'ContentPlaceHolder1_ImgLarge');
                $('html, body').animate({ scrollTop: $('#ContentPlaceHolder1_ImgLarge').offset().top }, 'slow');
                return false;
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
            &nbsp;
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
                                                <img class="img-left" title="Add New Product Fabric" alt="Add New Product Fabric" src="/App_Themes/<%=Page.Theme %>/images/add-product-icon.png" />
                                                <h2 style="padding-top: 3px;">Add New Product Fabric</h2>
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
                                                                <tr class="altrow">
                                                                    <td width="13%">
                                                                        <span class="star">*</span>Fabric Name:
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="txtFabric" runat="server" MaxLength="500" class="order-textfield"></asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                                <tr class="altrow">
                                                                    <td width="13%">
                                                                        <span class="star">*</span>Fabric Description:
                                                                    </td>
                                                                    <td>
                                                                        <asp:DropDownList ID="ddlFabricGroupType" onchange="chkHeight();" CssClass="order-list" runat="server"
                                                                            AutoPostBack="True" Width="180px">
                                                                        </asp:DropDownList>
                                                                    </td>
                                                                </tr>
                                                                <tr class="altrow">
                                                                    <td width="13%">
                                                                        <span class="star">*</span>Display Order:
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="txtDisplayOrder" onkeypress="return keyRestrict(event,'0123456789.');" runat="server" MaxLength="500" class="order-textfield"></asp:TextBox>
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
                                                                                            <h2>Images</h2>
                                                                                        </div>
                                                                                        <div class="main-title-right">
                                                                                            <a href="javascript:void(0);" class="show_hideImage" title="Close" onclick="return ShowHideButton('ImgImages','tdImages');">
                                                                                                <img id="ImgImages" src="/App_Themes/<%=Page.Theme %>/images/minimize.png" class="minimize"
                                                                                                    title="Minimize" alt="Minimize"></a>
                                                                                        </div>
                                                                                    </th>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td id="tdImages">
                                                                                        <div id="divImage" class="slidingDivImage">
                                                                                            <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                                                                                <tr>
                                                                                                    <td valign="middle">
                                                                                                        <img alt="Upload" id="ImgLarge" runat="server" width="150" style="margin-bottom: 5px; border: 1px solid darkgray" /><br />
                                                                                                    </td>
                                                                                                    <td valign="middle"></td>
                                                                                                </tr>
                                                                                                <tr>
                                                                                                    <td>
                                                                                                        <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                                                                                            <tr>
                                                                                                                <td width="10%">
                                                                                                                    <asp:FileUpload ID="fuProductIcon" runat="server" Width="220px" Style="border: 1px solid #1a1a1a; background: #f5f5f5; color: #000000;" />
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
                                                                                                    <td></td>
                                                                                                    <td id="tduploadPdf" runat="server" visible="false">
                                                                                                        <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                                                                                            <tr>
                                                                                                                <td width="10%"></td>
                                                                                                                <td width="9%"></td>
                                                                                                                <td width="64%"></td>
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
    <input type="hidden" id="hdnimage" runat="server" />
    <!--start tab--->
    <!--end tab--->
</asp:Content>
