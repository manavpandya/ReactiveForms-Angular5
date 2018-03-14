<%@ Page Title="" Language="C#" MasterPageFile="~/ADMIN/Admin.Master" AutoEventWireup="true"
    CodeBehind="FeedExport.aspx.cs" Inherits="Solution.UI.Web.ADMIN.FeedManagement.FeedExport" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts-main.js"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts.js"></script>
    <script language="javascript">
        function selectAll(on) {
            var allElts = document.forms['form1'].elements;
            var i;
            for (i = 0; i < allElts.length; i++) {
                var elt = allElts[i];
                if (elt.type == "checkbox") {
                    elt.checked = on;
                }
            }
        }

        function chkHeight() {
            var windowHeight = 0;
            windowHeight = $(document).height(); //window.innerHeight;

            document.getElementById('prepage').style.height = windowHeight + 'px';
            document.getElementById('prepage').style.display = '';
        }

        function checklist() {
            var allElts = document.getElementById('divchecklist').getElementsByTagName('input');
            var i;
            var total = 0;
            for (i = 0; i < allElts.length; i++) {
                var elt = allElts[i];
                if (elt.type == "checkbox" && elt.checked == true) {
                    total = 1;
                }
            }
            if (total == 0) {
                alert('Please select at least one Field.');
                return false;
            }

            return true;
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
                                                    <img class="img-left" title="Export Feed CSV" alt="Export Feed CSV" src="/App_Themes/<%=Page.Theme %>/Images/admin-list-icon.png">
                                                    <h2>
                                                        <asp:Label runat="server" Text="Export Feed CSV" ID="lblTitle"></asp:Label></h2>
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
                                                    <tr>
                                                        <td colspan="2" align="center">
                                                            <asp:Label ID="lblMessage" ForeColor="Red" runat="server"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr class="altrow">
                                                        <td style="width: 8%">
                                                            &nbsp;&nbsp;Store Name:
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="ddlStore" CssClass="order-list" Width="200px" runat="server"
                                                                AutoPostBack="True" OnSelectedIndexChanged="ddlStore_SelectedIndexChanged">
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                    <tr class="oddrow">
                                                        <td>
                                                            &nbsp;&nbsp;Feed Name:
                                                        </td>
                                                        <td style="width: 80%">
                                                            <asp:DropDownList ID="ddlFeedName" CssClass="order-list" Width="180px" runat="server"
                                                                AutoPostBack="True" OnSelectedIndexChanged="ddlFeedName_SelectedIndexChanged">
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                    <tr class="altrow" runat="server" id="trTop" visible="false">
                                                        <td>
                                                            &nbsp;
                                                        </td>
                                                        <td align="left">
                                                            <span><a id="A1" href="javascript:selectAll(true);">Check All</a> | <a id="A2" href="javascript:selectAll(false);">
                                                                Clear All</a> </span>
                                                        </td>
                                                    </tr>
                                                    <tr class="oddrow" id="trFields" visible="false" runat="server">
                                                        <td>
                                                            &nbsp;&nbsp;Field:
                                                        </td>
                                                        <td>
                                                            <div id="divchecklist">
                                                                <asp:CheckBoxList ID="ChklistFieldName" runat="server" RepeatColumns="10" RepeatDirection="Horizontal">
                                                                </asp:CheckBoxList>
                                                            </div>
                                                        </td>
                                                    </tr>
                                                    <tr class="altrow" runat="server" id="trBottom" visible="false">
                                                        <td>
                                                            &nbsp;
                                                        </td>
                                                        <td align="left">
                                                            <span><a id="lkbAllowAll" href="javascript:selectAll(true);">Check All</a> | <a id="lkbClearAll"
                                                                href="javascript:selectAll(false);">Clear All</a> </span>
                                                        </td>
                                                    </tr>
                                                    <tr class="oddrow">
                                                        <td>
                                                            &nbsp;
                                                        </td>
                                                        <td>
                                                            <asp:ImageButton ID="btnGenerateCSV" runat="server" OnClientClick="return checklist();"
                                                                AlternateText="Generate CSV" ToolTip="Generate CSV" OnClick="btnGenerateCSV_Click" />
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
    <div id="prepage" style="position: absolute; font-family: arial; font-size: 16; left: 0px;
        top: 0px; background-color: #000; opacity: 0.7; filter: alpha(opacity=70); layer-background-color: white;
        height: 100%; width: 100%; z-index: 1000; display: none;">
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
