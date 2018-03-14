<%@ Page Title="" Language="C#" MasterPageFile="~/ADMIN/Admin.Master" AutoEventWireup="true" CodeBehind="EFFHomepageRatotingbanner.aspx.cs" Inherits="Solution.UI.Web.ADMIN.Settings.EFFHomepageRatotingbanner" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts-main.js"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts.js"></script>
    <script src="/App_Themes/<%=Page.Theme %>/js/jqueryControl.js"></script>
    <script src="/App_Themes/<%=Page.Theme %>/js/jquery.icheck.min.js"></script>
    <script src="/App_Themes/<%=Page.Theme %>/js/CustomhtmlControl.js"></script>

    <script type="text/javascript">

        function gotonextlink() {
            var tt = 0;
            if (document.getElementById('flat-radio') != null && document.getElementById('flat-radio').checked == true) {
                tt = 1;
            }
            else if (document.getElementById('flat-radio-1') != null && document.getElementById('flat-radio-1').checked == true) {
                tt = 2;
            }
            else if (document.getElementById('flat-radio-2') != null && document.getElementById('flat-radio-2').checked == true) {
                tt = 3;
            }
            if (tt > 0) {
                window.location.href = 'EFFHomepageRotatorbannerupload.aspx?grouptype=new&&id=' + tt.toString() + '&storeid=' + document.getElementById('<%=ddlStore.ClientID %>').options[document.getElementById('<%=ddlStore.ClientID %>').selectedIndex].value;
            }
            else {

                jAlert('Please Select Layout Type!', 'Message', 'flat-radio');

            }
        }

        $(document).ready(function () {
            $('input').iCheck({
                checkboxClass: 'icheckbox_flat-red',
                radioClass: 'iradio_flat-red',
                increaseArea: '20%'
            });
        });
    </script>
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div id="content-width">
        <div class="content-row1">
            <div class="create-new-order" style="width: 100%; padding-top: 5px;">
                <table>
                    <tr>
                        <td>
                            Store:
                            <asp:DropDownList ID="ddlStore" runat="server" CssClass="product-type" Height="21px"
                                AutoPostBack="true" onselectedindexchanged="ddlStore_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
        <div class="content-row2">
            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                <tr>
                    <td height="5" align="left" valign="top">
                        <img src="/App_Themes/<%= Page.Theme %>/images/spacer.gif" width="1" height="5" />
                    </td>
                </tr>
                <tr>
                    <td height="5" align="left" valign="top">
                        <img src="/App_Themes/<%= Page.Theme %>/images/spacer.gif" width="1" height="5" />
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
                                            <th colspan="2">
                                                <div class="main-title-left">
                                                     <img class="img-left" title="Add Home Page Banner" alt="Add Home Page Banner" src="/App_Themes/<%=Page.Theme %>/Images/home-page-banner-icon.png" />
                                                    <h2>
                                                        <asp:Label ID="lblTitle" runat="server" Text="Add Home Page Banner">
                                                        </asp:Label>
                                                    </h2>
                                                </div>
                                                 <div style="float: right;">
                                                    <h2>
                                                        <asp:ImageButton ID="imgback" runat="server" OnClick="imgback_Click" />
                                                    </h2>
                                                </div>
                                            </th>
                                        </tr>
                                        <tr class="even-row">
                                            <td colspan="100%" colspan="2">
                                                <table width="100%" cellpadding="0" cellspacing="0">
                                                    <tr>
                                                        <td width="33%" valign="top">
                                                            <div style="-moz-border-radius: 5px 5px 5px 5px; background: #d7d7d7; padding: 5px;
                                                                width: 98%">
                                                                <table width="100%" cellpadding="0" cellspacing="0" border="0">
                                                                    <tr>
                                                                        <td align="center" style="padding-left: 45%;">
                                                                           <%-- <input tabindex="7" type="radio" checked id="flat-radio" name="flat-radio" />--%>
                                                                            <asp:Literal ID="ltradio" runat="server"></asp:Literal>
                                                                            <label for="flat-radio-1" style="float: left; margin-top: 2px; margin-left: 2px;">
                                                                                Layout 1</label>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td width="100%" style="padding: 5px;">
                                                                            <table cellpadding="0" cellspacing="0" style="border: dashed 1px #d7d7d7;" width="100%">
                                                                                <tr>
                                                                                    <td style="height: 321px; text-align: center; vertical-align: middle;">
                                                                                        1590 x 750
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </div>
                                                        </td>
                                                        <td width="33%" valign="top">
                                                            <div style="-moz-border-radius: 5px 5px 5px 5px; background: #d7d7d7; padding: 5px;
                                                                width: 98%">
                                                                <table width="100%" cellpadding="0" cellspacing="0" border="0" style="border: solid 1px #d7d7d7;
                                                                    -moz-border-radius: 10px 10px 10px 10px; background-color: #eee;">
                                                                    <tr>
                                                                        <td align="center" style="padding-left: 45%;">
                                                                            <%--<input tabindex="7" type="radio" id="flat-radio-1" name="flat-radio" />--%>
                                                                            <asp:Literal ID="ltradio1" runat="server"></asp:Literal>
                                                                            <label for="flat-radio-1" style="float: left; margin-top: 2px; margin-left: 2px;">
                                                                                Layout 2</label>
                                                                        </td>
                                                                    </tr>
                                                                      <tr>
                                                                        <td align="center" width="100%" style="padding: 5px;padding-top:0px;">
                                                                            <table cellpadding="0" cellspacing="0" align="center" border="0" width="100%">
                                                                                <tr>
                                                                                <td>
                                                                                 <table cellpadding="0" cellspacing="0" style="width:100%;vertical-align:top;">
                                                                                 <tr>
                                                                                  <td style="height: 317px;width:80%; text-align: center; vertical-align: middle; border: dashed 1px #d7d7d7;padding-left:2px;padding-bottom:2px;"
                                                                                        align="center"  valign="top">
                                                                                        1050 x 750
                                                                                    </td>
                                                                                 </tr>
                                                                                 </table> 
                                                                                </td>
                                                                                   
                                                                                    <td valign="top" style="width:20%" >
                                                                                        <table cellpadding="0" cellspacing="1" style="width:100%;vertical-align:top;">
                                                                                            <tr>
                                                                                                <td style="height: 317px;width:100%; text-align: center; vertical-align: top; border: dashed 1px #d7d7d7;"
                                                                                                    align="center">
                                                                                                    510 x 750
                                                                                                </td>
                                                                                            </tr>
                                                                                            <%--<tr>
                                                                                                <td style="height: 98px;width:100%; text-align: center; vertical-align: top; border: dashed 1px #d7d7d7;"
                                                                                                    align="center">
                                                                                                    380 x 115
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr>
                                                                                                <td style="height: 98px;width:100%; text-align: center; vertical-align: top; border: dashed 1px #d7d7d7;"
                                                                                                    align="center">
                                                                                                    380 x 115
                                                                                                </td>
                                                                                            </tr>--%>
                                                                                        </table>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </div>
                                                        </td>
                                                        <td width="33%" valign="top">
                                                            <div style="-moz-border-radius: 5px 5px 5px 5px; background: #d7d7d7; padding: 5px;display:none;
                                                                width: 98%">
                                                                <table width="100%" cellpadding="0" cellspacing="0" border="0" style="border: solid 1px #d7d7d7;
                                                                    -moz-border-radius: 10px 10px 10px 10px; background-color: #eee;">
                                                                    <tr>
                                                                        <td align="center" style="padding-left: 45%;">
                                                                        <asp:Literal ID="ltradio2" runat="server"></asp:Literal>
                                                                            <%--<input tabindex="7" type="radio" id="flat-radio-2" name="flat-radio" />--%>
                                                                            <label for="flat-radio-1" style="float: left; margin-top: 2px; margin-left: 2px;">
                                                                                Layout 3</label>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td align="center" width="100%" style="padding: 5px;padding-top:0px;">
                                                                            <table cellpadding="0" cellspacing="0" align="center" border="0" width="100%">
                                                                                <tr>
                                                                                <td>
                                                                                 <table cellpadding="0" cellspacing="0" style="width:100%;vertical-align:top;">
                                                                                 <tr>
                                                                                  <td style="height: 317px;width:80%; text-align: center; vertical-align: middle; border: dashed 1px #d7d7d7;padding-left:2px;padding-bottom:2px;"
                                                                                        align="center"  valign="top">
                                                                                        590 x 377
                                                                                    </td>
                                                                                 </tr>
                                                                                 </table> 
                                                                                </td>
                                                                                   
                                                                                    <td valign="top" style="width:20%" >
                                                                                        <table cellpadding="0" cellspacing="1" style="width:100%;vertical-align:top;">
                                                                                            <tr>
                                                                                                <td style="height: 98px;width:100%; text-align: center; vertical-align: top; border: dashed 1px #d7d7d7;"
                                                                                                    align="center">
                                                                                                    380 x 115
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr>
                                                                                                <td style="height: 98px;width:100%; text-align: center; vertical-align: top; border: dashed 1px #d7d7d7;"
                                                                                                    align="center">
                                                                                                    380 x 115
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr>
                                                                                                <td style="height: 98px;width:100%; text-align: center; vertical-align: top; border: dashed 1px #d7d7d7;"
                                                                                                    align="center">
                                                                                                    380 x 115
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
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="3">
                                                            <a onclick="javascript:gotonextlink();">
                                                                <img src="/App_Themes/<%=Page.Theme %>/Images/next-btn.png" /></a>
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
</asp:Content>

