<%@ Page Title="" Language="C#" MasterPageFile="~/ADMIN/Admin.Master" AutoEventWireup="true" CodeBehind="SmallHomePagebannerlist_MVC.aspx.cs" Inherits="Solution.UI.Web.ADMIN.Settings.SmallHomePagebannerlist_MVC" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
 <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts-main.js"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts.js"></script>
    <script src="/App_Themes/<%=Page.Theme %>/js/jqueryControl.js"></script>
    <script src="/App_Themes/<%=Page.Theme %>/js/jquery.icheck.min.js"></script>
    <script src="/App_Themes/<%=Page.Theme %>/js/CustomhtmlControl.js"></script>
    <script type="text/javascript" src="/js/popup.js"></script>
    <link href="/css/general.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">



        function makeInActive(storeid, id) {

            document.getElementById('<%=hdnTypeid.ClientID %>').value = id;
            document.getElementById('<%=btnInactive.ClientID %>').click();
        }
        function makeActive(storeid, id) {

            document.getElementById('<%=hdnTypeid.ClientID %>').value = id;
            document.getElementById('<%=btnActive.ClientID %>').click();
        }
        function ShowModelCredit(storeid, id) {

            window.scrollTo(0, 0);

            document.getElementById('frmdisplay1').src = 'HomelayoutPreview.aspx?storeid=' + storeid + '&id=' + id;
            centerPopup();
            loadPopup();
        }
        $(document).ready(function () {
            $('input').iCheck({
                checkboxClass: 'icheckbox_flat-green',
                increaseArea: '20%'
            });

            $("#fieldset1").mouseover(function () {


                // $("#legend1").attr('style', 'display:block;float:right;background-color:#eee;height:31px;');
                if (document.getElementById('legend1') != null) {
                    document.getElementById('legend1').style.display = '';

                }

            });
            $("#fieldset1").mouseout(function () {

                // $(this).fadeTo('slow',1);
                //$(this).fadeOut('slow');

                if (document.getElementById('legend1') != null) {
                    document.getElementById('legend1').style.display = '';
                }


            });

            $("#fieldset2").mouseover(function () {


                // $("#legend1").attr('style', 'display:block;float:right;background-color:#eee;height:31px;');
                if (document.getElementById('legend2') != null) {
                    document.getElementById('legend2').style.display = '';

                }

            });
            $("#fieldset2").mouseout(function () {

                // $(this).fadeTo('slow',1);
                //$(this).fadeOut('slow');

                if (document.getElementById('legend2') != null) {
                    document.getElementById('legend2').style.display = '';
                }

            });
            $("#fieldset3").mouseover(function () {


                // $("#legend1").attr('style', 'display:block;float:right;background-color:#eee;height:31px;');
                if (document.getElementById('legend3') != null) {
                    document.getElementById('legend3').style.display = '';

                }

            });
            $("#fieldset3").mouseout(function () {

                // $(this).fadeTo('slow',1);
                //$(this).fadeOut('slow');

                if (document.getElementById('legend3') != null) {
                    document.getElementById('legend3').style.display = '';
                }

            });


        });

        function Deleterecord(id) {
            if (confirm('Are you sure want ot delete this record?')) {
                document.getElementById('<%=hdnid.ClientID %>').value = id;
                document.getElementById('<%=btnDelete.ClientID %>').click();
                return true;
            }
            else {
                return false;
            }
            return false;

        }
        function getlayoutid(id) {
            document.getElementById('<%=hdnlayouid.ClientID %>').value = id;
            document.getElementById('<%=btnUpdate.ClientID %>').click();
        }

        function getdeleteid(id) {
            if (confirm('Are you sure want to Delete this Group?')) {
                document.getElementById('<%=hdnlayouid.ClientID %>').value = id;
                document.getElementById('<%=btnDeletegroup.ClientID %>').click(); return true;
            } else { return false; }

        }
        function keyRestrict(e, validchars) {

            var key = '', keychar = '';

            key = getKeyCode(e);

            if (key == null) return true;

            keychar = String.fromCharCode(key);

            keychar = keychar.toLowerCase();

            validchars = validchars.toLowerCase();

            if (validchars.indexOf(keychar) != -1)

                return true;

            if (key == null || key == 0 || key == 8 || key == 9 || key == 13 || key == 27 || key == 46)

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
    <div id="content-width">
        <div class="content-row1">
            <div class="create-new-order" style="width: 100%; padding-top: 5px;">
                <table>
                    <tr>
                        <td style="display:none;">
                            Store:
                            <asp:DropDownList ID="ddlStore" runat="server" CssClass="product-type" Height="21px"
                                AutoPostBack="true" OnSelectedIndexChanged="ddlStore_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td>
                           Banner Status:
                            <asp:DropDownList ID="ddlstatus" runat="server" CssClass="product-type" Height="21px" AutoPostBack="true" OnSelectedIndexChanged="ddlstatus_SelectedIndexChanged">
                                <asp:ListItem Text="ALL" Value="">ALL</asp:ListItem>
                                <asp:ListItem Text="ACTIVE" Value="1" Selected="True">ACTIVE</asp:ListItem>
                                <asp:ListItem Text="INACTIVE" Value="0">INACTIVE</asp:ListItem>
                                <asp:ListItem Text="FUTURE" Value="2">FUTURE</asp:ListItem>
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
                                            <th align="left" colspan="2">
                                                <div class="main-title-left">
                                                    <img class="img-left" title="Small Home Page Banner" alt="Small Home Page Banner" src="/App_Themes/<%=Page.Theme %>/Images/home-page-banner-icon.png" />
                                                    <h2>
                                                        <asp:Label ID="lblTitle" runat="server" Text="Small Home Page Banner">
                                                        </asp:Label>
                                                    </h2>
                                                </div>
                                                <div style="float: right;">
                                                    <h2>
                                                        <a href='SmallHomepageRatotingbanner.aspx?id=<%=ddlStore.SelectedValue %>'>
                                                            <img src='/App_Themes/<%= Page.Theme %>/button/smal-Add-Banner.png' border='0' /></a>
                                                    </h2>
                                                </div>
                                            </th>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                            </td>
                                        </tr>
                                        <tr class="even-row">
                                            <td colspan="100%" valign="top">
                                                <table width="100%" cellspacing="0" cellpadding="0" border="0" bgcolor="#ffffff"
                                                    class="content-table">
                                                    <tbody>
                                                        <tr>
                                                            <td class="border-td-sub" width="100%" valign="top" align="left">
                                                                <asp:Literal ID="ltactive" runat="server"></asp:Literal>
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
                    </td>
                </tr>
            </table>
        </div>
    </div>
    <div style="display: none;">
        <input type="hidden" id="hdnid" runat="server" value="0" />
        <asp:Button ID="btnDelete" runat="server" OnClick="btnDelete_Click" />
        <asp:Button ID="btnUpdate" runat="server" OnClick="btnUpdate_Click" />
          <asp:Button ID="btnDeletegroup" runat="server" OnClick="btnDeletegroup_Click" />
        <asp:Button ID="btnActive" runat="server" OnClick="btnActive_Click" />
        <asp:Button ID="btnInactive" runat="server" OnClick="btnInactive_Click" />
        <input type="hidden" id="hdnlayouid" runat="server" value="0" />
        <input type="hidden" id="hdnTypeid" runat="server" value="0" />
    </div>
    <div id="popupContact" style="z-index: 1000001; width: 1010px; height: 550px;">
        <table width="100%" border="0" cellspacing="0" cellpadding="3" style="border: 1px solid #444;
            font-size: 12px;">
            <tr style="height: 25px;">
                <td valign="middle" align="left" style="color: #fff; font-family: Arial,Helvetica,sans-serif;
                    font-weight: bold;">
                    &nbsp;
                </td>
                <td align="right" valign="top">
                    <asp:ImageButton ID="popupContactClose" Style="position: relative;" ImageUrl="/App_Themes/gray/images/cancel-icon.png"
                        runat="server" ToolTip="Close" OnClientClick="return false;"></asp:ImageButton>
                </td>
            </tr>
            <tr>
                <td colspan="2" valign="top">
                    <iframe id="frmdisplay1" frameborder="0" height="500px" width="1000px" frameborder="0"
                        scrolling="no"></iframe>
                </td>
            </tr>
        </table>
    </div>
    <div id="backgroundPopup" style="z-index: 1000000;">
    </div>
</asp:Content>

