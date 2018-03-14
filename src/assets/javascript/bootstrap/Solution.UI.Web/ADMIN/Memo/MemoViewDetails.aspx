<%@ Page Title="" Language="C#" MasterPageFile="~/ADMIN/Admin.Master" AutoEventWireup="true"
    CodeBehind="MemoViewDetails.aspx.cs" Inherits="Solution.UI.Web.ADMIN.Memo.MemoViewDetails" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script src="../../ckeditor/ckeditor.js" type="text/javascript"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts-main.js"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-calender-custom.js"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-calender.js"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts.js"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery.ui.accordion.js"></script>
    <%--<link href="http://ajax.googleapis.com/ajax/libs/jqueryui/1.8/themes/base/jquery-ui.css" rel="stylesheet" type="text/css"/>--%>
    <script type="text/javascript">
        $(document).ready(function () {
            $("#accordion").accordion();
        });
    </script>
    <script type="text/javascript">
        function checkmessage() {

            var oEditor = CKEDITOR.instances.ContentPlaceHolder1_txtDescription.getData();
            if (oEditor == '') {
                jAlert('Please Enter Message.', 'Message', ' CKEDITOR.instances.ContentPlaceHolder1_txtDescription');
                CKEDITOR.instances.ContentPlaceHolder1_txtDescription.focus();
                return false;
            }
            chkHeight();
            return true;

        }
        function chkHeight() {
            var windowHeight = 0;
            windowHeight = $('#tblDescription').innerHeight(); //window.innerHeight;
            document.getElementById('prepage').style.height = windowHeight + 'px';
            document.getElementById('prepage').style.display = '';
        }
        function ShowHours() {

            if (document.getElementById('ContentPlaceHolder1_txtHours') != null) {
                document.getElementById('ContentPlaceHolder1_txtHours').value = '';
                document.getElementById('ContentPlaceHolder1_txtHours').focus();
            }
            window.scrollTo(0, 0);
            centerPopup();

            document.getElementById('btnreadmore').click();

            document.getElementById('ContentPlaceHolder1_txtHours').focus();
            return false;
        }
        function checkvalidation() {
            if (document.getElementById('ContentPlaceHolder1_txtHours') != null && document.getElementById('ContentPlaceHolder1_txtHours').value == '') {
                jAlert('Please Enter Hours.', 'Message', 'ContentPlaceHolder1_txtHours');
                return false;
            }
            return true;
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

        function confirmClose() {

            return confirm('Are you sure you want to Close this Memo?');

        }

    </script>
    <div id="content-width">
        <div class="content-row1">
            <div class="create-new-order">
                &nbsp;</div>
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
                                            <th>
                                                <div class="main-title-left">
                                                    <img class="img-left" title="Internal Memo" alt="Internal Memo" src="/App_Themes/<%=Page.Theme %>/Images/header-links-icon.png" />
                                                    <h2>
                                                        <asp:Label runat="server" Text="Internal Memo" ID="lblTitle"></asp:Label></h2>
                                                </div>
                                            </th>
                                        </tr>
                                        <tr class="altrow">
                                            <td align="right">
                                                &nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <table width="100%" cellspacing="0" cellpadding="0" border="0">
                                                    <tbody>
                                                        <tr>
                                                            <td>
                                                                <div style="margin: 0;" class="content-title">
                                                                    <img alt="" title="" class="img-left" src="/App_Themes/<%=Page.Theme %>/images/title-left-bg.png">
                                                                    <h1>
                                                                        <span>Memo Detail</span>
                                                                    </h1>
                                                                    <img class="img-right" title="" alt="" src="/App_Themes/<%=Page.Theme %>/images/title-right-bg.png">
                                                                    <div style="float: right; padding: 10px 5px 0 0;">
                                                                        <asp:ImageButton ID="btnClose" OnClientClick="return confirmClose()" runat="server"
                                                                            OnClick="btnClose_Click" /></div>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <table width="100%" cellspacing="0" cellpadding="0" border="0" class="ticket-table">
                                                                    <tbody>
                                                                        <tr>
                                                                            <td width="1%" valign="middle" align="left">
                                                                                &nbsp;
                                                                            </td>
                                                                            <td width="4%" valign="middle" align="left" class="bottom">
                                                                                <strong>Subject :</strong>
                                                                            </td>
                                                                            <td width="45%" valign="middle" align="left" class="bottom">
                                                                                <asp:Label ID="lblSubject" runat="server" Text=""></asp:Label>
                                                                            </td>
                                                                            <td width="4%" valign="middle" align="left" class="bottom">
                                                                                <strong>Status :</strong>
                                                                            </td>
                                                                            <td width="45%" valign="middle" align="left" class="bottom">
                                                                                <asp:Label ID="lblStatus" runat="server" Text=""></asp:Label>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td width="1%" valign="middle" align="left">
                                                                                &nbsp;
                                                                            </td>
                                                                            <td valign="middle" align="left" class="bottom">
                                                                                <strong>Created :</strong>
                                                                            </td>
                                                                            <td valign="middle" align="left" class="bottom">
                                                                                <asp:Label ID="lblCreateOn" runat="server" Text=""></asp:Label>
                                                                            </td>
                                                                            <td valign="middle" align="left" class="bottom">
                                                                            </td>
                                                                            <td valign="middle" align="left" class="bottom">
                                                                            </td>
                                                                            <td width="1%" valign="middle" align="left">
                                                                                &nbsp;
                                                                            </td>
                                                                        </tr>
                                                                    </tbody>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                &nbsp;
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td valign="top" align="center" class="ticket-table-none-title">
                                                                <img width="16" height="16" src="/App_Themes/<%=Page.Theme %>/images/ticket-replies-icon.jpg"
                                                                    alt="" title="">
                                                                <span>Memo Replies.</span>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td valign="top" align="center">
                                                                <img width="668" height="14" src="/App_Themes/<%=Page.Theme %>/images/ticket-replies-bottom.jpg"
                                                                    alt="" title="">
                                                            </td>
                                                        </tr>
                                                    </tbody>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center">
                                                <div id="accordion" style="z-index: 1000000; width: 99%; margin-top: 10px; margin-bottom: 10px;">
                                                    <asp:Literal ID="ltrReply" runat="server"></asp:Literal>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                            </td>
                                        </tr>
                                        <tr align="center">
                                            <td>
                                                <table cellspacing="0" id="tblDescription" cellpadding="0" border="0" width="100%">
                                                    <tr>
                                                        <td>
                                                            <div style="margin: 0;" class="content-title">
                                                                <img alt="" title="" class="img-left" src="/App_Themes/<%=Page.Theme %>/images/title-left-bg.png">
                                                                <h1>
                                                                    <strong>
                                                                        <img alt="" title="" class="img-left" src="/App_Themes/<%=Page.Theme %>/images/add-inquiries-icon.png"></strong><span>Leave
                                                                            a Reply </span>
                                                                </h1>
                                                                <img class="img-right" title="" alt="" src="/App_Themes/<%=Page.Theme %>/images/title-right-bg.png">
                                                            </div>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2" align="left">
                                                            <div id="prepage" style="position: absolute; font-family: arial; font-size: 16; background: ;
                                                                layer-background-color: white; width: 100%; z-index: 1000; display: none;">
                                                                <table width="100%" align="center">
                                                                    <tr>
                                                                        <td align="center" style="color: #696969;" valign="middle">
                                                                            <div id="divLoaderImage" style="background-color: #fff; margin: 25% 20%; width: 215px;
                                                                                border: 1px solid #F26660; opacity: 1; filter: alpha(opacity=100);">
                                                                                <img style="margin-top: 7%;" src="/App_Themes/<%=Page.Theme %>/images/Loading1.gif"
                                                                                    alt=""><br>
                                                                                <b>Please wait...</b>
                                                                            </div>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </div>
                                                            <table cellpadding="0" cellspacing="0" width="100%" class="border-all">
                                                                <tr>
                                                                    <td align="left" style="vertical-align: middle;">
                                                                        E-mail To :
                                                                    </td>
                                                                    <td align="left" valign="top">
                                                                        &nbsp;&nbsp;<asp:Literal ID="ltEmailTo" runat="server"></asp:Literal>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="left" valign="top">
                                                                        <span style="color: red">&nbsp;</span>Message :
                                                                    </td>
                                                                    <td valign="top" colspan="3">
                                                                        <asp:TextBox TextMode="multiLine" ID="txtDescription" Rows="10" Columns="80" runat="server"></asp:TextBox>
                                                                        <script type="text/javascript">
                                                                            CKEDITOR.replace('<%= txtDescription.ClientID %>', { baseHref: '<%= ResolveUrl("~/ckeditor/") %>', height: 250 });
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
                                                                <tr>
                                                                    <td align="left">
                                                                        Attachment :
                                                                    </td>
                                                                    <td align="left">
                                                                        <asp:FileUpload ID="flUpload" runat="server" />
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="left">
                                                                    </td>
                                                                    <td align="left" align="center">
                                                                        <asp:ImageButton ID="btnSenmessage" OnClientClick="return checkmessage();" runat="server"
                                                                            ImageUrl="/App_Themes/<%=Page.Theme %>/images/submit.gif" OnClick="imgsendmessage_Click" />
                                                                        &nbsp;&nbsp;&nbsp;
                                                                        <asp:ImageButton ID="btnCancel" runat="server" AlternateText="Cancel" ToolTip="Cancel"
                                                                            OnClick="btnCancel_Click" CausesValidation="false" />
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
                    </td>
                </tr>
                <tr>
                    <td height="10" align="left" valign="top">
                        <img src="/App_Themes/<%=Page.Theme %>/images/spacer.gif" width="1" alt="" height="10" />
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
