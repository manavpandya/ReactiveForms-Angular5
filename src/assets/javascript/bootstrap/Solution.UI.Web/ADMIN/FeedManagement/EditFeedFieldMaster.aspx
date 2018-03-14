<%@ Page Title="" Language="C#" MasterPageFile="~/ADMIN/Admin.Master" AutoEventWireup="true"
    CodeBehind="EditFeedFieldMaster.aspx.cs" Inherits="Solution.UI.Web.ADMIN.FeedManagement.EditFeedFieldMaster" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts-main.js"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts.js"></script>
    <script type="text/javascript">
        function CheckValidations() {
            if (document.getElementById("ContentPlaceHolder1_ddlFeed") != null && document.getElementById("ContentPlaceHolder1_ddlFeed").selectedIndex == 0) {
                jAlert('Please select Feed.', 'Message', 'ContentPlaceHolder1_ddlFeed');
                $('html, body').animate({ scrollTop: $('#ContentPlaceHolder1_ddlFeed').offset().top }, 'slow');
                return false;
            }
            if ((document.getElementById('ContentPlaceHolder1_txtFieldName').value).replace(/^\s*\s*$/g, '') == '') {
                jAlert('Please Enter Field Name', 'Message', 'ContentPlaceHolder1_txtFieldName');
                $('html, body').animate({ scrollTop: $('#ContentPlaceHolder1_txtFieldName').offset().top }, 'slow');
                return false;
            }
            if (document.getElementById('ContentPlaceHolder1_ddlFeedType').selectedIndex == 0) {
                jAlert('Please Select Feed Type.', 'Message', 'ContentPlaceHolder1_ddlFeedType');
                $('html, body').animate({ scrollTop: $('#ContentPlaceHolder1_ddlFeedType').offset().top }, 'slow');
                return false;
            }
        }
        function onKeyPressPhone(e) {
            var key = window.event ? window.event.keyCode : e.which;
            if (key == 32 || key == 39 || key == 37 || key == 46 || key == 13 || key == 8 || key == 9 || key == 189 || key == 109 || key == 0 || key == 40 || key == 41 || key == 45) {
                return key;
            }
            var keychar = String.fromCharCode(key);
            var reg = /\d/;
            if (window.event)
                return event.returnValue = reg.test(keychar);
            else
                return reg.test(keychar);
        }
        function onKeyPressString(e) {
            var key = window.event ? window.event.keyCode : e.which;
            //alert(key);
            if ((key >= 65) && (key <= 90) || (key >= 97) && (key <= 122) || (key == 95 || key == 8 || key == 0)) {
                return key;
            }

            var keychar = String.fromCharCode(key);
            var reg = /\s/;
            if (window.event)
                return event.returnValue = reg.test(keychar);
            else
                return reg.test(keychar);
        }
        function onKeyPressNumber(e) {
            var key = window.event ? window.event.keyCode : e.which;
            if ((key > 47 && key < 58) || (key == 0 || key == 8)) {
                return key;
            }

            var keychar = String.fromCharCode(key);
            var reg = /\s/;
            if (window.event)
                return event.returnValue = reg.test(keychar);
            else
                return reg.test(keychar);
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
            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                <tr>
                    <td height="5" align="left" valign="top">
                        <img src="/App_Themes/<%=Page.Theme %>/images/spacer.gif" width="1" height="5">
                    </td>
                </tr>
                <tr>
                    <td height="5" align="left" valign="top">
                        <img src="/App_Themes/<%=Page.Theme %>/images/spacer.gif" width="1" height="5">
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
                                                    <img class="img-left" title="Add Feed Master" alt="Add Feed Master" src="/App_Themes/<%=Page.Theme %>/Images/admin-list-icon.png">
                                                    <h2>
                                                        <asp:Label runat="server" Text="Add Feed Master" ID="lblTitle"></asp:Label></h2>
                                                </div>
                                            </th>
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
                                                        <td>
                                                            <span class="star">*</span>Store Name :
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="ddlStore" class="order-list" runat="server" AutoPostBack="True"
                                                                OnSelectedIndexChanged="ddlStore_SelectedIndexChanged" Width="180px">
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                    <tr class="altrow">
                                                        <td>
                                                            <span class="star">*</span>Feed :
                                                        </td>
                                                        <td style="width: 80%">
                                                            <asp:DropDownList ID="ddlFeed" class="order-list" runat="server" Width="180px">
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                    <tr class="oddrow">
                                                        <td>
                                                            <span class="star">*</span>Field Name :
                                                        </td>
                                                        <td>
                                                            <asp:TextBox runat="server" ID="txtFieldName" CssClass="order-textfield" Width="250px"
                                                                onkeypress="return onKeyPressString(event);" MaxLength="50"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr class="altrow">
                                                        <td>
                                                            <span class="star">*</span>Feed Type :
                                                        </td>
                                                        <td style="width: 80%">
                                                            <asp:DropDownList ID="ddlFeedType" class="order-list" runat="server" Width="180px"
                                                                AutoPostBack="True" OnSelectedIndexChanged="ddlFeedType_SelectedIndexChanged">
                                                            </asp:DropDownList>
                                                            &nbsp;<asp:Literal ID="ltMore" runat="Server"></asp:Literal>
                                                        </td>
                                                    </tr>
                                                    <tr id="trRootCate" runat="server" visible="false" class="altrow">
                                                        <td>
                                                             <span class="star">*</span> Category Root Id :
                                                        </td>
                                                        <td style="width: 80%">
                                                            <asp:TextBox ID="txtCateRootId" runat="server" onkeypress="return onKeyPressNumber(event);"
                                                                CssClass="order-textfield" Width="100px"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr class="oddrow">
                                                        <td>
                                                             <span class="star">&nbsp;</span>Field Description :
                                                        </td>
                                                        <td>
                                                            <asp:TextBox runat="server" ID="txtFeedDesc" TextMode="MultiLine" CssClass="order-textfield"
                                                                Width="350px" Height="50px" MaxLength="50"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr class="altrow">
                                                        <td>
                                                             <span class="star">&nbsp;</span>Default Value :
                                                        </td>
                                                        <td>
                                                            <asp:TextBox runat="server" ID="txtDefaultValue" CssClass="order-textfield" Width="80px"
                                                                MaxLength="50"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr class="oddrow">
                                                        <td>
                                                             <span class="star">&nbsp;</span>Is Required :
                                                        </td>
                                                        <td>
                                                            <asp:CheckBox ID="chkIsBase" runat="server" />
                                                        </td>
                                                    </tr>
                                                    <tr class="altrow">
                                                        <td>
                                                             <span class="star">&nbsp;</span>Size :
                                                        </td>
                                                        <td>
                                                            Width :
                                                            <asp:TextBox runat="server" ID="txtWidth" CssClass="order-textfield" Width="60px"
                                                                onkeypress="return onKeyPressPhone(event);" MaxLength="50"></asp:TextBox>
                                                            &nbsp;Height :
                                                            <asp:TextBox runat="server" ID="txtHeight" CssClass="order-textfield" Width="60px"
                                                                onkeypress="return onKeyPressPhone(event);" MaxLength="50"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr class="oddrow">
                                                        <td>
                                                             <span class="star">&nbsp;</span>Limit :
                                                        </td>
                                                        <td>
                                                            <asp:TextBox runat="server" ID="txtLimit" CssClass="order-textfield" Width="60px"
                                                                onkeypress="return onKeyPressPhone(event);" MaxLength="50"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr class="altrow">
                                                        <td>
                                                             <span class="star">&nbsp;</span>Display Order :
                                                        </td>
                                                        <td>
                                                            <asp:TextBox runat="server" ID="txtDisplayOrder" CssClass="order-textfield" Width="60px"
                                                                onkeypress="return onKeyPressNumber(event);" MaxLength="50"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr class="oddrow">
                                            <td>
                                                <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                                    <td>
                                                        &nbsp;
                                                    </td>
                                                    <td style="width: 80%">
                                                        <asp:ImageButton ID="btnSave" runat="server" AlternateText="Save" ToolTip="Save"
                                                            OnClientClick="return CheckValidations();" OnClick="btnSave_Click" />
                                                        <asp:ImageButton ID="btncancel" runat="server" AlternateText="Cancel" ToolTip="Cancel"
                                                            OnClick="btncancel_Click" />
                                                    </td>
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
                        <img src="/App_Themes/<%=Page.Theme %>/images/spacer.gif" width="1" height="10">
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
</asp:Content>
