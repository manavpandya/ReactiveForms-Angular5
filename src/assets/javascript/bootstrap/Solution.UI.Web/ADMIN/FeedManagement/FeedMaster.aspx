<%@ Page Title="" Language="C#" MasterPageFile="~/ADMIN/Admin.Master" AutoEventWireup="true"
    CodeBehind="FeedMaster.aspx.cs" Inherits="Solution.UI.Web.ADMIN.FeedManagement.FeedMaster" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts-main.js"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts.js"></script>
    <script type="text/javascript">
        function CheckValidations() {

            if (document.getElementById("ContentPlaceHolder1_txtFeedName") != null && document.getElementById("ContentPlaceHolder1_txtFeedName").value.replace(/^\s+|\s+$/g, '') == '') {
                alert("Please Enter Feed Name.");
                $('html, body').animate({ scrollTop: $('#ContentPlaceHolder1_txtFeedName').offset().top }, 'slow');
                document.getElementById("ContentPlaceHolder1_txtFeedName").value = '';
                document.getElementById("ContentPlaceHolder1_txtFeedName").focus();
                return false;
            }
            return true;
        }

        function SetIsBaseFeedvalue() {
            document.getElementById("ContentPlaceHolder1_hdnIsBase").value = "0";
        }
        function confirmIsBase() {
            jConfirm('Base Feed already assigned. Do you want to change it while saving ?', 'Confirmation', function (r) {
                if (r == true) {

                    document.getElementById("ContentPlaceHolder1_chkIsBase").checked = true;
                    document.getElementById("ContentPlaceHolder1_hdnIsBase").value = "1";
                }
                else {
                    document.getElementById("ContentPlaceHolder1_chkIsBase").checked = false;
                    document.getElementById("ContentPlaceHolder1_hdnIsBase").value = "2";
                }
            });
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
                                                    <tr class="altrow">
                                                        <td>
                                                            &nbsp;&nbsp;Store Name:
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="ddlStore" CssClass="order-list" Width="180px" runat="server">
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                    <tr class="oddrow">
                                                        <td>
                                                            <span class="star">*</span>Feed Name:
                                                        </td>
                                                        <td style="width: 80%">
                                                            <asp:TextBox runat="server" ID="txtFeedName" CssClass="order-textfield" Width="250px"
                                                                MaxLength="50"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr class="altrow">
                                                        <td>
                                                            &nbsp;&nbsp;Is Base:
                                                        </td>
                                                        <td>
                                                            <asp:CheckBox ID="chkIsBase" runat="server" onchange="SetIsBaseFeedvalue();" onclick="SetIsBaseFeedvalue();" />
                                                        </td>
                                                    </tr>
                                                    <tr class="oddrow">
                                                        <td>
                                                            &nbsp; Tab Image:
                                                        </td>
                                                        <td>
                                                            <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                                                <tr>
                                                                    <td width="10%">
                                                                        <asp:FileUpload ID="FlTabUpload" runat="server" Width="220px" Style="border: 1px solid #1a1a1a;
                                                                            background: #f5f5f5; color: #000000;" />
                                                                    </td>
                                                                    <td width="9%">
                                                                        <asp:ImageButton ID="btnUploadTabImage" runat="server" AlternateText="Upload Tab Image"
                                                                            OnClick="btnUploadTabImage_Click" />
                                                                    </td>
                                                                    <td width="20%">
                                                                        <img alt="Upload" id="ImgTab" src="/App_Themes/<%=Page.Theme %>/images/icon-image.gif"
                                                                            runat="server" width="150" style="margin-bottom: 5px; border: 1px solid darkgray" />
                                                                    </td>
                                                                    <td width="44%">
                                                                        &nbsp;
                                                                        <asp:ImageButton ID="btnDeleteTab" runat="server" Visible="false" AlternateText="Delete"
                                                                            OnClick="btnDeleteTab_Click" ToolTip="Delete" />
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                    <tr class="altrow">
                                                        <td>
                                                            &nbsp; Hover Image:
                                                        </td>
                                                        <td>
                                                            <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                                                <tr>
                                                                    <td width="10%">
                                                                        <asp:FileUpload ID="FlHoverUpload" runat="server" Width="220px" Style="border: 1px solid #1a1a1a;
                                                                            background: #f5f5f5; color: #000000;" />
                                                                    </td>
                                                                    <td width="9%">
                                                                        <asp:ImageButton ID="btnUploadHoverImage" runat="server" AlternateText="Upload Hover Image"
                                                                            OnClick="btnUploadHoverImage_Click" />
                                                                    </td>
                                                                    <td width="20%">
                                                                        <img alt="Upload" id="ImgHover" src="/App_Themes/<%=Page.Theme %>/images/icon-image.gif"
                                                                            runat="server" width="150" style="margin-bottom: 5px; border: 1px solid darkgray" />
                                                                    </td>
                                                                    <td width="44%">
                                                                        &nbsp;
                                                                        <asp:ImageButton ID="btnDeleteHover" runat="server" Visible="false" AlternateText="Delete"
                                                                            ToolTip="Delete" OnClick="btnDeleteHover_Click" />
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr class="oddrow">
                                            <td>
                                                <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                                    <td>
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
    <div style="visibility: hidden;">
        <input type="hidden" runat="server" id="hdnIsBase" value="0" />
    </div>
</asp:Content>
