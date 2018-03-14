<%@ Page Title="" Language="C#" MasterPageFile="~/ADMIN/Admin.Master" AutoEventWireup="true"
    CodeBehind="EmailTemplate.aspx.cs" Inherits="Solution.UI.Web.ADMIN.Settings.EmailTemplate"
    ValidateRequest="false" %>

<%@ Register Assembly="CKEditor.NET" Namespace="CKEditor.NET" TagPrefix="CKEditor" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript" src="../../ckeditor/ckeditor.js"></script>
    <script src="../../ckeditor/_samples/sample.js" type="text/javascript"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts-main.js"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts.js"></script>
    <script type="text/javascript">
        function Checkfields() {

            if (document.getElementById('<%=txtLabel.ClientID %>').value == '') {
                jAlert('Please enter Label.', 'Message', '<%=txtLabel.ClientID %>');
                return false;
            }

            else if (document.getElementById('<%=txtSubject.ClientID %>').value == '') {
                jAlert('Please enter Subject.', 'Message', '<%=txtSubject.ClientID %>');
                return false;
            }
            return true;
        }
     
    </script>
    <script type="text/javascript">
        var testresults
        function checkemail1(str) {
            var filter = /^([\w-]+(?:\.[\w-]+)*)@((?:[\w-]+\.)*\w[\w-]{0,66})\.([a-z]{1,6}(?:\.[a-z]{1})?)$/i
            if (filter.test(str))
                testresults = true
            else {
                testresults = false
            }
            return (testresults)
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
                                                    <img class="img-left" title="Add Email template" alt="Add Email template" src="/App_Themes/<%=Page.Theme %>/Images/emailTemplate.png">
                                                    <h2>
                                                        <asp:Label ID="lblHeader" runat="server" Text="Add Email Template"></asp:Label></h2>
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
                                                        <td style="width: 12%">
                                                            <span class="star">&nbsp</span>Store Name:
                                                        </td>
                                                        <td style="width: 80%">
                                                             <asp:DropDownList ID="ddlStore" AutoPostBack="true" runat="server" CssClass="order-list"
                                                                Width="185px">
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                    <tr class="oddrow">
                                                        <td style="width: 12%">
                                                            <span class="star">*</span>Label:
                                                        </td>
                                                        <td style="width: 80%">
                                                            <asp:TextBox runat="server" ID="txtLabel" CssClass="order-textfield" MaxLength="50"
                                                                Width="350px"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                   <tr class="altrow">
                                                        <td valign="top">
                                                            <span class="star">&nbsp;</span>To Email:
                                                        </td>
                                                        <td>
                                                            <asp:TextBox runat="server" ID="txtto" CssClass="order-textfield" MaxLength="500"
                                                                Width="350px"></asp:TextBox><span>(Use ";" for multi email address)</span>
                                                        </td>
                                                    </tr>
                                                    <tr class="altrow">
                                                        <td valign="top">
                                                            <span class="star">&nbsp;</span>CC Email:
                                                        </td>
                                                        <td>
                                                            <asp:TextBox runat="server" ID="txtcc" CssClass="order-textfield" MaxLength="500"
                                                                Width="350px"></asp:TextBox><span>(Use ";" for multi email address)</span>
                                                        </td>
                                                    </tr>
                                                    <tr class="altrow">
                                                        <td valign="top">
                                                            <span class="star">*</span>Subject:
                                                        </td>
                                                        <td>
                                                            <asp:TextBox runat="server" ID="txtSubject" CssClass="order-textfield" MaxLength="500"
                                                                Width="350px"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr class="oddrow">
                                                        <td valign="top">
                                                            <span class="star">*</span>Email Body:
                                                        </td>
                                                        <td>
                                                            <asp:TextBox TextMode="multiLine" class="description-textarea" ID="txtMailBody" Rows="10"
                                                                Columns="80" runat="server" Style="border: solid 1px #e7e7e7; background: none repeat scroll 0 0 #E7E7E7;"></asp:TextBox>
                                                            <script type="text/javascript">
                                                                CKEDITOR.replace('<%= txtMailBody.ClientID %>', { baseHref: '<%= ResolveUrl("~/ckeditor/") %>', height: 400 });
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
                                                    <tr class="altrow">
                                                        <td valign="top">
                                                            <span class="star">&nbsp;</span>PO Template:
                                                        </td>
                                                        <td>
                                                            <asp:CheckBox ID="chkIsPO" runat="server" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr class="oddrow">
                                            <td>
                                                <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                                    <td align="left" style="width:13%">
                                                    </td>
                                                    <td style="width: 87%">
                                                        <asp:ImageButton ID="btnSaveTemplate" runat="server" AlternateText="Save" ToolTip="Save"
                                                            OnClientClick="return Checkfields();" OnClick="btnSaveTemplate_Click" />
                                                        <asp:ImageButton ID="btnCancelTemplate" runat="server" AlternateText="Cancel" ToolTip="Cancel"
                                                            CausesValidation="false" OnClick="btnCancelTemplate_Click" />
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
            </table>
        </div>
    </div>
</asp:Content>
