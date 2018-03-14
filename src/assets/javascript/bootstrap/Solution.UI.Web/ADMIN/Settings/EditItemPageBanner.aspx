<%@ Page Title="" Language="C#" MasterPageFile="~/ADMIN/Admin.Master" AutoEventWireup="true"
    CodeBehind="EditItemPageBanner.aspx.cs" Inherits="Solution.UI.Web.ADMIN.Settings.EditItemPageBanner" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
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
            $('#ContentPlaceHolder1_txtStartDate').focus(function () {
                $('#ContentPlaceHolder1_txtStartDate').datetimepicker('show');
            });
            $('#ContentPlaceHolder1_txtEndDate').focus(function () {
                $('#ContentPlaceHolder1_txtEndDate').datetimepicker('show');
            });
        });
    </script>
    <script type="text/javascript">
        function Checkfields() {

            if (document.getElementById('ContentPlaceHolder1_TxtbannerTitle').value == '') {
                jAlert('Please Enter Item Page Banner Title.', 'Required Information', 'ContentPlaceHolder1_TxtbannerTitle');
                return false;
            }
            if (document.getElementById('ContentPlaceHolder1_txtStartDate').value == '') {
                jAlert('Please Select Start Date.', 'Required Information', 'ContentPlaceHolder1_txtStartDate');
                return false;
            }
            else if (document.getElementById('ContentPlaceHolder1_txtEndDate').value == '') {
                jAlert('Please Select End Date.', 'Required Information', 'ContentPlaceHolder1_txtEndDate');
                return false;
            }
            var startDate = new Date(document.getElementById('ContentPlaceHolder1_txtStartDate').value);
            var endDate = new Date(document.getElementById('ContentPlaceHolder1_txtEndDate').value);
            if (startDate > endDate) {
                jAlert('Please Select Valid Date for Duration.', 'Required Information', 'ContentPlaceHolder1_txtEndDate');
                return false;
            }
            else if (document.getElementById('ContentPlaceHolder1_txtEndDate').value == '') {
                jAlert('Please Select End Date.', 'Required Information', 'ContentPlaceHolder1_txtEndDate');
                return false;
            }
            else if (document.getElementById('ContentPlaceHolder1_txtAllowedProducts').value == '') {
                jAlert('Please Select Allocated Product(s).', 'Required Information', 'ContentPlaceHolder1_txtAllowedProducts');
                return false;
            }
            //            else if (document.getElementById('ContentPlaceHolder1_FileUploadBanner').value == '') {
            //                jAlert('Please select banner to upload', 'Required Information');
            //                return false;
            //            }
            return true;
        }

        var myWindow;
        function openCenteredCrossSaleWindow(x) {
            createCookie('prskus', document.getElementById(x).value, 1)
            var width = 900;
            var height = 600;
            var left = parseInt((screen.availWidth / 2) - (width / 2));
            var top = parseInt((screen.availHeight / 2) - (height / 2));
            var StoreID = document.getElementById("ContentPlaceHolder1_ddlStore").options[document.getElementById("ContentPlaceHolder1_ddlStore").selectedIndex].value;
            var ProductID = 0;
            var windowFeatures = "width=" + width + ",height=" + height + ",status,resizable,scrollbars=yes,left=" + left + ",top=" + top + "screenX=" + left + ",screenY=" + top;
            myWindow = window.open('/Admin/Products/ProductSku.aspx?StoreID=' + StoreID + '&ProductID=' + ProductID + '&clientid=' + x, "subWind", windowFeatures);
        }
        function createCookie(name, value, days) {
            if (days) {
                var date = new Date();
                date.setTime(date.getTime() + (days * 24 * 60 * 60 * 1000));
                var expires = "; expires=" + date.toGMTString();
            }
            else var expires = "";
            document.cookie = name + "=" + value + expires + "; path=/";
        }

        function changeSizeLable() {
            if (document.getElementById('ContentPlaceHolder1_ddlBannerType') != null && document.getElementById('ContentPlaceHolder1_ddlBannerType').options[document.getElementById('ContentPlaceHolder1_ddlBannerType').selectedIndex].value == "1") {
                document.getElementById('ContentPlaceHolder1_lblBigImgSize').style.display = '';
                document.getElementById('ContentPlaceHolder1_lblSmallImgSize').style.display = 'none';
            }
            else {
                document.getElementById('ContentPlaceHolder1_lblBigImgSize').style.display = 'none';
                document.getElementById('ContentPlaceHolder1_lblSmallImgSize').style.display = '';
            }
        }
        
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="content-width">
        <div class="content-row1">
            <div class="create-new-order">
                &nbsp;</div>
        </div>
        <div class="content-row2">
            <table width="100%" cellspacing="0" cellpadding="0" border="0" bgcolor="#ffffff"
                class="content-table">
                <tbody>
                    <tr>
                        <td class="border-td-sub">
                            <table align="left" class="add-product" border="0" cellpadding="0" cellspacing="0"
                                width="100%">
                                <tr>
                                    <th colspan="2">
                                        <div class="main-title-left">
                                            <img class="img-left" title="Add Admin" alt="Add Admin" src="/App_Themes/<%=Page.Theme %>/Images/home-page-banner-icon.png" />
                                            <h2>
                                                <asp:Label runat="server" Text="Edit Home Page Banner" ID="lblTitle"></asp:Label></h2>
                                        </div>
                                    </th>
                                </tr>
                                <tr>
                                    <td colspan="2" align="center">
                                        <asp:Label ID="lblMsg" runat="server" CssClass="error"></asp:Label>
                                    </td>
                                </tr>
                                <tr class="altrow">
                                    <td colspan="2" align="right">
                                        <span style="color: Red;">*</span><asp:Label ID="Label1" runat="server"> is Required Field</asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" class="font-black01" style="width: 138px; height: 30px" valign="middle">
                                        <span style="color: #ff0033"></span><span style="color: #ff0033">*</span> Store
                                        Name :
                                    </td>
                                    <td align="left" style="height: 30px" valign="middle">
                                        <asp:DropDownList ID="ddlStore" runat="server" DataTextField="StoreName" DataValueField="StoreID"
                                            CssClass="add-product-list" Style="width: 200px;">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" class="font-black01" style="width: 138px; height: 30px" valign="middle">
                                        <span style="color: #ff0033"></span><span style="color: #ff0033">*</span> Banner
                                        Type:
                                    </td>
                                    <td align="left" style="height: 30px" valign="middle">
                                        <asp:DropDownList ID="ddlBannerType" runat="server" CssClass="add-product-list" Style="width: 200px;"
                                            onchange="changeSizeLable();" onclick="changeSizeLable();">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr class="altrow">
                                    <td align="left" valign="top">
                                        <span style="color: Red;">*</span> Banner Title :
                                    </td>
                                    <td align="left">
                                        <asp:TextBox ID="TxtbannerTitle" Width="350px" runat="server" CssClass="order-textfield"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="TxtbannerTitle"
                                            ErrorMessage="Please Enter Banner Title" CssClass="rferror" SetFocusOnError="True"
                                            ValidationGroup="AppConfig"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr class="altrow">
                                    <td align="left" valign="top">
                                        <span style="color: Red;">&nbsp;</span> Banner URL :
                                    </td>
                                    <td align="left">
                                        <asp:TextBox ID="TxtBannerURL" Width="350px" runat="server" CssClass="order-textfield"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr class="altrow">
                                    <td align="left" valign="top">
                                        <span style="color: Red;">*</span> Banner Duration :
                                    </td>
                                    <td align="left">
                                        Start Date :
                                        <asp:TextBox ID="txtStartDate" runat="server" CssClass="from-textfield" Width="70px"
                                            Style="margin-right: 3px;"></asp:TextBox>
                                        &nbsp;&nbsp; End Date :
                                        <asp:TextBox ID="txtEndDate" runat="server" CssClass="from-textfield" Width="70px"
                                            Style="margin-right: 3px;"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr class="altrow">
                                    <td align="left" valign="top">
                                        <span style="color: Red;">*</span>Allowed Products :
                                    </td>
                                    <td align="left">
                                        <asp:TextBox ID="txtAllowedProducts" runat="server" CssClass="status-textfield" Width="40%"
                                            TextMode="MultiLine" TabIndex="33"></asp:TextBox>&nbsp;&nbsp;&nbsp; <a id="aRelated"
                                                name="aRelated" onclick="openCenteredCrossSaleWindow('ContentPlaceHolder1_txtAllowedProducts');"
                                                style="margin-right: 15px; font-weight: bold; cursor: pointer;" tabindex="34">Select
                                                Product(s) </a>
                                    </td>
                                </tr>
                                <tr class="table_bg">
                                    <td align="left" class="font-black01" valign="top">
                                        &nbsp;&nbsp; Banner Image :
                                    </td>
                                    <td align="left">
                                        <asp:FileUpload ID="FileUploadBanner" runat="server" />
                                        <asp:Label ID="lblBigImgSize" runat="server" Text="Size should be 545 x 138" Style="display: none;"></asp:Label>
                                        <asp:Label ID="lblSmallImgSize" runat="server" Text="Size should be 200 x 100" Style="display: none;"></asp:Label>
                                        <div>
                                            <br />
                                            <img id="imgBanner" runat="server" visible="false" /></div>
                                    </td>
                                </tr>
                                <tr class="altrow" style="display: none;">
                                    <td align="left" valign="top">
                                        &nbsp;&nbsp; Display Order :
                                    </td>
                                    <td align="left">
                                        <asp:TextBox ID="TxtDisplayOrder" Width="40px" Style="text-align: center" MaxLength="2"
                                            runat="server" CssClass="order-textfield"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr class="table_bg" style="display: none;">
                                    <td align="left" valign="top">
                                        &nbsp;&nbsp;&nbsp;Active :
                                    </td>
                                    <td align="left">
                                        <asp:CheckBox ID="chkActive" runat="server" Checked="false" />
                                    </td>
                                </tr>
                                <tr class="altrow">
                                    <td align="left" height="30" valign="top" style="width: 137px">
                                    </td>
                                    <td align="left">
                                        <asp:ImageButton ID="imgSave" runat="server" AlternateText="Save" ToolTip="Save"
                                            OnClick="imgSave_Click" OnClientClick="return Checkfields();" />
                                        <asp:ImageButton ID="imgCancle" runat="server" AlternateText="Cancel" ToolTip="Cancel"
                                            OnClick="imgCancle_Click" />
                                </tr>
                            </table>
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
    </div>
</asp:Content>
