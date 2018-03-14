<%@ Page Title="" Language="C#" MasterPageFile="~/ADMIN/Admin.Master" AutoEventWireup="true"
    CodeBehind="VendorQuoteMailFormat.aspx.cs" Inherits="Solution.UI.Web.ADMIN.Orders.VendorQuoteMailFormat" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts-main.js"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts.js"></script>
    <script type="text/javascript" src="../../ckeditor/ckeditor.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="content-width">
        <div class="content-row1">
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
                                    <table width="100%" cellspacing="0" cellpadding="0" border="0" class="content-table border-td"
                                        style="padding: 2px;">
                                        <tbody>
                                            <tr>
                                                <th>
                                                    <div class="main-title-left" style="color: #fff; text-align: left; padding-top: 0px;">
                                                        Vendor Email for Warehouse P.O.
                                                    </div>
                                                    <div class="main-title-right">
                                                    </div>
                                                </th>
                                            </tr>
                                            <tr>
                                                <td style="padding: 2px; border: 1px solid #e7e7e7;">
                                                    <table cellpadding="1" cellspacing="2" width="100%">
                                                        <tr>
                                                            <td style="width: 100px;">
                                                            </td>
                                                            <td align="right">
                                                                <asp:ImageButton ID="btnPrint" Visible="false" OnClientClick="printAllCheck();" runat="server" />&nbsp;&nbsp;&nbsp;
                                                                <asp:ImageButton ID="btnSendmailToVendor" runat="server" 
                                                                    OnClientClick="return Validate();" onclick="btnSendmailToVendor_Click"
                                                                    />&nbsp;&nbsp;&nbsp; <a style="padding-right: 10px;"
                                                                        href="javascript:history.go(-1);">
                                                                        <img src="/App_Themes/<%=Page.Theme %>/images/back.png" alt="Go to Generate Warehouse PO"
                                                                            title="Go to Generate Warehouse PO" />
                                                                    </a>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td valign="top">
                                                                &nbsp;
                                                            </td>
                                                            <td valign="top">
                                                               <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                                                    <tr>
                                                                        <td class="ckeditor-table">
                                                                            <div id="getprint">
                                                                                <asp:TextBox TextMode="multiLine" class="description-textarea" ID="txtDescription"
                                                                                    Rows="10" Columns="80" runat="server" Style="border: solid 1px #e7e7e7; background: none repeat scroll 0 0 #E7E7E7;"></asp:TextBox>
                                                                            </div>
                                                                            <div id="divScript" runat="server" visible="false" style="padding-left: 5px;">
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
                                                                            </div>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                        </tbody>
                                    </table>
                                    <div id="divPrint" style="display: none;">
                                        <asp:Literal ID="ltPrint" runat="server"></asp:Literal>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </div>
        <div style="visibility: hidden">
            <iframe id="ifmcontentstoprint"></iframe>
        </div>
    </div>
</asp:Content>
