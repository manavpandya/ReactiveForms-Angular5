<%@ Page Title="" Language="C#" MasterPageFile="~/ADMIN/Admin.Master" AutoEventWireup="true"
    CodeBehind="Viewcustomerquotecomments.aspx.cs" Inherits="Solution.UI.Web.ADMIN.Orders.Viewcustomerquotecomments"
    ValidateRequest="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts-main.js"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts.js"></script>
    <script type="text/javascript" src="../../ckeditor/ckeditor.js"></script>
    <script src="../../ckeditor/_samples/sample.js" type="text/javascript"></script>
     <script type="text/javascript">
         function ValidatePage() {
             if (document.getElementById('txtsubject') != null && document.getElementById('txtsubject').value == '') {
                 jAlert('Please Enter Subject.', 'Message', 'txtSubject');
                 return false;
             }
             if (document.getElementById('txtmsgbody') != null && document.getElementById('txtmsgbody').value == '') {
                  jAlert('Please Enter Message.', 'Message', 'txtmsgbody');              
                 return false;
             }
             document.getElementById("prepage").style.display = '';
             return true;
         }
    </script>
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
                        <table width="100%" cellpadding="0" cellspacing="0" bgcolor="#FFFFFF" class="content-table">
                            <tr>
                                <td class="border-td-sub">
                                    <table width="100%" cellpadding="0" cellspacing="0" class="table">
                                        <tr>
                                            <th colspan="2">
                                                <div class="main-title-left">
                                                    <h2>
                                                        Customer Comment</h2>
                                                </div>
                                            </th>
                                        </tr>
                                        <tr>
                                            <td>
                                                Name
                                            </td>
                                            <td>
                                                <asp:Label ID="lblname" runat="server" Text="-"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Email
                                            </td>
                                            <td>
                                                <asp:Label ID="lblEmail" runat="server" Text="-"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Address
                                            </td>
                                            <td>
                                                <asp:Label ID="lblAddress" runat="server" Text="-"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td width="20%">
                                                Comment:
                                            </td>
                                            <td>
                                                <asp:Label ID="lblQuoteComment" runat="server"></asp:Label>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                        <table class="table"><tr><td>
                        <table width="100%" border="0" cellspacing="0" cellpadding="0">
                        <tr><td>Subject :</td> <td align="left"><asp:TextBox ID="txtsubject" runat="server"  CssClass="order-textfield" Width="400px"></asp:TextBox>
                         </td></tr>
                            <tr>
                            <td valign="top">Message:</td>
                                <td class="ckeditor-table">
                                    <asp:TextBox TextMode="multiLine" class="description-textarea" ID="txtmsgbody"
                                        TabIndex="27" Rows="10" Columns="80" runat="server" Style="border: solid 1px #e7e7e7;
                                        background: none repeat scroll 0 0 #E7E7E7;"></asp:TextBox>
                                    <script type="text/javascript">
                                        CKEDITOR.replace('<%= txtmsgbody.ClientID %>', { baseHref: '<%= ResolveUrl("~/ckeditor/") %>', height: 300 });
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
                            </tr><tr><td colspan="2" align="center">
                                <asp:ImageButton ID="imgsendmail" runat="server" onclick="imgsendmail_Click"  OnClientClick="return ValidatePage()" /></td></tr>
                        </table>
                        
                        
                        </td></tr></table>
                        
                    </td>
                </tr>
            </table>
        </div>
    </div>
</asp:Content>
